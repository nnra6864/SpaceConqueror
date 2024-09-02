using Core;
using NnUtils.Scripts;
using NnUtils.Scripts.Audio;
using Player;
using UnityEngine;

namespace Pickups
{
    public class PickupScript : NnBehaviour
    {
        protected static PlayerScript Player => GameManager.Player;
        private static AudioManager AudioManager => NnManager.AudioManager;

        [SerializeField] private Sound _collectSound;
        [SerializeField] private Sound _disappearSound;
        [SerializeField] private float _lifetime = 5;

        private void Start()
        {
            Invoke(nameof(Disappear), _lifetime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Collect();
            Destroy(gameObject);
        }

        protected virtual void Collect()
        {
            AudioManager.PlayAt(_collectSound, transform.position);
        }
        
        private void Disappear()
        {
            AudioManager.PlayAt(_disappearSound, transform.position);
            Destroy(gameObject);
        }
    }
}