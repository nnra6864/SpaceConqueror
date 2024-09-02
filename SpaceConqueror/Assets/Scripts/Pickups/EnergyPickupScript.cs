using UnityEngine;

namespace Pickups
{
    public class EnergyPickupScript : PickupScript
    {
        [SerializeField] private Vector2 _refillRange = new (0.1f, 0.25f);
        [SerializeField] private Vector2 _refillSpeedRange = new (0.025f, 0.25f);
        
        protected override void Collect()
        {
            base.Collect();
            Player.Energy.Level += Random.Range(_refillRange.x, _refillRange.y);
        }
    }
}