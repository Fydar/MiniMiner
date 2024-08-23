using System;
using System.Collections;
using UnityEngine;

namespace MiniMinerUnity
{
    [Serializable]
    public abstract class StateMachineState
    {
        protected Game Game;

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
}
