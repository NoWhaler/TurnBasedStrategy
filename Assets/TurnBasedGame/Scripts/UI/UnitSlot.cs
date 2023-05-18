using System;
using TMPro;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI
{
    public class UnitSlot : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        
        [field: SerializeField] public Image UnitPortrait { get; set; }

        [field: SerializeField] public TMP_Text UnitCountText { get; set; }
        
        private SelectionManager _selectionManager;

        private void OnEnable()
        {
            UnitCountText = GetComponentInChildren<TMP_Text>();
            
            _selectionManager = FindObjectOfType<SelectionManager>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (UISlotUnit != null)
                {
                    UISlotUnit.IsSelected = true;
                    _selectionManager.Unit = UISlotUnit;
                    Debug.Log("Select unit on panel");
                }
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                UISlotUnit.IsSelected = false;
                
                Debug.Log("Deselect unit on panel");
            }
        }

        public void UpdateCounterValue()
        {
            UnitCountText.text = UISlotUnit.UnitNumber.ToString();
        }
    }
}