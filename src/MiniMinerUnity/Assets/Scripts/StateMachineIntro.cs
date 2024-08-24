using System.Collections;
using UnityEngine;

namespace MiniMinerUnity
{
    public class StateMachineIntro : StateMachineState
    {
        public StateMachineIntro(Game game)
            : base(game)
        {

        }

        public IEnumerator IntroDialogue()
        {
            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "...");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.25f);

            Game.Setup.TalkingToCharacter.gameObject.SetActive(true);
            Game.Setup.TalkingToCharacterShake.PlayShake(1.0f);

            Game.Setup.IntroMusic.Play();


            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "HOWDY!!!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "I am the new mine director!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Thanks for clearing the mine from MONSTERS!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "The mine has now REOPENED!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle1, "Come to me for all your mining needs!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            Game.Setup.Dialogue.Text.Clear();
            yield return new WaitForSeconds(0.125f);
            Game.Setup.TalkingToCharacterShake.PlayShake(1.0f);
            Game.Setup.Dialogue.Text.SetText(Game.Setup.IntroStyle2, "NOW GET DIGGING!!!");
            yield return StartCoroutine(Game.Setup.Dialogue.WaitForUserInput());

            // Game.Setup.Dialogue.Text.Clear();

            yield return new WaitForSeconds(0.75f);
        }

        public override IEnumerator StateRoutine()
        {
            Game.Setup.SetActiveWorld(Game.Setup.VoidWorld);

            Game.Setup.Dialogue.gameObject.SetActive(true);
            Game.Setup.TalkingToCharacter.gameObject.SetActive(false);

            Game.Setup.CircleWipe.SetTime(1.0f);

            foreach (float time in new TimedLoop(0.5f))
            {
                Game.Setup.CircleWipe.SetTime(1.0f - time);
                yield return null;
            }

            yield return StartCoroutine(IntroDialogue());

            Game.Setup.IntroMusic.Stop();

            foreach (float time in new TimedLoop(0.5f))
            {
                Game.Setup.CircleWipe.SetTime(time);
                yield return null;
            }
            Game.Setup.Dialogue.gameObject.SetActive(false);
            Game.Setup.TalkingToCharacter.gameObject.SetActive(false);
        }
    }
}
