using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.DTO;
using Model.DTO.Storages;
using UnityEngine;

namespace Services.Data
{
    public class DataService : IDataService
    {
        private const string AvatarStoragePath = "AvatarStorage";
        private const string EmployeeStoragePath = "EmployeeStorage";
        private const string FavoriteStoragePath = "FavoriteStorage";

        public event Action FavoriteEmployeesUpdated;
        public IEnumerable<Employee> AllEmployees => _employees.Keys;
        public IEnumerable<Employee> FavoriteEmployees => _favoriteEmployees;

        private readonly IDataLoader _dataLoader;
        private readonly List<Employee> _favoriteEmployees;
        
        private Dictionary<Employee, EmployeeDTO> _employees;
    
        public DataService(IDataLoader loader)
        {
            _dataLoader = loader;
            _employees = new Dictionary<Employee, EmployeeDTO>();
            _favoriteEmployees = new List<Employee>();
        }

        public void Initialize(Action initialized)
        {
            FavoriteEmployeesStorage favoriteStorage = null;
            
            if (PlayerPrefs.HasKey(FavoriteStoragePath))
                favoriteStorage = JsonUtility.FromJson<FavoriteEmployeesStorage>(PlayerPrefs.GetString(FavoriteStoragePath));

            _dataLoader.LoadData(AvatarStoragePath, EmployeeStoragePath, favoriteStorage, employees =>
             {
                 _employees = employees;
                 
                 if (employees == null)
                     return;
                 
                 if (favoriteStorage != null)
                     _favoriteEmployees.AddRange(_employees.Where(e => e.Key.Favorite).Select(e => e.Key));

                 initialized?.Invoke();
             });
        }

        public void SetEmployeeFavorite(Employee employee, bool favorite)
        {
            if (employee.Favorite == favorite)
                return;

            if (!favorite)
                _favoriteEmployees.Remove(employee);

            if (favorite)
                _favoriteEmployees.Add(employee);

            employee.Favorite = favorite;
        
            FavoriteEmployeesUpdated?.Invoke();
        
            Save();
        }
    
        public void Save()
        {
            var favorite = new FavoriteEmployeesStorage
            {
                Employees = _favoriteEmployees.Select(e => e.Id).ToArray(),
            };

            PlayerPrefs.SetString(FavoriteStoragePath, JsonUtility.ToJson(favorite));
            PlayerPrefs.Save();
        }
    }
}