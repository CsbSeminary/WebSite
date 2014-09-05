using System;
using System.Web.UI;

using Csbs.Web.UI;

using Csbs.Data.Pages;
using Csbs.Utilities;

namespace Csbs
{
    public partial class View : Page
    {
        #region Fields

        private PageInfo _page = null;

        #endregion

        #region Properties

        public String PageName
        {
            get { return Page.Request.QueryString["name"]; }
        }

        public PageInfo CurrentPageInfo
        {
            get
            {
                if (!String.IsNullOrEmpty(PageName) && _page == null)
                    _page = PageManager.Current.FindPage(PageName);

                return _page;
            }
        }

        #endregion

        #region Initialization

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            try
            {
                if (CurrentPageInfo != null)
                    MasterPageFile = CurrentPageInfo.MasterPageFilePath;
                else
                    Page.Response.Redirect(Settings.Pages.System.PageNotFound);
            }
            catch
            {
                
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            try
            {
                if (CurrentPageInfo != null)
                {
                    if (CurrentPageInfo.AdminOnly && !Page.User.Identity.IsAuthenticated)
                        Page.Response.Redirect(Settings.Pages.System.Maintenance);

                    Page.Title = CurrentPageInfo.FullTitle;

                    CurrentPageInfo.RenderHead(ContentHeader);
                    CurrentPageInfo.RenderContent(ContentContainer);
                }
                else
                {
                    Page.Response.Redirect(Settings.Pages.System.PageNotFound);
                }
            }
            catch
            {
                Page.Response.Redirect(Settings.Pages.System.Maintenance);
            }
        }

        #endregion
    }
}
