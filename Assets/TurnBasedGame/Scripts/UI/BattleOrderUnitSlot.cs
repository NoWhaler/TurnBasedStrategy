using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI
{
    public class BattleOrderUnitSlot : MonoBehaviour
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        [field: SerializeField] public Image UnitPortrait { get; set; }
        
        [field: SerializeField] public Image UnitFrame { get; set; }
        
        [field: SerializeField] public TMP_Text UnitCountText { get; set; }


        private void OnEnable()
        {
            UnitFrame = GetComponent<Image>();
            UnitCountText = GetComponentInChildren<TMP_Text>();
        }

        public void UpdateCounter(int value)
        {
            UnitCountText.text = value.ToString();
        }
    }
}