namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public abstract class BaseState
    {
        protected UIStateFactory StateFactory { get; set; }
        protected StateMachine.UIStateMachine StateContext { get; set; }
        
        protected BaseState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory)
        {
            StateContext = currentStateContext;
            StateFactory = stateFactory;
        }

        public abstract void EnterState();
        protected abstract void UpdateState();
        protected abstract void ExitState();
        protected abstract void CheckSwitchStates();
        

        public void UpdateStates()
        {
            UpdateState();
        }

        protected void SwitchState(BaseState newState)
        {
            ExitState();
            newState.EnterState();
            StateContext.CurrentState = newState;
        }
        
        
    }
}