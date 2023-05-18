using System;
using System.Collections.Generic;
using System.Linq;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class OrderSlotController : MonoBehaviour
    {
        [SerializeField]
        private List<BattleOrderUnitSlot> _battleOrderUnitSlots = new ();

        private List<Unit> _allUnits = new List<Unit>();

        private PlayerArmy _playerArmy;
        private EnemyArmy _enemyArmy;
        private UIController _uiController;
        private TurnManager _turnManager;
        private PathfinderManager _pathfinderManager;
        private List<Unit> _sortedUnits;

        private void Start()
        {
            _playerArmy = FindObjectOfType<PlayerArmy>();
            _enemyArmy = FindObjectOfType<EnemyArmy>();
            
        }
        
        
        private void OnEnable()
        {
            _uiController = FindObjectOfType<UIController>();
            _turnManager = FindObjectOfType<TurnManager>();
            _pathfinderManager = FindObjectOfType<PathfinderManager>();
            
            _battleOrderUnitSlots = GetComponentsInChildren<BattleOrderUnitSlot>().ToList();

            _uiController.OnBattleStartToSetOrderSlots += StartCombat;
            _uiController.OnDefendClick += OrderSlots;
            _uiController.OnWaitClick += WaitOrderSlots;
            _turnManager.OnMakeMove += MakeMove;
        }

        private void OnDisable()
        {
            _uiController.OnBattleStartToSetOrderSlots -= StartCombat;
            _uiController.OnDefendClick -= OrderSlots;
            _uiController.OnWaitClick -= WaitOrderSlots;
            _turnManager.OnMakeMove -= MakeMove;
        }

        private void MakeMove()
        {
            var currentUnit = _sortedUnits[0];
    
            _sortedUnits.RemoveAt(0);
            _sortedUnits.Add(currentUnit);
            
            _turnManager.CurrentUnitTurn = _sortedUnits[0];

            _pathfinderManager.ClearTiles();
            _turnManager.ClosestTiles.Clear();
            _turnManager.AttackTiles.Clear();
            
            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            
            ShuffleOrder();
        }

        private void OrderSlots()
        {
            var currentUnit = _sortedUnits[0];
    
            _sortedUnits.RemoveAt(0);
            _sortedUnits.Add(currentUnit);
            
            _turnManager.CurrentUnitTurn = _sortedUnits[0];
            
            _pathfinderManager.ClearTiles();
            _turnManager.ClosestTiles.Clear();
            _turnManager.AttackTiles.Clear();

            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            
            ShuffleOrder();
        }

        private void WaitOrderSlots()
        {
            var currentUnit = _sortedUnits[0];
    
            _sortedUnits.RemoveAt(0);
            _sortedUnits.Add(currentUnit);
            
            _turnManager.CurrentUnitTurn = _sortedUnits[0];

            _pathfinderManager.ClearTiles();
            _turnManager.ClosestTiles.Clear();
            _turnManager.AttackTiles.Clear();

            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            
            ShuffleOrder();
        }

        private void StartCombat()
        {
            _allUnits = _playerArmy.Army;
            _allUnits.AddRange(_enemyArmy.Army);
            _sortedUnits = _allUnits
                .Where(unit => unit.gameObject.activeInHierarchy)
                .OrderBy(unit => -unit.Initiative)
                .ToList();

            foreach (var unit in _allUnits)
            {
                var unitCollider = unit.GetComponent<BoxCollider>();
                if (unitCollider != null)
                {
                    unitCollider.enabled = false;
                }
            }
            
            _turnManager.CurrentUnitTurn = _sortedUnits[0];
            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            ShuffleOrder();
            
        }

        private void ShuffleOrder()
        {
            for (int i = 0; i < _battleOrderUnitSlots.Count; i++)
            {
                _battleOrderUnitSlots[i].UISlotUnit = _sortedUnits[i % _sortedUnits.Count];
                _battleOrderUnitSlots[i].UnitPortrait.color = _battleOrderUnitSlots[i].UISlotUnit.UnitMaterial.sharedMaterial.color;
                _battleOrderUnitSlots[i].UpdateCounter(_battleOrderUnitSlots[i].UISlotUnit.UnitNumber);

                if (_battleOrderUnitSlots[i].UISlotUnit.UnitFractionType == UnitFractionType.Player)
                {
                    _battleOrderUnitSlots[i].UnitFrame.color = new Color32(25, 36, 231, 200);
                }
                else
                { 
                    _battleOrderUnitSlots[i].UnitFrame.color = new Color32(255, 0, 12, 165);
                }

                
            }
        }
    }
}