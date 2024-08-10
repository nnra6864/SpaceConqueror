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
        [SerializeField] private float _dashForce = 50;
        [SerializeField] private float _dashCooldown = 10;

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
            Player.IsDashing = true;
            Player.Collider.isTrigger = true;
            Player.Rigidbody.AddForce(_dashForce * Player.Energy.Level * transform.up, ForceMode2D.Impulse);
            Player.Energy.Level /= 2;
            TimeManager.ChangeTimeScale(
                new[] { 2f, 1f },
                new[] { 0f, 1f },
                new[] { Easings.Types.None, Easings.Types.CubicOut }
                );
            yield return new WaitForSecondsWhileNot(1f , () => TimeManager.IsPaused, true);
            Player.IsDashing = false;
            Player.Collider.isTrigger = false;
            _dashRoutine = null;
        }
    }
}