using System.Collections;

namespace GBJam8
{
	public class StateMachineRoot : StateMachineState
	{
		public StateMachineRoot(Game game)
			: base(game)
		{

		}

		public override IEnumerator StateRoutine()
		{
			Game.Setup.CircleWipe.SetTime(1.0f);

			var intro = new StateMachineIntro(Game);
			yield return StartCoroutine(intro.StateRoutine());


			while (true)
			{
				var overworld = new StateMachineOverworld(Game);
				yield return StartCoroutine(overworld.StateRoutine());

				var mining = new StateMachineMining(Game);
				yield return StartCoroutine(mining.StateRoutine());
			}
		}
	}
}