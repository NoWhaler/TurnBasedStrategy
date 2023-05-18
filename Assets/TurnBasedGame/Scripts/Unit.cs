using System;
using TurnBasedGame.Scripts.Enum;
using TurnBasedGame.Scripts.Interfaces;
using TurnBasedGame.Scripts.Managers;
using TurnBasedGame.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace TurnBasedGame.Scripts
{
    public class Unit : MonoBehaviour, IBattleUnit, IUnitColor
    {
        [field: Header("Unit stats")]
        [field: SerializeField] public float HealthPoints { get; set; }
        [field: SerializeField] public int Attack { get; set; }
        [field: SerializeField] public int Defence { get; set; }
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
        [field: SerializeField]public BattleTile BattleTile { get; set; }
        
        [field: SerializeField] public bool IsSelected { get; set; }
        [field: SerializeField] public bool IsMoved { get; set; }

        [field: SerializeField] public MeshRenderer UnitMaterial { get; set; }
        [field: SerializeField] public Sprite UnitPortrait { get; set; }

        public UnitsCountView UnitsCountView { get; set; }

        private void OnEnable()
        {
            UnitsCountView = GetComponentInChildren<UnitsCountView>();
        }
        
        private void OnMouseOver()
        {
            
        }
    }
}