using System;
using System.Collections.Generic;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

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

        private void OnEnable()
        {
            _directionManager = FindObjectOfType<DirectionManager>();
            _turnManager = FindObjectOfType<TurnManager>();
            _pathfinderManager = FindObjectOfType<PathfinderManager>();
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
                
                foreach (var tile in _shortestPath)
                {
                    tile.MeshRenderer.material.color = Color.grey;
                }
            }
        }

        private void OnMouseDown()
        {
            
            _turnManager.MoveUnitToTile(_shortestPath);
        }

        private void OnMouseExit()
        {
            _directionManager.UnsetDirection();
            foreach (var tile in _shortestPath)
            {
                tile.MeshRenderer.material.color = Color.green;
            }
            _shortestPath.Clear();
        }
    }
}