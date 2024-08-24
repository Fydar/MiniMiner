using MiniMinerUnity.DialogueSystem;
using System.Collections;
using UnityEngine.InputSystem;

namespace MiniMinerUnity
{
    public static class RumbleUtility
    {
        public static void Dig()
        {
            foreach (var gamepad in Gamepad.all)
            {
                CoroutineHelper.Start(PunchRoutine(gamepad));
            }
        }

        public static void Collapse()
        {
            foreach (var gamepad in Gamepad.all)
            {
                CoroutineHelper.Start(CollapseRoutine(gamepad));
            }
        }

        private static IEnumerator PunchRoutine(Gamepad gamepad)
        {
            foreach (var time in new TimedLoop(0.15f))
            {
                gamepad.SetMotorSpeeds(0.0f, 1.0f - time);
                yield return null;
            }
        }

        private static IEnumerator CollapseRoutine(Gamepad gamepad)
        {
            foreach (var time in new TimedLoop(1.0f))
            {
                gamepad.SetMotorSpeeds(1.0f, 1.0f);
                yield return null;
            }
            foreach (var time in new TimedLoop(1.0f))
            {
                gamepad.SetMotorSpeeds(1.0f - time, 1.0f - time);
                yield return null;
            }
        }
    }
}
