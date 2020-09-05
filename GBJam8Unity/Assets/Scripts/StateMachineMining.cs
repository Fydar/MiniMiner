﻿using GBJam8.State;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace GBJam8
{
	public class StateMachineMining : StateMachineState
	{
		public Vector2Int SelectionPosition = new Vector2Int(11, 9);

		public StateMachineMining(Game game)
			: base(game)
		{

		}

		private void UpdateEquipmentUI()
		{
			var equipment = Game.Setup.Equipment[Game.State.Player.SelectedEquipment];
			int equipmentLevel = Game.State.Player.Equipment[equipment.Identifier].Level;

			Game.Setup.CurrentEquipment.sprite = equipment.SmallIcon;
			Game.Setup.CurrentEquipmentLevelText.text = equipmentLevel.ToString();

			Game.Setup.CurrentEquipment.color = equipmentLevel == 0
				? Game.Setup.Darken
				: Color.white;

			Game.Setup.CurrentEquipmentLevelText.color = equipmentLevel == 0
				? Game.Setup.Darken
				: Color.white;

			Game.Setup.PreviousArrow.color = Game.State.Player.SelectedEquipment == 0
				? Game.Setup.Darken
				: Color.white;

			Game.Setup.NextArrow.color = Game.State.Player.SelectedEquipment == Game.Setup.Equipment.Length - 1
				? Game.Setup.Darken
				: Color.white;
		}

		private void UpdateBag()
		{
			foreach (var label in Game.Setup.BagCapacity)
			{
				label.text = $"{Game.State.Player.BagCapacity}/ {Game.State.Player.MaxBagCapacity}";
			}
		}

		public override IEnumerator StateRoutine()
		{
			Game.Setup.MiningSelection.gameObject.SetActive(false);

			var wallData = Game.State.GetOrGenerate(Game.Setup.PlayerPrefab.FacingTile);
			Game.Setup.WallRenderer.RenderWall(wallData);
			Game.Setup.SetActiveWorld(Game.Setup.WorldMining);

			float wallCracksTime = (float)wallData.WallHealth / wallData.WallMaxHealth;
			Game.Setup.CracksTransition.SetTime(1.0f - wallCracksTime);

			UpdateEquipmentUI();
			UpdateBag();

			foreach (float time in new TimedLoop(0.45f))
			{
				Game.Setup.SawToothWipe.SetTime(1.0f - time);
				yield return null;
			}

			// Screen Loop
			while (true)
			{
				Game.Setup.MiningSelection.gameObject.SetActive(false);
				Game.Setup.EquipmentSelector.gameObject.SetActive(true);
				while (true)
				{
					if (Input.GetKeyDown(KeyCode.W))
					{
						AudioManager.Play(Game.Setup.NudgeSound);

						SelectionPosition = new Vector2Int(
							SelectionPosition.x,
							3
						);
						break;
					}
					else if (Input.GetKeyDown(KeyCode.S))
					{
						AudioManager.Play(Game.Setup.NoSound);
					}
					if (Input.GetKeyDown(KeyCode.A))
					{
						if (Game.State.Player.SelectedEquipment > 0)
						{
							Game.State.Player.SelectedEquipment--;
							AudioManager.Play(Game.Setup.NudgeSound);

							UpdateEquipmentUI();
						}
						else
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
					}
					else if (Input.GetKeyDown(KeyCode.D))
					{
						if (Game.State.Player.SelectedEquipment <= Game.Setup.Equipment.Length - 2)
						{
							Game.State.Player.SelectedEquipment++;
							AudioManager.Play(Game.Setup.NudgeSound);

							UpdateEquipmentUI();
						}
						else
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
					}

					if (Input.GetKeyDown(KeyCode.C))
					{
						var equipment = Game.Setup.Equipment[Game.State.Player.SelectedEquipment];
						int equipmentLevel = Game.State.Player.Equipment[equipment.Identifier].Level;

						if (equipmentLevel == 0)
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
						else
						{
							AudioManager.Play(Game.Setup.NudgeSound);
							break;
						}
					}
					else if (Input.GetKeyDown(KeyCode.X))
					{
						foreach (float time in new TimedLoop(0.5f))
						{
							Game.Setup.CircleWipe.SetTime(time);
							yield return null;
						}
						yield return new WaitForSeconds(0.1f);
						yield break;
					}
					yield return null;
				}
				yield return null;

				Game.Setup.MiningSelection.gameObject.SetActive(true);
				Game.Setup.EquipmentSelector.gameObject.SetActive(false);
				while (true)
				{
					if (Input.GetKeyDown(KeyCode.W))
					{
						if (SelectionPosition.y <= 11)
						{
							SelectionPosition += new Vector2Int(0, 2);
							AudioManager.Play(Game.Setup.NudgeSound);
						}
						else
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
					}
					else if (Input.GetKeyDown(KeyCode.S))
					{
						if (SelectionPosition.y > 4)
						{
							SelectionPosition += new Vector2Int(0, -2);
							AudioManager.Play(Game.Setup.NudgeSound);
						}
						else
						{
							AudioManager.Play(Game.Setup.NudgeSound);
							yield return null;
							break;
						}
					}
					if (Input.GetKeyDown(KeyCode.A))
					{
						if (SelectionPosition.x > 3)
						{
							SelectionPosition += new Vector2Int(-2, 0);
							AudioManager.Play(Game.Setup.NudgeSound);
						}
						else
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
					}
					else if (Input.GetKeyDown(KeyCode.D))
					{
						if (SelectionPosition.x <= 17)
						{
							SelectionPosition += new Vector2Int(2, 0);
							AudioManager.Play(Game.Setup.NudgeSound);
						}
						else
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
					}

					Game.Setup.MiningSelection.transform.localPosition = new Vector3(SelectionPosition.x, SelectionPosition.y, 0.0f) * 0.5f;

					if (Input.GetKeyDown(KeyCode.X))
					{
						AudioManager.Play(Game.Setup.NudgeSound);
						yield return null;
						break;
					}
					else if (Input.GetKeyDown(KeyCode.C))
					{
						var equipment = Game.Setup.Equipment[Game.State.Player.SelectedEquipment];
						int equipmentLevel = Game.State.Player.Equipment[equipment.Identifier].Level;

						if (equipmentLevel == 0)
						{
							AudioManager.Play(Game.Setup.NoSound);
						}
						else
						{

							var brush = equipment.Levels[equipmentLevel - 1].Brush;
							var randomOffset = brush.SprayPattern.GetRandomSpray();

							foreach (float digTime in brush.Digs)
							{
								if (brush.SprayPattern.ResprayPerDig)
								{
									randomOffset = brush.SprayPattern.GetRandomSpray();
								}

								yield return new WaitForSeconds(digTime);

								int digSelectionCount = Random.Range(brush.DiggingPattern.MinimumDigs, brush.DiggingPattern.MaximumDigs + 1);

								var digPattern = brush.DiggingPattern.Entries.OrderByDescending(e => (Random.value * e.Chance) + e.LayerFudge);
								int performedDigs = 0;
								foreach (var digLocation in digPattern)
								{
									var digPos = SelectionPosition
										+ new Vector2Int(digLocation.Offset.x, digLocation.Offset.y)
										+ randomOffset.Offset;

									if (digPos.x < 0
										|| digPos.y < 0
										|| digPos.x >= 22
										|| digPos.y >= 16)
									{
										if (!brush.DiggingPattern.IsSmartBrush)
										{
											performedDigs++;
										}
										continue;
									}

									var current = wallData.Nodes[digPos.x, digPos.y];

									if (!brush.DiggingPattern.IsSmartBrush
										|| (current.Layers.Gravel || current.Layers.Surface))
									{
										wallData.Nodes[digPos.x, digPos.y] = new WallTileNode()
										{
											Layers = current.Layers.RemoveDestructableLayer()
										};

										performedDigs++;
										if (performedDigs >= digSelectionCount)
										{
											break;
										}
									}
								}

								AudioManager.Play(brush.HitSound);
								Game.Setup.MiningSelection.SetTrigger("Press");
								Game.Setup.WorldMining.WorldCamera.GetComponent<PerlinShake>().PlayShake(brush.ShakeIntencity);
								Game.Setup.HitDustParticles.Emit(brush.HitParticles);
								Game.Setup.DustFallParticles.Play();

								Game.Setup.WallRenderer.RenderWall(wallData);

								wallData.WallHealth -= Random.Range(brush.MinimumWallDamage, brush.MaximumWallDamage);

								wallCracksTime = (float)wallData.WallHealth / wallData.WallMaxHealth;
								Game.Setup.CracksTransition.SetTime(1.0f - wallCracksTime);

								for (int i = 0; i < wallData.Rewards.Length; i++)
								{
									var reward = wallData.Rewards[i];
									if (reward.Claimed)
									{
										continue;
									}

									bool isCovered = false;
									foreach (var point in reward.Type.DigPositions)
									{
										var nodePosition = new Vector2Int(reward.Offset.x + point.Offset.x,
											reward.Offset.y + point.Offset.y);

										var current = wallData.Nodes[nodePosition.x, nodePosition.y];

										if (current.Layers.Surface
											|| current.Layers.Gravel)
										{
											isCovered = true;
										}
									}
									if (!isCovered)
									{
										Game.Setup.MiningSelection.gameObject.SetActive(false);

										yield return new WaitForSeconds(0.25f);

										AudioManager.Play(reward.Type.FindSound);

										yield return new WaitForSeconds(0.5f);
										Game.Setup.MiningSelection.gameObject.SetActive(true);

										reward.Claimed = true;
										wallData.Rewards[i] = reward;

										if (Game.State.Player.Bag.ContainsKey(reward.Type))
										{
											Game.State.Player.Bag[reward.Type]++;
										}
										else
										{
											Game.State.Player.Bag[reward.Type] = 1;
										}
										UpdateBag();

										Game.Setup.WallRenderer.RenderWall(wallData);
									}
								}
							}

							if (wallData.IsCollapsed)
							{
								foreach (float time in new TimedLoop(0.5f))
								{
									Game.Setup.CircleWipe.SetTime(time);
									yield return null;
								}
								yield return new WaitForSeconds(0.1f);
								yield break;
							}
						}
					}

					yield return null;
				}
			}
		}
	}
}