using GBJam8.DialgoueSystem;
using System.Collections;
using UnityEngine;

namespace GBJam8
{
	public class PopupOptionText
	{
		public string Display;
		// public bool Disabled = false;
		public bool DefaultSelection;

		public PopupOption Renderer;
	}

	public class StateMachinePopup
	{
		private readonly Game Game;

		public PopupOptionText FinalSelected;

		public StateMachinePopup(Game game)
		{
			Game = game;
		}

		public IEnumerator StateRoutine(params PopupOptionText[] options)
		{
			Game.Setup.Dialogue.PopupDialogue.gameObject.SetActive(true);

			Game.Setup.Dialogue.PopupDialogueOptionsPool.Flush();
			foreach (var option in options)
			{
				var optionRenderer = Game.Setup.Dialogue.PopupDialogueOptionsPool.Grab(
					Game.Setup.Dialogue.PopupDialogueOptionsHolder);

				optionRenderer.SetContent(option.Display);

				option.Renderer = optionRenderer;
			}

			int currentlySelected = 0;
			for (int i = 0; i < options.Length; i++)
			{
				var option = options[i];
				if (option.DefaultSelection)
				{
					currentlySelected = i;
				}
			}

			while (true)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					currentlySelected--;
					if (currentlySelected < 0)
					{
						currentlySelected = options.Length - 1;
					}
					AudioManager.Play(Game.Setup.NudgeSound);
				}
				else if (Input.GetKeyDown(KeyCode.W))
				{
					currentlySelected++;
					if (currentlySelected >= options.Length)
					{
						currentlySelected = 0;
					}
					AudioManager.Play(Game.Setup.NudgeSound);
				}
				for (int i = 0; i < options.Length; i++)
				{
					var option = options[i];
					option.Renderer.SetState(i == currentlySelected);
				}

				if (Input.GetKeyDown(KeyCode.X))
				{
					break;
				}
				else if (Input.GetKeyDown(KeyCode.C))
				{
					FinalSelected = options[currentlySelected];
					break;
				}

				yield return null;
			}

			Game.Setup.Dialogue.PopupDialogue.gameObject.SetActive(false);
		}
	}
}