using System;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private float _slamDownVelocity;

        [Space] [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private LayerMask _interactionLayer;

        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _slamDownParticle;
        [SerializeField] private SpawnComponent _jumpParticle;
        [SerializeField] private ParticleSystem _hitParticles;

        [Space] [Header("Battle")] [SerializeField]
        private Transform _attackPoint;

        [SerializeField] private float _attackCheckRadius = 0.3f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _damagePushForce = 1;

        private readonly Collider2D[] _interactionResult = new Collider2D[5];
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int HasSword = Animator.StringToHash("has-sword");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private int _coins;
        private int _swordCount;
        private float _minVelocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
            _animator.SetBool(HasSword, _swordCount > 0);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _slamDownParticle.Spawn();
                }
            }
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunning, _direction.x != 0);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }

        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity = _jumpSpeed;
                _jumpParticle.Spawn();
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _allowDoubleJump = false;
                _jumpParticle.Spawn();
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private bool IsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius,
                Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);

            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(_attackPoint.position, Vector3.forward, _attackCheckRadius);
        }

        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        public void AddCoins(int coins)
        {
            _coins += coins;
            Debug.Log($"{coins} coins added. total coins: {_coins}");
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);

            if (_coins > 0)
            {
                // SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(_coins, 5);
            _coins -= numCoinsToDispose;

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticles.emission.SetBurst(0, burst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }

        public void AddSword()
        {
            _swordCount++;
        }

        public void Attack()
        {
            if (_swordCount <= 0) return;

            _animator.SetTrigger(AttackKey);
        }

        public void CheckAttackDamage()
        {
            CircleCast(_attackPoint.position, _attackCheckRadius, enemy =>
            {
                if (!enemy.gameObject.CompareTag("Enemy")) return;
                var health = enemy.GetComponent<HealthComponent>();
                if (health == null) return;

                health.Modify(-_damage);

                var body = enemy.gameObject.GetComponent<Rigidbody2D>();
                var direction = enemy.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                body.AddForce(direction * _damagePushForce, ForceMode2D.Impulse);
            });
        }

        private void CircleCast(Vector2 pos, float radius, Action<Collider2D> success, LayerMask? mask = null)
        {
            var size = mask == null
                ? Physics2D.OverlapCircleNonAlloc(pos, radius, _interactionResult)
                : Physics2D.OverlapCircleNonAlloc(pos, radius, _interactionResult, _interactionLayer);

            for (var i = 0; i < size; i++)
            {
                success.Invoke(_interactionResult[i]);
            }
        }
    }
}