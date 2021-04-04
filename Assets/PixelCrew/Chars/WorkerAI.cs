using System;
using System.Collections;
using PixelCrew.Components;
using PixelCrew.Town;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class WorkerAI : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Mob _mob;
        [SerializeField] private Cooldown _attackCooldown;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private bool _isDead;

        private Coroutine _currentTask;

        private Vector2 _moveDirection;
        private Animator _animator;
        private TownController _town;

        [SerializeField] private float _collectCooldown = 0.5f;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _mob = GetComponent<Mob>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            GetComponent<FoF>().SetTeam(_mob.TeamType);
            GetComponent<HealthComponent>().SetHealth(_mob.LevelData.HP);
            FindDestination();

            MoveToDestination();
            UpdateDirection();
            _currentTask = StartCoroutine(Ai());
            gameObject.layer = LayerMask.NameToLayer("Workers" + _mob.TeamType);
        }

        private IEnumerator Ai()
        {
            MoveToDestination();
            while (_moveDirection.x != 0)
            {
                MoveToDestination();
                yield return null;
            }

            _resourceZones[_currentZone].GetComponent<SpriteAnimation>().SetClip("open");
            _animator.SetTrigger("pick");

            yield return new WaitForSeconds(_collectCooldown);
            _town.AddCoins(_mob.LevelData.CoinPerPick);

            _currentZone = (int) Mathf.Repeat(_currentZone + 1, _resourceZones.Length);
            _currentTask = StartCoroutine(Ai());
        }

        private void Update()
        {
            if (_isDead)
            {
                StopCoroutine(_currentTask);
                return;
            }

            _attackCooldown.Tick();

            var isGrounded = _groundCheck.IsTouchingLayer;

            _animator.SetBool("is-ground", isGrounded);
            _animator.SetBool("is-running", _moveDirection.x != 0);
            _animator.SetBool("is-dead", _isDead);
        }

        public void Die()
        {
            _isDead = true;
            _animator.SetBool("is-dead", _isDead);
        }

        private void MoveToDestination()
        {
            _moveDirection = Destination - transform.position;
            if (Mathf.Abs(_moveDirection.x) < 0.2f)
            {
                _moveDirection.x = 0;
            }

            _moveDirection.Normalize();
        }

        private Transform[] _resourceZones;
        private int _currentZone;

        private void FindDestination()
        {
            var towns = FindObjectsOfType<TownController>();
            foreach (var town in towns)
            {
                if (town._teamType == _mob.TeamType)
                {
                    _town = town;
                    _resourceZones = town.GetComponent<ResourceZone>()._destinations;
                    return;
                }
            }

            throw new ArgumentException("Destination for worker not found");
        }

        private Vector3 Destination => _resourceZones[_currentZone].transform.position;

        private void FixedUpdate()
        {
            if (!_groundCheck.IsTouchingLayer) return;

            var velocity = _body.velocity;
            velocity.x = _moveDirection.x * _mob.LevelData.Speed;
            _body.velocity = velocity;

            UpdateDirection();
        }

        private void UpdateDirection()
        {
            var xDirection = _moveDirection.x;
            var scale = transform.localScale;
            if (xDirection > 0)
            {
                scale.x = Mathf.Abs(scale.x) * -1;
            }
            else if (xDirection < 0)
            {
                scale.x = Mathf.Abs(scale.x);
            }

            transform.localScale = scale;
        }
    }
}