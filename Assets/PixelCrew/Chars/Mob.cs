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

        public void Setup(MobState state, MobData mobData, TeamType teamType)
        {
            _teamType = teamType;
            _mobData = mobData;
            _state = state;
            _levelData = GetLevelData();

            var transform1 = transform;
            transform1.localScale = transform1.localScale * _levelData.Scale;
        }

        private LevelData GetLevelData()
        {
            var levelValue = _state.Level.Value;
            LevelData levelData = null;
            foreach (var lvlScale in _mobData.LvlData)
            {
                levelData = lvlScale;
                if (levelData.Level > levelValue) break;
            }

            return levelData;
        }
    }
}