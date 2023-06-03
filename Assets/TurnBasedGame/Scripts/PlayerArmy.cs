using System;
using System.Collections.Generic;
using TurnBasedGame.Scripts.UI;
using TurnBasedGame.Scripts.UI.ArmySlots;
using TurnBasedGame.Scripts.UI.Controller;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class PlayerArmy : MonoBehaviour
    {
        [field: SerializeField] public List<Unit> Army { get; set; } = new ();

        private UIController _uiController;
        private PlayerSlots _armySlotsController;

        public event Action OnArmySet;

        private void OnEnable()
        {
            _armySlotsController = FindObjectOfType<PlayerSlots>();
            _uiController = FindObjectOfType<UIController>();
            _uiController.OnShopPhaseEnd += SetPlayerArmyUnits;
        }


        private void SetPlayerArmyUnits()
        {
            foreach (var armySlot in _armySlotsController.allArmySlots)
            {
                if (armySlot.UISlotUnit != null)
                {
                    Army.Add(armySlot.UISlotUnit);
                }
            }

            for (int i = 0; i < Army.Count; i++)
            {
                var unit = Instantiate(Army[i], Vector3.zero, Quaternion.identity, transform);
                Army[i] = unit;
                unit.UnitsCountView.UpdateCountValue(unit.UnitNumber);
                unit.UnitsCountView._backGroundImage.color = Color.blue;
                unit.gameObject.SetActive(false);
            }
            
            OnArmySet?.Invoke();
        }
    }
}