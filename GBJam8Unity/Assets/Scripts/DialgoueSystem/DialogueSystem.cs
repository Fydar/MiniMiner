using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
	public TextPanel Text;
	public Text WaitingInputElipsis;

	public float ElipsisAnimationTime = 0.75f;
	private int currentElipsisIndex = 0;
	private float lastElipsisUpdateTime;

	private void Awake()
	{
		WaitingInputElipsis.gameObject.SetActive(true);
		Text.Clear();
	}

	public IEnumerator WaitForUserInput()
	{
		yield return null;

		while (!Text.IsComplete)
		{
			if (Input.GetKeyDown(KeyCode.X)
				|| Input.GetKeyDown(KeyCode.C))
			{
				Text.InstaCompete = true;
				yield return null;
				break;
			}
			yield return null;
		}

		WaitingInputElipsis.gameObject.SetActive(true);
		lastElipsisUpdateTime = 0.0f;
		currentElipsisIndex = 2;
		while (true)
		{
			if (lastElipsisUpdateTime + ElipsisAnimationTime < Time.realtimeSinceStartup)
			{
				currentElipsisIndex++;
				if (currentElipsisIndex == 3)
				{
					currentElipsisIndex = 0;
				}

				WaitingInputElipsis.text = new string('.', currentElipsisIndex + 1);
				lastElipsisUpdateTime = Time.realtimeSinceStartup;
			}

			if (Input.GetKeyDown(KeyCode.X)
				|| Input.GetKeyDown(KeyCode.C))
			{
				break;
			}
			yield return null;
		}
		WaitingInputElipsis.gameObject.SetActive(false);

		AudioManager.Play(Game.Instance.Setup.NudgeSound);
	}
}
