using GBJam8.State;
using System;
using UnityEngine;

namespace GBJam8
{
	public class Game : MonoBehaviour
	{
		public static Game Instance;

		[NonSerialized]
		public GameState State;
		public SceneSetup Setup;

		private StateMachineRoot rootState;

		private void Awake()
		{
			Instance = this;
			Setup.PlayerPrefab.game = this;
			Setup.ShopFader.alpha = 0.0f;
		}

		private void Start()
		{
			State = new GameState(Setup);

			Setup.EquipmentShopRendererPool.Flush();
			foreach (var equipment in Setup.Equipment)
			{
				var equipmentState = State.Player.Equipment[equipment.Identifier];

				var renderer = Setup.EquipmentShopRendererPool.Grab(Setup.EquipmentShopHolder);

				renderer.Setup(equipment, equipmentState);
				equipmentState.Renderer = renderer;
			}

			rootState = new StateMachineRoot(this);
			StartCoroutine(rootState.StateRoutine());
		}

		private void Update()
		{

		}
	}
}