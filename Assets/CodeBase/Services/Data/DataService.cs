using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.DTO;
using Model.DTO.Storages;
using Services.Serialization;
using UnityEngine;

namespace Services.Data
{
    public class DataService : IDataService
    {
        private const string AvatarStoragePath = "AvatarStorage";
        private const string EmployeeStoragePath = "EmployeeStorage";

        public event Action FavoriteEmployeesUpdated;
        public bool Initialized { get; private set; }
        public IEnumerable<Employee> AllEmployees => _employees.Keys;
        public IEnumerable<Employee> FavoriteEmployees => _favoriteEmployees;

        private readonly IDataLoader _dataLoader;
        private readonly ISerializationService _serializationService;
        private readonly List<Employee> _favoriteEmployees;
    
        private bool _isInInitializeProcess;
        private Dictionary<Employee, EmployeeDTO> _employees;
    
        public DataService(IDataLoader loader, ISerializationService serializationService)
        {
            _dataLoader = loader;
            _serializationService = serializationService;
            _employees = new Dictionary<Employee, EmployeeDTO>();
            _favoriteEmployees = new List<Employee>();
        }

        public void Initialize(Action initialized)
        {
            if (_isInInitializeProcess || Initialized) 
                return;

            _isInInitializeProcess = true;
        
            _dataLoader.LoadData(AvatarStoragePath, EmployeeStoragePath, employees =>
            {
                _employees = employees;
                Initialized = true;

                var favorite = _employees.Keys.Where(e => e.Favorite);
                _favoriteEmployees.AddRange(favorite);
            

                initialized?.Invoke();
            });
        }

        public void SetEmployeeFavorite(Employee employee, bool favorite)
        {
            if (employee.Favorite == favorite)
                return;
        
            _employees[employee].Favorite = favorite;
            employee.Favorite = favorite;
        
            if (!favorite)
                _favoriteEmployees.Remove(employee);

            if (favorite)
                _favoriteEmployees.Add(employee);
        
            FavoriteEmployeesUpdated?.Invoke();
        
            Save();
        }
    
        public void Save()
        {
            var storage = new EmployeeDtoStorage
            {
                Data = _employees.Select(kv => kv.Value).ToArray()
            };

            PlayerPrefs.SetString(EmployeeStoragePath, _serializationService.Serialize(storage));
            PlayerPrefs.Save();
        }
    }
}