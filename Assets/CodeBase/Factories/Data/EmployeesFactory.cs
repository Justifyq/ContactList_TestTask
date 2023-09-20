using System.Linq;
using Model;
using Model.DTO;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Factories.Data
{
    public class EmployeesFactory
    {
        public Employee CreateEmployee(EmployeeDTO employeeDto, EmployeeInfoDTO[] employeeInfoDtos, Avatar[] avatars)
        {
            EmployeeInfoDTO employeeInfo = employeeInfoDtos.First(e => e.id == employeeDto.EmployeeId);
            Sprite avatar = avatars.First(a => a.Id == employeeDto.AvatarId).Sprite;
            return new Employee(employeeInfo, avatar, employeeDto.Favorite);
        }
    }
}