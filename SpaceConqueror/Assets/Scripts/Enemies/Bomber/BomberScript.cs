using System.Collections;
using Core;
using NnUtils.Scripts;
using Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Enemies.Bomber
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BomberScript : NnBehaviour
    {
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");
        private static PlayerScript Player => GameManager.Player;
        private static TimeManager TimeManager => GameManager.TimeManager;
        private float _baseLightIntensity;
        private Material _mat;
        
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Light2D _emissionLight;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private float _accelTime = 3;
        [SerializeField] private float _accelRotation = 720;
        [SerializeField] private float _attackVelocity = 50;
        [SerializeField] private float _cooldown = 10;
        [SerializeField] private Vector2 _difficultyRange = new(0.75f, 1.25f);

        private void Reset() => _rb = GetComponent<Rigidbody2D>();
        private void Awake()
        {
            _baseLightIntensity = _emissionLight.intensity;
            _emissionLight.intensity = 0;
            _mat = _spriteRenderer.material;
            _mat.SetFloat(EmissionIntensity, 0);
        }

        private void Update()
        {
            if (!Player) return;
            StartNullRoutine(ref _attackRoutine, AttackRoutine());
        }

        private Coroutine _attackRoutine;

        private IEnumerator AttackRoutine()
        {
            var difficulty = Random.Range(_difficultyRange.x, _difficultyRange.y);
            var accelTime = _accelTime / difficulty;
            var accelRot = _accelRotation * Misc.RandomInvert * difficulty;
            var attackVel = _attackVelocity * difficulty;
            var accelVel = attackVel * 0.025f;
            var cooldown = _cooldown / difficulty - 2;
            Vector2 dir;

            float lerpPos = 0;
            while (lerpPos < 1)
            {
                dir = ((Vector2)(transform.position - Player.transform.position)).normalized;
                _rb.linearVelocity += accelVel * Time.deltaTime * dir;
                _rb.angularVelocity += accelRot * Time.deltaTime;
                
                var t = Misc.UpdateLerpPos(ref lerpPos, accelTime, easingType: Easings.Types.CubicIn);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
            }

            dir = ((Vector2)(Player.transform.position - transform.position)).normalized;
            _rb.AddForce(attackVel * dir, ForceMode2D.Impulse);

            while (lerpPos > 0)
            {
                var t = Misc.ReverseLerpPos(ref lerpPos, 2, easingType: Easings.Types.ExpoOut);
                _emissionLight.intensity = Mathf.LerpUnclamped(0, _baseLightIntensity, t);
                _mat.SetFloat(EmissionIntensity, t);
                yield return null;
            }
            
            yield return new WaitForSecondsWhileNot(cooldown, () => TimeManager.IsPaused);
            _attackRoutine = null;
        }
    }
}