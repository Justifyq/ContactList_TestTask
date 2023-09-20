using System;
using Model.DTO;
using UnityEngine;

namespace Model
{
    public class Employee
    {
        public event Action<bool> FavoriteChanged; 

        public string FullName { get; }
        public string Email { get; }
    
        public string IpAddress { get; }
        public string Gender { get; }
        public Sprite Avatar { get;  }

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
    

        public Employee(EmployeeInfoDTO employeeInfoDto, Sprite sprite, bool isFavorite)
        {
            FullName = $"{employeeInfoDto.first_name} {employeeInfoDto.last_name}";
            Email = employeeInfoDto.email;
            Gender = employeeInfoDto.gender;
            IpAddress = employeeInfoDto.ip_address;
            Avatar = sprite;
            _isFavorite = isFavorite;
        }
    }
}