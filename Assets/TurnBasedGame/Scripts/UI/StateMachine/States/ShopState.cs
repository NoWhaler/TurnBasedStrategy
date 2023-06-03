using UnityEngine;

namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class ShopState : BaseState
    {
        public ShopState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory) 
            : base(currentStateContext, stateFactory)
        {
        }

        public override void EnterState()
        {
            StateContext.ShopCanvas.enabled = true;
        }

        protected override void UpdateState()
        {
            CheckSwitchStates();
        }

        protected override void ExitState()
        {
            StateContext.ShopCanvas.enabled = false;
        }

        protected override void CheckSwitchStates()
        {
            if (StateContext.UIController.IsPreBattlePhaseStarted)
            {
                SwitchState(StateFactory.PreCombat());
            }
        }
    }
}