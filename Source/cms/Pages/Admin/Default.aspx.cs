using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Utilities;
using Csbs.Web.UI;

using Telerik.Web.UI;

namespace Csbs.Pages
{
    public partial class Default : RadAjaxPage
    {
        #region Properties

        private String Action
        {
            get
            {
                return Request[LinkUtils.ActionNameQueryStringKey] == null
                    ? String.Empty
                    : Request[LinkUtils.ActionNameQueryStringKey].ToLowerInvariant();
            }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            InitPage(Action);
            base.OnInit(e);

            if (!Page.IsPostBack)
            {
                var widgetsHolder = (ContentPlaceHolder) Page.Master.FindControl("Widgets");

                widgetsHolder.Visible = false;
            }
        }

        #endregion

        #region Helper methods

        private void InitPage(String action)
        {
            String controlPath = "~/Controls/AdminHome.ascx";
            String title = "Administrator";

            if (!String.IsNullOrEmpty(action))
            {
                switch (action.ToLower())
                {
                    case BlogFinder.ActionName:
                        controlPath = "~/Controls/BlogFinder.ascx";
                        title = "Article Finder";
                        break;
                    case BlogEditor.ActionName:
                        controlPath = "~/Controls/BlogEditor.ascx";
                        title = "Article Editor";
                        break;
                    case PageFinder.ActionName:
                        controlPath = "~/Controls/PageFinder.ascx";
                        title = "Page Finder";
                        break;
                    case PageCreator.ActionName:
                        controlPath = "~/Controls/PageCreator.ascx";
                        title = "Page Creator";
                        break;
                    case PageEditor.ActionName:
                        controlPath = "~/Controls/PageEditor.ascx";
                        title = "Page Editor";
                        break;
                    case MenuFinder.ActionName:
                        controlPath = "~/Controls/MenuFinder.ascx";
                        title = "Menu Finder";
                        break;
                    case MenuEditor.ActionName:
                        controlPath = "~/Controls/MenuEditor.ascx";
                        title = "Menu Editor";
                        break;
                    case MenuSectionEditor.ActionName:
                        controlPath = "~/Controls/MenuSectionEditor.ascx";
                        title = "Menu Section Editor";
                        break;
                    case SettingsEditor.ActionName:
                        controlPath = "~/Controls/SettingsEditor.ascx";
                        title = "Settings Editor";
                        break;
                }
            }

            if (!String.IsNullOrEmpty(controlPath))
            {
                Control control = LoadControl(controlPath);
                Container.Controls.Add(control);
            }
            else
            {
                Container.Controls.Add(new LiteralControl("<div style='margin: 150px 0; text-align: center;'>Page not found!</div>"));
            }

            Page.Title = "CSBS :: " + title;
            HeaderTitle.Text = title;
        }

        #endregion
    }
}