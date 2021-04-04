using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew
{
    public class PlayerInputComponent : MonoBehaviour
    {
        [SerializeField] private KeyCode _left;
        [SerializeField] private KeyCode _right;
        [SerializeField] private KeyCode _use;
        private float _holdTime = 0.2f;
        private float _holdTimer;

        [SerializeField] private UnityEvent _onLeft;
        [SerializeField] private UnityEvent _onRight;
        [SerializeField] private UnityEvent _onUse;
        [SerializeField] private UnityEvent _onHoldStart;
        [SerializeField] private UnityEvent _onHoldComplete;

        private bool _startHold;

        private void Update()
        {
            if (Input.GetKeyUp(_left))
            {
                _onLeft.Invoke();
            }

            if (Input.GetKeyUp(_right))
            {
                _onRight.Invoke();
            }

            if (Input.GetKeyDown(_use))
            {
                if (!_startHold)
                {
                    _holdTimer = _holdTime;
                    _startHold = true;
                }
            }

            if (_startHold)
            {
                if (_holdTimer < 0)
                {
                    _onHoldStart.Invoke();
                }
            }

            if (Input.GetKeyUp(_use))
            {
                _startHold = false;
                if (_holdTimer < 0)
                {
                    _onHoldComplete.Invoke();
                }
                else
                {
                    _onUse.Invoke();
                }
            }

            _holdTimer -= Time.deltaTime;
        }
    }
}