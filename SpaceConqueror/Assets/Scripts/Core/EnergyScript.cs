using System;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class EnergyScript : NnBehaviour
    {
        [SerializeField] private float _refillRate = 0.05f;
        public float RefillRate => _refillRate;
        
        private float _level = 1;
        public float Level
        {
            get => _level;
            set
            {
                _level = Mathf.Clamp01(value);
                OnLevelChanged?.Invoke(Level);
            }
        }
        public Action<float> OnLevelChanged;
        
        public void Update() => Level += _refillRate * Time.deltaTime;
    }
}