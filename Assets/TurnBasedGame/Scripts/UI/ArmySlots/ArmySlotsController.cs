using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI.ArmySlots
{
    public abstract class ArmySlotsController : MonoBehaviour
    {
        [SerializeField] public List<ArmySlot> allArmySlots = new List<ArmySlot>();

        private void Awake()
        {
            allArmySlots = GetComponentsInChildren<ArmySlot>().ToList();
        }
    }
}