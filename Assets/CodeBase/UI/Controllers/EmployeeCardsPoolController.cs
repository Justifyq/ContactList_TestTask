using Factories.Views;
using Model;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Controllers
{
    public class EmployeeCardsPoolController : MonoBehaviour
    {
        private const int PoolElementMaxLenght = 30;
        private const float CreatePosition = .05f;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform container;

        private EmployeePool _cards;
        private EmployeeCardFactory _cardFactory;
        private int _currIndex;

        private void Awake() => scrollRect.onValueChanged.AddListener(ScrollRect_OnValueChanged);

        private void OnDestroy() => scrollRect.onValueChanged.RemoveListener(ScrollRect_OnValueChanged);

        public void Construct(Employee[] employees, EmployeeCardFactory cardFactory)
        {
            _cards = new EmployeePool(employees, PoolElementMaxLenght);
            _cardFactory = cardFactory;
            CreateCards();
        }

        private void ScrollRect_OnValueChanged(Vector2 value)
        {
            if (scrollRect.verticalNormalizedPosition < CreatePosition) 
                CreateCards();
        }

        private void CreateCards()
        {
            if (_currIndex < _cards.Pool.Length) 
                _cardFactory.CreateCards(container, _cards.Pool[_currIndex++]);
        }
    }
}