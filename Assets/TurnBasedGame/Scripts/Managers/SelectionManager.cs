using UnityEngine;

namespace TurnBasedGame.Scripts.Managers
{
    public class SelectionManager : MonoBehaviour
    {
        [field: SerializeField] public Unit Unit { get; set; }
    }
}