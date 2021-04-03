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

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _mob = GetComponent<Mob>();
        }

        private void Start()
        {
            _destination = FindDestination();
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
            var direction = _destination - transform.position;
            direction.Normalize();
            UpdateDirection(direction.x);
            _body.velocity = new Vector2(direction.x * _speed, _body.velocity.y);
        }

        private void UpdateDirection(float xDirection)
        {
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