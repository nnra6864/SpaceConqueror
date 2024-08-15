using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    public class PlayerAim : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;
        private static TimeManager TimeManager => NnManager.TimeManager;
        private static Camera Cam => GameManager.Camera;

        private void Update()
        {
            if (TimeManager.IsPaused) return;
            var cursorPos = Cam.ScreenToWorldPoint(Input.mousePosition);
            var dir = cursorPos - Player.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Player.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
}