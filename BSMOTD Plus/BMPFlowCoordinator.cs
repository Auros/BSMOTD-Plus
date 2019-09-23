using BeatSaberMarkupLanguage;
using BSMOTD_Plus.ViewControllers;
using VRUI;
using IPA.Utilities;
using System.Collections.Generic;
using BSMOTD_Plus.Models;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Linq;
using UnityEngine;

namespace BSMOTD_Plus
{
    public class BMPFlowCoordinator : FlowCoordinator
    {
        protected static string APIURL = "https://bsmotdchannels.auros.dev/";

        public MainFlowCoordinator _mainFlowCoordinator;
        protected PostListViewController _postList;
        protected PostDetailViewController _postDetail;
        protected ChannelsViewController _channels;
        protected DismissableNavigationController _navCon;

        public static List<Channel> channels = new List<Channel>();
        public static List<AdvancedPost> posts = new List<AdvancedPost>();
        public static Channel activeChannel;


        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            if (firstActivation)
            {
                title = "BSMOTD Plus";

                _postList = BeatSaberUI.CreateViewController<PostListViewController>();
                _postDetail = BeatSaberUI.CreateViewController<PostDetailViewController>();
                _channels = BeatSaberUI.CreateViewController<ChannelsViewController>();

                _navCon = BeatSaberUI.CreateDismissableNavigationController();

                _postList.newPostClicked += ClickedPost;
                _channels.newChannelClicked += ClickedChannel;
                _navCon.didFinishEvent += Dismiss;

                
            }

            if (activationType == ActivationType.AddedToHierarchy)
            {
                SetViewControllersToNavigationConctroller(_navCon, _postList);
                ProvideInitialViewControllers(_navCon, _channels);
                SharedCoroutineStarter.instance.StartCoroutine(WaitC());
                firstClick = true;

                channels.Clear();
                SharedCoroutineStarter.instance.StartCoroutine(GetChannelInfo());
            }
        }

        private void ClickedChannel(Channel obj)
        {
            SharedCoroutineStarter.instance.StartCoroutine(GetNewsPost(obj.Source));
        }

        private void ClickedPost(AdvancedPost post)
        {
            if (!_postDetail.isInViewControllerHierarchy)
            {
                PushViewControllerToNavigationController(_navCon, _postDetail);

            }
            if (firstClick)
            {
                SharedCoroutineStarter.instance.StartCoroutine(WaitToSet(post));
                firstClick = false;
            }
            else
                _postDetail.SetDetails(post);

        }

        private IEnumerator WaitC()
        {
            yield return new WaitForSeconds(.5f);
            _channels.SetData(channels);
            _channels.newsListTableData.tableView.SelectCellWithIdx(0);
        }

        private IEnumerator WaitToSet(AdvancedPost post)
        {
            yield return new WaitForSeconds(.05f);
            _postDetail.SetDetails(post);
        }

        bool firstClick = true;
        private IEnumerator GetNewsPost(string URL)
        {
            UnityWebRequest getPosts = UnityWebRequest.Get(URL);
            yield return getPosts.SendWebRequest();

            if (getPosts.isNetworkError || getPosts.isHttpError)
                Logger.log.Error("Could not fetch posts from " + URL);
            else
            {
                JSONArray channelPosts = JSON.Parse(getPosts.downloadHandler.text).AsArray;

                if (channelPosts.Count == 0)
                    Dismiss(_navCon);

                foreach (JSONObject post in channelPosts)
                {
                    posts.Add(new AdvancedPost()
                    {
                        Image = post["image"],
                        Title = post["title"],
                        Description = post["description"],
                        Uploader = post["uploader"],
                        Uploaded = post["uploaded"]
                    });
                }
                title = $"BSMOTD - {activeChannel.Name}";
                _postList.SetData(posts);
            }
        }

        private IEnumerator WaitD()
        {
            yield return new WaitForSeconds(1f);
            Dismiss(_navCon);
        }

        private IEnumerator GetChannelInfo()
        {
            UnityWebRequest getChannels = UnityWebRequest.Get(APIURL);
            yield return getChannels.SendWebRequest();

            if (getChannels.isNetworkError || getChannels.isHttpError)
            {
                Logger.log.Error("Could not fetch public channel list.");
                title = "Host Connection Failed";
                SharedCoroutineStarter.instance.StartCoroutine(WaitD());
            }
                
            else
            {
                JSONArray channelNames = JSON.Parse(getChannels.downloadHandler.text).AsArray;

                foreach (JSONObject channel in channelNames)
                {
                    channels.Add(new Channel() {

                        Name = channel["name"],
                        Source = channel["source"],
                        ColorR = channel["colorR"].AsFloat,
                        ColorG = channel["colorG"].AsFloat,
                        ColorB = channel["colorB"].AsFloat,
                        Display = channel["display"].AsInt,
                        Image = channel["image"]
                    });
                }


                if (posts.Count == 0)
                {
                    activeChannel = channels.First();
                    SharedCoroutineStarter.instance.StartCoroutine(GetNewsPost(activeChannel.Source));
                }
                    
            }
        }

        private void Dismiss(DismissableNavigationController navCon)
        {
            (_mainFlowCoordinator as FlowCoordinator).InvokePrivateMethod("DismissFlowCoordinator", new object[] { this, null, false });
            //HarmonyUtil.UnpatchGame();
        }
    }
}
