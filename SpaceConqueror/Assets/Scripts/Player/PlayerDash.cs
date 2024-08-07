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
            if (TimeManager.IsPaused || Player.IsDashing) return;
            if (Input.GetKeyDown(KeyCode.LeftShift)) Dash();
        }

        private void Dash() => StartCoroutine(DashRoutine());

        private IEnumerator DashRoutine()
        {
            Player.IsDashing = true;
            Player.Rigidbody.AddForce(_dashForce * transform.up, ForceMode2D.Impulse);
            TimeManager.ChangeTimeScale(
                new[] { 2f, 1f },
                new[] { 0f, 1f },
                new[] { Easings.Types.None, Easings.Types.CubicOut }
                );
            yield return new WaitForSecondsWhileNot(5f , () => TimeManager.IsPaused, true);
            Player.IsDashing = false;
        }
    }
}