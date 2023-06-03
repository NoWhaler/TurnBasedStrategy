using System;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Interfaces;
using TurnBasedGame.Scripts.UI;
using TurnBasedGame.Scripts.UI.Controller;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class Unit : MonoBehaviour, IBattleUnit, IUnitColor
    {
        [field: SerializeField] public string UnitName { get; set; }
        
        [field: Header("Unit stats")]
        [field: SerializeField] public int CurrentHealthPoints { get; set; }
        [field: SerializeField] public int MaxHealthPoints { get; set; }
        
        [field: SerializeField] public int Attack { get; set; }
        [field: SerializeField] public int Defense { get; set; }
        [field: SerializeField] public int MinDamage { get; set; }
        [field: SerializeField] public int MaxDamage { get; set; }

        [field: SerializeField] public int AttackRange { get; set; } = 1;
        [field: SerializeField] public int MoveRange { get; set; }
        [field: SerializeField] public int Initiative { get; set; }
        [field: SerializeField] public int UnitNumber { get; set; }

        [field: Header("Unit type")]
        [field: SerializeField] public UnitType UnitType { get; set; }
        [field: SerializeField] public UnitFractionType UnitFractionType { get; set; }
        
        [field: Header("Unit placement")]
        public Vector3 Position { get; set; }
        [field: SerializeField] public BattleTile BattleTile { get; set; }
        
        [field: SerializeField] public bool IsSelected { get; set; }
        
        [field: SerializeField] public Sprite UnitPortrait { get; set; }

        [field: SerializeField] public UnitsCountView UnitsCountView { get; set; }

        [field: SerializeField] public bool HasCounterAttack { get; set; } = true;

        private bool IsDead { get; set; }

        public event Action<Unit> OnDeath;
        
        private void OnEnable()
        {
            CurrentHealthPoints = MaxHealthPoints;
            UnitsCountView = GetComponentInChildren<UnitsCountView>();
        }

        public void DealDamage(Unit attacker, Unit defender)
        {
            if (attacker != null && defender != null)
            {
                if (attacker != defender && attacker.UnitFractionType != defender.UnitFractionType)
                {
                    defender.GetDamage(attacker.MaxDamage * attacker.UnitNumber);
                    
                    DamageLogsController.Instance.LogDamageEvent(attacker, defender);

                    if (CheckUnitIsDead(defender))
                    {
                        DamageLogsController.Instance.LogDeathEvent(defender);
                    }

                    if (defender.HasCounterAttack && attacker.UnitType != UnitType.RangeUnit)
                    {
                        attacker.GetDamage(defender.MaxDamage * defender.UnitNumber);
                        DamageLogsController.Instance.LogDamageEvent(defender, attacker);
                        defender.HasCounterAttack = false;
                        
                        if (CheckUnitIsDead(attacker))
                        {
                            DamageLogsController.Instance.LogDeathEvent(attacker);
                        }
                    }
                }
            }
        }

        private void GetDamage(int value)
        {
            int remainingDamage = value;

            var totalDefenderHealthPoints = 0;

            if (CurrentHealthPoints != MaxHealthPoints)
            {
                totalDefenderHealthPoints = MaxHealthPoints * (UnitNumber - 1) + CurrentHealthPoints;
            }
            else if (CurrentHealthPoints == MaxHealthPoints)
            {
                totalDefenderHealthPoints = MaxHealthPoints * UnitNumber;
            }

            if (totalDefenderHealthPoints > remainingDamage)
            {
                int unitsLost = Mathf.FloorToInt((remainingDamage / (float)MaxHealthPoints));
                int remainingHealthPoints = remainingDamage % MaxHealthPoints;

                if (remainingHealthPoints >= CurrentHealthPoints)
                {
                    unitsLost++;
                    remainingHealthPoints -= CurrentHealthPoints;
                    CurrentHealthPoints = MaxHealthPoints - remainingHealthPoints;
                }
                else
                {
                    CurrentHealthPoints -= remainingHealthPoints;
                }

                UnitNumber -= unitsLost;
                UnitsCountView.UpdateCountValue(-unitsLost);
            }
            else
            {
                CurrentHealthPoints = 0;
                UnitsCountView.UpdateCountValue(-UnitNumber);
                UnitNumber = 0;
            }
        }

        private bool CheckUnitIsDead(Unit unit)
        {
            if (unit.UnitNumber <= 0)
            {
                OnDeath?.Invoke(unit);
                unit.BattleTile.Unit = null;
                unit.BattleTile = null;
                Destroy(unit.gameObject);

                return unit.IsDead = true;
            }

            return unit.IsDead = false;
        }
    }
}