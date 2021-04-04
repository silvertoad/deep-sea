using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] public UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        public int Hp => _health;
        public bool IsDead => _health <= 0;

        public void Modify(int changeValue)
        {
            _health += changeValue;
            _onDamage?.Invoke();
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        public void SetHealth(int health)
        {
            _health = health;
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            Modify(-_health);
        }
    }
}