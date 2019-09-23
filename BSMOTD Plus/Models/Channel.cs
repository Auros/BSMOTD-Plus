using UnityEngine;

namespace BSMOTD_Plus.Models
{
    public class Channel
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public float ColorR { get; set; }
        public float ColorG { get; set; }
        public float ColorB { get; set; }
        public int Display { get; set; }
        public string Image { get; set; }
        public Texture2D Texture { get; set; }
    }
}
