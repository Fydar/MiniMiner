﻿using GBJam8.DialgoueSystem;
using GBJam8.State;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GBJam8
{
	[Serializable]
	public class SceneSetup
	{
		[Header("Game Data")]
		public EquipmentItemTemplate[] Equipment;
		public RewardType[] Rewards;

		[Header("Dialogue")]
		public DialogueSystem Dialogue;
		public TextStyle IntroStyle1;
		public TextStyle IntroStyle2;

		[Space]
		public CanvasGroup ShopFader;
		public Text CurrencyText;
		public RectTransform EquipmentShopHolder;
		public EquipmentShopRendererPool EquipmentShopRendererPool;

		[Header("General")]
		public Canvas TransitionCanvas;
		public Transition CircleWipe;
		public Transition SawToothWipe;
		public Transition AngularWipe;
		public Transition LeftToRightWipe;
		public Transition RightToLeftWipe;
		public Transition TopToBottomWipe;
		public Transition BottomToTopWipe;

		[Header("Introduction")]
		public Image TalkingToCharacter;
		public PerlinShake TalkingToCharacterShake;

		[Header("Overworld")]
		public WorldData VoidWorld;
		public WorldData WorldOverworld;
		public PlayerController PlayerPrefab;

		[Header("Mining")]
		public WorldData WorldMining;
		public WallRenderer WallRenderer;
		public ParticleSystem DustParticles;
		public ParticleSystem HitDustParticles;
		public ParticleSystem DustFallParticles;
		public Animator MiningSelection;
		public Transition CracksTransition;

		[Header("Audio")]
		public SfxGroup NudgeSound;
		public SfxGroup StepSound;
		public SfxGroup NoSound;
		public SfxGroup OkaySound;

		public void SetActiveWorld(WorldData world)
		{
			VoidWorld.gameObject.SetActive(false);
			WorldOverworld.gameObject.SetActive(false);
			WorldMining.gameObject.SetActive(false);

			if (world != null)
			{
				world.gameObject.SetActive(true);
				TransitionCanvas.worldCamera = world.WorldCamera;
			}

			CircleWipe.SetTime(0.0f);
			SawToothWipe.SetTime(0.0f);
			AngularWipe.SetTime(0.0f);
			LeftToRightWipe.SetTime(0.0f);
			RightToLeftWipe.SetTime(0.0f);
			TopToBottomWipe.SetTime(0.0f);
			BottomToTopWipe.SetTime(0.0f);
		}
	}

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