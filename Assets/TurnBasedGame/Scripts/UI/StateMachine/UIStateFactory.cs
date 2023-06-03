using System.Collections.Generic;
using TurnBasedGame.Scripts.UI.UIStateMachine.States;
using TurnBasedGame.Scripts.UI.UIStateMachine.States.Enum;

namespace TurnBasedGame.Scripts.UI.UIStateMachine
{
    public class UIStateFactory 
    {
        private StateMachine.UIStateMachine _context;

        private readonly Dictionary<UIStates, BaseState> _states = new Dictionary<UIStates, BaseState>();

        public UIStateFactory(StateMachine.UIStateMachine uiStateMachine)
        {
            _context = uiStateMachine;

            _states[UIStates.PreCombat] = new PreCombatState(_context, this);
            _states[UIStates.Combat] = new CombatState(_context, this);
            _states[UIStates.Shop] = new ShopState(_context, this);
            _states[UIStates.Win] = new WinState(_context, this);
            _states[UIStates.Lose] = new LoseState(_context, this);
        }

        public BaseState PreCombat()
        {
            return _states[UIStates.PreCombat];
        }
        
        public BaseState Combat()
        {
            return _states[UIStates.Combat];
        }
        
        public BaseState Menu()
        {
            return _states[UIStates.Menu];
        }

        public BaseState ShopState()
        {
            return _states[UIStates.Shop];
        }

        public BaseState WinState()
        {
            return _states[UIStates.Win];
        }
        
        public BaseState LoseState()
        {
            return _states[UIStates.Lose];
        }
    }
}