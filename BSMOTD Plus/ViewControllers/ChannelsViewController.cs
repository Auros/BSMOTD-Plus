using System;
using System.Collections.Generic;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BSMOTD_Plus.Models;
using HMUI;
using UnityEngine;

namespace BSMOTD_Plus.ViewControllers
{
    public class ChannelsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMOTD_Plus.Views.channels.bsml";

        public Action<Channel> newChannelClicked;

        [UIComponent("list")]
        public CustomListTableData newsListTableData;

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);
            if (firstActivation)
            {
                rectTransform.anchorMin = new Vector3(0.5f, 0, 0);
                rectTransform.anchorMax = new Vector3(0.5f, 1, 0);
                rectTransform.sizeDelta = new Vector3(70, 0, 0);
            }
        }

        [UIAction("post-click")]
        private void ClickedRow(TableView table, int row)
        {
            //Logger.log.Info(row.ToString());
            newChannelClicked?.Invoke(cellInfo[row]);
            
        }
        List<Channel> cellInfo = new List<Channel>();

        public void SetData(List<Channel> channels)
        {
            newsListTableData.data.Clear();
            newsListTableData.tableView.ReloadData();
            cellInfo.Clear();

            foreach (var ch in channels)
            {
                var yeehaw = new CustomListTableData.CustomCellInfo(ch.Name, $"{ch.Source}");
                newsListTableData.data.Add(yeehaw);
                cellInfo.Add(ch);
                newsListTableData.tableView.ReloadData();

                SharedCoroutineStarter.instance.StartCoroutine(Utilities.LoadScripts.LoadSpriteCoroutine(ch.Image, (image) =>
                {
                    ch.Texture = image.texture;
                    yeehaw.icon = image.texture;
                    newsListTableData.tableView.ReloadData();

                }));

            }            
        }
    }
}
