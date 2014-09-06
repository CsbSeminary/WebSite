using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Controls;
using Csbs.Utilities;

namespace Csbs.Pages
{
    public partial class ViewControl : System.Web.UI.Page
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
                var widgetsHolder = (ContentPlaceHolder)Page.Master.FindControl("Widgets");

                widgetsHolder.Visible = false;
            }
        }

        #endregion

        #region Helper methods

        private void InitPage(String action)
        {
            String controlPath = null;
            String title = "&nbsp;";

            if (!String.IsNullOrEmpty(action))
            {
                switch (action.ToLower())
                {
                    case ChapelViewer.ActionName:
                        controlPath = "~/Controls/ChapelViewer.ascx";
                        title = "Chapel";
                        break;
                    case ScheduleViewer.ActionName:
                        controlPath = "~/Controls/ScheduleViewer.ascx";
                        title = "CSBS Event Calendar";
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
                Container.Controls.Add(new LiteralControl("<div style='margin: 150px 0; text-align: center;'>Control Not Found</div>"));
            }

            Page.Title = "CSBS :: " + title;
            // HeaderTitle.Text = title;
        }

        #endregion
    }
}