using System;
using UnityEngine;

[Serializable]
public class NoiseComponent
{
	public bool UseNoise;
	public float Intencity;
	public float Frequency;

	public bool UseFalloff;
	public AnimationCurve Falloff;
}

[Serializable]
public struct CharacterAnimations
{
	public char Character;
	public float PostDelay;
}

[Serializable]
public class AnimationComponent
{
	public int CharactersPerTick = 1;
	public float DelayPerTick = 0.1f;

	public CharacterAnimations[] Exceptions;
}

[Serializable]
public class AudioComponent
{
	public LoopGroup talkingLoop;
}

[CreateAssetMenu]
public class TextStyle : ScriptableObject
{
	public Font Font;
	public int FontSize = 7;
	public Color color = Color.white;

	[Space]
	public TextAnchor Alignment = TextAnchor.UpperLeft;

	[Space]
	public AudioComponent Audio;
	public NoiseComponent Shake;
	public AnimationComponent Animation;
}
