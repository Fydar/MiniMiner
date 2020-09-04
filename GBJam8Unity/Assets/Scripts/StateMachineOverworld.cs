using System.Collections;
using UnityEngine;

namespace GBJam8
{
	public class StateMachineOverworld : StateMachineState
	{
		public ReturnActionGoMining ReturnAction;


		public StateMachineOverworld(Game game)
			: base(game)
		{

		}

		public override IEnumerator StateRoutine()
		{
			Game.Setup.SetActiveWorld(Game.Setup.WorldOverworld);
			Game.Setup.CurrencyText.text = $"${Game.State.Player.Money}";

			foreach (float time in new TimedLoop(0.45f))
			{
				Game.Setup.CircleWipe.SetTime(1.0f - time);
				yield return null;
			}

			Game.Setup.PlayerPrefab.EnableInput = true;

			yield return null;

			while (true)
			{
				if (Input.GetKeyDown(KeyCode.C))
				{
					if (Game.Setup.PlayerPrefab.CanMine)
					{
						Game.Setup.PlayerPrefab.selector.SetTrigger("Press");
						AudioManager.Play(Game.Setup.NudgeSound);

						if (Game.State.Player.HasEquipment)
						{
							Game.Setup.PlayerPrefab.EnableInput = false;

							ReturnAction = new ReturnActionGoMining()
							{
								Target = Game.Setup.PlayerPrefab.FacingTile
							};

							foreach (float time in new TimedLoop(0.5f))
							{
								Game.Setup.CircleWipe.SetTime(time);
								yield return null;
							}
							yield return new WaitForSeconds(0.1f);
							yield break;
						}
						else
						{
							Game.Setup.PlayerPrefab.EnableInput = false;
							Game.Setup.Dialogue.gameObject.SetActive(true);


							Game.Setup.Dialogue.Text.Clear();
							yield return new WaitForSeconds(0.5f);
							Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "I don't have any tools yet!");
							yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

							Game.Setup.Dialogue.Text.Clear();
							yield return new WaitForSeconds(0.25f);
							Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Let's go visit the shop keeper!");
							yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());


							Game.Setup.Dialogue.gameObject.SetActive(false);
							Game.Setup.PlayerPrefab.EnableInput = true;
						}
					}
					else if (Game.Setup.PlayerPrefab.CanTalkToShopKeeper)
					{
						Game.Setup.PlayerPrefab.selector.SetTrigger("Press");
						AudioManager.Play(Game.Setup.NudgeSound);

						Game.Setup.PlayerPrefab.EnableInput = false;

						Game.Setup.TalkingToCharacter.gameObject.SetActive(true);
						Game.Setup.Dialogue.gameObject.SetActive(true);

						yield return StartCoroutine(ShopkeeperRoutine());

						Game.Setup.TalkingToCharacter.gameObject.SetActive(false);
						Game.Setup.Dialogue.gameObject.SetActive(false);

						Game.Setup.PlayerPrefab.EnableInput = true;
					}
				}

				yield return null;
			}
		}
		public IEnumerator ShopkeeperRoutine()
		{
			Game.Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.5f);
			Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Hay there, Adventurer!");
			yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

			Game.Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.25f);

			int rand = Random.Range(0, 2);
			if (rand == 0)
			{
				Game.Setup.TalkingToCharacterShake.PlayShake(1.0f);

				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "BUY MY STUFF!!!");
				yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());
			}
			else if (rand == 1)
			{
				Game.Setup.TalkingToCharacterShake.PlayShake(1.0f);

				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "I DROPPED MY GLASSES!!!");
				yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());
			}

			Game.Setup.Dialogue.Text.Clear();

			foreach (float time in new TimedLoop(0.5f))
			{
				Game.Setup.ShopFader.alpha = time;
				yield return null;
			}

			int currentlySelectedShopItem = 0;
			while (true)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					AudioManager.Play(Game.Setup.NudgeSound);
					break;
				}
				else if (Input.GetKeyDown(KeyCode.C))
				{
					AudioManager.Play(Game.Setup.NudgeSound);

					var equipment = Game.Setup.Equipment[currentlySelectedShopItem];
					var equipmentState = Game.State.Player.Equipment[equipment.Identifier];

					if (equipment.Levels.Length == equipmentState.Level)
					{
						// Equipment is at max level
					}
					else
					{
						var nextLevelData = equipment.Levels[equipmentState.Level];
						if (nextLevelData.Cost <= Game.State.Player.Money)
						{
							// Can afford to purchase

							Game.Setup.Dialogue.Text.Clear();
							yield return new WaitForSeconds(0.5f);
							if (equipmentState.Level == 0)
							{
								Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"Would you like to buy my {equipment.DisplayName.ToUpper()}?");
							}
							else
							{
								Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"Would you like me to upgrade your {equipment.DisplayName.ToUpper()}?");
							}
							yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput(waitOnceComplete: false));

							var popup = new StateMachinePopup(Game);
							yield return null;

							yield return StartCoroutine(popup.StateRoutine(
								new PopupOptionText()
								{
									Display = "Yes",
									DefaultSelection = false
								},
								new PopupOptionText()
								{
									Display = "No",
									DefaultSelection = true
								}
							));

							yield return new WaitForSeconds(0.25f);
							Game.Setup.Dialogue.Text.Clear();

							if (popup.FinalSelected != null
								&& popup.FinalSelected.Display == "Yes")
							{
								equipmentState.Level++;
								Game.State.Player.Money -= nextLevelData.Cost;
								Game.Setup.CurrencyText.text = $"${Game.State.Player.Money}";

								AudioManager.Play(equipment.UpgradeSound);
							}
						}
						else
						{
							// Cannot afford to purchase
							AudioManager.Play(Game.Setup.NoSound);
						}
					}
				}

				if (Input.GetKeyDown(KeyCode.S))
				{
					currentlySelectedShopItem++;
					if (currentlySelectedShopItem >= Game.Setup.Equipment.Length)
					{
						currentlySelectedShopItem = 0;
					}
					AudioManager.Play(Game.Setup.NudgeSound);
				}
				else if (Input.GetKeyDown(KeyCode.W))
				{
					currentlySelectedShopItem--;
					if (currentlySelectedShopItem < 0)
					{
						currentlySelectedShopItem = Game.Setup.Equipment.Length - 1;
					}
					AudioManager.Play(Game.Setup.NudgeSound);
				}

				for (int i = 0; i < Game.Setup.Equipment.Length; i++)
				{
					var equipment = Game.Setup.Equipment[i];
					var equipmentState = Game.State.Player.Equipment[equipment.Identifier];

					equipmentState.Renderer.Background.color
						= i == currentlySelectedShopItem
						? equipmentState.Renderer.SelectedBackgroundColor
						: equipmentState.Renderer.NormalBackgroundColor;
				}

				yield return null;
			}

			Game.Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.5f);
			Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Take care out there!");
			yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

			foreach (float time in new TimedLoop(0.5f))
			{
				Game.Setup.ShopFader.alpha = 1.0f - time;
				yield return null;
			}
		}
	}
}