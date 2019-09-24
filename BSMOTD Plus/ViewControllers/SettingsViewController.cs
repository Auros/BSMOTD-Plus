using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BSMOTD_Plus.ViewControllers
{
    public class SettingsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMOTD_Plus.Views.settings.bsml";

        [UIComponent("text")]
        private TextMeshProUGUI text;

        [UIComponent("yesb")]
        private Button yb;

        [UIComponent("nob")]
        private Button nb;

        bool onStartup;
        bool init = true;
        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (type == ActivationType.AddedToHierarchy)
            {
                init = true;

                onStartup = Plugin.config.GetBoolean("behavior", "onstartup") ?? true;

                if (onStartup)
                    Yes();
                else
                    No();

                init = false;
            }
        }

        [UIAction("yes")]
        private void Yes()
        {
            text.text = "The BSMOTD Plus menu <size=120%><b>WILL</b></size> appear when you first launch your game.";
            text.color = Color.green;
            yb.interactable = false;
            nb.interactable = true;

            if (!init)
                Plugin.config.SetBoolean("behavior", "onstartup", true, true);
        }

        [UIAction("no")]
        private void No()
        {
            text.text = "The BSMOTD Plus menu will <size=120%><b>NOT</b></size> appear when you first launch your game.";
            text.color = Color.red;
            yb.interactable = true;
            nb.interactable = false;

            if (!init)
                Plugin.config.SetBoolean("behavior", "onstartup", false, true);
        }
    }
}
