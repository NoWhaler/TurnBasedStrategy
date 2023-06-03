using System;
using System.Collections.Generic;
using System.Linq;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.UI;
using TurnBasedGame.Scripts.UI.ArmySlots;
using TurnBasedGame.Scripts.UI.Controller;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class EnemyArmy : MonoBehaviour
    {
        [field: SerializeField] public List<Unit> Army { get; set; } = new ();

        private BattleGrid _battleGrid;
        private UIController _uiController;
        private EnemySlots _enemySlots;

        private void OnEnable()
        {
            _enemySlots = FindObjectOfType<EnemySlots>();
            _battleGrid = FindObjectOfType<BattleGrid>();
            _uiController = FindObjectOfType<UIController>();
            _uiController.OnBattleStart += SetUnitsOnGrid;
            // SetUnits();
            
        }

        private void OnDisable()
        {
            _uiController.OnBattleStart -= SetUnitsOnGrid;
        }

        private void Start()
        {
            SetUnits();
        }

        private void SetUnits()
        {
            foreach (var armySlot in _enemySlots.allArmySlots)
            {
                if (armySlot.UISlotUnit != null)
                {
                    armySlot.UISlotUnit.UnitNumber = armySlot.CurrentUnitCount;
                    Army.Add(armySlot.UISlotUnit);
                    armySlot.IsEmpty = false;
                }
            }
        }

        private void SpawnUnits()
        {
            for (int i = 0; i < Army.Count; i++)
            {
                var unit = Instantiate(Army[i], Vector3.zero, Quaternion.identity, transform);
                Army[i] = unit;
                unit.gameObject.SetActive(false);
                unit.UnitFractionType = UnitFractionType.Enemy;
                unit.UnitsCountView.UpdateCountValue(unit.UnitNumber);
                unit.UnitsCountView._backGroundImage.color = Color.red;
            }
        }

        private void SetUnitsOnGrid()
        {
            SpawnUnits();
            var shuffledTiles = _battleGrid.EnemyPlacementTiles.OrderBy(t => Guid.NewGuid()).ToList();
           
            for (int i = 0; i < Army.Count; i++)
            {
                foreach (var tile in shuffledTiles)
                {
                    if (tile.IsEmpty)
                    {
                        tile.IsEmpty = false;
                        var unit = Army[i];
                        unit.BattleTile = tile;
                        tile.Unit = unit;
                        unit.gameObject.SetActive(true);
                        unit.transform.position = tile.transform.position + new Vector3(0f, 0.5f, 0f);
                        break;
                    }
                }
            }
        }
    }
}