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

							var wallData = Game.State.GetOrGenerate(Game.Setup.PlayerPrefab.FacingTile);

							if (wallData.IsCollapsed)
							{
								Game.Setup.PlayerPrefab.EnableInput = false;

								Game.Setup.Dialogue.gameObject.SetActive(true);
								Game.Setup.TalkingToCharacter.gameObject.SetActive(false);

								Game.Setup.Dialogue.Text.Clear();
								yield return new WaitForSeconds(0.25f);
								Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"This wall has already collapsed!");
								yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

								Game.Setup.Dialogue.gameObject.SetActive(false);

								Game.Setup.PlayerPrefab.EnableInput = true;
							}
							else if (wallData.HasCollectedAllRewards)
							{
								Game.Setup.PlayerPrefab.EnableInput = false;

								Game.Setup.Dialogue.gameObject.SetActive(true);
								Game.Setup.TalkingToCharacter.gameObject.SetActive(false);

								Game.Setup.Dialogue.Text.Clear();
								yield return new WaitForSeconds(0.25f);
								Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"You have already collected all the rewards!");
								yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

								Game.Setup.Dialogue.gameObject.SetActive(false);

								Game.Setup.PlayerPrefab.EnableInput = true;
							}
							else
							{
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
					else if (Game.Setup.PlayerPrefab.CanInteractWithBlockade)
					{
						Game.Setup.PlayerPrefab.selector.SetTrigger("Press");
						AudioManager.Play(Game.Setup.NudgeSound);
						Game.Setup.PlayerPrefab.EnableInput = false;

						Game.Setup.TalkingToCharacter.gameObject.SetActive(true);
						Game.Setup.Dialogue.gameObject.SetActive(true);

						yield return StartCoroutine(BoulderInteractionRoutine());

						Game.Setup.TalkingToCharacter.gameObject.SetActive(false);
						Game.Setup.Dialogue.gameObject.SetActive(false);

						Game.Setup.PlayerPrefab.EnableInput = true;
					}
				}

				yield return null;
			}
		}
		public IEnumerator BoulderInteractionRoutine()
		{
			string rarity = Game.Setup.RarityMap.GetTile(Game.Setup.PlayerPrefab.FacingTile).name;

			int removalCost;
			if (rarity == "rarity-2")
			{
				removalCost = 50;
			}
			else if (rarity == "rarity-3")
			{
				removalCost = 100;
			}
			else if (rarity == "rarity-4")
			{
				removalCost = 150;
			}
			else if (rarity == "rarity-5")
			{
				removalCost = 200;
			}
			else
			{
				removalCost = 250;
			}

			Game.Setup.InteractionShop.gameObject.SetActive(true);
			Game.Setup.InteractionShopCurrencyText.text = $"${Game.State.Player.Money}";

			if (Game.State.Player.Money >= removalCost)
			{
				Game.Setup.Dialogue.Text.Clear();
				yield return new WaitForSeconds(0.5f);

				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"Would you like me to remove this boulder for ${removalCost}?");

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
					Game.State.Player.Money -= removalCost;
					Game.Setup.InteractionShopCurrencyText.text = $"${Game.State.Player.Money}";

					AudioManager.Play(Game.Setup.BoulderBreak);

					yield return new WaitForSeconds(0.25f);

					AudioManager.Play(Game.Setup.BoulderBreak);

					Game.Setup.PlayerPrefab.decorationLayer.SetTile(Game.Setup.PlayerPrefab.FacingTile, null);

					yield return new WaitForSeconds(0.8f);
				}
			}
			else
			{
				Game.Setup.Dialogue.Text.Clear();
				yield return new WaitForSeconds(0.25f);
				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"It will cost you ${removalCost} to remove that boulder.");
				yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

				Game.Setup.Dialogue.Text.Clear();
				yield return new WaitForSeconds(0.25f);
				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, $"Try again when you have enough money.");
				yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());
			}
			Game.Setup.InteractionShop.gameObject.SetActive(false);
		}
		public IEnumerator ShopkeeperRoutine()
		{
			Game.Setup.RewardTab.gameObject.SetActive(false);
			Game.Setup.EquipmentTab.gameObject.SetActive(false);
			Game.Setup.ShopFader.gameObject.SetActive(true);

			Game.Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.5f);
			Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Hay there, Adventurer!");
			yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

			Game.Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.25f);

			if (Game.State.Player.BagCapacity > 0)
			{
				Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "What do you have for me?");
			}
			else
			{
				int rand = Random.Range(0, 2);
				Game.Setup.TalkingToCharacterShake.PlayShake(1.0f);
				if (rand == 0)
				{
					Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "BUY MY STUFF!!!");
				}
				else if (rand == 1)
				{
					Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "I DROPPED MY GLASSES!!!");
				}
			}

			yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());
			Game.Setup.Dialogue.Text.Clear();

			if (Game.State.Player.BagCapacity > 0)
			{
				Game.Setup.EquipmentTab.gameObject.SetActive(false);
				Game.Setup.RewardTab.gameObject.SetActive(true);

				Game.Setup.RewardGraphic.gameObject.SetActive(false);
				Game.Setup.RewardDetails.gameObject.SetActive(false);

				foreach (float time in new TimedLoop(0.5f))
				{
					Game.Setup.ShopFader.alpha = time;
					yield return null;
				}

				foreach (var item in Game.State.Player.Bag)
				{
					Game.Setup.RewardGraphic.sprite = item.Key.IconGraphic;
					Game.Setup.RewardQuantity.text = $"x{item.Value}";
					Game.Setup.RewardValueCounter.text = $"${item.Key.CurrencyValue}";

					foreach (var text in Game.Setup.RewardName)
					{
						text.text = item.Key.DisplayName;
					}

					Game.Setup.RewardGraphic.gameObject.SetActive(false);
					Game.Setup.RewardDetails.gameObject.SetActive(false);
					Game.Setup.RewardStarPool.Flush();

					yield return new WaitForSeconds(0.35f);

					AudioManager.Play(Game.Setup.UIAppearSound);
					Game.Setup.RewardGraphic.gameObject.SetActive(true);

					yield return new WaitForSeconds(0.35f);

					AudioManager.Play(Game.Setup.UIAppearSound);
					Game.Setup.RewardDetails.gameObject.SetActive(true);

					yield return new WaitForSeconds(0.35f);

					for (int i = 0; i < item.Key.StarRating; i++)
					{
						AudioManager.Play(Game.Setup.StarAppearSound);
						yield return new WaitForSeconds(0.1f);
						Game.Setup.RewardStarPool.Grab(Game.Setup.RewardStarHolder);
					}

					while (true)
					{
						if (Input.GetKeyDown(KeyCode.C)
							|| Input.GetKeyDown(KeyCode.X))
						{
							break;
						}
						yield return null;
					}
					yield return null;

					Game.State.Player.Money += item.Key.CurrencyValue * item.Value;
					Game.Setup.CurrencyText.text = $"${Game.State.Player.Money}";

					if (Game.Setup.CollectSound != null)
					{
						AudioManager.Play(Game.Setup.CollectSound);
					}
				}
				Game.State.Player.Bag.Clear();
			}
			else
			{
				foreach (float time in new TimedLoop(0.5f))
				{
					Game.Setup.ShopFader.alpha = time;
					yield return null;
				}
			}
			yield return null;

			Game.Setup.RewardTab.gameObject.SetActive(false);
			Game.Setup.EquipmentTab.gameObject.SetActive(true);

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
							for (int i = 0; i < Game.Setup.Equipment.Length; i++)
							{
								var otherEquipment = Game.Setup.Equipment[i];
								var otherEquipmentState = Game.State.Player.Equipment[otherEquipment.Identifier];

								otherEquipmentState.Renderer.Background.color
									= otherEquipmentState.Renderer.NormalBackgroundColor;
							}

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

								yield return new WaitForSeconds(0.8f);
							}

							for (int i = 0; i < Game.Setup.Equipment.Length; i++)
							{
								var otherEquipment = Game.Setup.Equipment[i];
								var otherEquipmentState = Game.State.Player.Equipment[otherEquipment.Identifier];

								otherEquipmentState.Renderer.Background.color
									= i == currentlySelectedShopItem
									? otherEquipmentState.Renderer.SelectedBackgroundColor
									: otherEquipmentState.Renderer.NormalBackgroundColor;
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