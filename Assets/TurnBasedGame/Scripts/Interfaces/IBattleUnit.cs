using TurnBasedGame.Scripts.Enum;
using UnityEngine;

namespace TurnBasedGame.Scripts.Interfaces
{
    public interface IBattleUnit
    {
        Vector3 Position { get; set;}
        UnitType UnitType { get; set; }
    }
}