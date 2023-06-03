namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class PreCombatState : BaseState
    {
        public PreCombatState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory) 
            : base(currentStateContext, stateFactory) { }

        public override void EnterState()
        {
            StateContext.PreCombatCanvas.enabled = true;
        }
        protected override void UpdateState()
        {
            CheckSwitchStates();
        }

        protected override void ExitState()
        {
            StateContext.PreCombatCanvas.enabled = false;
        }
        
        protected override void CheckSwitchStates()
        {
            if (StateContext.UIController.IsCombatPhaseStarted)
            {
                SwitchState(StateFactory.Combat());
            }
        }
    }
}