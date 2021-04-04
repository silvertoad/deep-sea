using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NReco.Csv;
using PixelCrew.Chars;
using UnityEngine;

namespace PixelCrew.Data
{
    [CreateAssetMenu(fileName = "MobsData", menuName = "Data/MobsData", order = 1)]
    public class MobsData : ScriptableObject
    {
        [SerializeField] public MobData[] _list;

        public MobData Get(string itemDataId)
        {
            return _list.First(x => x.Id == itemDataId);
        }

        [ContextMenu("Update Levels")]
        private void UpdateLevels()
        {
            var reader =
                new CsvReader(new StreamReader(Path.Combine(Application.dataPath, "PixelCrew/Data/levels.csv")), ",");
            var sharky = _list.First(x => x.Id == "sharky");
            var crabby = _list.First(x => x.Id == "crabby");

            var sharkyLevels = new List<LevelData>();
            var crabbyevels = new List<LevelData>();

            var table = new List<List<string>>();
            while (reader.Read())
            {
                var rowData = new List<string>();
                for (int i = 0; i < reader.FieldsCount; i++)
                {
                    rowData.Add(reader[i]);
                }

                table.Add(rowData);
            }

            table.RemoveAt(0);

            for (int i = 0; i < table.Count; i++)
            {
                var tableRow = table[i];
                crabbyevels.Add(Extract(tableRow, 0));
                sharkyLevels.Add(Extract(tableRow, 8));
            }

            sharky.SetLevelData(sharkyLevels.ToArray());
            crabby.SetLevelData(crabbyevels.ToArray());
        }

        private LevelData Extract(List<string> data, int offset)
        {
            var level = new LevelData();
            level.SetData(data.GetRange(offset, 8));
            return level;
        }
    }

    [Serializable]
    public class MobData
    {
        [SerializeField] private Mob _prefab;
        [SerializeField] private string _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private LevelData[] _lvlData;

        public LevelData[] LvlData => _lvlData;

        public Mob Prefab => _prefab;
        public string Id => _id;
        public Sprite Icon => _icon;

        public void SetLevelData(LevelData[] data)
        {
            _lvlData = data;
        }

        public LevelData GetLevelData(int level)
        {
            LevelData levelData = null;
            foreach (var lvlScale in LvlData)
            {
                levelData = lvlScale;
                if (levelData.Level > level) break;
            }

            return levelData;
        }
    }

    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _level;
        [SerializeField] private float _scale = 1;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _hp = 1;
        [SerializeField] private float _upgradeTime = 1;
        [SerializeField] private float _speed = 1;
        [SerializeField] private int _price = 1;
        [SerializeField] private int _coinPerPick;

        public int CoinPerPick => _coinPerPick;

        public int Level => _level;
        public float Scale => _scale;
        public int Damage => _damage;
        public int HP => _hp;
        public float UpgradeTime => _upgradeTime;
        public int Price => _price;
        public float Speed => _speed;

        public void SetData(List<string> data)
        {
            _level = ParseInt(data[0]);
            _upgradeTime = ParseFloat(data[1]);
            _price = ParseInt(data[2]);
            _damage = ParseInt(data[3]);
            _hp = ParseInt(data[4]);
            _scale = ParseFloat(data[5]);
            _coinPerPick = ParseInt(data[6]);
            _speed = ParseFloat(data[7]);
        }

        private int ParseInt(string data)
        {
            if (int.TryParse(data, out var parsed))
            {
                return parsed;
            }

            return default;
        }
        
        private float ParseFloat(string data)
        {
            if (float.TryParse(data, out var parsed))
            {
                return parsed;
            }

            return default;
        }
    }
}