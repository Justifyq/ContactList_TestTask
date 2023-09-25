using System;
using Model.DTO;
using UnityEngine;

namespace Model
{
    public class Employee
    {
        public event Action AvatarUpdated
        {
            add => _avatar.SpriteUpdated += value;
            remove => _avatar.SpriteUpdated -= value;
        }
        
        public event Action<bool> FavoriteChanged;

        public int Id;
        
        public string FullName { get; }
        public string Email { get; }
    
        public string IpAddress { get; }
        public string Gender { get; }
        
        public Sprite Avatar => _avatar.Sprite;

        private Avatar _avatar;

        public bool Favorite
        {
            get => _isFavorite;

            set
            {
                if (value == _isFavorite)
                    return;

                _isFavorite = value;
                FavoriteChanged?.Invoke(value);
            }
        }

        private bool _isFavorite;
        
        public Employee(EmployeeInfoDTO employeeInfoDto, Avatar avatar, bool isFavorite)
        {
            Id = employeeInfoDto.id;
            FullName = $"{employeeInfoDto.first_name} {employeeInfoDto.last_name}";
            Email = employeeInfoDto.email;
            Gender = employeeInfoDto.gender;
            IpAddress = employeeInfoDto.ip_address;
            _avatar = avatar;
            _isFavorite = isFavorite;
        }
    }
}