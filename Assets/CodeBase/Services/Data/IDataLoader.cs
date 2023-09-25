using System;
using System.Collections.Generic;
using Model;
using Model.DTO;

namespace Services.Data
{
    public interface IDataLoader
    {
        void LoadData(string avatarStoragePath, string employeeStoragePath, FavoriteEmployeesStorage favoriteEmployeesStorage, Action<Dictionary<Employee, EmployeeDTO>> loaded);
    }
}