using System.Collections;
using UnityEngine;

namespace GBJam8
{
	public class StateMachineMenu : StateMachineState
	{
		public StateMachineMenu(Game game)
			: base(game)
		{

		}

		public override IEnumerator StateRoutine()
		{
			Setup.CircleWipe.SetTime(1.0f);
			yield return new WaitForSeconds(1.0f);

			foreach (float time in new TimedLoop(2.0f))
			{
				Setup.CircleWipe.SetTime(1.0f - time);
				yield return null;
			}

			yield return null;
		}
	}
}