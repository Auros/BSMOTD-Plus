using BeatSaberMarkupLanguage;
using BS_Utils.Utilities;
using BSMOTD.UI;
using Harmony;
using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSMOTD.Patches
{
    [HarmonyPatch(typeof(MainFlowCoordinator), "DidActivate")]
    public class MainFlowCoordinator_DidActivate
    {
        static void Postfix(bool firstActivation)
        {
            if (firstActivation)
            {
                SharedCoroutineStarter.instance.StartCoroutine(Wait());
            }
        }

        static IEnumerator Wait()
        {
            yield return new WaitForSecondsRealtime(.025f);
            BSMOTDManager.instance.InvokeFlowCoordinator();
        }
    }
}
