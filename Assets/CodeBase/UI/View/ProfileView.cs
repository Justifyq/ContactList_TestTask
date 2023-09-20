using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    [Serializable]
    public class ProfileView : IDisposable
    {
        public event Action CloseButtonClicked;

        [SerializeField] private GameObject panel;
        [SerializeField] private Button button;
        [SerializeField] private Image avatar;
        [SerializeField] private Toggle favorite;
        [SerializeField] private Text gender;
        [SerializeField] private Text fullName;
        [SerializeField] private Text contactIp;
        [SerializeField] private Text contactMail;

        public void Show(Employee employee)
        {
            avatar.sprite = employee.Avatar;
            favorite.isOn = employee.Favorite;
        
            gender.text = employee.Gender;
            contactMail.text = employee.Email;
            fullName.text = employee.FullName;
            contactIp.text = employee.IpAddress;
        
            panel.SetActive(true);
        }

        public void Close() => panel.SetActive(false);

        public void Construct() => button.onClick.AddListener(Button_OnClick);

        public void Dispose() => button.onClick.RemoveListener(Button_OnClick);

        private void Button_OnClick() => CloseButtonClicked?.Invoke();
    }
}