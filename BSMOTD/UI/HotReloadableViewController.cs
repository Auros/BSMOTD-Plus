using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using System.IO;
using System.Reflection;

namespace BSMOTD.UI
{
    public abstract class HotReloadableViewController : BSMLViewController
    {
        public static void RefreshViewController(HotReloadableViewController viewController, bool forceReload = false)
        {
            if (viewController.ContentChanged || forceReload)
            {
                try
                {
                    viewController.__Deactivate(DeactivationType.NotRemovedFromHierarchy, false);
                    for (int i = 0; i < viewController.transform.childCount; i++)
                        Destroy(viewController.transform.GetChild(i).gameObject);
                    viewController.__Activate(ActivationType.NotAddedToHierarchy);
                }
                catch (Exception ex)
                {
                    Logger.log?.Error(ex);
                }
            }
            else Logger.log.Info("Did not reload controller");
        }

        public abstract string ResourceName { get; }
        public abstract string ResourceFilePath { get; }

        private string _content;
        public override string Content
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                {
                    try
                    {
                        _content = File.ReadAllText(ResourceFilePath);
                    }
                    catch
                    {
                        Logger.log?.Warn($"Unable to read file {ResourceFilePath} for {name}");
                    }
                }
                else if (!string.IsNullOrEmpty(ResourceName))
                    _content = Utilities.GetResourceContent(Assembly.GetAssembly(GetType()), ResourceName);
                return _content;
            }
        }

        public bool ContentChanged { get; protected set; }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            if (ContentChanged && !firstActivation)
            {
                ContentChanged = false;
                BSMLParser.instance.Parse(Content, gameObject, this);
            }
            base.DidActivate(firstActivation, type);
        }
        public void MarkDirty()
        {
            ContentChanged = true;
            _content = null;
        }


    }
}