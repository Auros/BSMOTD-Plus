using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HMUI.FlowCoordinator;

namespace BSMOTD.UI
{
    public class SettingsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMOTD.Views.settings.bsml";

        [UIValue("launch")]
        public bool Enabled
        {
            get => Plugin.config.Value.Launch == LaunchType.Always;
            set
            {
                if (value)
                    Plugin.config.Value.Launch = LaunchType.Always;
                else
                    Plugin.config.Value.Launch = LaunchType.Never;
                Plugin.configProvider.Store(Plugin.config.Value);
            }
        }

        [UIValue("images")]
        public bool Images {
            get => Plugin.config.Value.LoadPostImages;
            set
            {
                Plugin.config.Value.LoadPostImages = value;
                Plugin.configProvider.Store(Plugin.config.Value);
            }
        }

    }
}
