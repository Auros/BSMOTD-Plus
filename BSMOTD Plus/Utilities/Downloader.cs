using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace BSMOTD_Plus.Utilities
{
    public class Downloader
    {
    }

    public static class Sprites
    {
        public static Material NoGlowMat
        {
            get
            {
                if (noGlowMat == null)
                {
                    noGlowMat = new Material(Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlow").First())
                    {
                        name = "UINoGlowCustom"
                    };
                }
                return noGlowMat;
            }
        }
        private static Material noGlowMat;

        private static Sprite _blankSprite = null;
        public static Sprite BlankSprite
        {
            get
            {
                if (!_blankSprite)
                    _blankSprite = Sprite.Create(Texture2D.blackTexture, new Rect(), Vector2.zero);
                return _blankSprite;
            }
        }
    }

    public class LoadScripts
    {
        //static public Dictionary<string, Sprite> _cachedSprites = new Dictionary<string, Sprite>();

        public static IEnumerator LoadSpriteCoroutine(string spritePath, Action<Sprite> done)
        {
            Texture2D tex;

            //if (_cachedSprites.ContainsKey(spritePath))
            //{
            //    done?.Invoke(_cachedSprites[spritePath]);
            //    yield break;
            //}

            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(spritePath))
            {
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    Logger.log.Error("Connection Error!");
                }
                else
                {
                    tex = DownloadHandlerTexture.GetContent(www);
                    tex.wrapMode = TextureWrapMode.Clamp;
                    yield return new WaitForSeconds(.05f);
                    var newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f, 100, 1);
                    //_cachedSprites.Add(spritePath, newSprite);
                    done?.Invoke(newSprite);
                }
            }
        }
    }


}