using System.Collections;
using TMPro;
using TurnBasedGame.Scripts.UI.StatsDescription;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.ArmySlots
{
    public abstract class ArmySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        
        [field: SerializeField] public Image UnitPortrait { get; set; }

        [field: SerializeField] private bool _isEmpty = true;

        [field: SerializeField] public int CurrentUnitCount { get; set; } = 0;

        [field: SerializeField] private TMP_Text UnitCountText { get; set; }

        [field: SerializeField] private Stats _unitStats;
        
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
            _unitStats = FindObjectOfType<Stats>();
            UnitCountText = GetComponentInChildren<TMP_Text>();
            UnitCountText.text = CurrentUnitCount.ToString();

            IsEmpty = UISlotUnit == null;
        }

        public void ResetUnitsCount()
        {
            CurrentUnitCount = 0;
            UnitCountText.text = CurrentUnitCount.ToString();
        }

        public void UpdateUnitsCount(int value)
        {
            CurrentUnitCount += value;
            UnitCountText.text = CurrentUnitCount.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (UISlotUnit != null)
            {
                UISlotUnit.CurrentHealthPoints = UISlotUnit.MaxHealthPoints;
                _unitStats.GetComponent<Image>().enabled = true;
                _unitStats.ShowStats(UISlotUnit);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _unitStats.GetComponent<Image>().enabled = false;
            _unitStats.ClearStats();
        }
    }
}