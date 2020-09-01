using System;
using UnityEngine;

[Serializable]
public class Transition
{
	public Material CircleWipe;

	public void SetTime(float time)
	{
		CircleWipe.SetFloat("_animateTime", Mathf.Clamp01(time));
	}
}
