using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BeatSaberMarkupLanguage.Components.CustomListTableData;

namespace BSMOTD
{
    public class Channel : CustomCellInfo
    {
        //public string title;
        //public string content;
        //public DateTime uploaded;
        //public string poster;
        public string name;
        public string description;
        public Texture2D texture;
        public Color theme;
        public string code;
        public bool active;


        public Channel() : base("", "", null)
        {

        }

        public Channel(string nx, string dx, string ix, float rx, float gx, float bx, string cx) : base("", "", null)
        {
            /*title = tx;
            content = cx;
            uploaded = DateTime.Parse(ux);
            poster = px;

            text = title;
            subtext = uploaded.ToShortDateString(); // + " at " + uploaded.ToShortTimeString() + " UTC";
            */

            name = nx;
            
            description = dx;

            theme = new Color(rx, gx, bx);
            code = cx;

            text = nx;
            subtext = dx;

            //var e = Plugin.config.Value.ActiveChannels.ToList().Where(x => x.Equals(cx));
            //active = e.Count() > 0;

            SharedCoroutineStarter.instance.StartCoroutine(SiaUtil.Utilities.LoadScripts.LoadTextureCoroutine(ix, (tex) =>
            {
                
                texture = tex;
                icon = tex;
                Logger.log.Info("Downloaded: " + ix);
            }));
        }
    }
}
