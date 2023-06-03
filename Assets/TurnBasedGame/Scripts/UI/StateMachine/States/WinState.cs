using TurnBasedGame.Scripts.GameConfiguration;
using TurnBasedGame.Scripts.GameInput;
using TurnBasedGame.Scripts.Managers;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI.UIStateMachine.States
{
    public class WinState : BaseState
    {
        public WinState(StateMachine.UIStateMachine currentStateContext, UIStateFactory stateFactory) : base(currentStateContext, stateFactory)
        {
        }

        public override void EnterState()
        {
            StateContext.WinCanvas.enabled = true;

            SelectionController.IsPutPhaseInputEnded = false;
            
            LevelManager.Instance.CurrentLevel += 1;
            PlayerPrefs.SetInt(PrefsList.CurrentLevelPref, LevelManager.Instance.CurrentLevel);
            PlayerPrefs.Save();
        }

        protected override void UpdateState()
        {
            CheckSwitchStates();
        }

        protected override void ExitState()
        {
            StateContext.WinCanvas.enabled = false;
        }

        protected override void CheckSwitchStates()
        {
            
        }
    }
}