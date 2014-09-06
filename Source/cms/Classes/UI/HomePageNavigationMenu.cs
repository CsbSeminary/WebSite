using System;
using System.Text;
using System.Web.UI;

using Csbs;

using Csbs.Web.UI;

using Csbs.Data;
using Csbs.Data.Menu;

namespace Csbs.Web.UI
{
    public class HomePageNavigationMenu : Control
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
        
        #endregion

        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(MenuName))
            {
                MenuInfo menu = MenuManager.Current.FindMenuByName(MenuName);

                if (menu != null)
                {
                    StringBuilder sb = new StringBuilder();

                    menu.Render(sb, null, null, MenuRenderingType.Vertical);

                    writer.Write(sb);
                }
            }
        }

        #endregion
    }
}