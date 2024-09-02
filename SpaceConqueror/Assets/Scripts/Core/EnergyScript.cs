using System;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class EnergyScript : NnBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private float _refillRate = 0.05f;
        [SerializeField] private float _maxRefillRate = 0.25f;
        public float RefillRate
        {
            get => _refillRate;
            set
            {
                if (Mathf.Approximately(_refillRate, value)) return;
                _refillRate = Mathf.Clamp(value, 0.05f, _maxRefillRate);
                OnRefillRateChanged?.Invoke(_refillRate);
            }
        }
        public Action<float> OnRefillRateChanged;

        private float _level = 1;
        public float Level
        {
            get => _level;
            set
            {
                if (Mathf.Approximately(_level, value)) return;
                _level = Mathf.Clamp01(value);
                if (_fillImage) _fillImage.fillAmount = Level;
                OnLevelChanged?.Invoke(Level);
            }
        }
        public Action<float> OnLevelChanged;
        
        public void Update() => Level += _refillRate * Time.deltaTime;
    }
}