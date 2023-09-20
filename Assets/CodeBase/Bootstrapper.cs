using Factories.Views;
using Services.Data;
using Services.Network;
using Services.Serialization;
using UI.Controllers;
using UnityEngine;
using Utils;

public class Bootstrapper : MonoBehaviour, ICoroutineRunner
{ 
    [SerializeField] private Transform employeesContainer;
    [SerializeField] private EmployeeCardController prefab;
    [SerializeField] private ProfileController profileController;
    [SerializeField] private EmployeeFavoritePanelController employeeFavorite;
    
    private IDataService _dataService;
    
    private void Awake()
    {
        ISerializationService serializationService = new JsonSerializationService();
        ISpriteLoaderService spriteLoaderService = new HttpSpriteLoader(this);

        IDataLoader dataLoader = new DataLoader(serializationService, spriteLoaderService);
        _dataService = new DataService(dataLoader, serializationService);
        
        _dataService.Initialize(Initialized);

    }
    
    private void Initialized()
    {
        var employeeCardFactory = new EmployeeCardFactory(prefab, profileController, _dataService);
        employeeCardFactory.CreateCards(employeesContainer, _dataService.AllEmployees);
        employeeFavorite.Construct(_dataService, employeeCardFactory);
    }
}