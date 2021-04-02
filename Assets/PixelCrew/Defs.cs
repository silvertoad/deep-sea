using PixelCrew.Data;
using UnityEngine;

namespace PixelCrew
{
    [CreateAssetMenu(fileName = "Defs", menuName = "Data/Defs", order = 1)]
    public class Defs : ScriptableObject
    {
        [SerializeField] public MobsData Mobs;

        private static Defs _instance;
        public static Defs I => _instance == null ? _instance = Resources.Load<Defs>("Defs") : _instance;
    }
}