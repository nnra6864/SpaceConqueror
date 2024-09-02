using Core;
using UnityEngine;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _hud;
        
        public void Play()
        {
            gameObject.SetActive(false);
            if (GameManager.Player) return;
            Instantiate(_player);
            _hud.SetActive(true);
        }

        public void Quit() => Application.Quit();
    }
}
