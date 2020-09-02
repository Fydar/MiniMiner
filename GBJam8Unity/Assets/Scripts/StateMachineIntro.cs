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
		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.5f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "...");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);

		Setup.TalkingToCharacter.gameObject.SetActive(true);
		Setup.TalkingToCharacterShake.PlayShake(1.0f);

		Setup.Dialogue.Text.SetText(Setup.IntroStyle2, "HOWDY!!!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "I am the new mine director!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "Thanks for clearing the mine from MONSTERS!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "The mine has now REOPENED!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "Come to me for all your mining needs!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		Setup.Dialogue.Text.Clear();
		yield return new WaitForSeconds(0.25f);
		Setup.TalkingToCharacterShake.PlayShake(1.0f);
		Setup.Dialogue.Text.SetText(Setup.IntroStyle2, "NOW GET DIGGING!!!");
		yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

		// Setup.Dialogue.Text.Clear();

		yield return new WaitForSeconds(0.75f);
	}

	public override IEnumerator StateRoutine()
	{
		Setup.SetActiveWorld(Setup.VoidWorld);
		Setup.TalkingToCharacter.gameObject.SetActive(false);

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
