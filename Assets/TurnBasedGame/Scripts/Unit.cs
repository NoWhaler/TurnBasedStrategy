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
        
        [field: SerializeField] public int UnitLevel { get; set; }
        
        [field: Header("Unit stats")]
        
        // "CurrentHealthPoints" та "MaxHealthPoints": поточні та максимальні очки здоров'я юніта
        [field: SerializeField] public int CurrentHealthPoints { get; set; }
        [field: SerializeField] public int MaxHealthPoints { get; set; }
        
        //"Attack" та "Defense": значення атаки та захисту юніта.
        [field: SerializeField] public int Attack { get; set; }
        [field: SerializeField] public int Defense { get; set; }
        
        // "MinDamage" та "MaxDamage": мінімальна та максимальна шкода, яку може завдати юніт.
        [field: SerializeField] public int MinDamage { get; set; }
        [field: SerializeField] public int MaxDamage { get; set; }

        // "AttackRange": дальність атаки юніта.
        
        [field: SerializeField] public int AttackRange { get; set; } = 1;
        
        // "MoveRange": дальність переміщення юніта.
        [field: SerializeField] public int MoveRange { get; set; }
        
        // "Initiative": ініціатива юніта, що впливає на черговість ходу юніту.
        [field: SerializeField] public int Initiative { get; set; }
        
        // "UnitNumber": кількість юнітів в одній групі.
        [field: SerializeField] public int UnitNumber { get; set; }

        [field: Header("Unit type")]
        [field: SerializeField] public UnitType UnitType { get; set; }
        [field: SerializeField] public UnitFractionType UnitFractionType { get; set; }
        
        [field: Header("Unit placement")]
        public Vector3 Position { get; set; }
        public BattleTile BattleTile { get; set; }
        
        public bool IsSelected { get; set; }
        
        [field: SerializeField] public Sprite UnitPortrait { get; set; }

        public UnitsCountView UnitsCountView { get; set; }

        public bool HasCounterAttack { get; set; } = true;

        private bool IsDead { get; set; }

        public event Action<Unit> OnDeath;
        
        private void OnEnable()
        {
            CurrentHealthPoints = MaxHealthPoints;
            UnitsCountView = GetComponentInChildren<UnitsCountView>();
        }

        // Відповідає за нанесення шкоди одному юніту від іншого.
        // Він перевіряє, чи виконуються необхідні умови для атаки та розраховує шкоду, яку отримає оборонюючийся юніт
        
        public void DealDamage(Unit attacker, Unit defender)
        {
            if (attacker != null && defender != null)
            {
                if (attacker != defender && attacker.UnitFractionType != defender.UnitFractionType)
                {
                    int damageAttacker = DamageFormulaCalculation(attacker, defender);
                    defender.GetDamage(damageAttacker);
                    
                    DamageLogsController.Instance.LogDamageEvent(attacker, defender, damageAttacker);

                    if (CheckUnitIsDead(defender))
                    {
                        DamageLogsController.Instance.LogDeathEvent(defender);
                    }

                    if (defender.HasCounterAttack && attacker.UnitType != UnitType.RangeUnit)
                    {
                        int damageDefender = DamageFormulaCalculation(defender, attacker);
                        
                        attacker.GetDamage(damageDefender);
                        DamageLogsController.Instance.LogDamageEvent(defender, attacker, damageDefender);
                        defender.HasCounterAttack = false;
                        
                        if (CheckUnitIsDead(attacker))
                        {
                            DamageLogsController.Instance.LogDeathEvent(attacker);
                        }
                    }
                }
            }
        }
        
        // Обробляє отримання шкоди юнітом та оновлює відповідні значення здоров'я та кількості юнітів


        public int DamageFormulaCalculation(Unit attacker, Unit defender)
        {
            if (attacker.Attack >= defender.Defense)
            {
                var damage = Mathf.FloorToInt(attacker.UnitNumber *
                                              UnityEngine.Random.Range(attacker.MinDamage, attacker.MaxDamage) *
                                              (1 + 0.05f * (attacker.Attack - defender.Defense)));

                return damage;
            }
            
            if (attacker.Attack <= defender.Defense)
            {
                var damage = Mathf.FloorToInt(attacker.UnitNumber *
                                              UnityEngine.Random.Range(attacker.MinDamage, attacker.MaxDamage) /
                                              (1 + 0.05f * (defender.Defense - attacker.Attack)));

                return damage;
            }

            return 0;
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