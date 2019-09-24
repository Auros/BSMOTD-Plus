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
    public class PostListViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMOTD_Plus.Views.postlist.bsml";

        public Action<AdvancedPost> newPostClicked;

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

            if (type == ActivationType.AddedToHierarchy)
            {
                newsListTableData.tableView.ClearSelection();
            }
        }

        [UIAction("post-click")]
        private void ClickedRow(TableView table, int row)
        {
            //Logger.log.Info(row.ToString());
            newPostClicked?.Invoke(cellInfo[row]);
            
        }
        List<AdvancedPost> cellInfo = new List<AdvancedPost>();

        public void Clear()
        {
            newsListTableData.data.Clear();
            newsListTableData.tableView.ReloadData();
        }

        public void SetData(List<AdvancedPost> posts)
        {
            newsListTableData.data.Clear();
            newsListTableData.tableView.ReloadData();
            cellInfo.Clear();
            

            foreach (var post in posts)
            {
                var pT = DateTime.Parse(post.Uploaded);
                pT = pT.ToUniversalTime();
                
                var taz = pT.ToShortDateString() + " at " + pT.ToShortTimeString() + " UTC";

                var yeehaw = new CustomListTableData.CustomCellInfo(post.Title, $"by {post.Uploader} at {taz}");
                newsListTableData.data.Add(yeehaw);
                cellInfo.Add(post);
                newsListTableData.tableView.ReloadData();
                
                SharedCoroutineStarter.instance.StartCoroutine(Utilities.LoadScripts.LoadSpriteCoroutine(post.Image, (image) =>
                {
                    post.Texture = image.texture;
                    yeehaw.icon = image.texture;
                    newsListTableData.tableView.ReloadData();
                    
                }));
            }            
        }
    }
}
