using System.Collections;
using UnityEngine;
using Utility;

public class StateMachineIntro : StateMachineState
{
	public StateMachineIntro(Game game)
		: base(game)
	{

	}

	public IEnumerator IntroDialogue()
	{
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "... hi?");

		yield return new WaitForSeconds(5.0f);

		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "Hello there! How are you doing?");

		yield return new WaitForSeconds(5.0f);
	}

	public override IEnumerator StateRoutine()
	{
		Setup.SetActiveWorld(Setup.VoidWorld);

		Setup.Dialogue.gameObject.SetActive(true);
		Setup.CircleWipe.SetTime(1.0f);

		foreach (float time in new TimedLoop(0.5f))
		{
			Setup.CircleWipe.SetTime(1.0f - time);
			yield return null;
		}

		yield return StartCoroutine(IntroDialogue());


		foreach (float time in new TimedLoop(0.5f))
		{
			Setup.CircleWipe.SetTime(time);
			yield return null;
		}
		Setup.Dialogue.gameObject.SetActive(false);
	}
}
