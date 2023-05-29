using TurnBasedGame.Scripts.Enum;
using UnityEngine;

namespace TurnBasedGame.Scripts.Managers
{
    public class DirectionManager: MonoBehaviour
    {
        [field: SerializeField] private AttackDirection CurrentAttackDirection { get; set; }
        
        [field: SerializeField] public bool IsAttacking { get; set; }

        public void SetDirection(AttackDirection attackDirection)
        {
            CurrentAttackDirection = attackDirection;
        }

        public void UnsetDirection()
        {
            CurrentAttackDirection = AttackDirection.Default ;
        }
    }
}