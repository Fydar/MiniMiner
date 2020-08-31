using System;
using UnityEngine;

[Serializable]
public struct DiggingEntry
{
	public Vector2Int Offset;
	public float Chance;
}

[Serializable]
public class DiggingPattern
{
	public DiggingEntry[] Entries;
}
