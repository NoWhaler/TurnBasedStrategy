using TurnBasedGame.Scripts.UI.UIStateMachine.States;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI.UIStateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [field: SerializeField] public Canvas PreCombatCanvas { get; set; }
        [field: SerializeField] public Canvas CombatCanvas { get; set; }
        
        [field: SerializeField] public Canvas ShopCanvas { get; set; }
        
        public UIController UIController { get; set; }
        public BaseState CurrentState { get; set; }
        private UIStateFactory StateFactory { get; set; }

        private void Awake()
        {
            UIController = GetComponent<UIController>();
            StateFactory = new UIStateFactory(this);
            CurrentState = StateFactory.ShopState();
            CurrentState.EnterState();
        }

        private void Update()
        {
            CurrentState.UpdateStates();
        }
    }
}