using Harmony;
using System.Reflection;

namespace BSMOTD_Plus.Utilities
{
    public class HarmonyUtil
    {
        static HarmonyInstance harmony;

        /// <summary>
        /// Patch all harmony class modifications in the current assembly.
        /// </summary>
        public static void PatchGame()
        {
            if (harmony == null)
                harmony = HarmonyInstance.Create("com.auros.BeatSaber.BSMOTDPlus");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Unpatch all harmony class modifications in the current assembly.
        /// </summary>
        public static void UnpatchGame()
        {
            if (harmony != null)
                harmony.UnpatchAll();
        }
    }
}
