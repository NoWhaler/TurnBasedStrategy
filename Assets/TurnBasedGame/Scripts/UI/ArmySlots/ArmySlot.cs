using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.ArmySlots
{
    public abstract class ArmySlot : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        
        [field: SerializeField] public Image UnitPortrait { get; set; }

        [field: SerializeField] private bool _isEmpty = true;

        [field: SerializeField] public int CurrentUnitCount { get; set; } = 0;

        [field: SerializeField] private TMP_Text UnitCountText { get; set; }
        
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
            UnitCountText = GetComponentInChildren<TMP_Text>();
            UnitCountText.text = CurrentUnitCount.ToString();

            IsEmpty = UISlotUnit == null;
            
        }

        public void UpdateUnitsCount(int value)
        {
            CurrentUnitCount += value;
            UnitCountText.text = CurrentUnitCount.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                
            }
        }
    }
}