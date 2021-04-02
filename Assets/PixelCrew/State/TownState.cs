using System;
using PixelCrew.State;
using UnityEngine;

namespace PixelCrew.Town
{
    [Serializable]
    public class TownState
    {
        [SerializeField] public IntProperty Coins = new IntProperty();
        [SerializeField] public MobState[] Mobs = new MobState[0];
    }
}