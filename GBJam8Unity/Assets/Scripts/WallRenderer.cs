using GBJam8.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GBJam8
{
	public class WallRenderer : MonoBehaviour
	{
		public Tilemap LayerBedrock;
		public Tilemap LayerRock;
		public Tilemap LayerGravel;
		public Tilemap LayerSurface;

		public TileBase TileBedrock;
		public TileBase TileRock;
		public TileBase TileGravel;
		public TileBase TileSurface;

		public float Frequency = 0.01f;

		[Header("Rewards")]
		public Transform RewardHolder;
		public SpriteRendererPool RewardPool;

		public void RenderWall(WallTileData wallTileData)
		{
			RewardPool.Flush();

			LayerBedrock.ClearAllTiles();
			LayerRock.ClearAllTiles();
			LayerGravel.ClearAllTiles();
			LayerSurface.ClearAllTiles();

			var bedrockPositions = new List<Vector3Int>();
			var rockPositions = new List<Vector3Int>();
			var gravelPositions = new List<Vector3Int>();
			var surfacePositions = new List<Vector3Int>();

			for (int x = 0; x < 22; x++)
			{
				for (int y = 0; y < 16; y++)
				{
					var pos = new Vector3Int(x, y, 0);

					bedrockPositions.Add(pos);

					var node = wallTileData.Nodes[x, y];
					if (node.Layers.Rock)
					{
						rockPositions.Add(pos);
					}
					if (node.Layers.Gravel)
					{
						gravelPositions.Add(pos);
					}
					if (node.Layers.Surface)
					{
						surfacePositions.Add(pos);
					}
				}
			}

			LayerBedrock.SetTiles(bedrockPositions.ToArray(), AsTileMap(TileBedrock, bedrockPositions));
			LayerRock.SetTiles(rockPositions.ToArray(), AsTileMap(TileRock, rockPositions));
			LayerGravel.SetTiles(gravelPositions.ToArray(), AsTileMap(TileGravel, gravelPositions));
			LayerSurface.SetTiles(surfacePositions.ToArray(), AsTileMap(TileSurface, surfacePositions));

			foreach (var reward in wallTileData.Rewards)
			{
				if (reward.Claimed)
				{
					continue;
				}

				var clone = RewardPool.Grab(RewardHolder);

				clone.transform.localPosition = new Vector3(reward.Offset.x * 0.5f, reward.Offset.y * 0.5f, 0.0f);

				clone.sprite = reward.Type.MiningGraphic;
			}
		}

		TileBase[] AsTileMap(TileBase baseTile, List<Vector3Int> positions)
		{
			var array = new TileBase[positions.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = baseTile;
			}
			return array;
		}
	}
}