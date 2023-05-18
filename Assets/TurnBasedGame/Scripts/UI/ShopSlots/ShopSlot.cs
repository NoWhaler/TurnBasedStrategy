using TMPro;
using TurnBasedGame.Scripts.UI.ArmySlots;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI
{
    public class ShopSlot : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        [field: SerializeField] public Image UnitPortrait { get; set; }
        
        [field: SerializeField] public TMP_Text UnitCostText { get; set; }

        [field: SerializeField] private bool _isEmpty = true;
        
        [field: SerializeField] public int CostValue { get; set; }

        private PlayerSlots _armySlotsController;

        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                if (value)
                {
                    UnitPortrait.sprite = null;
                    UnitPortrait.enabled = false;
                }
                else
                {
                    UnitPortrait.sprite = UISlotUnit.UnitPortrait;
                    UnitPortrait.enabled = true;
                }

            }
        }


        private void OnEnable()
        {
            _armySlotsController = FindObjectOfType<PlayerSlots>();
            UnitCostText = GetComponentInChildren<TMP_Text>();
            UnitCostText.text = CostValue.ToString();
            IsEmpty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                foreach (var slot in _armySlotsController.allArmySlots)
                {
                    if (slot.IsEmpty)
                    {
                        slot.UISlotUnit = UISlotUnit;
                        slot.IsEmpty = false;
                        slot.UnitPortrait = UnitPortrait;
                        slot.UISlotUnit.UnitNumber = 1;
                        slot.UpdateUnitsCount(1);
                        return;
                    }
                    if (!slot.IsEmpty && slot.UISlotUnit == UISlotUnit)
                    {
                        slot.UISlotUnit.UnitNumber += 1;
                        slot.UpdateUnitsCount(1);
                        return;
                    }
                }
            }
        }
    }
}