using UnityEngine;

namespace Pickups
{
    public class EnergyPickupScript : PickupScript
    {
        [SerializeField] private Vector2 _regenRange = new (5, 25);
        
        protected override void Collect()
        {
            base.Collect();
            Player.Energy.Level += Random.Range(_regenRange.x, _regenRange.y);
        }
    }
}