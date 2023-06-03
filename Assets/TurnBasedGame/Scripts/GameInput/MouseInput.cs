using System;
using UnityEngine;

namespace TurnBasedGame.Scripts.GameInput
{
    public class MouseInput : MonoBehaviour
    {
        public event Action OnLeftMouseDownToSelect;

        public event Action OnRightMouseClickToDeselect;

        private void Update()
        {
            SelectPosition();
            DeselectGameObject();
        }

        private void SelectPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseDownToSelect?.Invoke();
                
            }
        }

        private void DeselectGameObject()
        {
            if (Input.GetMouseButtonDown(1))
            {
                OnRightMouseClickToDeselect?.Invoke();
            }
        }
    }
}