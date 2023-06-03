using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class CurrencyView : MonoBehaviour
    {
        private TMP_Text _coinsText;
        
        [field: SerializeField] public int CurrentValue { get; set; }
        
        [field: SerializeField] public int MaxValue { get; set; }

        private void Start()
        {
            _coinsText = GetComponentInChildren<TMP_Text>();
            ResetValue();
        }

        public void ResetValue()
        {
            CurrentValue = MaxValue;
            _coinsText.text = CurrentValue.ToString();
        }

        public void UpdateCurrentCoinsValue(int value)
        {
            CurrentValue -= value;
            _coinsText.text = CurrentValue.ToString();
        }
    }
}