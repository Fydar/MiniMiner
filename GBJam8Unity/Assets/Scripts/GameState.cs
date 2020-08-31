using System.Collections.Generic;
using UnityEngine;

public class GameState
{
	public Dictionary<Vector3Int, WallTileData> WallTiles { get; }

	public GameState()
	{
		WallTiles = new Dictionary<Vector3Int, WallTileData>();
	}

	public WallTileData GetOrGenerate(Vector3Int position)
	{
		if (!WallTiles.TryGetValue(position, out var wallTile))
		{
			wallTile = WallTileData.GenerateBasic();
			WallTiles[position] = wallTile;
		}
		return wallTile;
	}
}
