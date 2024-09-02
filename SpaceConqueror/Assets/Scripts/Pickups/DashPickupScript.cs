using UnityEngine;

namespace Pickups
{
    public class DashPickupScript : PickupScript
    {
        [SerializeField] private Vector2 _forceRange = new (5, 25);
        [SerializeField] private Vector2Int _damageRange = new (5, 25);
        
        protected override void Collect()
        {
            base.Collect();
            Player.Dash.DashForce += Random.Range(_forceRange.x, _forceRange.y);
            Player.Dash.DashDamage += Random.Range(_damageRange.x, _damageRange.y);
        }
    }
}