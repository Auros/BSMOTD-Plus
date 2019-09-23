using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BSMOTD_Plus.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BSMOTD_Plus.ViewControllers
{
    public class PostDetailViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMOTD_Plus.Views.postdetail.bsml";


        [UIComponent("articlepicture")]
        private RawImage image;

        [UIComponent("articletitle")]
        private TextMeshProUGUI titleText;

        [UIComponent("description")]
        TextPageScrollView scrollView;

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

        public void SetDetails(AdvancedPost post)
        {
            titleText.text = post.Title;
            image.texture = post.Texture;
            scrollView.SetText(post.Description);
            scrollView.SetText("Placeholder");
            scrollView.SetText(post.Description);
        }
    }
}
