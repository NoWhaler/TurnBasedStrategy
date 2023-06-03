using TurnBasedGame.Scripts.Enum;
using UnityEngine;

namespace TurnBasedGame.Scripts.Managers
{
    public class AIManager : MonoBehaviour
    {
        private TurnManager _turnManager;

        private PathfinderManager _pathfinderManager;

        private PlayerArmy _playerArmy;

        private void Start()
        {
            _playerArmy = FindObjectOfType<PlayerArmy>();
            _turnManager = FindObjectOfType<TurnManager>();
            _pathfinderManager = FindObjectOfType<PathfinderManager>();
        }

        private void ChooseAction()
        {
            // Assuming there's only one enemy unit
            var enemyUnit = _turnManager.CurrentUnitTurn;

            // Get the player unit closest to the enemy
            var closestPlayerUnit = GetClosestPlayerUnit(enemyUnit);

            // Calculate the shortest path to the player unit
            var shortestPath = _pathfinderManager.CalculateShortestPath(closestPlayerUnit.BattleTile);

            // Move the enemy unit along the shortest path
            _turnManager.MoveUnitToTile(shortestPath, closestPlayerUnit);

            // Check if the player unit is in attack range
            if (IsPlayerUnitInRange(enemyUnit, closestPlayerUnit))
            {
                _turnManager.RangeAttack(closestPlayerUnit);
            }
            else
            {
                // The enemy unit has moved but cannot attack this turn
                // You can perform other actions or end the turn here
            }
        }
        
        
        private Unit GetClosestPlayerUnit(Unit enemyUnit)
        {
            // Implement logic to find the player unit closest to the enemy
            // You can iterate over all player units and calculate distances to find the closest one
            // Return the closest player unit

            return enemyUnit;
        }

        private void MoveEnemyUnit(Unit enemyUnit, BattleTile targetTile)
        {
            _pathfinderManager.HighlightGreen(null); // Clear any highlighted tiles
            _pathfinderManager.HighLightShortestPath(
                _pathfinderManager.CalculateShortestPath(targetTile)); // Highlight the shortest path to the target tile
            _turnManager.MoveUnitToTile(_pathfinderManager.CalculateShortestPath(targetTile),
                null); // Move the enemy unit to the target tile
            // You may need to modify the MoveUnitToTile method in the TurnManager script to handle the enemy's movement properly
        }

        private void AttackEnemyUnit(Unit enemyUnit, BattleTile targetTile)
        {
            // Implement your logic to choose the target unit to attack on the target tile
            // You can iterate through the units on the target tile and choose the first player unit you find
            // For example:
            Unit targetUnit = null;
            if (targetTile.Unit != null && targetTile.Unit.UnitFractionType != UnitFractionType.Enemy)
            {
                targetUnit = targetTile.Unit;
            }

            if (targetUnit != null)
            {
                _pathfinderManager.HighlightGreen(null); // Clear any highlighted tiles
                _turnManager.RangeAttack(targetUnit); // Perform the attack on the target unit.
            }
        }
        
        private bool IsPlayerUnitInRange(Unit enemyUnit, Unit playerUnit)
        {
            // Implement logic to check if the player unit is in attack range of the enemy unit
            // You can calculate the distance between the units and compare it with the attack range of the enemy unit
            // Return true if the player unit is in range, false otherwise

            foreach (var unit in _playerArmy.Army)
            {
                foreach (var tile in _turnManager.AttackTiles)
                {
                    if (tile.Unit == unit)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
    }
}