using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMinerUnity.DialgoueSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextPanel Text;
        public Text WaitingInputElipsis;

        public float ElipsisAnimationTime = 0.75f;
        private int currentElipsisIndex = 0;
        private float lastElipsisUpdateTime;

        [Space]
        public RectTransform PopupDialogue;
        public RectTransform PopupDialogueOptionsHolder;
        public PopupOptionPool PopupDialogueOptionsPool;

        private void Awake()
        {
            WaitingInputElipsis.gameObject.SetActive(true);
            PopupDialogue.gameObject.SetActive(false);

            Text.Clear();
        }

        public IEnumerator WaitForUserInput(bool waitOnceComplete = true)
        {
            yield return null;

            while (!Text.IsComplete)
            {
                if (GameboyInput.Instance.GameboyControls.B.WasPressedThisFrame()
                    || GameboyInput.Instance.GameboyControls.A.WasPressedThisFrame())
                {
                    Text.InstaCompete = true;
                    yield return null;
                    break;
                }
                yield return null;
            }

            if (waitOnceComplete)
            {
                WaitingInputElipsis.gameObject.SetActive(true);
                lastElipsisUpdateTime = 0.0f;
                currentElipsisIndex = 2;
                while (true)
                {
                    if (lastElipsisUpdateTime + ElipsisAnimationTime < Time.realtimeSinceStartup)
                    {
                        currentElipsisIndex++;
                        if (currentElipsisIndex == 3)
                        {
                            currentElipsisIndex = 0;
                        }

                        WaitingInputElipsis.text = new string('.', currentElipsisIndex + 1);
                        lastElipsisUpdateTime = Time.realtimeSinceStartup;
                    }

                    if (GameboyInput.Instance.GameboyControls.B.WasPressedThisFrame()
                        || GameboyInput.Instance.GameboyControls.A.WasPressedThisFrame())
                    {
                        break;
                    }
                    yield return null;
                }
                WaitingInputElipsis.gameObject.SetActive(false);

                AudioManager.Play(Game.Instance.Setup.NudgeSound);
            }
        }
    }
}