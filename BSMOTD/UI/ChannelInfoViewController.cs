using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BSMOTD.UI
{
    public class ChannelInfoViewController : HotReloadableViewController
    {
        public override string ResourceName => "BSMOTD.Views.channel-info.bsml";

        public override string ResourceFilePath => BeatSaber.InstallPath + "\\BSMOTDHR\\channel-info.bsml";

        [UIComponent("image")]
        private RawImage image;

        [UIComponent("name")]
        private TextMeshProUGUI name;
        [UIComponent("desc")]
        private TextMeshProUGUI desc;

        [UIAction("cancel")]
        private void Clicked()
        {
            Logger.log.Info("EWRWE");
            Resources.FindObjectsOfTypeAll<BSMOTDFlowCoordinator>()?.First().Dismiss(this);
        }

        public void SetData(Channel chn)
        {
            name.text = $"[{chn.code}] {chn.name}";
            desc.text = chn.description;
            image.texture = chn.texture;
        }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);
            if (firstActivation)
            {
                var element = image.gameObject.AddComponent<AspectRatioFitter>();
                element.aspectRatio = 1f;
                element.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            }
        }
    }
}
