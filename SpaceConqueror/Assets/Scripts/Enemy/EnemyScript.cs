using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Enemy
{
    public class EnemyScript : NnBehaviour, IHittable
    {
        [SerializeField] private float _health;
        public float Health
        {
            get => _health;
            private set
            {
                _health = value < 0 ? 0 : value;
                if (_health <= 0) Die();
            }
        }

        public void GetHit(float damage)
        {
            Health -= damage;
        }

        public float GetHealth() => Health;

        private void Die() => Destroy(gameObject);
    }
}