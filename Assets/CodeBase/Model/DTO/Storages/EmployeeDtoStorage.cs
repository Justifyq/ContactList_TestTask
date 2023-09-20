using System;
using UnityEngine.Serialization;

namespace Model.DTO.Storages
{
    [Serializable]
    public class EmployeeDtoStorage
    {
        [FormerlySerializedAs("data")] public EmployeeDTO[] Data;
    }
}