using UnityEngine;

namespace PixelCrew.Components
{
    public class AddSwordComponent : MonoBehaviour
    {
        private Hero _hero;

        private void Start()
        {
            _hero = FindObjectOfType<Hero>();
        }

        public void AddSword()
        {
            _hero.AddSword();
        }
    }
}