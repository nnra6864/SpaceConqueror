using System.Collections;
using Core;
using NnUtils.Scripts;
using NnUnityEasings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class StatsScript : MonoBehaviour
    {
        private PlayerScript _player;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;
        [SerializeField] private TMP_Text _scoreTMP;

        private bool _init;

        private void Awake()
        {
            GameManager.OnPlayerRegistered += Init;
            GameManager.OnPlayerUnregistered += Deinit;
        }

        private void OnDestroy()
        {
            GameManager.OnPlayerRegistered -= Init;
            GameManager.OnPlayerUnregistered -= Deinit;
        }

        private void Init(PlayerScript player)
        {
            if (_init) return;

            _player = player;

            SetHealth(_player.Health);
            _player.OnHealthChanged += SetHealth;
            SetScore(_player.Score);
            _player.OnScoreChanged += SetScore;
            SetEnergy(_player.Energy.Level);
            _player.Energy.OnLevelChanged += SetEnergy;

            _init = true;
        }

        private void Deinit()
        {
            if (!_init) return;
            if (!_player) return;

            _player.OnHealthChanged -= SetHealth;
            _player.OnScoreChanged -= SetScore;
            _player.Energy.OnLevelChanged -= SetEnergy;

            _init = false;
        }

        // TODO: Animate
        public void Show()
        {
            _group.alpha = 1;
        }

        public void Hide()
        {
            _group.alpha = 0;
        }

        private void SetHealth(float health) => 
            this.RestartRoutine(ref _animateHealthRoutine, AnimateBarRoutine(_healthBar, health / _player.MaxHealth));
        private void SetEnergy(float energy) =>
            this.RestartRoutine(ref _animateEnergyRoutine, AnimateBarRoutine(_energyBar, energy));
        
        private void SetScore(int score) =>
            this.RestartRoutine(ref _animateScoreRoutine, AnimateScoreRoutine(_scoreTMP, _player.Score));

        private Coroutine _animateHealthRoutine;
        private Coroutine _animateEnergyRoutine;
        private Coroutine _animateScoreRoutine;

        private IEnumerator AnimateBarRoutine(Image bar, float val)
        {
            var startVal = bar.fillAmount;
            float lerpPos = 0;
            
            while (lerpPos < 1)
            {
                var t = Misc.Tween(ref lerpPos, 0.75f, Easing.ExpoOut);
                bar.fillAmount = Mathf.LerpUnclamped(startVal, val, t);
                yield return null;
            }
        }

        private IEnumerator AnimateScoreRoutine(TMP_Text tmp, int val)
        {
            var startVal = int.Parse(_scoreTMP.text);
            float lerpPos = 0;
            var lerpTime = Mathf.Abs(val - startVal) / 10f;
            
            while (lerpPos < 1)
            {
                var t = Misc.Tween(ref lerpPos, lerpTime, Easing.QuartOut);
                var res = Mathf.LerpUnclamped(startVal, val, t);
                tmp.text = $"{(int)res}";
                yield return null;
            }
        }
    }
}
