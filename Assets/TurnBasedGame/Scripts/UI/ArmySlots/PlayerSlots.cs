using System;
using TurnBasedGame.Scripts.UI.Controller;

namespace TurnBasedGame.Scripts.UI.ArmySlots
{
    public class PlayerSlots : ArmySlotsController
    {
        private UIController _uiController;

        private CurrencyView _currencyView;


        private void OnEnable()
        {
            _uiController = FindObjectOfType<UIController>();
            _currencyView = FindObjectOfType<CurrencyView>();
            
            _uiController.OnResetUnits += ResetUnits;
        }

        private void OnDisable()
        {
            _uiController.OnResetUnits -= ResetUnits;
        }

        private void ResetUnits()
        {
            foreach (var slot in allArmySlots)
            {
                slot.CurrentUnitCount = 0;
                slot.UISlotUnit = null;
                slot.UnitPortrait.sprite = null;
                slot.IsEmpty = true;
                
                _currencyView.ResetValue();
            }
        }
    }
}