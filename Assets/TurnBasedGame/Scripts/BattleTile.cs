using System;
using TurnBasedGame.Scripts.GameInput;
using TurnBasedGame.Scripts.Interfaces;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class BattleTile : MonoBehaviour, IBattleTile
    {
        [field: SerializeField] public Unit Unit { get; set; }

        [field: SerializeField] public bool IsEmpty { get; set; } = true;

        [field: SerializeField] public MeshRenderer MeshRenderer { get; set; }


        private SelectionController _selectionController;


        private void OnEnable()
        {
            _selectionController = FindObjectOfType<SelectionController>();
            MeshRenderer = GetComponent<MeshRenderer>();
        }
        
        private void OnMouseEnter()
        {
            _selectionController.EnterTile(this);
        }

        private void OnMouseExit()
        {
            _selectionController.ExitTile();
        }
    }
}