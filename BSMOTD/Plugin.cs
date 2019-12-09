using BeatSaberMarkupLanguage.MenuButtons;
using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace BSMOTD
{
    public class Plugin : IBeatSaberPlugin, IDisablablePlugin
    {
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        static MenuButton btn;
        internal static HarmonyInstance harmony;
        public void OnEnable()
        {
            harmony = HarmonyInstance.Create("com.auros.BSMOTD");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
            SharedCoroutineStarter.instance.StartCoroutine(BSMOTDManager.instance.LoadChannels());
            btn = new MenuButton("BSMOTD", "BSMOTD Menu", BSMOTDManager.instance.InvokeFlowCoordinator, true);
            MenuButtons.instance.RegisterButton(btn);
        }

        public void OnDisable()
        {

        }


        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;

            configProvider = cfgProvider;
            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig || v.Value == null && v.Value.RegenerateConfig)
                {
                    p.Store(v.Value = new PluginConfig() { RegenerateConfig = false });
                }
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            
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
            
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
