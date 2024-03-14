using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniMinerUnity.State
{
	[Serializable]
	public class PlayerState
	{
		public int Money;
		public Vector2 Position;
		public Vector2Int Heading;
		public Dictionary<RewardType, int> Bag;
		public Dictionary<string, EquipmentState> Equipment;
		public int SelectedEquipment;

		public int MaxBagCapacity = 16;
		public int BagCapacity
		{
			get
			{
				int total = 0;
				foreach (var item in Bag)
				{
					total += item.Value;
				}
				return total;
			}
		}

		private readonly SceneSetup setup;

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
			Bag = new Dictionary<RewardType, int>();
			foreach (var equipment in setup.Equipment)
			{
				Equipment[equipment.Identifier] = new EquipmentState()
				{
					Level = equipment.StartingLevel
				};
			}
		}
	}
}
