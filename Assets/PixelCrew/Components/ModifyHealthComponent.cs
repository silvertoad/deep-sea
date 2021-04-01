using UnityEngine;

namespace PixelCrew.Components
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _healthChange;

        public void ApplyChange(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.Modify(_healthChange);
            }
        }
    }
}