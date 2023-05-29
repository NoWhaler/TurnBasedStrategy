using TurnBasedGame.Scripts.GameInput;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class TargetSelection : MonoBehaviour
    {
        private Unit Unit { get; set; }

        private SelectionController _selectionController;
        private TurnManager _turnManager;


        private void OnEnable()
        {
            _selectionController = FindObjectOfType<SelectionController>();
            Unit = GetComponent<Unit>();
        }


        private void OnMouseEnter()
        {
            if (Unit != null)
            {
                if (SelectionController.IsPutPhaseInputEnded)
                {
                    _selectionController.EnterUnit(Unit);
                }
            }
        }

        private void OnMouseExit()
        {
            _selectionController.ExitUnit();
        }
    }
}