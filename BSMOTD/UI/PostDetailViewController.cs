using BeatSaberMarkupLanguage.Attributes;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BSMOTD.UI
{
    public class PostDetailViewController : HotReloadableViewController
    {
        public override string ResourceName => "BSMOTD.Views.post-detail.bsml";

        public override string ResourceFilePath => BeatSaber.InstallPath + "\\BSMOTDHR\\post-detail.bsml";

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);

            if (firstActivation)
            {
                rectTransform.anchorMin = new Vector3(0.5f, 0, 0);
                rectTransform.anchorMax = new Vector3(0.5f, 1, 0);
                rectTransform.sizeDelta = new Vector3(70, 0, 0);

                if (image.gameObject.GetComponent<AspectRatioFitter>() == null)
                {
                    var element = image.gameObject.AddComponent<AspectRatioFitter>();
                    element.aspectRatio = 1f;
                    element.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                }
            }
        }

        [UIComponent("articlepicture")]
        private RawImage image;

        [UIComponent("articletitle")]
        private TextMeshProUGUI titleText;

        [UIComponent("description")]
        TextPageScrollView scrollView;

        public void SetDetails(Post post)
        {
            titleText.text = post.title;
            image.texture = post.texture;
            scrollView.SetText(post.content);
        }
    }
}
