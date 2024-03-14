using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMinerUnity.DialgoueSystem
{
	public class TextPanel : MonoBehaviour
	{
		public Text TextElement;

		private TextStyle currentStyle;
		private string currentText;
		private bool breakoutFlag = false;
		public bool IsComplete = false;
		public bool InstaCompete = false;

		private float shakeElapsed;

		private void Start()
		{
			CoroutineHelper.Start(RunningRoutine());
		}

		private void Update()
		{
			if (currentStyle != null)
			{
				if (currentStyle.Shake.UseNoise)
				{
					var originalCamPos = TextElement.transform.localPosition;
					shakeElapsed += Time.deltaTime;

					float percentComplete = shakeElapsed / currentStyle.Shake.FalloffDuration;
					percentComplete = currentStyle.Shake.Falloff.Evaluate(percentComplete);

					float damper = 1.0f - Mathf.Clamp01(percentComplete);

					float alpha = percentComplete * currentStyle.Shake.Frequency;

					float x = Mathf.PerlinNoise(alpha, 0) * 2.0f - 1.0f;
					float y = Mathf.PerlinNoise(0, alpha) * 2.0f - 1.0f;

					x *= damper * currentStyle.Shake.Intencity;
					y *= damper * currentStyle.Shake.Intencity;

					TextElement.transform.localPosition = new Vector3(
						Mathf.Round(x * 16.0f) / 16.0f,
						Mathf.Round(y * 16.0f) / 16.0f,
						originalCamPos.z);
				}
			}
		}

		public void SetText(TextStyle style, string text)
		{
			currentStyle = style;
			currentText = text;

			TextElement.color = style.color;
			TextElement.font = style.Font;
			TextElement.fontSize = style.FontSize;
			TextElement.alignment = style.Alignment;
			breakoutFlag = true;
			IsComplete = false;
			InstaCompete = false;
		}

		public void Clear()
		{
			currentText = "";
			TextElement.text = currentText;
			breakoutFlag = true;
			IsComplete = false;
			InstaCompete = false;
		}

		private IEnumerator RunningRoutine()
		{
			while (true)
			{
				while (!breakoutFlag)
				{
					yield return null;
				}
				breakoutFlag = false;

				if (string.IsNullOrEmpty(currentText))
				{
					continue;
				}

				var interpolator = new LinearInterpolator(42.0f);
				var fader = new EffectFader(interpolator)
				{
					TargetValue = 1.0f
				};

				AudioManager.Play(currentStyle.Audio.talkingLoop, fader);
				shakeElapsed = 0.0f;

				for (int i = 0; i < currentText.Length; i++)
				{
					TextElement.text = $"{currentText.Substring(0, i + 1)}<color=\"#000\">{currentText.Substring(i + 1)}</color>";

					yield return new WaitForSeconds(currentStyle.Animation.DelayPerTick);

					if (breakoutFlag)
					{
						break;
					}

					foreach (var exception in currentStyle.Animation.Exceptions)
					{
						if (exception.Character == currentText[i])
						{
							fader.TargetValue = 0.0f;
							yield return new WaitForSeconds(exception.PostDelay);
							fader.TargetValue = 1.0f;

							if (breakoutFlag)
							{
								break;
							}
						}
					}

					if (breakoutFlag)
					{
						break;
					}

					if (InstaCompete)
					{
						i = currentText.Length - 2;
						InstaCompete = false;
						continue;
					}
				}
				breakoutFlag = false;

				fader.TargetValue = 0.0f;
				IsComplete = true;
			}
		}
	}
}