using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;
        [SerializeField] private float _speed = 30;
        [SerializeField] private float _maxSpeed = 6;

        private void Update()
        {
            Move(GetDirection());
        }

        private Vector2 GetDirection() => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        private void Move(Vector2 dir)
        {
            var vel = Player.Rigidbody.linearVelocity;
            var velDelta = _speed * Time.deltaTime * dir;
            Player.Rigidbody.linearVelocity += Misc.CapVelocityDelta(vel, velDelta, _maxSpeed);
        }
    }
}