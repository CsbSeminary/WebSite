using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

using Iris.Web;
using Iris.Web.UI;
using Iris.Web.UI.Skins;

using Kernel;

using Telerik.Web.UI;

using Seminary.Controls;

namespace Seminary.Pages
{
    public partial class DefaultPage : Page
    {
        #region Constants

        private const String BackCommandMenuXmlPath = "~/App_Data/Menus/CommandBar-Back.xml";

        #endregion

        #region Fields

        private Literal _appNameAndVersion;

        private RadMenu _backCommands;

        #endregion

        #region Properties

        private String Action
        {
            get { return Request["action"]; }
        }

        private String Name
        {
            get { return Request["name"]; }
        }

        #endregion

        #region Methods (initialization)

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            SetMasterAndTheme();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!String.IsNullOrEmpty(Request.ServerVariables["HTTP_X_ORIGINAL_URL"]))
                ((HtmlForm)ControlHelper.GetControl(Master, "HtmlForm")).Action = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];

            _appNameAndVersion = (Literal)Master.FindControl("ApplicationNameAndVersion");

            _backCommands = (RadMenu)Master.FindControl("BackCommandMenu");

            HtmlForm form = (HtmlForm) Master.FindControl("HtmlForm");
            form.Attributes["class"] = "shift";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadActionControl();

            MenuHelper.LoadMenu(this);

            if(!IsPostBack)
                LoadCommandMenus();

            HtmlGenericControl BannerBlurb = (HtmlGenericControl)Page.Master.FindControl("BannerBlurb");
            if (BannerBlurb != null)
            {
                BannerBlurb.InnerHtml = Name == "index.html"
                    ? "Shift iQ is a cloud-based solution to deliver skills development and training programs.<br />Create a competent and capable workforce throughout your organization."
                    : "Shift iQ is the skills development solution for a competent and capable workforce.";
            }

            HtmlAnchor LearnMoreLink = (HtmlAnchor)Page.Master.FindControl("LearnMoreLink");
            if (LearnMoreLink != null)
            {
                LearnMoreLink.InnerText = String.Empty;
                LearnMoreLink.HRef = "/community.html";
            }
        }

        #endregion

        #region Methods (action controls)

        private void LoadActionControl()
        {
            String controlPath = GetActionControlPath();

            if (!String.IsNullOrEmpty(controlPath))
            {
                Control actionControl = LoadControl(controlPath);
                ActionControlContainer.Controls.Add(actionControl);

                if (_appNameAndVersion != null)
                    _appNameAndVersion.Text = String.Format("Version {1}", KernelSettings.ApplicationName, KernelSettings.ApplicationVersion);
            }

            HiddenField RateScreenUrl = (HiddenField)ControlHelper.GetControl(Page, "RateScreenUrl");

            if(RateScreenUrl != null)
                RateScreenUrl.Value = "/pages/default.aspx?action=submit-rating";
        }

        #endregion

        #region Methods (menu)

        private void LoadCommandMenus()
        {
            if (_backCommands != null)
            {
                String xml = GetMenuXml(BackCommandMenuXmlPath);
                _backCommands.LoadXml(xml);

                ScreenHistoryHelper helper = new ScreenHistoryHelper();
                helper.LoadMenu(_backCommands);

                _backCommands.Visible = false;
            }
        }

        private static String GetMenuXml(String relativePath)
        {
            String physicalPath = HttpContext.Current.Server.MapPath(relativePath);

            XmlDocument doc = new XmlDocument();
            doc.Load(physicalPath);

            return doc.InnerXml;
        }

        #endregion

        #region Methods (helpers)

        private String GetActionControlPath()
        {
            String action = (Action ?? String.Empty).ToLower(CultureHelper.Current);

            switch (action)
            {
                case "view-html":
                    return GetHtmlControlPath();
                default:
                    HttpResponseHelper.SendHttp404();
                    return null;
            }
        }

        private String GetHtmlControlPath()
        {
            String pageName = (Name ?? String.Empty).ToLower(CultureHelper.Current);
            String controlName;

            switch (pageName)
            {
                default:
                    controlName = "~/Controls/PageViewer.ascx";
                    break;
            }

            return controlName;
        }

        private void SetMasterAndTheme()
        {
            String pageName = (Name ?? String.Empty).ToLower(CultureHelper.Current);

            // MasterPageFile = pageName == "index.html" ? "~/App_Masters/InSite/Index.master" : "~/App_Masters/InSite/Content.master";
            // Theme = "InSite";
        }

        #endregion
    }
}