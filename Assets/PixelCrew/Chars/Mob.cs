using PixelCrew.Data;
using PixelCrew.State;
using PixelCrew.Town;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class Mob : MonoBehaviour
    {
        [SerializeField] private MobState _state;
        [SerializeField] private MobData _mobData;
        [SerializeField] private TeamType _teamType;
        [SerializeField] private LevelData _levelData;

        public MobState State => _state;
        public MobData MobData => _mobData;
        public TeamType TeamType => _teamType;

        public LevelData LevelData => _levelData;
        public TeamType EnemiesTeam => _teamType == TeamType.A ? TeamType.B : TeamType.A;

        public void Setup(MobState state, MobData mobData, TeamType teamType)
        {
            _teamType = teamType;
            _mobData = mobData;
            _state = state;
            _levelData = _mobData.GetLevelData(_state.Level.Value);

            var transform1 = transform;
            transform1.localScale = transform1.localScale * _levelData.Scale;
        }
    }
}