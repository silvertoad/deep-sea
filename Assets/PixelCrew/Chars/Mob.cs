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
		private LevelData _levelData;

		public MobState State => _state;
		public MobData MobData => _mobData;
		public TownType TownType => _townType;

		public void Setup(MobState state, MobData mobData, TownType townType)
		{
			_townType = townType;
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