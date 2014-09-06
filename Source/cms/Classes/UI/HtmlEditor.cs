using System;
using System.IO;
using System.Collections;

using Telerik.Web.UI;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class HtmlEditor : RadEditor
    {
        #region Overriden methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Skin = "Sitefinity";
            AutoResizeHeight = true;
            EnableResize = true;
            ContentAreaCssFile = "~/Styles/Editor.css";
            ToolsFile = "~/Styles/EditorTools.xml";

            ImageManager.ViewPaths = new[] { Settings.HtmlEditor.Downloads.Images };
            ImageManager.UploadPaths = new[] { Settings.HtmlEditor.Downloads.Images };
            ImageManager.DeletePaths = new[] { Settings.HtmlEditor.Downloads.Images };
            ImageManager.MaxUploadFileSize = 10485760;

            DocumentManager.ViewPaths = new[] { Settings.HtmlEditor.Downloads.Documents };
            DocumentManager.UploadPaths = new[] { Settings.HtmlEditor.Downloads.Documents };
            DocumentManager.DeletePaths = new[] { Settings.HtmlEditor.Downloads.Documents };
            DocumentManager.MaxUploadFileSize = 10485760;

            FlashManager.ViewPaths = new[] { Settings.HtmlEditor.Downloads.Flash };
            FlashManager.UploadPaths = new[] { Settings.HtmlEditor.Downloads.Flash };
            FlashManager.DeletePaths = new[] { Settings.HtmlEditor.Downloads.Flash };

            MediaManager.ViewPaths = new[] { Settings.HtmlEditor.Downloads.Media };
            MediaManager.UploadPaths = new[] { Settings.HtmlEditor.Downloads.Media };
            MediaManager.DeletePaths = new[] { Settings.HtmlEditor.Downloads.Media };
            MediaManager.MaxUploadFileSize = 10485760;

            TemplateManager.ViewPaths = new[] { Settings.HtmlEditor.Downloads.Templates };
            TemplateManager.UploadPaths = new[] { Settings.HtmlEditor.Downloads.Templates };
            TemplateManager.DeletePaths = new[] { Settings.HtmlEditor.Downloads.Templates };

            SpellCheckSettings.DictionaryLanguage = "en-US";
            SpellCheckSettings.FragmentIgnoreOptions = FragmentIgnoreOptions.All;
            SpellCheckSettings.DictionaryPath = "~/App_Data/RadSpell";

            if (!Page.IsPostBack)
                GetLinks();
        }

        #endregion
        
        #region Helper methods

        private void GetLinks()
        {
            Links.Clear();

            PageManager.Current.AddEditorPageLinks(Links);
            BlogManager.Current.AddEditorPageLinks(Links);
        }

        #endregion
    }
}