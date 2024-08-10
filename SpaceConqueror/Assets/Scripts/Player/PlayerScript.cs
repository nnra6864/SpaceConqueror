using Core;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(EnergyScript))]
    public class PlayerScript : NnBehaviour
    {
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

        [SerializeField] private Light2D _emissionLight;
        public Light2D EmissionLight => _emissionLight;
        private float _emissionLightIntensity;

        private Renderer _renderer;

        [Header("Values")]
        
        [SerializeField] private float _energyRefillRate = 0.1f;
        
        private static readonly int EmissionStrength = Shader.PropertyToID("EmissionStrength");
        
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

        private void Awake()
        {
            GameManager.Player = this;
            _emissionLightIntensity = _emissionLight.intensity;
            _energy.OnLevelChanged += OnEnergyChanged;
        }

        private void OnEnergyChanged(float energy)
        {
            _spriteRenderer.material.SetFloat(EmissionStrength, energy);
            _emissionLight.intensity = _emissionLightIntensity * energy;
        }
    }
}