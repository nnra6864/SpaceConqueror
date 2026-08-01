using System;
using Core;
using UnityEngine;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;

        public void Play()
        {
            if (!GameManager.Playing) GameManager.Play();
            else GameManager.Resume();
        }

        public void Restart()
        {
            //TODO: Implement Restart
        }

        public void Quit() => Application.Quit();

        // TODO: Implement anims
        public void Show()
        {
            _group.alpha = 1;
        }

        public void Hide()
        {
            _group.alpha = 0;
        }
    }
}
