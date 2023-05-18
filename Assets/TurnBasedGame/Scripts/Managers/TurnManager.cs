using System;
using System.Collections;
using System.Collections.Generic;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.GameInput;
using UnityEngine;

namespace TurnBasedGame.Scripts.Managers
{
    public class TurnManager : MonoBehaviour
    {
        [field: SerializeField] public Unit CurrentUnitTurn { get; set; }
        
        [field: SerializeField] public List<BattleTile> ClosestTiles { get; set; }
        
        [field: SerializeField] public List<BattleTile> AttackTiles { get; set; }

        private SelectionController _selectionController;

        public event Action OnMakeMove;

        [field: SerializeField] public bool IsMoving { get; set; }
        private void OnEnable()
        {
            _selectionController = FindObjectOfType<SelectionController>();

            _selectionController.OnSelectedTileToMove += MoveUnitToTile;
        }

        public void MoveUnitToTile(List<BattleTile> shortestPath)
        {
            StartCoroutine(MoveUnitAlongPathCo(shortestPath));
        }


        private IEnumerator MoveUnitAlongPathCo(List<BattleTile> shortestPath)
        {
            if (shortestPath.Count != 0)
            {
                IsMoving = true;
                foreach (var tile in shortestPath)
                {
                    yield return StartCoroutine(MoveUnitCo(tile));
                }
                shortestPath[^1].IsEmpty = false;
                _selectionController.ShortestPath.Clear();
                IsMoving = false;

                OnMakeMove?.Invoke();
            }
        }


        private IEnumerator MoveUnitCo(BattleTile battleTile)
        {
            float elapsedTime = 0;
            var startPosition = CurrentUnitTurn.transform.position;
            while (elapsedTime <= 0.1f)
            {
                CurrentUnitTurn.transform.position = Vector3.Lerp(startPosition,
                    battleTile.transform.position + new Vector3(0f, 0.5f, 0f), elapsedTime / 0.1f);  
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            battleTile.Unit = CurrentUnitTurn;
            CurrentUnitTurn.BattleTile.IsEmpty = true;
            CurrentUnitTurn.BattleTile.Unit = null;
            CurrentUnitTurn.BattleTile = battleTile;
            CurrentUnitTurn.transform.position = battleTile.transform.position + new Vector3(0f, 0.5f, 0f);
        }

        
    }
}