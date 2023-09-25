using System;
using UnityEngine.Serialization;

namespace Model.DTO.Storages
{
    [Serializable]
    public class EmployeeDtoStorage
    { 
        public EmployeeDTO[] Data;
    }
}