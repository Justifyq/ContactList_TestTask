using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    [Serializable]
    public class EmployeeCardView : IDisposable
    {
        public event Action<bool> FavoriteChanged;
        public event Action DisplayButtonClicked;  
    
        public bool Favorite
        {
            get => favoriteToggle.isOn;
            set
            {
                if (favoriteToggle.isOn == value)
                    return;
            
                favoriteToggle.isOn = value;
            }
        }

        [SerializeField] private Toggle favoriteToggle;
        [SerializeField] private Button displayProfileButton;
        [SerializeField] private Text fullNameText;
        [SerializeField] private Text emailAndIpText;
        [SerializeField] private Image avatar;

        public void Construct(Employee employee)
        {
            favoriteToggle.onValueChanged.AddListener(Toggle_OnValueChanged);
            displayProfileButton.onClick.AddListener(Button_OnClick);
        
            fullNameText.text = employee.FullName;
            emailAndIpText.text = $"{employee.Email} | {employee.IpAddress}";
            Favorite = employee.Favorite;
            avatar.sprite = employee.Avatar;
        }
    
        public void Dispose()
        {
            displayProfileButton.onClick.RemoveListener(Button_OnClick);
            favoriteToggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);
        }

        private void Button_OnClick() => DisplayButtonClicked?.Invoke();
    
        private void Toggle_OnValueChanged(bool value) => FavoriteChanged?.Invoke(value);
    }
}