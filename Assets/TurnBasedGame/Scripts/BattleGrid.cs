using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class BattleGrid : MonoBehaviour
    {
        [field: SerializeField] public int Rows { get; set; }
        [field: SerializeField] public int Columns { get; set; }

        [SerializeField] private GameObject _tilePrefab;

        public List<BattleTile> PlayerPlacementTiles { get; set; } = new List<BattleTile>();

        public List<BattleTile> EnemyPlacementTiles { get; set; } = new List<BattleTile>();

        private void Start()
        {
            PlayerPlacementTiles = GetComponentsInChildren<BattleTile>().ToList();
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            var startPos = transform.position;
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    var spawnPos = new Vector3(startPos.x + col, startPos.y, startPos.z + row);
                    var tile = Instantiate(_tilePrefab, spawnPos, Quaternion.identity);
                    tile.transform.parent = transform;

                    if (col < 2)
                    {
                        PlayerPlacementTiles.Add(tile.GetComponent<BattleTile>());
                    }

                    if (col >= Columns - 2)
                    {
                        EnemyPlacementTiles.Add(tile.GetComponent<BattleTile>());
                    }
                }
            }
        }
    }
}
