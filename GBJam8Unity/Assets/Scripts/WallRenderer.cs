using GBJam8.State;
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

			for (int x = 0; x < 22; x++)
			{
				for (int y = 0; y < 16; y++)
				{
					LayerBedrock.SetTile(new Vector3Int(x, y, 0), TileBedrock);

					var node = wallTileData.Nodes[x, y];
					if (node.Layers.Rock)
					{
						LayerRock.SetTile(new Vector3Int(x, y, 0), TileRock);
					}
					if (node.Layers.Gravel)
					{
						LayerGravel.SetTile(new Vector3Int(x, y, 0), TileGravel);
					}
					if (node.Layers.Surface)
					{
						LayerSurface.SetTile(new Vector3Int(x, y, 0), TileSurface);
					}
				}
			}

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
	}
}