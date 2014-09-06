using System;
using System.Web.UI.WebControls;

using Telerik.Web.UI;
using Telerik.Web.UI.Editor.DialogControls;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class ManagerOpener : RadDialogOpener
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Skin = "Sitefinity";

            FileManagerDialogParameters imageManagerParameters = new FileManagerDialogParameters();
            imageManagerParameters.ViewPaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageManagerParameters.UploadPaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageManagerParameters.DeletePaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageManagerParameters.MaxUploadFileSize = 10485760;
            //imageManagerParameters.SearchPatterns = new String[] { "*.jpg", "*.png", "*.gif" };

            DialogDefinition imageManager = new DialogDefinition(typeof(ImageManagerDialog), imageManagerParameters);
            imageManager.ClientCallbackFunction = "ImageManagerCallback";
            imageManager.Width = Unit.Pixel(694);
            imageManager.Height = Unit.Pixel(440);

            DialogDefinitions.Add("ImageManager", imageManager);

            FileManagerDialogParameters documentManagerParameters = new FileManagerDialogParameters();
            documentManagerParameters.ViewPaths = new String[] { Settings.HtmlEditor.Downloads.Documents };
            documentManagerParameters.UploadPaths = new String[] { Settings.HtmlEditor.Downloads.Documents };
            documentManagerParameters.DeletePaths = new String[] { Settings.HtmlEditor.Downloads.Documents };
            documentManagerParameters.MaxUploadFileSize = 10485760;

            DialogDefinition documentManager = new DialogDefinition(typeof(DocumentManagerDialog), documentManagerParameters);
            documentManager.ClientCallbackFunction = "DocumentManagerCallback";
            documentManager.Width = Unit.Pixel(694);
            documentManager.Height = Unit.Pixel(440);

            DialogDefinitions.Add("DocumentManager", documentManager);

            FileManagerDialogParameters imageEditorParameters = new FileManagerDialogParameters();
            imageEditorParameters.ViewPaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageEditorParameters.UploadPaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageEditorParameters.DeletePaths = new String[] { Settings.HtmlEditor.Downloads.Images };
            imageEditorParameters.MaxUploadFileSize = 10485760;

            DialogDefinition imageEditor = new DialogDefinition(typeof(ImageEditorDialog), imageEditorParameters);
            imageEditor.Width = Unit.Pixel(832);
            imageEditor.Height = Unit.Pixel(520);
            DialogDefinitions.Add("ImageEditor", imageEditor);
        }
    }
}