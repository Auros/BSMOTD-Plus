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

        public Channel currentChannel;

        [UIComponent("image")]
        private RawImage image;
        [UIComponent("name")]
        private TextMeshProUGUI name;
        [UIComponent("desc")]
        private TextMeshProUGUI desc;
        [UIComponent("stream")]
        private Button stream;

        [UIAction("cancel")]
        private void Clicked()
        {
            //When you're lazy
            Resources.FindObjectsOfTypeAll<BSMOTDFlowCoordinator>()?.First().Dismiss(this);
        }

        [UIAction("addrem")]
        private void ButtonClick()
        {
            StreamMod();
        }

        private void StreamMod()
        {
            if (currentChannel.active == true)
            {
                BSMOTDManager.instance.RemovePosts(currentChannel);
                currentChannel.active = false;
                stream.GetComponentInChildren<TextMeshProUGUI>().text = "Add To Stream";
                
                Plugin.config.Value.ActiveChannels?.Remove(currentChannel.name);
            } 
            else
            {
                StartCoroutine(BSMOTDManager.instance.AddPosts(currentChannel));
                currentChannel.active = true;
                stream.GetComponentInChildren<TextMeshProUGUI>().text = "Remove From Stream";
                if (Plugin.config.Value.ActiveChannels == null)
                    Plugin.config.Value.ActiveChannels = new List<string>();
                Plugin.config.Value.ActiveChannels?.Remove(currentChannel.name);
                Plugin.config.Value.ActiveChannels.Add(currentChannel.name);
                
            }
            Plugin.configProvider.Store(Plugin.config.Value);
        }

        public void SetData(Channel chn)
        {
            currentChannel = chn;
            name.text = $"[{chn.code}] {chn.name}";
            desc.text = chn.description;
            image.texture = chn.texture;

            if (currentChannel.active == false)
                stream.GetComponentInChildren<TextMeshProUGUI>().text = "Add To Stream";
            else
                stream.GetComponentInChildren<TextMeshProUGUI>().text = "Remove From Stream";
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
