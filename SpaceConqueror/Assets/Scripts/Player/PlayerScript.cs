using NnUtils.Scripts;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerScript : NnBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        public Rigidbody2D Rigidbody
        {
            get => _rigidbody;
            private set => _rigidbody = value;
        }

        private bool _isDashing;
        public bool IsDashing
        {
            get => _isDashing;
            set => _isDashing = value;
        }
        
        private void Reset()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Awake()
        {
            GameManager.Player = this;
        }
    }
}