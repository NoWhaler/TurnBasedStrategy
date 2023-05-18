using System;
using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class CurrencyView : MonoBehaviour
    {
        private TMP_Text _coinsText;
        [field: SerializeField] private int CurrentValue { get; set; }


        private void Start()
        {
            _coinsText = GetComponentInChildren<TMP_Text>();
            _coinsText.text = CurrentValue.ToString();
        }

        public void UpdateCurrentCoinsValue(int value)
        {
            CurrentValue += value;
            _coinsText.text = CurrentValue.ToString();
        }
    }
}