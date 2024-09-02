using UnityEngine;

namespace Pickups
{
    public class MovementPickupScript : PickupScript
    {
        [SerializeField] private Vector2 _speedRange = new (1, 3);
        [SerializeField] private Vector2 _forceRange = new (5, 25);
        [SerializeField] private Vector2Int _damageRange = new (5, 25);
        [SerializeField] private Vector2 _dashCooldown = new (0.5f, 2);
        
        protected override void Collect()
        {
            base.Collect();
            Player.Movement.Speed += Random.Range(_speedRange.x, _speedRange.y);
            Player.Dash.DashForce += Random.Range(_forceRange.x, _forceRange.y);
            Player.Dash.DashDamage += Random.Range(_damageRange.x, _damageRange.y);
            Player.Dash.DashCooldown -= Random.Range(_dashCooldown.x, _dashCooldown.y);
        }
    }
}