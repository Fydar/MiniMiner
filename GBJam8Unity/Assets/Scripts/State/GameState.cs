using System.Collections.Generic;
using UnityEngine;

namespace GBJam8.State
{
	public class GameState
	{
		public Dictionary<string, WallTileData> WallTiles;
		public Dictionary<string, EquipmentState> Equipment;

		public GameState()
		{
			WallTiles = new Dictionary<string, WallTileData>();
			Equipment = new Dictionary<string, EquipmentState>();
		}

		public WallTileData GetOrGenerate(Vector3Int position)
		{
			string asString = $"{position.x},{position.y},{position.z}";
			if (!WallTiles.TryGetValue(asString, out var wallTile))
			{
				wallTile = WallTileData.GenerateBasic();
				WallTiles[asString] = wallTile;
			}
			return wallTile;
		}
	}
}