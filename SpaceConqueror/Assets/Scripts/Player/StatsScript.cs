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
        private static PlayerScript Player => GameManager.Player;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;
        [SerializeField] private TMP_Text _scoreTMP;

        private bool _init;
        
        private void Awake()
        {
            GameManager.OnSpawned += OnSpawned;
        }

        private void OnSpawned()
        {
            Init();
        }

        private void OnDestroy() => Deinit();

        private void OnDisable() => Deinit();

        private void Init()
        {
            if (_init) return;
            SetHealth(Player.Health);
            Player.OnHealthChanged += SetHealth;
            SetScore(Player.Score);
            Player.OnScoreChanged += SetScore;
            SetEnergy(Player.Energy.Level);
            Player.Energy.OnLevelChanged += SetEnergy;
            _init = true;
        }

        private void Deinit()
        {
            if (!_init) return;
            if (!Player) return;
            Player.OnHealthChanged -= SetHealth;
            Player.OnScoreChanged -= SetScore;
            Player.Energy.OnLevelChanged -= SetEnergy;
            _init = false;
        }

        private void SetHealth(float health) => 
            this.RestartRoutine(ref _animateHealthRoutine, AnimateBarRoutine(_healthBar, health / Player.MaxHealth));
        private void SetEnergy(float energy) =>
            this.RestartRoutine(ref _animateEnergyRoutine, AnimateBarRoutine(_energyBar, energy));
        
        private void SetScore(int score) =>
            this.RestartRoutine(ref _animateScoreRoutine, AnimateScoreRoutine(_scoreTMP, Player.Score));

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
