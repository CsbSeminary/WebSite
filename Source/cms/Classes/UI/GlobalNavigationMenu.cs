using System;
using System.Web.UI;

using Csbs;

using Csbs.Web.UI;
using System.Collections.Generic;

using Csbs.Data.Menu;
using Csbs.Data.Pages;
using Csbs.Data.Articles;

namespace Csbs.Web.UI
{
    public class GlobalNavigationMenu : Control
    {
        #region Constants

        private const String MenuNameViewStateKey = "MenuName";
        private const String CssClassViewStateKey = "CssClass";

        #endregion

        #region Properties

        public String MenuName
        {
            get { return (String)ViewState[MenuNameViewStateKey]; }
            set { ViewState[MenuNameViewStateKey] = value; }
        }

        public String CssClass
        {
            get
            {
                if (ViewState[CssClassViewStateKey] == null)
                    return "global-navigation-menu";

                return (String)ViewState[CssClassViewStateKey];
            }
            set
            {
                ViewState[CssClassViewStateKey] = value;
            }
        }

        #endregion

        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);

            writer.RenderBeginTag("div");

            if (!String.IsNullOrEmpty(MenuName))
            {
                PageInfo currentPage = Page is Csbs.View ? (Page as Csbs.View).CurrentPageInfo : null;
                BlogArticle currentArticle = Page is Csbs.Pages.Entries ? (Page as Csbs.Pages.Entries).CurrentArticle : null;

                MenuManager.Current.Render(writer, new String[] { MenuName }, currentPage, currentArticle, Data.MenuRenderingType.Horizontal);
            }

            writer.RenderEndTag();
        }

        #endregion
    }
}