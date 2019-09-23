using BSMOTD_Plus.Utilities;
using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BSMOTD_Plus
{
    public class Plugin : IBeatSaberPlugin
    {

        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        public void OnApplicationStart()
        {
            Logger.log.Debug("OnApplicationStart");
            HarmonyUtil.PatchGame();
        }

        public void OnApplicationQuit()
        {
            Logger.log.Debug("OnApplicationQuit");
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
