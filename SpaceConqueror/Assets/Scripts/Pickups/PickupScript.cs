using System;
using System.Collections;
using Core;
using NnUtils.Scripts;
using NnUtils.Scripts.Audio;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pickups
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PickupScript : NnBehaviour
    {
        protected static PlayerScript Player => GameManager.Player;
        private static AudioManager AudioManager => NnManager.AudioManager;

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private ParticleSystem _collectParticles;
        [SerializeField] private Sound _collectSound;
        [SerializeField] private float _lifetime = 5;

        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<Collider2D>();
        }

        private IEnumerator Start()
        {
            _col.enabled = false;
            
            //Adds a force in the random dir when spawning
            var dir = Random.insideUnitCircle.normalized;
            _rb.AddForce(dir * Random.Range(5, 10), ForceMode2D.Impulse);
            _rb.angularVelocity = Random.Range(-180, 180);
            
            //Prevents instant pickup and adds a nice animation
            var startScale = transform.localScale;
            float lerpPos = 0;
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, easingType: Easings.Types.ExpoOut);
                transform.localScale = Vector3.LerpUnclamped(Vector3.zero, startScale, t);
                yield return null;
            }

            _col.enabled = true;
            
            //Slowly disappear
            lerpPos = 0;
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, _lifetime, easingType: Easings.Types.CubicIn);
                transform.localScale = Vector3.LerpUnclamped(startScale, Vector3.zero, t);
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Collect();
            Destroy(gameObject);
        }

        protected virtual void Collect()
        {
            _collectParticles.transform.SetParent(null);
            _collectParticles.Play();
            Destroy(_collectParticles.gameObject, _collectParticles.main.startLifetime.constantMax + 0.5f);
            AudioManager.PlayAt(_collectSound, transform.position);
        }
    }
}