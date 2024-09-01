using System.Collections;
using Core;
using NnUtils.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class StatsScript : NnBehaviour
    {
        private static PlayerScript Player => GameManager.Player;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;
        [SerializeField] private TMP_Text _scoreTMP;
        
        private void Awake()
        {
            Player.OnHealthChanged += SetHealth;
            Player.OnScoreChanged += SetScore;
            Player.Energy.OnLevelChanged += SetEnergy;
        }

        private void OnDestroy()
        {
            Player.OnHealthChanged -= SetHealth;
            Player.OnScoreChanged -= SetScore;
            Player.Energy.OnLevelChanged -= SetEnergy;
        }

        private void SetHealth(float health) => 
            RestartRoutine(ref _animateHealthRoutine, AnimateBarRoutine(_healthBar, health / Player.MaxHealth));
        private void SetEnergy(float energy) =>
            RestartRoutine(ref _animateEnergyRoutine, AnimateBarRoutine(_energyBar, energy));
        
        private void SetScore(int score) =>
            RestartRoutine(ref _animateScoreRoutine, AnimateScoreRoutine(_scoreTMP, Player.Score));

        private Coroutine _animateHealthRoutine;
        private Coroutine _animateEnergyRoutine;
        private Coroutine _animateScoreRoutine;

        private IEnumerator AnimateBarRoutine(Image bar, float val)
        {
            var startVal = bar.fillAmount;
            float lerpPos = 0;
            
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 0.75f, easingType: Easings.Types.ExpoOut);
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
                var t = Misc.UpdateLerpPos(ref lerpPos, lerpTime, easingType: Easings.Types.QuartOut);
                var res = Mathf.LerpUnclamped(startVal, val, t);
                tmp.text = $"{(int)res}";
                yield return null;
            }
        }
    }
}
