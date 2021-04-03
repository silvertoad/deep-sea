using System;
using PixelCrew.Town;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class FighterAI : MonoBehaviour
    {
        private Rigidbody2D _body;
        private Mob _mob;
        private Vector3 _destination;
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _attackCooldown = 1;
        [SerializeField] private TagCheck _attackRange;
        [SerializeField] private LayerCheck _groundCheck;

        private Coroutine _currentTask;

        private Vector2 _moveDirection;
        private SpriteAnimation _animation;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _mob = GetComponent<Mob>();
            _animation = GetComponent<SpriteAnimation>();
        }

        private void Start()
        {
            _destination = FindDestination();
        }

        private void Update()
        {
            _moveDirection = Vector2.zero;
            var isGrounded = _groundCheck.IsTouchingLayer;

            if (isGrounded)
            {
                if (_attackRange.IsInAttackRange)
                {
                    AttackMode();
                }
                else
                {
                    MoveToDestination();
                }
            }
            else
            {
                _animation.SetClip("in-air");
            }
        }

        private void AttackMode()
        {
            _animation.SetClip("attack");
        }

        public void OnAttack()
        {
            var enemies = _attackRange.GetEnemies();
        }

        public void AttackCooldown()
        {
        }

        private void MoveToDestination()
        {
            _animation.SetClip("run");
            _moveDirection = _destination - transform.position;
            _moveDirection.Normalize();
        }

        private Vector3 FindDestination()
        {
            var towns = FindObjectsOfType<TownController>();
            foreach (var town in towns)
            {
                if (town._townType != _mob.TownType)
                    return town.DestinationPoint.position;
            }

            throw new ArgumentException("Destination for fighter not found");
        }

        private void FixedUpdate()
        {
            var velocity = _body.velocity;
            velocity.x = _moveDirection.x * _speed;
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