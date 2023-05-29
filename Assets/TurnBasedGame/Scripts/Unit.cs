using System;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Interfaces;
using TurnBasedGame.Scripts.Managers;
using TurnBasedGame.Scripts.UI;
using TurnBasedGame.Scripts.UI.StatsDescription;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts
{
    public class Unit : MonoBehaviour, IBattleUnit, IUnitColor
    {
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
        [field: SerializeField] public bool IsMoved { get; set; }

        [field: SerializeField] public MeshRenderer UnitMaterial { get; set; }
        [field: SerializeField] public Sprite UnitPortrait { get; set; }

        public UnitsCountView UnitsCountView { get; set; }
        
        private Stats _unitStats { get; set; } 

        private void OnEnable()
        {
            CurrentHealthPoints = MaxHealthPoints;
            _unitStats = FindObjectOfType<Stats>();
            UnitsCountView = GetComponentInChildren<UnitsCountView>();
        }

        public void DealDamage(Unit attacker, Unit defender)
        {
            defender.GetDamage(attacker.MaxDamage * attacker.UnitNumber);
        }

        public void GetDamage(int value)
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
        
        private void OnMouseEnter()
        {
            Debug.Log("Enter");
            _unitStats.GetComponent<Image>().enabled = true;
            _unitStats.ShowStats(this);
        }

        private void OnMouseExit()
        {
            Debug.Log("Exit");
            _unitStats.GetComponent<Image>().enabled = false;
            _unitStats.ClearStats();
        }
    }
}