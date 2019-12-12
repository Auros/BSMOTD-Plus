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
        protected NavigationController navigationController;

        protected ChannelPostViewController _channelPostVC;
        protected ChannelListViewController _channelListVC;
        protected ChannelInfoViewController _channelInfoVC;
        protected PostDetailViewController _postDetailVC;
        protected SettingsViewController _settingsVC;
        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            title = "BSMOTD";

            if (firstActivation && activationType == ActivationType.AddedToHierarchy)
            {
                navigationController = BeatSaberUI.CreateViewController<NavigationController>();

                _channelPostVC = BeatSaberUI.CreateViewController<ChannelPostViewController>();
                _channelListVC = BeatSaberUI.CreateViewController<ChannelListViewController>();
                _channelInfoVC = BeatSaberUI.CreateViewController<ChannelInfoViewController>();
                _postDetailVC = BeatSaberUI.CreateViewController<PostDetailViewController>();
                _settingsVC = BeatSaberUI.CreateViewController<SettingsViewController>();
                _channelListVC.channelClicked += ChannelList_channelClicked;
                _channelPostVC.postClicked += ChannelPost_postClicked;

                SetViewControllerToNavigationConctroller(navigationController, _channelPostVC);
                ProvideInitialViewControllers(navigationController, _channelListVC, _settingsVC);
                showBackButton = true;

            }
            StartCoroutine(HotReloadCoroutine());
        }

        private void ChannelPost_postClicked(Post post)
        {
            if (!_postDetailVC.isInViewControllerHierarchy)
            {
                PushViewControllerToNavigationController(navigationController, _postDetailVC);
            }
            _postDetailVC.SetDetails(post);
            SiaUtil.widePeepoHappy.MenuColorChanger.instance.SetColorOvertime(post.channel.theme, .25f);
        }

        public void Dismiss(ViewController vc)
        {
            _channelListVC.customListTableData.tableView.ClearSelection();
            DismissViewController(vc);
            if (_postDetailVC.isInViewControllerHierarchy)
                PopViewControllerFromNavigationController(navigationController);
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
                    if (_postDetailVC.ContentChanged)
                        HotReloadableViewController.RefreshViewController(_postDetailVC);
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
            if (e.FullPath == _postDetailVC.ResourceFilePath)
                _postDetailVC.MarkDirty();
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            SiaUtil.widePeepoHappy.MenuColorChanger.instance.RevertColors();
            if (_channelInfoVC.isInViewControllerHierarchy)
                DismissViewController(_channelInfoVC);
            _channelListVC.customListTableData.tableView.ClearSelection();
            base.BackButtonWasPressed(topViewController);
            BSMOTDManager.instance.MainFlowCoordinator.InvokePrivateMethod("DismissFlowCoordinator", new object[] { this, null, false });
        }
    }
}
