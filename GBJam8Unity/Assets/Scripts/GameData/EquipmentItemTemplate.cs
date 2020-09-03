using UnityEngine;

namespace GBJam8
{
	[CreateAssetMenu]
	public class EquipmentItemTemplate : ScriptableObject
	{
		public string Identifier;
		public string DisplayName;

		[Space]
		public int StartingLevel = -1;
		public EquipmentItemLevelData[] Levels = new EquipmentItemLevelData[1];
	}
}