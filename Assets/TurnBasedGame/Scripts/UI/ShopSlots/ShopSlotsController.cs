using System.Collections.Generic;
using System.Linq;
using TurnBasedGame.Scripts.UI.ShopSlots;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI
{
    public class ShopSlotsController : MonoBehaviour
    {
        [SerializeField] private List<ShopSlot> allShopSlots = new List<ShopSlot>();

        private void Start()
        {
            allShopSlots = GetComponentsInChildren<ShopSlot>().ToList();
        }
    }
}