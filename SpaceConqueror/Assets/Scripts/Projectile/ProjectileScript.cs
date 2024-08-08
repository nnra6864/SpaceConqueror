using NnUtils.Scripts;
using UnityEngine;

namespace Projectile
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileScript : NnBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _projectileSpeed = 25f;
        [SerializeField] private float _lifetime = 5;

        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Destroy(gameObject, _lifetime);
        }

        private void Update()
        {
            Move();
        }

        protected virtual void Move()
        {
            _rb.linearVelocity = transform.up * _projectileSpeed;
        }
    }
}