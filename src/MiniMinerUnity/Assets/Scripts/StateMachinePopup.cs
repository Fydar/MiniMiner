using MiniMinerUnity.DialgoueSystem;
using System.Collections;
using UnityEngine;

namespace MiniMinerUnity
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
				if (GameboyInput.Instance.GameboyControls.Move.WasPressedThisFrame() && GameboyInput.Instance.GameboyControls.Move.ReadValue<Vector2>().y < -0.25f)
				{
					currentlySelected--;
					if (currentlySelected < 0)
					{
						currentlySelected = options.Length - 1;
					}
					AudioManager.Play(Game.Setup.NudgeSound);
				}
				else if (GameboyInput.Instance.GameboyControls.Move.WasPressedThisFrame() && GameboyInput.Instance.GameboyControls.Move.ReadValue<Vector2>().y > 0.25f)
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

				if (GameboyInput.Instance.GameboyControls.B.WasPressedThisFrame())
				{
					break;
				}
				else if (GameboyInput.Instance.GameboyControls.A.WasPressedThisFrame())
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