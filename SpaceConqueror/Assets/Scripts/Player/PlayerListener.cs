using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(AudioListener))]
    public class PlayerListener : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;

        private void Update()
        {
            if (!Player) return;
            transform.position = Player.transform.position;
        }
    }
}