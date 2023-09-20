using UnityEngine;
using UnityEngine.UI;

namespace UI.Controllers
{
    [RequireComponent(typeof(Toggle))]
    public class TogglePanel : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        
        private Toggle _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
            Toggle_OnValueChanged(_toggle.isOn);
        }

        private void OnDestroy() => _toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);

        private void Toggle_OnValueChanged(bool isOn) => panel.SetActive(isOn);
    }
}