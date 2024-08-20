using System.Collections;
using Core;
using NnUtils.Scripts;
using NnUtils.Scripts.Audio;
using Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BomberScript : NnBehaviour, IHittable
    {
        private const float ChargeVelMultiplier = 0.025f; //Used to multiply the velocity when charging
        private const float DimTime = 2; //Time in seconds to dim the lights and emission
        
        private static readonly WaitForSeconds UpdateInterval = new(0.1f); //Cached the update interval
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");
        private static PlayerScript Player => GameManager.Player;
        private static TimeManager TimeManager => NnManager.TimeManager;
        private static AudioManager AudioManager => NnManager.AudioManager;
        
        private float _baseLightIntensity;
        private bool _isAttacking;
        private Material _mat;

        [Header("Components")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Light2D _emissionLight;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sound _dashSound;
        [SerializeField] private Sound _hitSound;

        [Header("Values")]
        [SerializeField] private Vector2 _difficultyRange = new(0.75f, 1.25f);
        [SerializeField] private float _attackDistance = 10;
        [SerializeField] private float _retreatDistance = 30;
        [SerializeField] private float _chargeTime = 3;
        [SerializeField] private float _chargeRotation = 720;
        [SerializeField] private float _attackVelocity = 50;
        [SerializeField] private float _cooldown = 5;

        [SerializeField] private float _health;
        public float Health
        {
            get => _health;
            set
            {
                _health = value < 0 ? 0 : value;
                if (_health <= 0) Die();
            }
        }
        
        private void Reset()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            _baseLightIntensity = _emissionLight.intensity;
            _emissionLight.intensity = 0;
            _mat = _spriteRenderer.material;
            _mat.SetFloat(EmissionIntensity, 0);
        }

        private void Start() => StartCoroutine(UpdateRoutine());

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isAttacking) return;
            if (!other.gameObject.TryGetComponent<IHittable>(out var hittable)) return;
            hittable.GetHit(Mathf.Clamp(_rb.linearVelocity.magnitude + _rb.angularVelocity, 5, 25));
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                if (PlayerDead()) yield break;
                
                if (Vector3.Distance(Player.transform.position, transform.position) <= _attackDistance)
                    StartNullRoutine(ref _attackRoutine, AttackRoutine());
                
                yield return UpdateInterval;
            }
        }

        private Coroutine _attackRoutine;
        private IEnumerator AttackRoutine()
        {
            _isAttacking = true;
            var difficulty = Random.Range(_difficultyRange.x, _difficultyRange.y);
            var chargeTime = _chargeTime / difficulty;
            var chargeRot = _chargeRotation * Misc.RandomInvert * difficulty;
            var attackVel = _attackVelocity * difficulty;
            var chargeVel = attackVel * ChargeVelMultiplier;
            var cooldown = _cooldown / difficulty - DimTime;
            Vector2 dir;

            //Charging
            float lerpPos = 0;
            while (lerpPos < 1)
            {
                if (PlayerDead()) yield break;
                dir = ((Vector2)(transform.position - Player.transform.position)).normalized;
                _rb.linearVelocity += chargeVel * Time.deltaTime * dir;
                _rb.angularVelocity += chargeRot * Time.deltaTime;
                
                var t = Misc.UpdateLerpPos(ref lerpPos, chargeTime, easingType: Easings.Types.CubicIn);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
            }

            if (PlayerDead()) yield break;
            
            //Attack
            dir = ((Vector2)(Player.transform.position - transform.position)).normalized;
            _rb.AddForce(attackVel * dir, ForceMode2D.Impulse);
            AudioManager.PlayAt(_dashSound, transform.position, 1.5f - difficulty * 0.5f);

            //Dimming lights and emission
            while (lerpPos > 0)
            {
                var t = Misc.ReverseLerpPos(ref lerpPos, DimTime, easingType: Easings.Types.ExpoOut);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
                if (PlayerDead()) yield break;
            }

            //Don't deal damage anymore
            _isAttacking = false;
            
            //Cooldown
            yield return new WaitForSecondsWhileNot(cooldown, () => TimeManager.IsPaused);
            
            if (PlayerDead()) yield break;
            
            //Retreat based on distance
            if (Vector3.Distance(Player.transform.position, transform.position) > _retreatDistance)
            {
                _attackRoutine = null;
                yield break;
            }
            
            //Start attacking again
            RestartRoutine(ref _attackRoutine, AttackRoutine());
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void GetHit(float damage)
        {
            Health -= damage;
            AudioManager.PlayAt(_hitSound, transform.position);
        }

        public float GetHealth() => Health;

        private bool PlayerDead()
        {
            if (Player) return false;
            Die();
            return true;
        }
    }
}