using System.Collections;
using UnityEngine;

namespace MiniMinerUnity.DialogueSystem
{
    public class CoroutineHelper : MonoBehaviour
    {
        public static void Start(IEnumerator enumerator)
        {
            var clone = new GameObject("Coroutine");
            var comp = clone.AddComponent<CoroutineHelper>();
            comp.RunCoroutine(enumerator);
        }

        public void RunCoroutine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
    }
}
