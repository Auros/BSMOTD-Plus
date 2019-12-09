using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMOTD
{
    public class PluginConfig
    {
        public bool RegenerateConfig = true;
        public string SelectedChannel = "";
        public DateTime LastPostReceived;
        public string[] ActiveChannels;
        public bool LoadPostImages = true;
        public LaunchType Launch = LaunchType.Always;
    }

    public enum LaunchType
    {
        Always,
        NewPostsOnly,
        Never
    }
}
