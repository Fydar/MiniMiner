using System.Collections;
using UnityEngine;

public class StateMachineRoot : StateMachineState
{
	public StateMachineRoot(Game game)
		: base(game)
	{

	}

	public override IEnumerator StateRoutine()
	{
		Setup.CircleWipe.SetTime(1.0f);
		yield return new WaitForSeconds(1.0f);

		while (true)
		{
			var overworld = new StateMachineOverworld(Game);
			yield return StartCoroutine(overworld.StateRoutine());

			var mining = new StateMachineMining(Game);
			yield return StartCoroutine(mining.StateRoutine());
		}
	}
}
