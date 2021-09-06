using UnityEngine;

namespace GBJam8.Brushes
{
	[CreateAssetMenu(menuName = "Mining/Brush")]
	public class MiningBrush : ScriptableObject
	{
		public int MinimumWallDamage = 10;
		public int MaximumWallDamage = 20;

		[Header("Excavation")]
		public float[] Digs = new float[1];
		public SprayPattern SprayPattern;
		public DiggingPattern DiggingPattern;

		[Header("Effects")]
		public SfxGroup HitSound;
		public float ShakeIntencity = 1.0f;
		public int HitParticles = 60;
	}
}
