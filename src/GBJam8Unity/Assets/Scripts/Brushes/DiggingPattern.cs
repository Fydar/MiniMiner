using System;
using UnityEngine;

namespace GBJam8.Brushes
{
	[Serializable]
	public struct DiggingEntry
	{
		public Vector2Int Offset;
		public float Chance;
		public float LayerFudge;
	}

	[Serializable]
	public class DiggingPattern
	{
		public DiggingEntry[] Entries;
		public int MinimumDigs;
		public int MaximumDigs;

		[Tooltip("Smart brushes avoid wasting digs on empty tiles.")]
		public bool IsSmartBrush;
	}
}