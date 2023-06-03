using System.Collections;
using System.Collections.Generic;
using TurnBasedGame.Scripts.Enum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnBasedGame.Scripts.Managers
{
    public class PathfinderManager : MonoBehaviour
    {
        private BattleGrid _battleGrid;

        private TurnManager _turnManager;
        
        private void Start()
        {
            _battleGrid = FindObjectOfType<BattleGrid>();
            _turnManager = FindObjectOfType<TurnManager>();
        }
        
        public void GetTilesInRange(Unit unit)
        {
            var startTile = unit.BattleTile;
            var openSet = new List<BattleTile> { startTile };
            var closedSet = new HashSet<BattleTile>();

            var tileCosts = new Dictionary<BattleTile, float>();
            tileCosts[startTile] = 0f;

            while (openSet.Count > 0)
            {
                var currentTile = GetLowestCostTile(openSet, tileCosts);

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                foreach (var neighbor in GetNeighbors(currentTile))
                {
                    if (!closedSet.Contains(neighbor))
                    {
                        var newCost = tileCosts[currentTile] + GetMoveCost(currentTile, neighbor);
                        if (!openSet.Contains(neighbor) || newCost < tileCosts[neighbor])
                        {
                            tileCosts[neighbor] = newCost;
                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }

            
            foreach (var tile in tileCosts.Keys)
            {
                if (tileCosts[tile] <= unit.MoveRange)
                {
                    _turnManager.ClosestTiles.Add(tile);
                    foreach (var neighbor in GetNeighborsForAttack(tile))
                    {
                        if (!_turnManager.AttackTiles.Contains(neighbor))
                        {
                            _turnManager.AttackTiles.Add(neighbor);
                        }
                        
                    }
                }
            }

            foreach (var tile in _turnManager.ClosestTiles)
            {
                tile.MeshRenderer.material.color = Color.green;
            }
        }
        
        private List<BattleTile> GetNeighbors(BattleTile tile)
        {
            var neighbors = new List<BattleTile>();

            int x = (int)tile.transform.position.x;
            int z = (int)tile.transform.position.z;

            if (z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x, 0, z + 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x, 0, z - 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1 && z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z + 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x > 0 && z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z + 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1 && z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z - 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            if (x > 0 && z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z - 1));
                if (neighbor != null && !neighbor.Unit)
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }
        
        
        private List<BattleTile> GetNeighborsForAttack(BattleTile tile)
        {
            var neighbors = new List<BattleTile>();

            int x = (int)tile.transform.position.x;
            int z = (int)tile.transform.position.z;

            if (z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x, 0, z + 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x, 0, z - 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1 && z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z + 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x > 0 && z < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z + 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x < _battleGrid.Columns - 1 && z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x + 1, 0, z - 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            if (x > 0 && z > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(x - 1, 0, z - 1));
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }

            return neighbors;
        }

        private BattleTile GetTileAtPosition(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, 0.1f, LayerMask.GetMask("BattleTile"));
            foreach (Collider collider in colliders)
            {
                BattleTile tile = collider.GetComponent<BattleTile>();
                if (tile != null)
                {
                    return tile;
                }
            }
            return null;
        }

        private float GetMoveCost(BattleTile currentTile, BattleTile neighborTile)
        {
            var distance = Vector3.Distance(currentTile.transform.position, neighborTile.transform.position);
            if (distance <= 1f)
            {
                return 1f;
            }
            else if (distance <= 2f)
            {
                return 1.4f;
            }
            else
            {
                return 1.4f + Mathf.Floor((distance - 2f) / 2f) * 1.4f;
            }
        }

        private BattleTile GetLowestCostTile(List<BattleTile> tiles, Dictionary<BattleTile, float> tileCosts)
        {
            BattleTile lowestCostTile = null;
            float lowestCost = float.MaxValue;

            foreach (var tile in tiles)
            {
                if (tileCosts.TryGetValue(tile, out float cost) && cost < lowestCost)
                {
                    lowestCost = cost;
                    lowestCostTile = tile;
                }
            }

            return lowestCostTile;
        }

        public void ClearTiles()
        {
            foreach (var tile in _turnManager.ClosestTiles)
            {
                tile.MeshRenderer.material.color = new Color32(207, 98, 85, 255);
            }
        }

        public void HighlightGreen(List<BattleTile> shortestPathList)
        {
            if (shortestPathList != null)
            {
                foreach (var tile in shortestPathList)
                {
                    tile.MeshRenderer.material.color = Color.green;
                }
            }
        }

        public void HighLightShortestPath(List<BattleTile> battleTiles)
        {
            foreach (var tile in battleTiles)
            {
                tile.MeshRenderer.material.color = Color.grey;
            }
        }
        
        private List<BattleTile> GetNeighborsForMoving(BattleTile tile)
        {
            var neighbors = new List<BattleTile>();

            int currentX = (int)tile.transform.position.x;
            int currentZ = (int)tile.transform.position.z;

            if (currentZ < _battleGrid.Rows - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX, 0, currentZ + 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }
            
            if (currentZ > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX, 0, currentZ - 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentX < _battleGrid.Columns - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX + 1, 0, currentZ));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentX > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX - 1, 0, currentZ));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentZ < _battleGrid.Rows - 1 && currentX > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX - 1, 0, currentZ + 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentZ < _battleGrid.Rows - 1 && currentX < _battleGrid.Columns - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX + 1, 0, currentZ + 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentZ > 0 && currentX > 0)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX - 1, 0, currentZ - 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            if (currentZ > 0 && currentX < _battleGrid.Columns - 1)
            {
                var neighbor = GetTileAtPosition(new Vector3(currentX + 1, 0, currentZ - 1));
                if (neighbor != null && !neighbor.Unit) neighbors.Add(neighbor);
            }

            return neighbors;
        }

        public List<BattleTile> CalculateShortestPath(BattleTile targetTile)
        {
            var startTile = _turnManager.CurrentUnitTurn.BattleTile;

            var openSet = new List<BattleTile> { startTile };
            var closedSet = new HashSet<BattleTile>();

            var cameFrom = new Dictionary<BattleTile, BattleTile>();
            var gScore = new Dictionary<BattleTile, int>();

            gScore[startTile] = 0;

            while (openSet.Count > 0)
            {
                var currentTile = GetLowestFScoreTile(openSet, gScore, targetTile);
                if (currentTile == targetTile)
                {
                    return ReconstructPath(cameFrom, targetTile);
                }

                openSet.Remove(currentTile);
                closedSet.Add(currentTile);

                foreach (var neighbor in GetNeighborsForMoving(currentTile))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    var tentativeGScore = gScore[currentTile] + 1;
                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = currentTile;
                        gScore[neighbor] = tentativeGScore;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return new List<BattleTile>();
        }
        
        private BattleTile GetLowestFScoreTile(List<BattleTile> tiles, Dictionary<BattleTile, int> gScore, BattleTile targetTile)
        {
            BattleTile lowestFScoreTile = null;
            int lowestFScore = int.MaxValue;

            foreach (var tile in tiles)
            {
                var fScore = gScore[tile] + GetManhattanDistance(tile, targetTile);
                if (fScore < lowestFScore)
                {
                    lowestFScore = fScore;
                    lowestFScoreTile = tile;
                }
            }

            return lowestFScoreTile;
        }
        
        private int GetManhattanDistance(BattleTile tileA, BattleTile tileB)
        {
            int deltaX = Mathf.Abs((int)tileA.transform.position.x - (int)tileB.transform.position.x);
            int deltaZ = Mathf.Abs((int)tileA.transform.position.z - (int)tileB.transform.position.z);

            return deltaX + deltaZ;
        }
        
        
        private List<BattleTile> ReconstructPath(Dictionary<BattleTile, BattleTile> cameFrom, BattleTile targetTile)
        {
            var path = new List<BattleTile> { targetTile };
            var currentTile = targetTile;

            while (cameFrom.ContainsKey(currentTile))
            {
                currentTile = cameFrom[currentTile];
                path.Insert(0, currentTile);
            }

            return path;
        }
        
        
        public BattleTile GetClosestTileInDirection(Vector3 position, AttackDirection direction)
        {
            BattleTile closestTile = null;
            float closestDistance = float.MaxValue;
            foreach (var tile in _turnManager.ClosestTiles)
            {
                Vector3 directionVector = (tile.transform.position - position).normalized;

                AttackDirection tileDirection = GetAttackDirectionFromVector(directionVector);

                if (tileDirection == direction)
                {
                    float distance = Vector3.Distance(position, tile.transform.position);
                    if (distance <= closestDistance)
                    {
                        closestTile = tile;
                        closestDistance = distance;
                    }
                }
            }

            return closestTile;
        }

        private AttackDirection GetAttackDirectionFromVector(Vector3 direction)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);

            if (angle < 0)
            {
                angle += 360f;
            }

            if (angle < 22.5f || angle >= 337.5f)
            {
                return AttackDirection.Up;
            }
            else if (angle >= 22.5f && angle < 67.5f)
            {
                return AttackDirection.UpRight;
            }
            else if (angle >= 67.5f && angle < 112.5f)
            {
                return AttackDirection.Right;
            }
            else if (angle >= 112.5f && angle < 157.5f)
            {
                return AttackDirection.DownRight;
            }
            else if (angle >= 157.5f && angle < 202.5f)
            {
                return AttackDirection.Down;
            }
            else if (angle >= 202.5f && angle < 247.5f)
            {
                return AttackDirection.DownLeft;
            }
            else if (angle >= 247.5f && angle < 292.5f)
            {
                return AttackDirection.Left;
            }
            else if (angle >= 292.5f && angle < 337.5f)
            {
                return AttackDirection.UpLeft;
            }
    
            return AttackDirection.Default;
        }
    }
}