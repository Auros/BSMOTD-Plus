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
        public string name;
        public string description;
        public string source;
        public Texture2D texture;
        public Color theme;
        public string code;
        public bool active;


        public Channel() : base("", "", null)
        {

        }

        public Channel(string nx, string dx, string sx, string ix, float rx, float gx, float bx, string cx, bool def = false) : base("", "", null)
        {
            name = nx;
            description = dx;
            source = sx;
            theme = new Color(rx, gx, bx);
            code = cx;
            text = nx;
            subtext = dx;

            if (Plugin.config.Value.ActiveChannels != null)
            {
                var e = Plugin.config.Value.ActiveChannels.Where(x => x.Equals(nx)).ToList();
                active = e.Count() > 0;
            }
            else
            {
                active = def;
            }

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
