using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMOTD.UI
{
    public class ChannelPostViewController : HotReloadableViewController
    {
        public override string ResourceName => "BSMOTD.Views.post-list.bsml";

        public override string ResourceFilePath => BeatSaber.InstallPath + "\\BSMOTDHR\\post-list.bsml";

        [UIComponent("list")]
        public CustomListTableData customListTableData;
    }
}
