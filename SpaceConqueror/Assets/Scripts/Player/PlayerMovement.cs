using System;
using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : NnBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed = 30;
        [SerializeField] private float _maxSpeed = 6;
        [SerializeField] [Range(0, 1)] private float _friction = 0.9f;

        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Move(GetDirection());
            ApplyFriction();
            ApplyFriction();
        }

        private Vector2 GetDirection() => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        private void Move(Vector2 dir)
        {
            var vel = _rb.linearVelocity;
            var velDelta = _speed * Time.deltaTime * dir;
            _rb.linearVelocity += Misc.CapVelocityDelta(vel, velDelta, _maxSpeed);
        }
        
        private void ApplyFriction() => _rb.linearVelocity *= Mathf.Pow(1 - _friction, Time.deltaTime);
    }
}