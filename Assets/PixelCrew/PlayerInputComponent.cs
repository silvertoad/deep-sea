using System;
using UnityEngine;

namespace PixelCrew
{
    public class PlayerInputComponent : MonoBehaviour
    {
        [SerializeField] private KeyCode _left;
        [SerializeField] private KeyCode _right;
        [SerializeField] private KeyCode _use;

        private void Update()
        {
        }
    }
}