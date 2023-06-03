using UnityEngine;

namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class CombatState : BaseState
    {
        public CombatState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory) 
            : base(currentStateContext, stateFactory) { }

        public override void EnterState()
        {
            StateContext.CombatCanvas.enabled = true;
        }

        protected override void UpdateState()
        {
            CheckSwitchStates();   
        }

        protected override void ExitState()
        {
            StateContext.CombatCanvas.enabled = false;
        }
        
        protected override void CheckSwitchStates()
        {
            if (StateContext.UIController.IsWinStateActive)
            {
                SwitchState(StateFactory.WinState());
            }

            if (StateContext.UIController.IsLoseStateActive)
            {
                SwitchState(StateFactory.LoseState());
            }
        }
    }
}