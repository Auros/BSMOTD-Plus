using Harmony;
using UnityEngine;
using System.Collections;

namespace BSMOTD_Plus.Patches
{
    [HarmonyPatch(typeof(MainFlowCoordinator))]
    [HarmonyPatch("DidActivate")]
    class MainFlowCoordinatorDidActivate
    {
        public static bool firstTimeHappened = false;

        static bool isOnStartup;
        static void Postfix()
        {
            isOnStartup = Plugin.config.GetBoolean("behavior", "onstartup") ?? true;
            if (!firstTimeHappened && isOnStartup)
                SharedCoroutineStarter.instance.StartCoroutine(Wait());

            if (isOnStartup == false)
                firstTimeHappened = true;
        }

        static IEnumerator Wait()
        {
            yield return new WaitForSeconds(.03f);
            Assistant.Instance.SummonUI();
            firstTimeHappened = true;
        }
    }
}
