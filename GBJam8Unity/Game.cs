using System;
using System.Collections;
using UnityEngine;

public class RootState : GameState
{
	public RootState(Game game)
		: base(game)
	{

	}

	public override IEnumerator StateRoutine()
	{
		var menu = new MenuState(Game);

		yield return menu.StateRoutine();
	}
}

[Serializable]
public class SceneSetup
{
	[Header("General")]
	public Transition CircleWipe;

	[Header("Overworld")]
	public GameObject WorldOverworld;

	[Header("Mining")]
	public GameObject WorldMining;
}

public class Game : MonoBehaviour
{
	public SceneSetup Setup;

	private RootState rootState;

	private void Start()
	{
		rootState = new RootState(this);
		StartCoroutine(rootState.StateRoutine());
	}

	private void Update()
	{

	}
}
