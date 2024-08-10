using System;
using System.Collections;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    public class PlayerDash : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;
        private static TimeManager TimeManager => GameManager.TimeManager;
        [SerializeField] private float _dashDuration = 1;
        [SerializeField] private float _dashCooldown = 10;

        [SerializeField] private float _dashForce = 25;
        [SerializeField] private float _maxDashForce = 100;
        public float DashForce
        {
            get => _dashForce;
            set => _dashForce = Mathf.Clamp(value, 0, _maxDashForce);
        }
        public Action<float> OnForceChanged;

        [SerializeField] private int _dashDamage;
        [SerializeField] private float _maxDashDamage = 100;
        public int DashDamage
        {
            get => _dashDamage;
            set => _dashDamage = (int)Mathf.Clamp(value, 0, _maxDashDamage);
        }
        public Action<int> OnDamageChanged;


        private void Update()
        {
            if (TimeManager.IsPaused) return;
            if (Input.GetKeyDown(KeyCode.LeftShift)) Dash();
        }

        private void Dash()
        {
            if (Player.Energy.Level < 0.25f) return;
            StartNullRoutine(ref _dashRoutine, DashRoutine());
        }

        private Coroutine _dashRoutine;
        private IEnumerator DashRoutine()
        {
            var energy = Player.Energy.Level;
            Player.IsDashing = true;
            Player.Collider.isTrigger = true;
            Player.Rigidbody.AddForce(_dashForce * energy * transform.up, ForceMode2D.Impulse);
            Player.Energy.Level /= 2;
            
            TimeManager.ChangeTimeScale(
                new[] { 2f, 1f },
                new[] { 0f, 1f },
                new[] { Easings.Types.None, Easings.Types.CubicOut }
                );
            
            yield return new WaitForSecondsWhileNot(_dashDuration * energy, () => TimeManager.IsPaused, true);
            Player.IsDashing = false;
            Player.Collider.isTrigger = false;
            
            yield return new WaitForSecondsWhileNot(_dashCooldown, () => TimeManager.IsPaused, true);
            _dashRoutine = null;
        }
    }
}