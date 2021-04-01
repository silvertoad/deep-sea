using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectUtils
    {
        public static bool IsInLayer(this GameObject go, LayerMask mask)
        {
            return mask == (mask | (1 << go.layer));
        }
    }
}