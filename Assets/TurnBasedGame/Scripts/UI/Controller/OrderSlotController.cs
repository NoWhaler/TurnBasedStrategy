using System.Collections.Generic;
using System.Linq;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.Controller
{
    public class OrderSlotController : MonoBehaviour
    {
        [SerializeField] private List<BattleOrderUnitSlot> _battleOrderUnitSlots = new ();

        [SerializeField] private List<Unit> _allUnits = new List<Unit>();

        [SerializeField] private Image _currentUnitPortrait;

        private PlayerArmy _playerArmy;
        private EnemyArmy _enemyArmy;
        private UIController _uiController;
        private TurnManager _turnManager;
        private PathfinderManager _pathfinderManager;
        
        [SerializeField] private List<Unit> _sortedUnits;

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
            _turnManager.OnMakeAttack += DoAttack;
        }

        private void OnDisable()
        {
            foreach (var unit in _allUnits)
            {
                unit.OnDeath -= UnitDeath;
            }
            
            _uiController.OnBattleStartToSetOrderSlots -= StartCombat;
            _uiController.OnDefendClick -= OrderSlots;
            _uiController.OnWaitClick -= WaitOrderSlots;
            _turnManager.OnMakeMove -= MakeMove;
            _turnManager.OnMakeAttack -= DoAttack;
        }
        
        /// <summary>
        /// Обробляє смерть одиниці і виконує відповідні дії,
        /// такі як видалення зі списків армій та перевірка на виграш або програш
        /// </summary>
        private void UnitDeath(Unit unit)
        {
            if (_allUnits.Contains(unit) && _sortedUnits.Contains(unit))
            {
                _allUnits.Remove(unit);
                _sortedUnits.Remove(unit);
                
                ShuffleOrder();
            }

            if (_enemyArmy.Army.Contains(unit))
            {
                _enemyArmy.Army.Remove(unit);

                if (_enemyArmy.Army.Count == 0)
                {
                    _uiController.IsWinStateActive = true;
                }
            }

            if (_playerArmy.Army.Contains(unit))
            {
                _playerArmy.Army.Remove(unit);
                
                if (_playerArmy.Army.Count == 0)
                {
                    _uiController.IsLoseStateActive = true;
                }
            }
        }

        /// <summary>
        /// Викликається при здійсненні руху бойової одиниці.
        /// Переставляє поточну одиницю на кінець списку та оновлює інтерфейс
        /// </summary>
        private void MakeMove()
        {
            var currentUnit = _sortedUnits[0];
    
            _sortedUnits.RemoveAt(0);
            _sortedUnits.Add(currentUnit);
            
            SwapCurrentUnit();

            _pathfinderManager.ClearTiles();
            _turnManager.ClosestTiles.Clear();
            _turnManager.AttackTiles.Clear();
            
            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            
            ShuffleOrder();
        }

        
        /// <summary>
        /// Викликається при здійсненні атаки бойовою одиницею.
        /// Переставляє поточну одиницю на кінець списку та оновлює інтерфейс.
        /// </summary>
        private void DoAttack()
        {
            var currentUnit = _sortedUnits[0];
    
            _sortedUnits.RemoveAt(0);
            _sortedUnits.Add(currentUnit);
            
            SwapCurrentUnit();

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
            
            SwapCurrentUnit();
            
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
            
            SwapCurrentUnit();

            _pathfinderManager.ClearTiles();
            _turnManager.ClosestTiles.Clear();
            _turnManager.AttackTiles.Clear();

            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            
            ShuffleOrder();
        }
        
        /// <summary>
        /// Замінює поточну бойову одиницю на першу в списку і оновлює інтерфейс
        /// </summary>
        private void SwapCurrentUnit()
        {
            _turnManager.CurrentUnitTurn = _sortedUnits[0];
            _currentUnitPortrait.sprite = _turnManager.CurrentUnitTurn.UnitPortrait;
            
            if (!_turnManager.CurrentUnitTurn.HasCounterAttack)
            {
                _turnManager.CurrentUnitTurn.HasCounterAttack = true;
            }
        }

        private void StartCombat()
        {
            _allUnits = new List<Unit>(_playerArmy.Army);
            _allUnits.AddRange(_enemyArmy.Army);
            _sortedUnits = _allUnits
                .Where(unit => unit.gameObject.activeInHierarchy)
                .OrderBy(unit => -unit.Initiative)
                .ToList();
            
            foreach (var unit in _allUnits)
            {
                unit.OnDeath += UnitDeath;
            }

            foreach (var unit in _allUnits)
            {
                var unitCollider = unit.GetComponent<BoxCollider>();
                if (unitCollider != null)
                {
                    unitCollider.enabled = false;
                }
            }
            
            _turnManager.CurrentUnitTurn = _sortedUnits[0];
            _currentUnitPortrait.sprite = _turnManager.CurrentUnitTurn.UnitPortrait;
            _pathfinderManager.GetTilesInRange(_turnManager.CurrentUnitTurn);
            ShuffleOrder();
            
        }

        
        /// <summary>
        /// Перемішує порядок бойових одиниць у списку та оновлює інтерфейс слотів
        /// </summary>
        private void ShuffleOrder()
        {
            for (int i = 0; i < _battleOrderUnitSlots.Count; i++)
            {
                _battleOrderUnitSlots[i].UISlotUnit = _sortedUnits[i % _sortedUnits.Count];
                _battleOrderUnitSlots[i].UnitPortrait.sprite = _battleOrderUnitSlots[i].UISlotUnit.UnitPortrait;
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