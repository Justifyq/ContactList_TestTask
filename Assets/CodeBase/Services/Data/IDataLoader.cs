using System;
using System.Collections.Generic;
using Model;
using Model.DTO;

namespace Services.Data
{
    public interface IDataLoader
    {
        void LoadData(string avatarStoragePath, string employeeStoragePath, Action<Dictionary<Employee, EmployeeDTO>> loaded);
    }
}