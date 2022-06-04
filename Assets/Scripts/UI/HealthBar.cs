using System;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        public Slider healthSlider;
        public Text healthText;
        private float _maxHealth;

        public void Initialize(PlayerHealth player)
        {
            _maxHealth = player.maxHealth;
            GetComponent<CanvasGroup>().alpha = 1;
            healthSlider.maxValue = _maxHealth;
            player.OnHealthChange += DisplayHealth;
            DisplayHealth(_maxHealth);
        }

        private void DisplayHealth(float health)
        {
            if (health <= 0)
                GetComponent<CanvasGroup>().alpha = 0;
            healthSlider.value = health;
            healthText.text = $"{health}/{_maxHealth}";
        }
    }
}