using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class ToggleBoolComponent : MonoBehaviour
    {
        [SerializeField] private BoolAction _action;
        [SerializeField] private bool _state;

        public void Toggle()
        {
            _state = !_state;
            _action?.Invoke(_state);
        }

        [Serializable]
        public class BoolAction : UnityEvent<bool>
        {
        }
    }
}