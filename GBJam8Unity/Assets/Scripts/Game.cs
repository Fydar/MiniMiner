﻿using System;
using UnityEngine;

[Serializable]
public class SceneSetup
{
	[Header("General")]
	public Transition CircleWipe;

	[Header("Overworld")]
	public GameObject WorldOverworld;
	public PlayerController PlayerPrefab;

	[Header("Mining")]
	public GameObject WorldMining;
	public WallRenderer WallRenderer;
	public ParticleSystem DustParticles;
	public Animator Selection;
}

public class Game : MonoBehaviour
{
	public SceneSetup Setup;
	public GameState State;

	private StateMachineRoot rootState;

	private void Start()
	{
		State = new GameState();

		rootState = new StateMachineRoot(this);
		StartCoroutine(rootState.StateRoutine());
	}

	private void Update()
	{

	}
}
