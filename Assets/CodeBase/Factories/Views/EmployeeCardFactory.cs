using System.Collections.Generic;
using System.Linq;
using Model;
using Services.Data;
using UI.Controllers;
using UnityEngine;

namespace Factories.Views
{
    public class EmployeeCardFactory
    {
        private readonly EmployeeCardController _prefab;
        private readonly ProfileController _profileController;
        private readonly IDataService _dataService;

        public EmployeeCardFactory(EmployeeCardController prefab, ProfileController profileController, IDataService dataService)
        {
            _prefab = prefab;
            _dataService = dataService;
            _profileController = profileController;
        }

        public EmployeeCardController[] CreateCards(Transform container, IEnumerable<Employee> employees)
        {
            var cardControllers = new EmployeeCardController[employees.Count()];

            var index = 0;
            foreach (Employee employee in employees) 
                cardControllers[index++] = CreateCard(container, employee);

            return cardControllers;
        }

        public EmployeeCardController CreateCard(Transform container, Employee employee)
        {
            var card = Object.Instantiate(_prefab, container);
            card.Construct(employee, _profileController, _dataService);
            return card;
        }
    }
}