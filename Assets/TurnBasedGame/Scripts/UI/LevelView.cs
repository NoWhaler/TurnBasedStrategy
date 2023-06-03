using TMPro;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class LevelView : MonoBehaviour
    {
        private TMP_Text _levelText;

        private void OnEnable()
        {
            _levelText = GetComponentInChildren<TMP_Text>();
        }

        public void UpdateValue(int value)
        {
            _levelText.text = $"Рівень { LevelManager.Instance.CurrentLevel.ToString()}";
        }
    }
}