using Model;
using UI.View;
using UnityEngine;

namespace UI.Controllers
{
    public class ProfileController : MonoBehaviour
    {
        [SerializeField] private ProfileView profileView;
    
        private void Awake()
        {
            profileView.CloseButtonClicked += ProfileView_OnCloseButtonClicked;
            profileView.Construct();
        }

        private void OnDestroy()
        {
            profileView.CloseButtonClicked -= ProfileView_OnCloseButtonClicked;
            profileView.Dispose();
        }

        public void ShowProfile(Employee employee) => profileView.Show(employee);
    
        private void ProfileView_OnCloseButtonClicked() => profileView.Close();
    }
}