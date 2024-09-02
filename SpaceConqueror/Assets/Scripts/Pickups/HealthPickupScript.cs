using UnityEngine;

namespace Pickups
{
    public class HealthPickupScript : PickupScript
    {
        [SerializeField] private Vector2Int _regenRange = new (5, 25);
        
        protected override void Collect()
        {
            base.Collect();
            Player.Health += Random.Range(_regenRange.x, _regenRange.y);
        }
    }
}