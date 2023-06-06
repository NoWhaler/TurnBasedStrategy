using System;
using TurnBasedGame.Scripts.GameInput;
using TurnBasedGame.Scripts.Managers;
using TurnBasedGame.Scripts.UI.ShopSlots;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.Controller
{
    public class UIController : MonoBehaviour
    {
        
        public ShopSlot CurrentShopSlot { get; set; }

        private StateMachine.UIStateMachine _stateMachine;

        private LevelView _levelView;
        
        public event Action OnBattleStartToSetOrderSlots;
        
        public event Action OnBattleStart;

        public event Action OnShopPhaseEnd;

        public event Action OnWaitClick;
        
        public event Action OnDefendClick;

        public event Action OnResetUnits;

        public event Action<ShopSlot> OnBuyUnit;

        public bool IsCombatPhaseStarted { get; set; }
        
        public bool IsPreBattlePhaseStarted { get; set; }
        
        public bool IsWinStateActive { get; set; }
        
        public bool IsLoseStateActive { get; set; }

        private void Start()
        {
            _levelView = FindObjectOfType<LevelView>();
            
            _levelView.UpdateValue(LevelManager.Instance.CurrentLevel);
        }

        public void OnStartCombatButtonClick()
        {
            IsCombatPhaseStarted = true; 
            OnBattleStartToSetOrderSlots?.Invoke();
            SelectionController.IsPutPhaseInputEnded = true;
            _stateMachine = GetComponent<StateMachine.UIStateMachine>();
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

        public void OnDefendActionButtonClick()
        {
            OnDefendClick?.Invoke();
        }

        public void OnBuyUnitButtonClick()
        {
            OnBuyUnit?.Invoke(CurrentShopSlot);   
        }

        public void OnResetButtonClick()
        {
            OnResetUnits?.Invoke();
        }

        public void OnRestartButtonClick()
        {
            LevelManager.Instance.RestartLevel();
        }

        public void OnNextLevelClick()
        {
            LevelManager.Instance.LoadNextLevel();
        }

        public void OnOpenDamageLogger()
        {
            if (_stateMachine.LoggerCanvas.enabled == false)
            {
                _stateMachine.LoggerCanvas.enabled = true;
                var raycaster = _stateMachine.LoggerCanvas.GetComponent<GraphicRaycaster>();
                raycaster.enabled = true;
            }
        }

        public void OnCloseDamageLogger()
        {
            _stateMachine.LoggerCanvas.enabled = false;
            var raycaster = _stateMachine.LoggerCanvas.GetComponent<GraphicRaycaster>();
            raycaster.enabled = false;
        }
    }
}