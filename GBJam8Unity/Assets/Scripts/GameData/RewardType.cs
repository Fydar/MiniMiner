﻿using System.Linq;
using UnityEngine;

namespace GBJam8
{
	[CreateAssetMenu]
	public class RewardType : ScriptableObject
	{
		public int CurrencyValue;
		public int StarRating;
		public string DisplayName;
		public Sprite IconGraphic;
		public Sprite MiningGraphic;
		public SfxGroup FindSound;
		public RewardDigPosition[] DigPositions;

		[Space]
		[Range(0.0f, 1.0f)]
		public float ChanceOfApperence = 1.0f;
		public int MinimumGems = 1;

		public int Width
		{
			get
			{
				return DigPositions.Select(dig => dig.Offset.x).Max() + 1;
			}
		}

		public int Height
		{
			get
			{
				return DigPositions.Select(dig => dig.Offset.y).Max() + 1;
			}
		}

		public int Footprint => Width * Height;
	}
}