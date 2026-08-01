using System;
using Core;
using Menu;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(EnergyScript))]
    public class PlayerScript : MonoBehaviour , IHittable
    {
        private static readonly int EmissionIntensity = Shader.PropertyToID("_EmissionIntensity");
        private AudioManager _audioManager;

        private bool _init;

        [Header("Components")]
        
        [SerializeField] private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody => _rigidbody;

        [SerializeField] private Collider2D _collider;
        public Collider2D Collider => _collider;
        
        [SerializeField] private EnergyScript _energy;
        public EnergyScript Energy => _energy;

        [SerializeField] private PlayerAim _aim;
        public PlayerAim Aim => _aim;

        [SerializeField] private PlayerZoom _zoom;
        public PlayerZoom Zoom => _zoom;
        
        [SerializeField] private PlayerMovement _movement;
        public PlayerMovement Movement => _movement;

        [SerializeField] private PlayerDash _dash;
        public PlayerDash Dash => _dash;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        [SerializeField] private ParticleSystem _idleParticles;
        [SerializeField] private ParticleSystem _deathParticles;
        
        [SerializeField] private Light2D _emissionLight;
        public Light2D EmissionLight => _emissionLight;

        [SerializeField] private Sound _hitSound;

        private float _emissionLightIntensity;
        private Renderer _renderer;

        
        [Header("Values")]
        [SerializeField] private float _maxHealth = 100;
        public float MaxHealth => _maxHealth;
        
        private float _health;
        public float Health
        {
            get => _health;
            set
            {
                if (Mathf.Approximately(_health, value)) return;
                _health = Mathf.Clamp(value, 0, _maxHealth);
                OnHealthChanged?.Invoke(Health);
                if (Health <= 0) Die();
            }
        }
        public Action<float> OnHealthChanged;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                if (_score == value) return;
                _score = value;
                OnScoreChanged?.Invoke(_score);
            }
        }
        public Action<int> OnScoreChanged;


        private bool _isDashing;
        public bool IsDashing
        {
            get => _isDashing;
            set => _isDashing = value;
        }
        
        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _aim = GetComponent<PlayerAim>();
            _zoom = GetComponent<PlayerZoom>();
            _energy = GetComponent<EnergyScript>();
            _movement = GetComponent<PlayerMovement>();
            _dash = GetComponent<PlayerDash>();
        }

        private void OnEnable() => Init();

        private void Awake() => Init();

        private void OnDisable() => Deinit();

        private void OnDestroy() => Deinit();

        private void Init()
        {
            if (_init) return;
            _audioManager = AudioManager.Instance;
            Health = MaxHealth;
            _emissionLightIntensity = _emissionLight.intensity;
            _energy.OnLevelChanged += OnEnergyChanged;
            GameManager.RegisterPlayer(this);
            _init = true;
        }

        private void Deinit()
        {
            if (!_init) return;
            _energy.OnLevelChanged -= OnEnergyChanged;
            GameManager.UnregisterPlayer();
            _init = false;
        }

        private void OnEnergyChanged(float energy)
        {
            _spriteRenderer.material.SetFloat(EmissionIntensity, energy);
            _emissionLight.intensity = _emissionLightIntensity * energy;
        }

        public void GetHit(float damage)
        {
            Health -= damage;
            _audioManager.Play(_hitSound);
        }

        public float GetHealth() => Health;

        private void Die()
        {
            _idleParticles.transform.SetParent(null);
            _idleParticles.Stop();
            Destroy(_idleParticles.gameObject, _idleParticles.main.startLifetime.constantMax + 0.5f);

            _deathParticles.transform.SetParent(null);
            _deathParticles.Play();
            Destroy(_deathParticles.gameObject, _deathParticles.main.startLifetime.constantMax + 0.5f);

            Deinit();
            GameManager.Stop();
            Destroy(gameObject);
        }
    }
}
