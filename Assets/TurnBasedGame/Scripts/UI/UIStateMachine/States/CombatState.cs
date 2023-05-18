using UnityEngine;

namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class CombatState : BaseState
    {
        public CombatState(UIStateMachine currentStateContext, UIStateFactory stateFactory) 
            : base(currentStateContext, stateFactory) { }

        public override void EnterState()
        {
            StateContext.CombatCanvas.enabled = true;
        }

        protected override void UpdateState()
        {
            
        }

        protected override void ExitState()
        {
            StateContext.CombatCanvas.enabled = false;
        }
        
        protected override void CheckSwitchStates()
        {
            
        }
    }
}