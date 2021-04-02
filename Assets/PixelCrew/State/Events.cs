using System;
using UnityEngine.Events;

namespace PixelCrew.State
{
    public class Events
    {
        [Serializable]
        public class StringEvent : UnityEvent<string>
        {
        }
    }
}