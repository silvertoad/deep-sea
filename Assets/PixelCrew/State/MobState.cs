using System;

namespace PixelCrew.State
{
    [Serializable]
    public class MobState
    {
        public string Id;
        public IntProperty Level = new IntProperty();
        public IntProperty Count = new IntProperty();
    }
}