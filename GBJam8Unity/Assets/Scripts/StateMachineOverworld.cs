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
				if (Input.GetKeyDown(KeyCode.C))
				{
					if (Setup.PlayerPrefab.CanMine)
					{
						Setup.PlayerPrefab.selector.SetTrigger("Press");
						AudioManager.Play(Setup.NudgeSound);

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
					else if (Setup.PlayerPrefab.CanTalkToShopKeeper)
					{
						Setup.PlayerPrefab.selector.SetTrigger("Press");
						AudioManager.Play(Setup.NudgeSound);

						Setup.PlayerPrefab.EnableInput = false;

						Setup.TalkingToCharacter.gameObject.SetActive(true);
						Setup.Dialogue.gameObject.SetActive(true);

						yield return StartCoroutine(ShopkeeperRoutine());

						Setup.TalkingToCharacter.gameObject.SetActive(false);
						Setup.Dialogue.gameObject.SetActive(false);

						Setup.PlayerPrefab.EnableInput = true;
					}
				}

				yield return null;
			}
		}
		public IEnumerator ShopkeeperRoutine()
		{
			Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.5f);
			Setup.Dialogue.Text.SetText(Setup.IntroStyle1, "Hay there, Adventurer!");
			yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());

			Setup.Dialogue.Text.Clear();
			yield return new WaitForSeconds(0.25f);

			int rand = Random.Range(0, 2);
			if (rand == 0)
			{
				Setup.TalkingToCharacterShake.PlayShake(1.0f);

				Setup.Dialogue.Text.SetText(Setup.IntroStyle2, "BUY MY STUFF!!!");
				yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());
			}
			else if (rand == 1)
			{
				Setup.TalkingToCharacterShake.PlayShake(1.0f);

				Setup.Dialogue.Text.SetText(Setup.IntroStyle2, "I DROPPED MY GLASSES!!!");
				yield return StartCoroutine(Setup.Dialogue.WaitForUserInput());
			}

			foreach (float time in new TimedLoop(0.5f))
			{
				Setup.ShopFader.alpha = time;
				yield return null;
			}

			int currentlySelectedShopItem = 0;
			while (true)
			{
				if (Input.GetKeyDown(KeyCode.X))
				{
					break;
				}

				if (Input.GetKeyDown(KeyCode.S))
				{
					currentlySelectedShopItem--;
					if (currentlySelectedShopItem < 0)
					{
						currentlySelectedShopItem = Setup.Equipment.Length - 1;
					}
				}


				yield return null;
			}

			foreach (float time in new TimedLoop(0.5f))
			{
				Setup.ShopFader.alpha = 1.0f - time;
				yield return null;
			}

			yield return new WaitForSeconds(0.5f);
		}
	}
}