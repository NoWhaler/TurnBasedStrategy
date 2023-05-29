using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI.StatsDescription
{
    public class Description : MonoBehaviour
    {
        public TMP_Text DescriptionText { get; private set; }

        private void OnEnable()
        {
            DescriptionText = GetComponent<TMP_Text>();
        }
    }
}