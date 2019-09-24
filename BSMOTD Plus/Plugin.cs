using BSMOTD_Plus.Utilities;
using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;
using LibConf;
using LibConf.Providers;
using IPA.Utilities;

namespace BSMOTD_Plus
{
    public class Plugin : IBeatSaberPlugin
    {
        public static IConfigProvider config;

        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        public void OnApplicationStart()
        {
            HarmonyUtil.PatchGame();

            config = Conf.CreateConfig(ConfigType.YAML, BeatSaber.UserDataPath, "BSMOTDPlus");
        }

        public void OnApplicationQuit()
        {

        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {
            
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MenuCore")
            {
                CustomUI.MenuButton.MenuButtonUI.AddButton("BSMOTD", Assistant.Instance.SummonUI);
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
