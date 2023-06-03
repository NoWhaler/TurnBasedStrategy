using System;
using System.Collections;
using System.Collections.Generic;
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

        private DirectionManager _directionManager;

        public event Action OnMakeMove;

        public event Action OnMakeAttack;

        [field: SerializeField] public bool IsMoving { get; set; }
        
        private void OnEnable()
        {
            _selectionController = FindObjectOfType<SelectionController>();
            _directionManager = FindObjectOfType<DirectionManager>();

            _selectionController.OnSelectedTileToMove += MoveUnitToTile;
        }

        private void DealDamage(Unit defender)
        {
            CurrentUnitTurn.DealDamage(CurrentUnitTurn, defender);
            _directionManager.IsAttacking = false;
            
        }

        public void MoveUnitToTile(List<BattleTile> shortestPath, Unit defender)
        {
            StartCoroutine(MoveUnitAlongPathCo(shortestPath, () =>
            {
                DealDamage(defender);
            }));
            
        }


        public void RangeAttack(Unit defender)
        {
            if (_directionManager.IsAttacking)
            {
                CurrentUnitTurn.DealDamage(CurrentUnitTurn, defender);
                OnMakeAttack?.Invoke();
            }
        }
        

        private IEnumerator MoveUnitAlongPathCo(List<BattleTile> shortestPath, Action onAttack)
        {
            if (shortestPath.Count != 0)
            {
                IsMoving = true;
                foreach (var tile in shortestPath)
                {
                    yield return StartCoroutine(MoveUnitCo(tile));
                }
                shortestPath[^1].IsEmpty = false;
                
                IsMoving = false;
                
                _selectionController.ShortestPath.Clear();

                if (_directionManager.IsAttacking)
                {
                    onAttack?.Invoke();
                    OnMakeAttack?.Invoke();
                }

                else
                {
                    OnMakeMove?.Invoke();
                }
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