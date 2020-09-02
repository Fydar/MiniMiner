using System.Collections;
using UnityEngine;
using Utility;

public class StateMachineOverworld : StateMachineState
{
	public ReturnActionGoMining ReturnAction;

	public StateMachineOverworld(Game game)
		: base(game)
	{

	}

	public override IEnumerator StateRoutine()
	{
		Setup.SetActiveWorld(Setup.WorldOverworld);

		foreach (float time in new TimedLoop(0.45f))
		{
			Setup.CircleWipe.SetTime(1.0f - time);
			yield return null;
		}

		Setup.PlayerPrefab.EnableInput = true;

		yield return null;

		while (true)
		{
			if (Setup.PlayerPrefab.CanMine && Input.GetKeyDown(KeyCode.C))
			{
				Setup.PlayerPrefab.selector.SetTrigger("Press");

				Setup.PlayerPrefab.EnableInput = false;

				ReturnAction = new ReturnActionGoMining()
				{
					Target = Setup.PlayerPrefab.FacingTile
				};

				foreach (float time in new TimedLoop(0.5f))
				{
					Setup.CircleWipe.SetTime(time);
					yield return null;
				}
				yield return new WaitForSeconds(0.1f);
				yield break;
			}

			yield return null;
		}
	}
}
