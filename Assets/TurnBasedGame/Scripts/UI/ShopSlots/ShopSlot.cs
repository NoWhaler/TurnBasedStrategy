using System;
using TMPro;
using TurnBasedGame.Scripts.UI.ArmySlots;
using TurnBasedGame.Scripts.UI.Controller;
using TurnBasedGame.Scripts.UI.StatsDescription;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.ShopSlots
{
    public class ShopSlot : MonoBehaviour, IPointerClickHandler
    {
        [field: SerializeField] public Unit UISlotUnit { get; set; }
        [field: SerializeField] public Image UnitPortrait { get; set; }
        
        [field: SerializeField] public TMP_Text UnitCostText { get; set; }

        [field: SerializeField] private bool _isEmpty = true;
        
        [field: SerializeField] public int CostValue { get; set; }

        private PlayerSlots _armySlotsController;

        private UIController _uiController;

        private StatsPanel _statsPanel;

        private CurrencyView _currencyView;

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
            _uiController = FindObjectOfType<UIController>();
            _statsPanel = FindObjectOfType<StatsPanel>();
            _currencyView = FindObjectOfType<CurrencyView>();
            _armySlotsController = FindObjectOfType<PlayerSlots>();
            UnitCostText = GetComponentInChildren<TMP_Text>();
            
            UnitCostText.text = CostValue.ToString();
            IsEmpty = false;

            _uiController.OnBuyUnit += BuyUnit;
        }

        private void OnDisable()
        {
            _uiController.OnBuyUnit -= BuyUnit;
        }

        private void BuyUnit(ShopSlot currentSlot)
        {
            if (currentSlot == this)
            {
                foreach (var slot in _armySlotsController. allArmySlots)
                {
                    if (_currencyView.CurrentValue >= CostValue)
                    {
                        if (slot.IsEmpty)
                        {
                            slot.UISlotUnit = _statsPanel.CurrentUnit;
                            slot.IsEmpty = false;
                            slot.UnitPortrait.sprite = UnitPortrait.sprite;
                            slot.UISlotUnit.UnitNumber = _statsPanel.CurrentUnitsToBuy;
                            slot.UpdateUnitsCount(_statsPanel.CurrentUnitsToBuy);
                            _currencyView.UpdateCurrentCoinsValue(CostValue * _statsPanel.CurrentUnitsToBuy);
                            
                            ResetScrollBar();
                            return;
                        }

                        if (!slot.IsEmpty && slot.UISlotUnit == _statsPanel.CurrentUnit)
                        {
                            slot.UISlotUnit.UnitNumber += _statsPanel.CurrentUnitsToBuy;
                            slot.UpdateUnitsCount(_statsPanel.CurrentUnitsToBuy);
                            _currencyView.UpdateCurrentCoinsValue(CostValue * _statsPanel.CurrentUnitsToBuy);
                            
                            ResetScrollBar();
                            return;
                        }
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _statsPanel.OpenPanel(UISlotUnit);
                _uiController.CurrentShopSlot = this;

               ResetScrollBar();
            }
        }


        private void ResetScrollBar()
        {
            _statsPanel.Scrollbar.numberOfSteps = Mathf.FloorToInt((float)_currencyView.CurrentValue / CostValue);
            _statsPanel.MaxUnitsToBuy = _statsPanel.Scrollbar.numberOfSteps;
            _statsPanel.MinUnits.text = "0";
            _statsPanel.MaxUnits.text = _statsPanel.Scrollbar.numberOfSteps.ToString();
        }
    }
}