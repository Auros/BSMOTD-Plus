using BeatSaberMarkupLanguage;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSMOTD.UI
{
    public class BSMOTDFlowCoordinator : FlowCoordinator
    {
        protected ChannelPostViewController _channelPostVC;
        protected ChannelListViewController _channelListVC;
        protected ChannelInfoViewController _channelInfoVC;
        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            title = "BSMOTD";

            if (firstActivation && activationType == ActivationType.AddedToHierarchy)
            {
                _channelPostVC = BeatSaberUI.CreateViewController<ChannelPostViewController>();
                _channelListVC = BeatSaberUI.CreateViewController<ChannelListViewController>();
                _channelInfoVC = BeatSaberUI.CreateViewController<ChannelInfoViewController>();
                _channelListVC.channelClicked += ChannelList_channelClicked;
                

                ProvideInitialViewControllers(_channelPostVC, _channelListVC);
                showBackButton = true;
            }
            StartCoroutine(HotReloadCoroutine());
        }

        public void Dismiss(ViewController vc)
        {
            DismissViewController(vc);
        }

        private void ChannelList_channelClicked(Channel chn)
        {
            Plugin.config.Value.SelectedChannel = chn.name;
            if (!_channelInfoVC.isInViewControllerHierarchy)
            {
                PresentViewController(_channelInfoVC);
                _channelInfoVC.SetData(chn);
            }
        }

        private IEnumerator<WaitForSeconds> HotReloadCoroutine()
        {
            var waitTime = new WaitForSeconds(.5f);
            using (var watcher = new FileSystemWatcher())
            {
                watcher.Path = Path.GetDirectoryName(_channelPostVC.ResourceFilePath);
                watcher.Filter = "*.bsml";
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += Watcher_Changed;
                watcher.EnableRaisingEvents = true;
                while (isActivated)
                {
                    if (_channelPostVC.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_channelPostVC);
                    if (_channelListVC.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_channelListVC);
                    if (_channelInfoVC.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_channelInfoVC);
                    yield return waitTime;
                }
            }
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == _channelPostVC.ResourceFilePath)
                _channelPostVC.MarkDirty();
            if (e.FullPath == _channelListVC.ResourceFilePath)
                _channelListVC.MarkDirty();
            if (e.FullPath == _channelInfoVC.ResourceFilePath)
                _channelInfoVC.MarkDirty();
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            if (_channelInfoVC.isInViewControllerHierarchy)
                DismissViewController(_channelInfoVC);
            _channelListVC.customListTableData.tableView.ClearSelection();
            base.BackButtonWasPressed(topViewController);
            BSMOTDManager.instance.MainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { this, null, false });
        }
    }
}
