using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour
{
	public Text TextElement;

	private TextStyle currentStyle;
	private string currentText;
	private bool resetFlag = false;

	private void Start()
	{
		StartCoroutine(RunningRoutine());
	}

	public void SetText(TextStyle style, string text)
	{
		currentStyle = style;
		currentText = text;

		TextElement.color = style.color;
		TextElement.font = style.Font;
		TextElement.fontSize = style.FontSize;
		TextElement.alignment = style.Alignment;
		resetFlag = true;
	}

	private IEnumerator RunningRoutine()
	{
		while (true)
		{
			while (!resetFlag)
			{
				yield return null;
			}
			resetFlag = false;

			var interpolator = new LinearInterpolator(42.0f);
			var fader = new EffectFader(interpolator)
			{
				TargetValue = 1.0f
			};

			AudioManager.Play(currentStyle.Audio.talkingLoop, fader);

			for (int i = 0; i < currentText.Length; i++)
			{
				TextElement.text = $"{currentText.Substring(0, i + 1)}<color=\"#000\">{currentText.Substring(i + 1)}</color>";

				yield return new WaitForSeconds(currentStyle.Animation.DelayPerTick);

				foreach (var exception in currentStyle.Animation.Exceptions)
				{
					if (exception.Character == currentText[i])
					{
						fader.TargetValue = 0.0f;

						yield return new WaitForSeconds(exception.PostDelay);

						fader.TargetValue = 1.0f;
					}
				}

				if (resetFlag)
				{
					break;
				}
			}

			fader.TargetValue = 0.0f;
		}
	}
}
