using System.Collections.Generic;
using System.Linq;
using Model;
using Model.DTO;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Factories.Data
{
    public class EmployeeFactory
    {
        private int _generatedSpriteId = -1;

        private EmployeeDTO[] CreateEmployeeDtos(EmployeeInfoDTO[] employeeInfoDtos, int avatarsCount)
        {
            var employeeDtos = new EmployeeDTO[employeeInfoDtos.Length];

            var avatarsIds = new int[avatarsCount];
            
            for (var i = 0; i < avatarsIds.Length; i++) 
                avatarsIds[i] = i;

            for (var i = 0; i < employeeInfoDtos.Length; i++)
            {
                employeeDtos[i] = new EmployeeDTO
                {
                    EmployeeId = employeeInfoDtos[i].id,
                    AvatarId = GetRandomSpriteId(avatarsIds)
                };
            }

            return employeeDtos;
        }

        private int GetRandomSpriteId(int[] avatarDtos)
        {
            if (_generatedSpriteId >= 0)
                (avatarDtos[^1], avatarDtos[_generatedSpriteId]) = (avatarDtos[_generatedSpriteId], avatarDtos[^1]);
        
            _generatedSpriteId = Random.Range(0, avatarDtos.Length - 1);
            return avatarDtos[_generatedSpriteId];
        }
        
        public Dictionary<Employee, EmployeeDTO> CreateEmployees(EmployeeInfoDTO[] employeeInfoDtos, Avatar[] avatars, FavoriteEmployeesStorage favoriteEmployeesStorage = null)
        {
            var dtos = CreateEmployeeDtos(employeeInfoDtos, avatars.Length);
            return CreateEmployees(dtos, employeeInfoDtos, avatars, favoriteEmployeesStorage);
        }

        public Dictionary<Employee, EmployeeDTO> CreateEmployees(EmployeeDTO[] employeeDtos, EmployeeInfoDTO[] employeeInfoDtos, Avatar[] avatars, FavoriteEmployeesStorage favoriteEmployeesStorage = null)
        {
            var employees = new Dictionary<Employee, EmployeeDTO>();
            
            foreach (EmployeeDTO dto in employeeDtos)
            {
                EmployeeInfoDTO employeeInfo = employeeInfoDtos.First(e => e.id == dto.EmployeeId);
                Avatar avatar = avatars.First(a => a.Id == dto.AvatarId);
                bool isFavorite = favoriteEmployeesStorage != null && favoriteEmployeesStorage.Employees.Contains(employeeInfo.id);
                var employee = new Employee(employeeInfo, avatar, isFavorite);
                
                employees.Add(employee, dto);
            }
            
            return employees;
        } 
    }
}