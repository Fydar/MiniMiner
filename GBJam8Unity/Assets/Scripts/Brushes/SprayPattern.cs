using System;
using UnityEngine;

[Serializable]
public struct SprayEntry
{
	public Vector2Int Offset;
	public int Weight;
}

[Serializable]
public class SprayPattern
{
	public SprayEntry[] Sprays;

	public SprayEntry GetRandomSpray()
	{
		if (Sprays.Length == 1)
		{
			return Sprays[0];
		}

		int totalChance = 0;
		int accumulator = 0;
		foreach (var track in Sprays)
		{
			totalChance += track.Weight;
		}
		int randomValue = UnityEngine.Random.Range(0, totalChance);
		foreach (var track in Sprays)
		{
			accumulator += track.Weight;
			if (randomValue < accumulator)
			{
				return track;
			}
		}
		throw new InvalidOperationException("Something went wrong!");
	}
}
