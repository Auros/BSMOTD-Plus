using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BeatSaberMarkupLanguage.Components.CustomListTableData;

namespace BSMOTD
{
    public class Post : CustomCellInfo
    {
        public string title;
        public string content;
        public DateTime uploaded;
        public string poster;
        public Texture2D texture;
        public Channel channel;

        public Post() : base("", "", null)
        {

        }

        public Post(string tx, string cx, string ux, string px, string ix, Channel chn) : base("", "", null)
        {
            channel = chn;

            title = tx;
            content = cx;
            uploaded = DateTime.Parse(ux);
            poster = px;

            text = $"[{chn.code}] {title}";
            subtext = $"on {uploaded.ToShortDateString()} by {poster}";

            if (!string.IsNullOrEmpty(ix) && Plugin.config.Value.LoadPostImages)
            {
                SharedCoroutineStarter.instance.StartCoroutine(SiaUtil.Utilities.LoadScripts.LoadTextureCoroutine(ix, (tex) =>
                {
                    texture = tex;
                    icon = tex;
                }));
            }
        }
    }
}
