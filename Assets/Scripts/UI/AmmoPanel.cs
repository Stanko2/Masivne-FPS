using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoPanel : MonoBehaviour
    {
        public Text remainingText;
        private PlayerShoot _player;
        private int _maxAmmo;
        
        public void Initialize(PlayerShoot player)
        {
            _player = player;
            _maxAmmo = player.Gun.ammoCount;
            UpdateAmmo(_maxAmmo);
            player.OnShoot += UpdateAmmo;
        }

        private void UpdateAmmo(int ammoCount)
        {
            if (ammoCount == -1)
                ammoCount = _maxAmmo;
            remainingText.text = $"{ammoCount} / {_maxAmmo}";
        }
    }
}