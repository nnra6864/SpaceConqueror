using System.Collections;
using Core;
using NnUtils.Scripts;
using DamageDealer;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : NnBehaviour
    {
        private static TimeManager TimeManager => GameManager.TimeManager;
        private static PlayerScript Player => GameManager.Player;
        
        [SerializeField] private ProjectileScript _projectilePrefab;
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private float _knockback = 5;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
        }

        private void Attack() => StartNullRoutine(ref _attackRoutine, AttackRoutine());
        private Coroutine _attackRoutine;
        private IEnumerator AttackRoutine()
        {
            var projectile = Instantiate(_projectilePrefab, transform);
            projectile.transform.SetParent(null);
            Player.Rigidbody.AddForce(-transform.up * _knockback, ForceMode2D.Impulse);
            TimeManager.ChangeTimeScale(
                new[] { 2f, 1f },
                new[] { 0f, 1f },
                new[] { Easings.Types.None, Easings.Types.CubicOut }
                );
            yield return new WaitForSecondsWhileNot(_cooldown, () => TimeManager.IsPaused, true);
            _attackRoutine = null;
            yield return null;
        }
    }
}