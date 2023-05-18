using TurnBasedGame.Scripts.Enum;
using UnityEngine;

namespace TurnBasedGame.Scripts.Managers
{
    public class DirectionManager: MonoBehaviour
    {
        [field: SerializeField] public AttackDirection CurrentAttackDirection { get; set; }

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