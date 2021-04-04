using System;
using PixelCrew.Components;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI
{
    public class LifeBar : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        private HealthComponent _health;
        private int _maxHp;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _maxHp = _health.Hp;
        }

        public void Update()
        {
            _bar.fillAmount = (float) _health.Hp / _maxHp;
        }
    }
}