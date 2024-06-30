using System.Collections;
using UnityEngine;

namespace MiniMinerUnity
{
    public class StateMachineMainMenu : StateMachineState
    {
        public StateMachineMainMenu(Game game)
            : base(game)
        {

        }

        public override IEnumerator StateRoutine()
        {
            Game.Setup.SetActiveWorld(Game.Setup.VoidWorld);

            Game.Setup.CircleWipe.SetTime(0.0f);

            Game.Setup.MainMenu.gameObject.SetActive(true);
            Game.Setup.MainMenuMusic.Play();

            Game.Setup.MainMenuPart1.gameObject.SetActive(false);
            Game.Setup.MainMenuPart2.gameObject.SetActive(false);
            Game.Setup.MainMenuPart3.gameObject.SetActive(false);
            Game.Setup.MainMenuContinueText.gameObject.SetActive(false);

            yield return new WaitForSeconds(1.0f);
            Game.Setup.MainMenuPart1.gameObject.SetActive(true);
            AudioManager.Play(Game.Setup.MainMenuPunch);

            yield return new WaitForSeconds(1.0f);
            Game.Setup.MainMenuPart2.gameObject.SetActive(true);
            AudioManager.Play(Game.Setup.MainMenuPunch);

            yield return new WaitForSeconds(1.0f);
            Game.Setup.MainMenuPart3.gameObject.SetActive(true);
            AudioManager.Play(Game.Setup.MainMenuPunch);

            yield return new WaitForSeconds(1.0f);
            Game.Setup.MainMenuContinueText.gameObject.SetActive(true);

            while (true)
            {
                if (GameboyInput.Instance.GameboyControls.A.WasPressedThisFrame())
                {
                    break;
                }
                yield return null;
            }

            Game.Setup.MainMenuMusic.Stop();
            yield return new WaitForSeconds(0.4f);
            AudioManager.Play(Game.Setup.MainMenuContinueSound);

            foreach (float time in new TimedLoop(0.75f))
            {
                Game.Setup.CircleWipe.SetTime(time);
                yield return null;
            }
            Game.Setup.MainMenu.gameObject.SetActive(false);

            yield return null;
        }
    }
}