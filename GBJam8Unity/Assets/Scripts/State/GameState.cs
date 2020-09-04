using System;
using System.Collections.Generic;
using UnityEngine;

namespace GBJam8.State
{
	[Serializable]
	public class PlayerState
	{
		public int Money;
		public Vector2 Position;
		public Vector2Int Heading;
		public Dictionary<string, EquipmentState> Equipment;

		private SceneSetup setup;

		public bool HasEquipment
		{
			get
			{
				foreach (var equipment in Equipment)
				{
					if (equipment.Value.Level > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public PlayerState(SceneSetup setup)
		{
			this.setup = setup;

			Equipment = new Dictionary<string, EquipmentState>();
			foreach (var equipment in setup.Equipment)
			{
				Equipment[equipment.Identifier] = new EquipmentState()
				{
					Level = equipment.StartingLevel
				};
			}
		}
	}

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
				wallTile = WallTileData.GenerateBasic();
				WallTiles[asString] = wallTile;
			}
			return wallTile;
		}
	}
}