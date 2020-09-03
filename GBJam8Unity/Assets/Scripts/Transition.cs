﻿using System;
using UnityEngine;

namespace GBJam8
{
	[Serializable]
	public class Transition
	{
		public Material CircleWipe;

		public void SetTime(float time)
		{
			CircleWipe.SetFloat("_animateTime", Mathf.Clamp01(time));
		}
	}
}