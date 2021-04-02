using PixelCrew.Data;
using PixelCrew.State;
using PixelCrew.Town;
using UnityEngine;

namespace PixelCrew.Chars
{
    public class Mob : MonoBehaviour
    {
        private MobState _state;
        private MobData _mobData;
        private TownType _townType;

        public MobState State => _state;
        public MobData MobData => _mobData;
        public TownType TownType => _townType;

        public void Setup(MobState state, MobData mobData, TownType townType)
        {
            _townType = townType;
            _mobData = mobData;
            _state = state;

            var scale = transform.localScale;
            transform.localScale = scale * GetScale().Scale;
        }

        private LevelScale GetScale()
        {
            var levelValue = _state.Level.Value;
            LevelScale levelScale = null;
            foreach (var lvlScale in _mobData.LvlToScale)
            {
                levelScale = lvlScale;
                if (levelScale.Level > levelValue) break;
            }

            return levelScale;
        }
    }
}