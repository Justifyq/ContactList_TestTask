using System.Collections.Generic;
using System.Linq;
using Factories.Views;
using Model;
using Services.Data;
using UnityEngine;

namespace UI.Controllers
{
    public class EmployeeFavoritePanelController : MonoBehaviour
    {
        [SerializeField] private Transform container;
        
        private readonly Dictionary<Employee, GameObject> _favoriteEmployees = new();
        
        private EmployeeCardFactory _employeeCardFactory;
        private IDataService _dataService;


        private void OnDestroy()
        {
            if (_dataService != null)
                _dataService.FavoriteEmployeesUpdated -= DataService_OnFavoriteEmployeesUpdated;
        }

        public void Construct(IDataService dataService, EmployeeCardFactory cardFactory)
        {
            _dataService = dataService;
            _employeeCardFactory = cardFactory;
            _dataService.FavoriteEmployeesUpdated += DataService_OnFavoriteEmployeesUpdated;
            AddAllEmployees();
        }
        
        private void DataService_OnFavoriteEmployeesUpdated()
        {
            RemoveEmployees();
            AddEmployees();
        }

        private void RemoveEmployees()
        {
            if (_dataService.FavoriteEmployees == null)
                return;
            
            var toRemove = _favoriteEmployees.Keys.FirstOrDefault(em => !_dataService.FavoriteEmployees.Contains(em));
            
            if (toRemove == null)
                return;

            Destroy(_favoriteEmployees[toRemove]);
            _favoriteEmployees.Remove(toRemove);
        }

        private void AddEmployees()
        {
            if (_dataService.FavoriteEmployees == null || !_dataService.FavoriteEmployees.Any())
                return;
            
            var toAdd = _dataService.FavoriteEmployees.FirstOrDefault(em => !_favoriteEmployees.ContainsKey(em));
            
            if (toAdd == null)
                return;

            var card = _employeeCardFactory.CreateCard(container, toAdd);
            _favoriteEmployees.Add(toAdd, card.gameObject);
        }

        private void AddAllEmployees()
        {
            if (_dataService.FavoriteEmployees == null || !_dataService.FavoriteEmployees.Any())
                return;
            
            var toAdd = _dataService.FavoriteEmployees.Where(em => !_favoriteEmployees.ContainsKey(em));

            foreach (Employee employee in toAdd)
            {
                var card = _employeeCardFactory.CreateCard(container, employee);
                _favoriteEmployees.Add(employee, card.gameObject);
            }
        }
    }
}