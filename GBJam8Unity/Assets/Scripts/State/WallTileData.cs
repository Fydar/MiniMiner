using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GBJam8.State
{
	public class WallTileData
	{
		public int WallMaxHealth = 150;
		public int WallHealth = 150;
		public WallTileNode[,] Nodes;
		public WallTileRewardData[] Rewards;

		public bool IsCollapsed
		{
			get
			{
				return WallHealth <= 0;
			}
		}

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
					if (noise > 0.5f)
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

			var rewards = new List<WallTileRewardData>();
			var orderedRewards = Game.Instance.Setup.Rewards
				.OrderByDescending(reward => reward.Footprint)
				.ToArray();

			foreach (var rewardType in orderedRewards)
			{
				float roll = Random.value;
				if (roll < rewardType.ChanceOfApperence)
				{
					for (int k = 0; k < rewardType.MinimumGems; k++)
					{
						for (int i = 0; i < 1000; i++)
						{
							int x = Random.Range(2, 21 - rewardType.Width);
							int y = Random.Range(2, 15 - rewardType.Height);

							bool canPlace = true;
							bool isCovered = false;
							foreach (var digPosition in rewardType.DigPositions)
							{
								var nodePosition = new Vector2Int(x + digPosition.Offset.x, y + digPosition.Offset.y);

								var current = wallTile.Nodes[nodePosition.x, nodePosition.y];
								if (current.Layers.Rock)
								{
									canPlace = false;
									break;
								}
								if (!isCovered
									&& (current.Layers.Gravel
									|| current.Layers.Surface))
								{
									isCovered = true;
								}
							}

							var rewardToAdd = new WallTileRewardData()
							{
								Type = rewardType,
								Offset = new Vector2Int(x, y),
								Claimed = false,
							};

							foreach (var reward in rewards)
							{
								if (rewardToAdd.Volume.Overlaps(reward.Volume))
								{
									canPlace = false;
									break;
								}
							}

							if (isCovered && canPlace)
							{
								rewards.Add(rewardToAdd);
								break;
							}
						}
					}
				}
			}
			wallTile.Rewards = rewards.ToArray();

			return wallTile;
		}
	}
}