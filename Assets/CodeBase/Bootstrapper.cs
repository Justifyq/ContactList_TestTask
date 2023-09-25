using System.Linq;
using Factories.Views;
using Services.Data;
using Services.Network;
using UI.Controllers;
using UnityEngine;
using Utils;

public class Bootstrapper : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] private EmployeeCardController prefab;
    [SerializeField] private ProfileController profileController;
    [SerializeField] private EmployeeFavoritePanelController employeeFavorite;
    [SerializeField] private EmployeeCardsPoolController cardsPoolController;
    
    private IDataService _dataService;
    
    private void Awake()
    {
        ISpriteLoaderService spriteLoaderService = new HttpSpriteLoader(this);
        IContactsLoader contactsLoader = new HttpContactLoader(this);
        
        IDataLoader dataLoader = new DataLoader(spriteLoaderService, contactsLoader);
        _dataService = new DataService(dataLoader);
        
        _dataService.Initialize(() =>
        {
            var employeeCardFactory = new EmployeeCardFactory(prefab, profileController, _dataService);
            cardsPoolController.Construct(_dataService.AllEmployees.ToArray(),employeeCardFactory);
            employeeFavorite.Construct(_dataService, employeeCardFactory);
        });

    }
}