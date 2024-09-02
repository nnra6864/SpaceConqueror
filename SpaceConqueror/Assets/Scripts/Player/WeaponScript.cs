using System.Collections;
using Core;
using NnUtils.Scripts;
using DamageDealer;
using NnUtils.Scripts.Audio;
using UnityEngine;

namespace Player
{
    public class WeaponScript : NnBehaviour
    {
        private static TimeManager TimeManager => NnManager.TimeManager;
        private static AudioManager AudioManager => NnManager.AudioManager;
        private static PlayerScript Player => GameManager.Player;
        
        [SerializeField] private Ammo _ammo;
        public Ammo Ammo
        {
            get => _ammo;
            set
            {
                if (Equals(value, _ammo)) return;
                _ammo = value;
            }
        }
        
        [SerializeField] private float _cooldown = 1;
        [SerializeField] private float _knockback = 5;

        private void Update()
        {
            if (TimeManager.IsPaused) return;
            if (Input.GetKeyDown(KeyCode.Mouse0)) Attack();
        }

        private void Attack()
        {
            if (Player.Energy.Level < Ammo.Energy) return;
            StartNullRoutine(ref _attackRoutine, AttackRoutine());
        }
        private Coroutine _attackRoutine;
        private IEnumerator AttackRoutine()
        {
            var projectile = Instantiate(Ammo, transform);
            projectile.transform.SetParent(null);
            Player.Rigidbody.AddForce(-transform.up * _knockback, ForceMode2D.Impulse);
            Player.Energy.Level -= Ammo.Energy;
            TimeManager.ChangeTimeScale(
                new[] { 2f, 1f },
                new[] { 0f, 1f },
                new[] { Easings.Types.None, Easings.Types.CubicOut }
                );
            AudioManager.Play(_ammo.DischargeSound.Name);
            yield return new WaitForSecondsWhileNot(_cooldown, () => TimeManager.IsPaused, true);
            _attackRoutine = null;
            yield return null;
        }
    }
}