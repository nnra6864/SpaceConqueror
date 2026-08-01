using UnityEngine;

namespace Player
{
    public class HUD : MonoBehaviour
    {
        // TODO: Implement anims
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
