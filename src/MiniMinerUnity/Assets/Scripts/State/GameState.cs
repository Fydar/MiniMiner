using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniMinerUnity.State
{
	[Serializable]
	public class GameState
	{
		public PlayerState Player;
		public Dictionary<string, WallTileData> WallTiles;

		public GameState(SceneSetup setup)
		{
			Player = new PlayerState(setup);
			WallTiles = new Dictionary<string, WallTileData>();
		}

		public WallTileData GetOrGenerate(Vector3Int position)
		{
			string asString = $"{position.x},{position.y},{position.z}";
			if (!WallTiles.TryGetValue(asString, out var wallTile))
			{
				wallTile = WallTileData.GenerateBasic(
					Game.Instance.Setup.RarityMap.GetTile(position).name
				);
				WallTiles[asString] = wallTile;
			}
			return wallTile;
		}
	}
}