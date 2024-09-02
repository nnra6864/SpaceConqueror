using UnityEngine;

namespace Pickups
{
    [System.Serializable]
    public struct PickupLootTableItem
    {
        public PickupScript Pickup;
        [Range(0, 100)] public float Chance;
    }
}