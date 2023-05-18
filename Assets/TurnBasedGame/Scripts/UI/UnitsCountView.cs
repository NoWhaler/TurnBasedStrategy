using System;
using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class UnitsCountView : MonoBehaviour
    {
        private TMP_Text _countText;
        private int Value { get; set; }

        private void OnEnable()
        {
            _countText = GetComponentInChildren<TMP_Text>();
        }

        public void UpdateCountValue(int value)
        {
            Value += value;
            if (Value <= 0)
            {
                gameObject.SetActive(false);
            }
            _countText.text = Value.ToString();
        }
    }
}