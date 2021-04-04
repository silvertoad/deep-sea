using DG.Tweening;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class DieAnimationComponent : MonoBehaviour
    {
        [SerializeField] private float _bubbleTime = 10f;
        [SerializeField] private float _disappear = 5f;
        private HealthComponent _health;
        private SpriteRenderer _sprite;
        private Transform _bubble;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            _health = GetComponent<HealthComponent>();
            _bubble = GameObject.FindWithTag("bubble").transform;
            _health._onDie.AddListener(OnDie);
        }

        private void OnDie()
        {
            transform.DOMoveY(_bubble.transform.position.y, _bubbleTime)
                .OnComplete(() =>
                {
                    var targetColor = _sprite.color;
                    targetColor.a = 0;
                    _sprite.DOColor(targetColor, _disappear).OnComplete(() => { Destroy(gameObject); });
                });
        }
    }
}