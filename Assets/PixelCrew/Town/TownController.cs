using System.Linq;
using PixelCrew.State;
using UnityEngine;

namespace PixelCrew.Town
{
    public class TownController : MonoBehaviour
    {
        [SerializeField] public TownState _state;
        [SerializeField] public Transform _spawnPoint;
        [SerializeField] public TeamType _teamType;
        [SerializeField] public Transform _destinationPoint;

        public TownState State => _state;

        public Transform SpawnPoint => _spawnPoint;

        public TeamType TeamType => _teamType;

        public Transform DestinationPoint => _destinationPoint;

        private void Awake()
        {
            if (_state.Mobs?.Length == 0)
            {
                _state.Mobs = Defs.I.Mobs._list.Select(x => new MobState
                {
                    Id = x.Id
                }).ToArray();
            }
            
            Spawn("crabby");
        }

        public void Spawn(string id)
        {
            var state = _state.Mobs.First(x => x.Id == id);
            var def = Defs.I.Mobs.Get(id);
            var price = def.GetLevelData(state.Level.Value).Price;
            if (price > _state.Coins.Value) return;

            _state.Coins.Value -= price;
            state.Count.Value++;
            var mob = Instantiate(def.Prefab, _spawnPoint.position, Quaternion.identity);
            mob.Setup(state, def, _teamType);
        }

        public void AddCoins(int coins)
        {
            _state.Coins.Value += coins;
        }
    }

    public enum TeamType
    {
        A,
        B,
        NA
    }
}