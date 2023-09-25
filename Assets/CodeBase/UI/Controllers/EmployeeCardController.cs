using System;
using Model;
using Services.Data;
using UI.View;
using UnityEngine;

namespace UI.Controllers
{
    public class EmployeeCardController : MonoBehaviour
    {
        [SerializeField] private EmployeeCardView employeeCardView;
    
        private Employee _employee;
        private IDataService _dataService;
        private ProfileController _profileController;

        private void Awake()
        {
            employeeCardView.FavoriteChanged += EmployeeCardView_OnFavoriteChanged;
            employeeCardView.DisplayButtonClicked += EmployeeCardView_OnDisplayButtonClicked;
        }

        private void OnDestroy()
        {
            employeeCardView.FavoriteChanged -= EmployeeCardView_OnFavoriteChanged;
            employeeCardView.DisplayButtonClicked -= EmployeeCardView_OnDisplayButtonClicked;

            if (_employee != null)
            {
                _employee.FavoriteChanged -= Employee_OnFavoriteChanged;
                _employee.AvatarUpdated -= Employee_OnAvatarUpdated;
            }

            employeeCardView.Dispose();
        }
    
        public void Construct(Employee employee, ProfileController profileController, IDataService dataService)
        {
            _employee = employee;
            _dataService = dataService;
            _profileController = profileController;
            
            employeeCardView.Construct(employee);
            
            _employee.AvatarUpdated += Employee_OnAvatarUpdated;
            _employee.FavoriteChanged += Employee_OnFavoriteChanged;
        }

        private void Employee_OnAvatarUpdated() => employeeCardView.SetAvatar(_employee.Avatar);

        private void EmployeeCardView_OnFavoriteChanged(bool isFavorite) => _dataService.SetEmployeeFavorite(_employee, isFavorite);

        private void EmployeeCardView_OnDisplayButtonClicked() => _profileController.ShowProfile(_employee);
    
        private void Employee_OnFavoriteChanged(bool favorite) => employeeCardView.Favorite = favorite;
    }
}