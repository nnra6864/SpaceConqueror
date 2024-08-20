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
        public float RefillRate => _refillRate;
        
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