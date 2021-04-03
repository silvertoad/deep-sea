using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class TagCheck : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private Vector3 _rayOffset;

        public bool IsInAttackRange { get; private set; }
        public readonly List<GameObject> Enemies = new List<GameObject>();
        
        private RaycastHit2D[] _hits = new RaycastHit2D[5];
        private int _size;

        private void Update()
        {
            _size = Physics2D.RaycastNonAlloc(transform.position, _rayOffset, _hits, _rayOffset.magnitude);
            IsInAttackRange = false;
            for (var i = 0; i < _size; i++)
            {
                var hitCollider = _hits[i].collider;
                var isInAttackRange = hitCollider.CompareTag(_tag);
                IsInAttackRange |= isInAttackRange;
                if (isInAttackRange)
                {
                    
                }
            }
        }

        public List<GameObject> GetEnemies()
        {
            return new List<GameObject>();
        }
    }
}