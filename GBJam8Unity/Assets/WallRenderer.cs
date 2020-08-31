using UnityEngine;
using UnityEngine.Tilemaps;

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

	public void RenderWall(WallTileData wallTileData)
	{
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
	}
}
