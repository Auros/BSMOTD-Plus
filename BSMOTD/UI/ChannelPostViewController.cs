using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSMOTD.UI
{
    public class ChannelPostViewController : HotReloadableViewController
    {
        public override string ResourceName => "BSMOTD.Views.post-list.bsml";

        public override string ResourceFilePath => BeatSaber.InstallPath + "\\BSMOTDHR\\post-list.bsml";

        [UIComponent("list")]
        public CustomListTableData customListTableData;

        [UIAction("post-click")]
        private void ClickedRow(TableView table, int row)
        {
            postClicked?.Invoke(customListTableData.data[row] as Post);
        }

        public Action<Post> postClicked;

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);
            if (firstActivation)
            {
                rectTransform.anchorMin = new Vector3(0.5f, 0, 0);
                rectTransform.anchorMax = new Vector3(0.5f, 1, 0);
                rectTransform.sizeDelta = new Vector3(70, 0, 0);
            }

            if (BSMOTDManager.instance.ListDirty)
            {
                List<Post> sorted = BSMOTDManager.instance.posts.OrderByDescending(q => q.uploaded).ToList();

                customListTableData.data.Clear();
                customListTableData.data.AddRange(sorted);
                customListTableData.tableView.ReloadData();
            }
        }
    }
}
