using System.Collections;
using System.Linq;
using UnityEngine;
using Utility;

public class StateMachineMining : StateMachineState
{
	public Vector2Int SelectionPosition = new Vector2Int(11, 9);

	public StateMachineMining(Game game)
		: base(game)
	{

	}

	public override IEnumerator StateRoutine()
	{
		Setup.MiningSelection.gameObject.SetActive(false);

		var wallData = Game.State.GetOrGenerate(Game.Setup.PlayerPrefab.FacingTile);
		Setup.WallRenderer.RenderWall(wallData);
		Setup.SetActiveWorld(Setup.WorldMining);


		float wallCracksTime = (float)wallData.WallHealth / wallData.WallMaxHealth;
		Setup.CracksTransition.SetTime(1.0f - wallCracksTime);

		foreach (float time in new TimedLoop(0.45f))
		{
			Setup.SawToothWipe.SetTime(1.0f - time);
			yield return null;
		}

		Setup.MiningSelection.gameObject.SetActive(true);

		while (true)
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (SelectionPosition.y <= 11)
				{
					SelectionPosition += new Vector2Int(0, 2);
					AudioManager.Play(Setup.NudgeSound);
				}
				else
				{
					AudioManager.Play(Setup.NoSound);
				}
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				if (SelectionPosition.y > 4)
				{
					SelectionPosition += new Vector2Int(0, -2);
					AudioManager.Play(Setup.NudgeSound);
				}
				else
				{
					AudioManager.Play(Setup.NoSound);
				}
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				if (SelectionPosition.x > 3)
				{
					SelectionPosition += new Vector2Int(-2, 0);
					AudioManager.Play(Setup.NudgeSound);
				}
				else
				{
					AudioManager.Play(Setup.NoSound);
				}
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				if (SelectionPosition.x <= 17)
				{
					SelectionPosition += new Vector2Int(2, 0);
					AudioManager.Play(Setup.NudgeSound);
				}
				else
				{
					AudioManager.Play(Setup.NoSound);
				}
			}

			Setup.MiningSelection.transform.localPosition = new Vector3(SelectionPosition.x, SelectionPosition.y, 0.0f) * 0.5f;

			if (Input.GetKeyDown(KeyCode.X))
			{
				foreach (float time in new TimedLoop(0.5f))
				{
					Setup.CircleWipe.SetTime(time);
					yield return null;
				}
				yield return new WaitForSeconds(0.1f);
				yield break;
			}
			else if (Input.GetKeyDown(KeyCode.C))
			{
				var brush = Game.State.Brushes[0];
				var randomOffset = brush.SprayPattern.GetRandomSpray();

				foreach (float digTime in brush.Digs)
				{
					yield return new WaitForSeconds(digTime);

					int digSelectionCount = Random.Range(brush.DiggingPattern.MinimumDigs, brush.DiggingPattern.MaximumDigs + 1);

					var digPattern = brush.DiggingPattern.Entries.OrderByDescending(e => (Random.value * e.Chance) + e.LayerFudge);
					int performedDigs = 0;
					foreach (var digLocation in digPattern)
					{
						var digPos = SelectionPosition
							+ new Vector2Int(digLocation.Offset.x, digLocation.Offset.y)
							+ randomOffset.Offset;

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
					Setup.MiningSelection.SetTrigger("Press");
					Setup.WorldMining.WorldCamera.GetComponent<PerlinShake>().PlayShake(brush.ShakeIntencity);
					Setup.HitDustParticles.Emit(brush.HitParticles);
					Setup.DustFallParticles.Play();

					Setup.WallRenderer.RenderWall(wallData);

					wallData.WallHealth -= Random.Range(brush.MinimumWallDamage, brush.MaximumWallDamage);

					wallCracksTime = (float)wallData.WallHealth / wallData.WallMaxHealth;
					Setup.CracksTransition.SetTime(1.0f - wallCracksTime);

					if (wallData.IsCollapsed)
					{
						foreach (float time in new TimedLoop(0.5f))
						{
							Setup.CircleWipe.SetTime(time);
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
