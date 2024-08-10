using Core;
using UnityEngine;

namespace DamageDealer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileScript : Ammo
    {
        [Header("Projectile")]
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IHittable>(out var hittable))
                DealDamage(hittable);
            Die();
        }

        private  void Move()
        {
            _rb.linearVelocity = transform.up * _projectileSpeed;
        }
    }
}