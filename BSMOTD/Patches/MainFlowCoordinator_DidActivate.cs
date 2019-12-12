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
        public static bool ItsMyFirstTimeOniiChan = true;
        static void Postfix(bool firstActivation)
        {
            if (firstActivation)
            {
                if (ItsMyFirstTimeOniiChan && Plugin.config.Value.Launch == LaunchType.Always)
                    SharedCoroutineStarter.instance.StartCoroutine(Wait());
            }
        }

        static IEnumerator Wait()
        {
            ItsMyFirstTimeOniiChan = false;
            yield return new WaitForSecondsRealtime(.025f);
            BSMOTDManager.instance.InvokeFlowCoordinator();
        }
    }
}
