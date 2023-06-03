using TurnBasedGame.Scripts.GameInput;

namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class LoseState : BaseState
    {
        public LoseState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory) : base(currentStateContext, stateFactory)
        {
            
        }

        public override void EnterState()
        {
            StateContext.LoseCanvas.enabled = true;
            SelectionController.IsPutPhaseInputEnded = false;
        }

        protected override void UpdateState()
        {
            CheckSwitchStates();
        }

        protected override void ExitState()
        {
            StateContext.LoseCanvas.enabled = false;
        }

        protected override void CheckSwitchStates()
        {
            
        }
    }
}