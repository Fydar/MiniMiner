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

			var mainMenu = new StateMachineMainMenu(Game);
			yield return StartCoroutine(mainMenu.StateRoutine());

			var intro = new StateMachineIntro(Game);
			yield return StartCoroutine(intro.StateRoutine());

			Game.Setup.WorldMusic.Play();
			while (true)
			{
				Game.Setup.WorldMusic.volume = Game.Setup.WorldMusicOverworldVolume;

				var overworld = new StateMachineOverworld(Game);
				yield return StartCoroutine(overworld.StateRoutine());

				Game.Setup.WorldMusic.volume = Game.Setup.WorldMusicMiningVolume;

				var mining = new StateMachineMining(Game);
				yield return StartCoroutine(mining.StateRoutine());
			}
		}
	}
}