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

        static void Postfix()
        {
            if (!firstTimeHappened)
                SharedCoroutineStarter.instance.StartCoroutine(Wait());
            //return true;
        }

        static IEnumerator Wait()
        {
            yield return new WaitForSeconds(.03f);
            Assistant.Instance.SummonUI();
            firstTimeHappened = true;
        }
    }
}
