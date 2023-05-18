using System;
using System.Collections.Generic;
using System.Linq;
using TurnBasedGame.Scripts.Enum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnBasedGame.Scripts.UI
{
    public class SlotsController : MonoBehaviour
    {
        [field: SerializeField] private List<UnitSlot> UnitSlots { get; set; } = new ();

        [SerializeField] private PlayerArmy _playerArmy;
        
        private void Awake()
        {
            UnitSlots = GetComponentsInChildren<UnitSlot>().ToList();
            _playerArmy = FindObjectOfType<PlayerArmy>();
        }
        
        private void OnEnable()
        {
            _playerArmy.OnArmySet += SetUpSlots;
        }

        private void SetUpSlots()
        {
            foreach (var slot in UnitSlots)
            {
                foreach (var armyUnit in _playerArmy.Army)
                {
                    if (UnitSlots.IndexOf(slot) == _playerArmy.Army.IndexOf(armyUnit))
                    {
                        slot.UISlotUnit = armyUnit;
                        slot.UnitPortrait.sprite = armyUnit.UnitPortrait;
                        slot.UpdateCounterValue();
                    }
                }
            }
        }
    }
}