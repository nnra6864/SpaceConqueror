using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pickups
{
    public class PickupLootTableScript : MonoBehaviour
    {
        [SerializeField] private List<PickupLootTableItem> _pickupItems;

        public List<PickupScript> GetPickups() => 
            (from item in _pickupItems where IsChosen(item.Chance) select item.Pickup).ToList();

        private bool IsChosen(float chance) => Random.Range(0f, 100f) < chance;
    }
}
