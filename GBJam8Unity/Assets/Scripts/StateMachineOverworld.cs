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
		Setup.WorldMining.SetActive(false);
		Setup.WorldOverworld.SetActive(true);

		foreach (float time in new TimedLoop(0.5f))
		{
			Setup.CircleWipe.SetTime(1.0f - time);
			yield return null;
		}

		yield return null;

		while (true)
		{
			if (Setup.PlayerPrefab.CanMine && Input.GetKeyDown(KeyCode.X))
			{
				ReturnAction = new ReturnActionGoMining()
				{
					Target = Setup.PlayerPrefab.FacingTile
				};

				foreach (float time in new TimedLoop(0.25f))
				{
					Setup.CircleWipe.SetTime(time);
					yield return null;
				}

				yield break;
			}

			yield return null;
		}
	}
}
