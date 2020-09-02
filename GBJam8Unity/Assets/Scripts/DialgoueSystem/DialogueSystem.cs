using System.Collections;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
	public TextPanel Text;

	public IEnumerator WaitForUserInput()
	{
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.X)
				|| Input.GetKeyDown(KeyCode.C))
			{
				break;
			}
			yield return null;
		}
		AudioManager.Play(Game.Instance.Setup.NudgeSound);
	}
}
