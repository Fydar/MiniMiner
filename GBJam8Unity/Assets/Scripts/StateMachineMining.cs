using System.Collections;
using UnityEngine;
using Utility;

public class StateMachineMining : StateMachineState
{
	public StateMachineMining(Game game)
		: base(game)
	{

	}

	public override IEnumerator StateRoutine()
	{
		Setup.WorldMining.SetActive(true);
		Setup.WorldOverworld.SetActive(false);

		Setup.WallRenderer.RenderWall(Game.State.GetOrGenerate(Game.Setup.PlayerPrefab.FacingTile));

		foreach (float time in new TimedLoop(0.5f))
		{
			Setup.CircleWipe.SetTime(1.0f - time);
			yield return null;
		}

		yield return null;

		while (true)
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
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
