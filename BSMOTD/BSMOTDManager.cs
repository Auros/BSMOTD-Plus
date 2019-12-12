using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BS_Utils.Utilities;
using BSMOTD.UI;
using SiaUtil.External;
using UnityEngine;
using UnityEngine.Networking;

namespace BSMOTD
{
    public enum VersionCodes
    {
        Ambrosia
    }

    public class BSMOTDManager : PersistentSingleton<BSMOTDManager>
    {
        private void Awake()
        {
            ChannelAdded += CHN_ADD;
            ChannelRemoved += CHN_DES;
            ListModified += CHN_MOD;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ChannelAdded -= CHN_ADD;
            ChannelRemoved -= CHN_DES;
            ListModified -= CHN_MOD;
        }

        private void CHN_ADD(Channel obj) { }
        private void CHN_DES(Channel obj) { }
        private void CHN_MOD()
        {
            //we stan loona in this household

            ListDirty = true;
        }

        public bool ListDirty { get; internal set; }
        

        internal VersionCodes currentCode = VersionCodes.Ambrosia;
        internal BSMOTDFlowCoordinator flowC;

        private MainFlowCoordinator _mainFlowCoordinator;
        internal MainFlowCoordinator MainFlowCoordinator
        {
            get
            {
                if (_mainFlowCoordinator == null)
                    _mainFlowCoordinator = Resources.FindObjectsOfTypeAll<MainFlowCoordinator>().FirstOrDefault();
                return _mainFlowCoordinator;
            }
        }

        public void InvokeFlowCoordinator()
        {
            if (flowC == null)
                flowC = BeatSaberUI.CreateFlowCoordinator<BSMOTDFlowCoordinator>();
            MainFlowCoordinator.InvokeMethod("PresentFlowCoordinator", flowC, null, false, false);
        }

        internal List<Channel> channels = new List<Channel>();
        internal List<Post> posts = new List<Post>();

        internal Action<Channel> ChannelAdded;
        internal Action<Channel> ChannelRemoved;
        internal Action ListModified;

        internal IEnumerator LoadChannels()
        {
            channels = new List<Channel>();
            UnityWebRequest getChannels = UnityWebRequest.Get("https://cdn.auros.dev/bsmotd/publicchannels.json");
            yield return getChannels.SendWebRequest();

            if (getChannels.isNetworkError || getChannels.isHttpError)
                Logger.log.Error("Could not fetch public channel list.");
            else
            {
                JSONArray channelNames = JSON.Parse(getChannels.downloadHandler.text).AsArray;
                foreach (JSONObject channel in channelNames)
                {
                    if (Enum.TryParse(channel["ver"], out VersionCodes code) && code >= currentCode)
                        channels.Add(new Channel(channel["name"], channel["description"], channel["source"], channel["image"], channel["color"]["r"].AsFloat, channel["color"]["g"].AsFloat, channel["color"]["b"].AsFloat, channel["code"], channel["default"]));
                }
                LoadAllActivePosts();
            }
        }

        internal void LoadAllActivePosts()
        {
            foreach (var chn in channels)
            {
                if (chn.active)
                    StartCoroutine(AddPosts(chn));
            }
        }

        internal IEnumerator AddPosts(Channel chn)
        {
            List<Post> newPosts = new List<Post>();
            UnityWebRequest getPosts = UnityWebRequest.Get(chn.source);
            yield return getPosts.SendWebRequest();
            
            if (getPosts.isNetworkError || getPosts.isHttpError)
                Logger.log.Error("Could not fetch " + chn.name + "'s posts.");
            else
            {
                JSONArray postNames = JSON.Parse(getPosts.downloadHandler.text).AsArray;
                if (chn.code == "MNC")
                {
                    foreach (JSONObject post in postNames)
                    {
                        newPosts.Add(new Post(
                           post["post"]["title"],
                           post["post"]["content"],
                           post["post"]["uploaded"],
                           post["user"] + "#" + post["disc"],
                           post["post"]["image"],
                           chn
                        ));
                    }
                }
                else
                {
                    foreach (JSONObject post in postNames)
                    {
                        newPosts.Add(new Post(
                           post["title"],
                           post["content"],
                           post["uploaded"],
                           post["author"],
                           post["image"],
                           chn
                        ));
                    }
                }
                if (newPosts.Count != 0)
                {
                    posts.AddRange(newPosts);
                    ListModified.Invoke();
                }
            }
        }

        internal void RemovePosts(Channel chn)
        {
            List<Post> isolated = posts.Where(x => x.channel == chn).ToList();
            foreach (var iso in isolated)
                posts.Remove(iso);
            ListModified.Invoke();
        }
    }
}
