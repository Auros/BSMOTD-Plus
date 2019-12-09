using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSMOTD.UI
{
    public class ChannelListViewController : HotReloadableViewController
    {
        public override string ResourceName => "BSMOTD.Views.channel-list.bsml";

        public override string ResourceFilePath => BeatSaber.InstallPath + "\\BSMOTDHR\\channel-list.bsml";

        [UIComponent("list")]
        public CustomListTableData customListTableData;

        [UIAction("channel-click")]
        private void ClickedRow(TableView table, int row)
        {
            channelClicked?.Invoke(customListTableData.data[row] as Channel);
        }

        public Action<Channel> channelClicked;

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (firstActivation && type == ActivationType.AddedToHierarchy)
            {
                customListTableData.data.AddRange(BSMOTDManager.instance.channels);
                foreach (var p in customListTableData.data)
                {
                    p.icon = (p as Channel).texture;
                }
                customListTableData.tableView.ReloadData();
            }
        }

    }
}
