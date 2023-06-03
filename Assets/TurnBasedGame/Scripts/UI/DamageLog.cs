using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class DamageLog : MonoBehaviour
    {
        public TMP_Text LogText { get; set; }

        private void OnEnable()
        {
            LogText = GetComponent<TMP_Text>();
        }
    }
}