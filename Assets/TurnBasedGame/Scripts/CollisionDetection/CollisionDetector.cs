using System.Collections.Generic;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Managers;
using TurnBasedGame.Scripts.UI.StatsDescription;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.CollisionDetection
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private AttackDirection _attackDirection;

        private DirectionManager _directionManager;

        private TurnManager _turnManager;

        private PathfinderManager _pathfinderManager;

        private List<BattleTile> _shortestPath = new List<BattleTile>();

        private BattleTile _targetTile;

        private Stats _unitStats;

        private void OnEnable()
        {
            _directionManager = FindObjectOfType<DirectionManager>();
            _turnManager = FindObjectOfType<TurnManager>();
            _pathfinderManager = FindObjectOfType<PathfinderManager>();
            _unitStats = FindObjectOfType<Stats>();
        }

        private void OnMouseEnter()
        {
            _directionManager.SetDirection(_attackDirection);
            var unit = GetComponentInParent<Unit>();
            
            if (unit != null)
            {
                _targetTile = _pathfinderManager.GetClosestTileInDirection(unit.BattleTile.transform.position, _attackDirection);
            }
            
            if (_targetTile != null)
            {
                _shortestPath = _pathfinderManager.CalculateShortestPath(_targetTile);
                
                // foreach (var tile in _shortestPath)
                // {
                //     tile.MeshRenderer.material.color = Color.grey;
                // }
            }
            
            
        }

        private void OnMouseDown()
        {
            var selectedUnit = GetComponentInParent<Unit>();
            if (!_turnManager.IsMoving && selectedUnit != null)
            {
                if (_turnManager.CurrentUnitTurn.UnitType == UnitType.MeleeUnit)
                {
                    _directionManager.IsAttacking = true;
                
                    _turnManager.MoveUnitToTile(_shortestPath, selectedUnit);
                }

                else if (_turnManager.CurrentUnitTurn.UnitType == UnitType.RangeUnit
                         && selectedUnit.UnitFractionType != _turnManager.CurrentUnitTurn.UnitFractionType) 
                {
                    _directionManager.IsAttacking = true;
                    _turnManager.RangeAttack(selectedUnit);
                }
            }
        }
        

        private void OnMouseExit()
        {
            _directionManager.UnsetDirection();
        }
    }
}