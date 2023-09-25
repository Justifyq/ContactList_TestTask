using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Factories.Data;
using Model;
using Model.DTO;
using Model.DTO.Storages;
using Services.Network;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Services.Data
{
    public class DataLoader : IDataLoader
    {
        private const int SpriteCount = 5;
        private const string DefaultIcoPath = "progile-avatar-bg";

        private readonly AvatarFactory _avatarFactory;
        private readonly EmployeeFactory _employeeFactory;
        private readonly IContactsLoader _contactsLoader;
        
        private EmployeeInfoDTO[] _employeeInfoDtos;
        private AvatarDtoStorage _avatarsStorage;
        private Avatar[] _avatars;

        public DataLoader(ISpriteLoaderService spriteLoaderService, IContactsLoader contactsLoader)
        {
            _avatarFactory = new AvatarFactory(Resources.Load<Sprite>(DefaultIcoPath), spriteLoaderService);
            _contactsLoader = contactsLoader;
            _employeeFactory = new EmployeeFactory();
        }


        public void LoadData(string avatarStoragePath, string employeeStoragePath,
            FavoriteEmployeesStorage favoriteEmployeesStorage, Action<Dictionary<Employee, EmployeeDTO>> loaded)
        {
            _contactsLoader.Load(storage =>
            {
                _employeeInfoDtos = storage.data;
                loaded?.Invoke(LoadData(avatarStoragePath, employeeStoragePath, favoriteEmployeesStorage));
            });
        }

        public Dictionary<Employee, EmployeeDTO> LoadData(string avatarStoragePath, string employeeStoragePath, FavoriteEmployeesStorage favoriteEmployeesStorage = null)
        {
            _avatars = LoadAvatars(avatarStoragePath);
            return LoadEmployees(employeeStoragePath, _avatars, favoriteEmployeesStorage);
        }

        private Avatar[] LoadAvatars(string avatarStoragePath)
        {
            if (!PlayerPrefs.HasKey(avatarStoragePath))
                return _avatarFactory.CreateAvatars(SpriteCount, avatarDtos => AvatarDtoCreated(avatarStoragePath, avatarDtos));

            AvatarDtoStorage storage = JsonUtility.FromJson<AvatarDtoStorage>(PlayerPrefs.GetString(avatarStoragePath));
            return _avatarFactory.CreateAvatars(storage);
        }

        private Dictionary<Employee, EmployeeDTO> LoadEmployees(string storagePath, Avatar[] avatars, FavoriteEmployeesStorage favoriteEmployeesStorage = null)
        {
            if (!PlayerPrefs.HasKey(storagePath))
            {
                Dictionary<Employee, EmployeeDTO> createdEmployees = _employeeFactory.CreateEmployees(_employeeInfoDtos, avatars, favoriteEmployeesStorage);
                EmployeesDtoCreated(storagePath,createdEmployees.Values.ToArray());
                return createdEmployees;
            }

            EmployeeDTO[] employeeDtos = JsonUtility.FromJson<EmployeeDtoStorage>(PlayerPrefs.GetString(storagePath)).Data;
                
            Dictionary<Employee, EmployeeDTO> employees = _employeeInfoDtos.Length == employeeDtos.Length 
                ? _employeeFactory.CreateEmployees(employeeDtos, _employeeInfoDtos, avatars, favoriteEmployeesStorage) 
                : AppendNewEmployees(avatars, favoriteEmployeesStorage, employeeDtos);

            return employees;
        }

        private Dictionary<Employee, EmployeeDTO> AppendNewEmployees(Avatar[] avatars, FavoriteEmployeesStorage favoriteEmployeesStorage,
            EmployeeDTO[] employeeDtos)
        {
            int lastEmployeeId = employeeDtos.Max(e => e.EmployeeId);

            EmployeeInfoDTO[] toAdd = _employeeInfoDtos.Where(e => e.id > lastEmployeeId).ToArray();
            Dictionary<Employee, EmployeeDTO> employees = _employeeFactory.CreateEmployees(employeeDtos, _employeeInfoDtos, avatars, favoriteEmployeesStorage);
            Dictionary<Employee, EmployeeDTO> newEmployees = _employeeFactory.CreateEmployees(toAdd, avatars, favoriteEmployeesStorage);

            foreach ((Employee employee, EmployeeDTO employeeDto) in newEmployees)
                employees.Add(employee, employeeDto);
            
            return employees;
        }
        
        private void AvatarDtoCreated(string avatarStoragePath, AvatarDto[] dtos)
        {
            var storage = new AvatarDtoStorage()
            {
                Data = dtos,
            };

            SaveToPrefs(avatarStoragePath, storage);
        }

        private void EmployeesDtoCreated(string storagePath, EmployeeDTO[] dtos)
        {
            var storage = new EmployeeDtoStorage()
            {
                Data = dtos,
            };

            SaveToPrefs(storagePath, storage);
        }

        private void SaveToPrefs<T>(string storagePath, T toSave) where T : class
        {
            string json = JsonUtility.ToJson(toSave);
            PlayerPrefs.SetString(storagePath, json);
            PlayerPrefs.Save();
        }
    }
}