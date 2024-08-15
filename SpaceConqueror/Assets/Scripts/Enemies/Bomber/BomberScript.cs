using System.Collections;
using Core;
using NnUtils.Scripts;
using Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Enemies.Bomber
{
    [RequireComponent(typeof(EnemyScript), typeof(Rigidbody2D))]
    public class BomberScript : NnBehaviour
    {
        private const float ChargeVelMultiplier = 0.025f; //Used to multiply the velocity when charging
        private const float DimTime = 2; //Time in seconds to dim the lights and emission
        
        private static readonly WaitForSeconds UpdateInterval = new(0.1f); //Cached the update interval
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");
        private static PlayerScript Player => GameManager.Player;
        private static TimeManager TimeManager => NnManager.TimeManager;
        
        private float _baseLightIntensity;
        private Material _mat;

        [Header("Components")]
        [SerializeField] private EnemyScript _enemy;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Light2D _emissionLight;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("Values")]
        [SerializeField] private Vector2 _difficultyRange = new(0.75f, 1.25f);
        [SerializeField] private float _attackDistance = 10;
        [SerializeField] private float _retreatDistance = 30;
        [SerializeField] private float _chargeTime = 3;
        [SerializeField] private float _chargeRotation = 720;
        [SerializeField] private float _attackVelocity = 50;
        [SerializeField] private float _cooldown = 5;

        private void Reset()
        {
            _enemy = GetComponent<EnemyScript>();
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

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                if (!Player)
                {
                    _enemy.Die();
                    yield break;
                }
                
                if (Vector3.Distance(Player.transform.position, transform.position) <= _attackDistance)
                    StartNullRoutine(ref _attackRoutine, AttackRoutine());
                
                yield return UpdateInterval;
            }
        }

        private Coroutine _attackRoutine;
        private IEnumerator AttackRoutine()
        {
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
                dir = ((Vector2)(transform.position - Player.transform.position)).normalized;
                _rb.linearVelocity += chargeVel * Time.deltaTime * dir;
                _rb.angularVelocity += chargeRot * Time.deltaTime;
                
                var t = Misc.UpdateLerpPos(ref lerpPos, chargeTime, easingType: Easings.Types.CubicIn);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
            }

            //Attack
            dir = ((Vector2)(Player.transform.position - transform.position)).normalized;
            _rb.AddForce(attackVel * dir, ForceMode2D.Impulse);

            //Dimming lights and emission
            while (lerpPos > 0)
            {
                var t = Misc.ReverseLerpPos(ref lerpPos, DimTime, easingType: Easings.Types.ExpoOut);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
            }
            
            //Cooldown
            yield return new WaitForSecondsWhileNot(cooldown, () => TimeManager.IsPaused);
            
            //Retreat based on distance
            if (Vector3.Distance(Player.transform.position, transform.position) > _retreatDistance)
            {
                _attackRoutine = null;
                yield break;
            }
            
            //Start attacking again
            RestartRoutine(ref _attackRoutine, AttackRoutine());
        }
    }
}