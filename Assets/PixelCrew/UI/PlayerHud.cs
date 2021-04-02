using PixelCrew.Town;
using PixelCrew.UI.Mobs;
using UnityEngine;

namespace PixelCrew.UI
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private TownController _town;
        [Space] [SerializeField] private MobList _mobList;
        [SerializeField] private SpellList _spellList;

        private void Start()
        {
            _mobList.SetData(_town.State.Mobs);
        }
    }
}