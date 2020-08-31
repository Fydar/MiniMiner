using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class StateMachineState
{
	protected Game Game;

	protected SceneSetup Setup => Game.Setup;

	public StateMachineState(Game game)
	{
		Game = game;
	}

	public abstract IEnumerator StateRoutine();

	public Coroutine StartCoroutine(IEnumerator enumerator)
	{
		return Game.StartCoroutine(enumerator);
	}
}
