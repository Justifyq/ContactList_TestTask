using Model.DTO;
using Services.Serialization;
using UnityEngine;
using Avatar = Model.Avatar;

namespace Factories.Data.DTO
{
    public class EmployeeDtoFactory
    {
        private readonly ISerializationService _serializationService;
        private int _generatedSpriteId = -1;
    
        public EmployeeDTO[] CreateEmployeeDtos(EmployeeInfoDTO[] employeeInfoDtos, Avatar[] avatars)
        {
            var employeeDtos = new EmployeeDTO[employeeInfoDtos.Length];

            for (var i = 0; i < employeeInfoDtos.Length; i++)
            {
                employeeDtos[i] = new EmployeeDTO
                {
                    EmployeeId = employeeInfoDtos[i].id,
                    Favorite = false,
                    AvatarId = GetRandomSpriteId(avatars)
                };
            }

            return employeeDtos;
        }

        private int GetRandomSpriteId(Avatar[] avatarDtos)
        {
            if (_generatedSpriteId >= 0)
                (avatarDtos[^1], avatarDtos[_generatedSpriteId]) = (avatarDtos[_generatedSpriteId], avatarDtos[^1]);
        
            _generatedSpriteId = Random.Range(0, avatarDtos.Length - 1);
            return avatarDtos[_generatedSpriteId].Id;
        }
    }
}