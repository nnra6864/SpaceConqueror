using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace DamageDealer
{
    public class DamageDealer : NnBehaviour, IHittable
    {
        [Header("Damage Dealer")]
        [SerializeField] protected float _damage;
        public float Damage => _damage;
        
        [SerializeField] protected float _health;
        public float Health
        {
            get => _health;
            protected set
            {
                Health = value < 0 ? 0 : value;
                if (Health <= 0) Die();
            }
        }

        protected virtual void DealDamage(IHittable hittable) => hittable.GetHit(Damage);
        protected virtual void TakeDamage(float damage) => Health -= damage;
        protected virtual void Die() => Destroy(gameObject);

        public void GetHit(float damage) => TakeDamage(damage);
        public float GetHealth() => Health;
    }
}