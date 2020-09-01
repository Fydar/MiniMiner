using UnityEngine;

public class WallTileData
{
	public WallTileNode[,] Nodes;

	public WallTileData()
	{
		Nodes = new WallTileNode[22, 16];
	}

	public static WallTileData GenerateBasic()
	{
		var wallTile = new WallTileData();

		var offset = new Vector2(Random.value, Random.value);
		for (int x = 0; x < 22; x++)
		{
			for (int y = 0; y < 16; y++)
			{
				float noise = Mathf.PerlinNoise((x + offset.x) * 0.3f, (y + offset.y) * 0.3f);
				if (noise > 0.6f)
				{
					wallTile.Nodes[x, y].Layers.Rock = true;
				}
			}
		}

		offset = new Vector2(Random.value * 1000, Random.value);
		for (int x = 0; x < 22; x++)
		{
			for (int y = 0; y < 16; y++)
			{
				float noise = Mathf.PerlinNoise((x + offset.x) * 0.25f, (y + offset.y) * 0.25f);
				if (noise > 0.2f)
				{
					wallTile.Nodes[x, y].Layers.Gravel = true;
				}
				if (noise > 0.35f)
				{
					wallTile.Nodes[x, y].Layers.Surface = true;
				}
			}
		}

		return wallTile;
	}
}
