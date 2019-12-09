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
    public class BSMOTDManager : PersistentSingleton<BSMOTDManager>
    {
        internal bool ShowMenuQueue = false;
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
                    channels.Add(new Channel(channel["name"], channel["description"], channel["image"], channel["color"]["r"].AsFloat, channel["color"]["g"].AsFloat, channel["color"]["b"].AsFloat, channel["code"]));

                }
            }
        }
    }
}
