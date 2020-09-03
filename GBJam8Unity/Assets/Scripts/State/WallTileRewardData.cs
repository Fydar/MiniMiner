using UnityEngine;

namespace GBJam8.State
{
	public struct WallTileRewardData
	{
		public RewardType Type;
		public Vector2Int Offset;
		public bool Claimed;

		public RectInt Volume => new RectInt(Offset, new Vector2Int(Type.Width, Type.Height));
	}
}