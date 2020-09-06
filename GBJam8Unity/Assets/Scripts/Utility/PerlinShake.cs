using UnityEngine;

namespace GBJam8
{
	public class PerlinShake : MonoBehaviour
	{
		public float duration = 0.5f;
		public float frequency = 1.0f;
		public float magnitude = 0.1f;

		public AnimationCurve falloff;

		private float elapsed;
		private float intensity;

		private void Start()
		{
			intensity = 0.0f;
			elapsed = 1000000.0f;
		}

		public void PlayShake(float intensity)
		{
			this.intensity = intensity;
			elapsed = 0.0f;
		}

		private void Update()
		{
			var originalCamPos = transform.localPosition;

			elapsed += Time.deltaTime;

			float percentComplete = elapsed / duration;
			percentComplete = falloff.Evaluate(Mathf.Clamp01(percentComplete));

			float alpha = percentComplete * frequency * intensity;

			float x = (Mathf.PerlinNoise(alpha, 0) * 2.0f) - 1.0f;
			float y = (Mathf.PerlinNoise(0, alpha) * 2.0f) - 1.0f;

			x *= percentComplete * magnitude * intensity;
			y *= percentComplete * magnitude * intensity;

			transform.localPosition = new Vector3(
				Mathf.Round(x * 16.0f) / 16.0f,
				Mathf.Round(y * 16.0f) / 16.0f,
				originalCamPos.z);
		}
	}
}