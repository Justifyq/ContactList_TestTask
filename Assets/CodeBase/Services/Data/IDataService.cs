using System;
using System.Collections.Generic;
using Model;

namespace Services.Data
{
    public interface IDataService
    {
        event Action FavoriteEmployeesUpdated;
        IEnumerable<Employee> AllEmployees { get; }
        IEnumerable<Employee> FavoriteEmployees { get; }

        void Initialize(Action initialized);
    
        void SetEmployeeFavorite(Employee employee, bool favorite);
        void Save();
    }
}