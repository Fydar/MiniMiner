using UnityEngine;

namespace MiniMinerUnity.State
{
    public struct WallTileRewardData
    {
        public RewardType Type;
        public Vector2Int Offset;
        public bool Claimed;

        public RectInt Volume => new(Offset, new Vector2Int(Type.Width, Type.Height));
    }
}