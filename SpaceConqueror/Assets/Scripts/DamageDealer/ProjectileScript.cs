using System;
using Core;
using NnUtils.Scripts;
using NnUtils.Scripts.Audio;
using UnityEngine;

namespace DamageDealer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileScript : Ammo
    {
        private static AudioManager AudioManager => NnManager.AudioManager;
        
        [Header("Projectile")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private ParticleSystem _idleParticles;
        [SerializeField] private ParticleSystem _destroyParticles;
        [SerializeField] private Sound _hitSound;
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
            AudioManager.PlayAt(_hitSound, transform.position);
            Die();
        }

        private void Move()
        {
            _rb.linearVelocity = transform.up * _projectileSpeed;
        }

        private void OnDestroy()
        {
            _idleParticles.Stop();
            _idleParticles.transform.SetParent(null);
            Destroy(_idleParticles, _idleParticles.main.startLifetime.constantMax + 1);
            
            _destroyParticles.transform.SetParent(null);
            _destroyParticles.Play();
            Destroy(_destroyParticles, _destroyParticles.main.startLifetime.constantMax + 1);
        }
    }
}