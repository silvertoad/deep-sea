using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.Town;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class FighterAI : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Mob _mob;
        private Vector3 _destination;
        [SerializeField] private Cooldown _attackCooldown;
        [SerializeField] private LayerCheck _groundCheck;
        [Space] [SerializeField] private Transform _rayOffset;
        [SerializeField] private LayerMask _enemiesLayer;

        private Coroutine _currentTask;
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[5];

        private Vector2 _moveDirection;
        private Animator _animator;
        private readonly List<HealthComponent> _enemies = new List<HealthComponent>();
        [SerializeField] private float _attackThinkTime = 0.1f;
        private HealthComponent _health;
        private AudioSource _hitSound;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _mob = GetComponent<Mob>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<HealthComponent>();
            _hitSound = GetComponent<AudioSource>();
        }

        private void Start()
        {
            GetComponent<FoF>().SetTeam(_mob.TeamType);
            GetComponent<HealthComponent>().SetHealth(_mob.LevelData.HP);
            _destination = FindDestination();
            MoveToDestination();
            UpdateDirection();
            gameObject.layer = LayerMask.NameToLayer("Enemies" + _mob.TeamType);
            var enemiesName = _mob.EnemiesTeam;
            _enemiesLayer = LayerMask.GetMask(
                "Enemies" + enemiesName, "Workers" + enemiesName);
        }

        private void Update()
        {
            if (_health.IsDead) return;

            _attackCooldown.Tick();

            _moveDirection = Vector2.zero;
            var isGrounded = _groundCheck.IsTouchingLayer;

            if (isGrounded)
            {
                var enemies = GetEnemies();
                var hasEnemies = enemies.Count > 0;
                if (hasEnemies)
                {
                    if (_attackCooldown.IsReady)
                    {
                        StartCoroutine(AttackMode(enemies));
                    }
                }
                else
                {
                    MoveToDestination();
                }
            }

            _animator.SetBool("is-ground", isGrounded);
            _animator.SetBool("is-running", _moveDirection.x != 0);
            _animator.SetBool("is-dead", _health.IsDead);
        }

        private List<HealthComponent> GetEnemies()
        {
            _enemies.Clear();
            var distance = Vector3.Distance(transform.position, _rayOffset.position);
            var direction = (_rayOffset.position - transform.position).normalized;
            var size = Physics2D.RaycastNonAlloc(transform.position, direction, _hits, distance,
                _enemiesLayer);
            for (var i = 0; i < size; i++)
            {
                var hitCollider = _hits[i].collider;
                var fof = hitCollider.GetComponent<FoF>();
                if (fof == null || !fof.CanAttack(_mob.TeamType)) continue;

                var hp = hitCollider.GetComponent<HealthComponent>();
                if (hp == null || hp.Hp <= 0) continue;

                _enemies.Add(hp);
            }

            return _enemies;
        }

        private IEnumerator AttackMode(List<HealthComponent> enemies)
        {
            _attackCooldown.Reset();
            _animator.SetTrigger("attack");
            _hitSound.Play();
            yield return new WaitForSeconds(_attackThinkTime);
            enemies.ForEach(x => x.Modify(-_mob.LevelData.Damage));
        }

        public void Die()
        {
            _animator.SetBool("is-dead", _health.IsDead);
        }

        private void MoveToDestination()
        {
            _moveDirection = _destination - transform.position;
            _moveDirection.Normalize();
        }

        private Vector3 FindDestination()
        {
            var towns = FindObjectsOfType<TownController>();
            foreach (var town in towns)
            {
                if (town._teamType != _mob.TeamType)
                {
                    return town.DestinationPoint.position;
                }
            }

            throw new ArgumentException("Destination for fighter not found");
        }

        private void FixedUpdate()
        {
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
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = _enemies.Count > 0 ? Color.green : Color.blue;
            Handles.DrawLine(transform.position, _rayOffset.position);
        }
  #endif
  }

    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _cooldown = 1;
        private float _timer;

        public void Tick()
        {
            _timer -= Time.deltaTime;
        }

        public void Reset()
        {
            _timer = _cooldown;
        }

        public bool IsReady => _timer <= 0;
    }
}