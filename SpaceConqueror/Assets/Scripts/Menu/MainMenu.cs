using System;
using Core;
using UnityEngine;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _hud;
        [SerializeField] private GameObject _health, _energy, _score;
        
        public void Play()
        {
            gameObject.SetActive(false);
            if (GameManager.Player) return;
            Instantiate(_player);
            _hud.SetActive(true);
        }

        public void Quit() => Application.Quit();

        private void OnEnable()
        {
            _health.SetActive(false);
            _energy.SetActive(false);
        }

        private void OnDisable()
        {
            _health.SetActive(true);
            _energy.SetActive(true);
            _score.SetActive(true);
        }
    }
}
