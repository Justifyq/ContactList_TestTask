using System;
using System.Collections.Generic;
using Factories.Data;
using Factories.Data.DTO;
using Model;
using Model.DTO;
using Model.DTO.Storages;
using Services.Network;
using Services.Serialization;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Services.Data
{
    public class DataLoader : IDataLoader
    {
        private const int SpriteCount = 5;
    
        private const string DefaultIcoPath = "progile-avatar-bg";
        private const string EmployeeInfoStoragePath = "test_task_mock_data";

        private readonly ISerializationService _serializationService;
        private readonly ISpriteLoaderService _spriteLoaderService;
        private readonly AvatarDtoFactory _avatarDtoFactory;
        private readonly AvatarFactory _avatarFactory;
        private readonly EmployeeDtoFactory _employeeDtoFactory;
        private readonly EmployeesFactory _employeesFactory;
        private readonly EmployeeInfoDTO[] _employeeInfoDtos;
        
        public DataLoader(ISerializationService serializationService, ISpriteLoaderService spriteLoaderService)
        {
            _serializationService = serializationService;
            _spriteLoaderService = spriteLoaderService;
        
            _avatarDtoFactory = new AvatarDtoFactory();
            _avatarFactory = new AvatarFactory();
        
            _employeeDtoFactory = new EmployeeDtoFactory();
            _employeesFactory = new EmployeesFactory();

            string json = Resources.Load<TextAsset>(EmployeeInfoStoragePath).text;
            _employeeInfoDtos =  _serializationService.Deserialize<EmployeesInfoStorage>(json).data;
        }

        public void LoadData(string avatarStoragePath, string employeeStoragePath, Action<Dictionary<Employee, EmployeeDTO>> loaded) => 
            LoadAvatars(avatarStoragePath, av => loaded?.Invoke(LoadEmployees(employeeStoragePath, av)));

        private void LoadAvatars(string avatarStoragePath, Action<Avatar[]> loaded)
        {
            if (PlayerPrefs.HasKey(avatarStoragePath))
                LoadExistedAvatarDtos(avatarStoragePath, CreateAvatars);
            else
                DownloadAvatars(avatarStoragePath, CreateAvatars);


            void CreateAvatars(AvatarDto[] dtos)
            {
                Avatar[] avatars = _avatarFactory.CreateAvatars(dtos);
                loaded?.Invoke(avatars);
            }
        }

        private Dictionary<Employee, EmployeeDTO> LoadEmployees(string storagePath, Avatar[] avatars)
        {
            EmployeeDTO[] employeeDtos = PlayerPrefs.HasKey(storagePath) ?
                _serializationService.Deserialize<EmployeeDtoStorage>(PlayerPrefs.GetString(storagePath)).Data
                : _employeeDtoFactory.CreateEmployeeDtos(_employeeInfoDtos, avatars);

            var employees = new Dictionary<Employee, EmployeeDTO>();
        
            foreach (EmployeeDTO dto in employeeDtos)
            {
                Employee employee = _employeesFactory.CreateEmployee(dto, _employeeInfoDtos, avatars);
                employees.Add(employee, dto);
            }

            return employees;
        }
    
        private void LoadExistedAvatarDtos(string avatarStoragePath, Action<AvatarDto[]> callback)
        {
            AvatarDto[] avatarDtos = _serializationService.Deserialize<AvatarDtoStorage>(PlayerPrefs.GetString(avatarStoragePath)).Data;
            callback?.Invoke(avatarDtos);
        }
    
        private void DownloadAvatars(string storagePath, Action<AvatarDto[]> callback)
        {
            _spriteLoaderService.LoadSprites(SpriteCount, sprites =>
            {
                bool loadSuccess = true;
                if (sprites == null || sprites.Length < SpriteCount)
                {
                    sprites = new Sprite[SpriteCount];
                    Sprite defaultSprite = Resources.Load<Sprite>(DefaultIcoPath);
                
                    for (var i = 0; i < sprites.Length; i++) 
                        sprites[i] = defaultSprite;

                    loadSuccess = false;
                }

                AvatarDto[] avatarDtos = _avatarDtoFactory.CreateAvatarDtos(sprites);

                if (loadSuccess)
                {
                    var storage = new AvatarDtoStorage
                    {
                        Data = avatarDtos
                    };
                
                    PlayerPrefs.SetString(storagePath, _serializationService.Serialize(storage));
                    PlayerPrefs.Save();
                }
                callback?.Invoke(avatarDtos);
            });
        }
    }
}