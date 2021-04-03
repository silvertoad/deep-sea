using System;
using System.Linq;
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
    }

    [Serializable]
    public class LevelData
    {
        [SerializeField] private int _level;
        [SerializeField] private float _scale = 1;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _hp = 1;

        public int Level => _level;
        public float Scale => _scale;
        public int Damage => _damage;
        public int HP => _hp;
    }
}