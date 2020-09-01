using System;
using UnityEngine;

[Serializable]
public class SceneSetup
{
	[Header("Game Data")]
	public MiningBrush[] StartingBrushes;

	[Header("General")]
	public Canvas TransitionCanvas;
	public Transition CircleWipe;
	public Transition SawToothWipe;
	public Transition AngularWipe;
	public Transition LeftToRightWipe;
	public Transition RightToLeftWipe;
	public Transition TopToBottomWipe;
	public Transition BottomToTopWipe;

	[Header("Overworld")]
	public WorldData WorldOverworld;
	public PlayerController PlayerPrefab;

	[Header("Mining")]
	public WorldData WorldMining;
	public WallRenderer WallRenderer;
	public ParticleSystem DustParticles;
	public ParticleSystem HitDustParticles;
	public Animator MiningSelection;

	[Header("Audio")]
	public SfxGroup NudgeSound;
	public SfxGroup StepSound;
	public SfxGroup NoSound;
	public SfxGroup OkaySound;

	public void SetActiveWorld(WorldData world)
	{
		WorldOverworld.gameObject.SetActive(false);
		WorldMining.gameObject.SetActive(false);

		world.gameObject.SetActive(true);

		TransitionCanvas.worldCamera = world.WorldCamera;

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
	public SceneSetup Setup;
	public GameState State;

	private StateMachineRoot rootState;

	private void Awake()
	{
		Setup.PlayerPrefab.game = this;
	}

	private void Start()
	{
		State = new GameState()
		{
			Brushes = Setup.StartingBrushes
		};

		rootState = new StateMachineRoot(this);
		StartCoroutine(rootState.StateRoutine());
	}

	private void Update()
	{

	}
}
