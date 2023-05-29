using System;
using TurnBasedGame.Scripts.GameInput;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        public event Action OnBattleStartToSetOrderSlots;
        
        public event Action OnBattleStart;

        public event Action OnShopPhaseEnd;

        public event Action OnWaitClick;
        
        public event Action OnAttackClick;
        
        public event Action OnDefendClick;
        
        public event Action OnMoveClick;
        
        public bool IsCombatPhaseStarted { get; set; }
        
        public bool IsPreBattlePhaseStarted { get; set; }

        public void OnStartCombatButtonClick()
        {
            IsCombatPhaseStarted = true; 
            OnBattleStartToSetOrderSlots?.Invoke();
            SelectionController.IsPutPhaseInputEnded = true;
        }

        public void OnReadyToFightButtonClick()
        {
            IsPreBattlePhaseStarted = true;
            OnShopPhaseEnd?.Invoke();
            OnBattleStart?.Invoke();
        }

        public void OnWaitActionButtonClick()
        {
            OnWaitClick?.Invoke();
        }

        public void OnAttackActionButtonClick()
        {
            OnAttackClick?.Invoke();
        }

        public void OnDefendActionButtonClick()
        {
            OnDefendClick?.Invoke();
        }

        public void OnMoveActionButtonClick()
        {
            OnMoveClick?.Invoke();
        }
    }
}