using System;
using System.Collections;
using System.Web.UI;

using Csbs.Utilities;

using Csbs.Data;
using Csbs.Data.Pages;

namespace Csbs.Web.UI
{
    public partial class PageEditor : UserControl
    {
        #region Constants

        public const String ActionName = "edit-page";

        public const String PageIdQueryStringKey = "id";

        #endregion

        #region Proeprties

        private Int32? PageID
        {
            get { return StringHelper.ToInt32Nullable(Request[PageIdQueryStringKey]); }
        }

        private PageInfo PageInfo
        {
            get
            {
                if (_page == null && PageID.HasValue)
                    _page = PageManager.Current.FindPage(PageID.Value);

                return _page;
            }
        }

        #endregion

        #region Fields

        private PageInfo _page = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SaveButton.Click += SaveButton_Click;
            CancelButton.Click += CancelButton_Click;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (PageInfo != null)
                {
                    DataBind();
                    SetInputValues();
                }
                else
                {
                    ShowMessage("Page is undefined.");
                    JsRedirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(PageFinder.ActionName)));
                }
            }
        }

        #endregion

        #region Event handlers

        private void SaveButton_Click(Object sender, EventArgs e)
        {
            Boolean isCancel = false;

            try
            {
                if (PageInfo != null)
                {
                    // Tab 1
                    if (String.Compare(PageInfo.Name, PageName.Text, true) != 0)
                    {
                        if (!PageInfo.SetName(PageName.Text))
                        {
                            isCancel = true;
                            ShowMessage(String.Format("Page named '{0}' already created.", PageName.Text));
                        }
                    }

                    PageInfo.Title = PageTitle.Text;
                    PageInfo.Theme = MasterTheme.Value;

                    // Tab 2
                    PageInfo.HeaderLayout = (ContentHeaderLayoutType)HeaderLayout.ValueAsInt32.Value;
                    PageInfo.HeaderPosition = (ContentHeaderPositionType)HeaderPosition.ValueAsInt32.Value;
                    PageInfo.HeaderTitle = HeaderTitle.Text;
                    PageInfo.HeaderLinkVisible = ShowHeaderLink.Checked;
                    PageInfo.HeaderLinkUrl =
                        !String.IsNullOrEmpty(HeaderLinkUrl.Value)
                            ? HeaderLinkUrl.Value
                            : HeaderLinkUrl.Text;
                    PageInfo.HeaderLinkText = HeaderLinkText.Text;
                    PageInfo.HeaderLinkTarget = HeaderLinkTarget.Value;

                    // Tab 3
                    PageInfo.ContentLayout = (ContentLayoutType)ContentLayout.ValueAsInt32.Value;
                    PageInfo.ContentPostion = (ContentPositionType)PageContentColumnPosition.ValueAsInt32.Value;
                    PageInfo.SideColumnSize = (ContentLayoutSideColumnSize)SideColumnSize.ValueAsInt32.Value;
                    PageInfo.ShowColumnsSeparator = ShowColumnSeparator.Checked;
                    PageInfo.MenuPosition = (MenuPositionType)MenuPosition.ValueAsInt32.Value;
                    SetMenuItems(PageInfo, MenuItemsCache.Value);

                    // Tab 4
                    PageInfo.AdminOnly = AdminOnly.ValueAsBoolean.Value;
                    PageInfo.MetaDescription = MetaDescription.Text;
                    PageInfo.MetaKeywords = MetaKeywords.Text;

                    if (!isCancel)
                    {
                        if (PageInfo.SetContentHtml(ContentColumnEditor.Content, SideContentColumnEditor.Content, HeaderTemplateEditor.Content))
                        {
                            PageManager.Current.Save();

                            ShowMessage("Your changes have been saved.");
                            JsRedirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(PageFinder.ActionName)));
                        }
                        else
                        {
                            isCancel = true;
                            ShowMessage(String.Format("An error has occured when HTML file saving.", PageName.Text));
                        }
                    }
                }
            }
            finally
            {
                PageManager.Current.Reset();
            }
        }

        private void CancelButton_Click(Object sender, EventArgs e)
        {
            LinkUtils.RedirectToAdminPage(PageFinder.ActionName);
        }

        #endregion

        #region Setting and getting input values

        private void GetInputValues()
        {

        }

        private void SetInputValues()
        {
            // Tab 1
            PageName.Text = PageInfo.Name;
            PageTitle.Text = PageInfo.Title;
            MasterPage.Value = PageInfo.MasterPage;

            MasterTheme.DataBind();
            MasterTheme.Value = PageInfo.Theme;

            // Tab 2
            HeaderLayout.ValueAsInt32 = (Int32)PageInfo.HeaderLayout;
            HeaderPosition.ValueAsInt32 = (Int32)PageInfo.HeaderPosition;
            HeaderTitle.Text = PageInfo.HeaderTitle;
            ShowHeaderLink.Checked = PageInfo.HeaderLinkVisible;
            HeaderLinkUrl.Text = PageInfo.HeaderLinkUrl;
            HeaderLinkText.Text = PageInfo.HeaderLinkText;
            HeaderLinkTarget.Value = PageInfo.HeaderLinkTarget;
            HeaderTemplateEditor.Content = PageInfo.HeaderTemplateHtml;

            // Tab 3
            ContentLayout.ValueAsInt32= (Int32)PageInfo.ContentLayout;
            PageContentColumnPosition.ValueAsInt32= (Int32)PageInfo.ContentPostion;
            SideColumnSize.ValueAsInt32= (Int32)PageInfo.SideColumnSize;
            ShowColumnSeparator.Checked = PageInfo.ShowColumnsSeparator;
            MenuPosition.ValueAsInt32 = (Int32)PageInfo.MenuPosition;
            MenuItemsCache.Value = GetMenuItems(PageInfo.GetMenuItems());

            // Step 4
            AdminOnly.ValueAsBoolean = PageInfo.AdminOnly;
            MetaDescription.Text = PageInfo.MetaDescription;
            MetaKeywords.Text = PageInfo.MetaKeywords;
            ContentColumnEditor.Content = PageInfo.ContentColumnHtml;
            SideContentColumnEditor.Content = PageInfo.SideColumnHtml;
        }

        private static String GetMenuItems(IEnumerable items)
        {
            String result = String.Empty;

            foreach (String item in items)
            {
                result += ";" + item;
            }

            return String.IsNullOrEmpty(result) ? result : result.Substring(1);
        }

        private static void SetMenuItems(PageInfo page, String menuItems)
        {
            page.ClearMenuItems();

            if (!String.IsNullOrEmpty(menuItems))
            {
                String[] items = menuItems.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (String menuItemsName in items)
                {
                    page.AddMenuItem(menuItemsName);
                }
            }
        }

        #endregion

        #region PreRender

        protected override void OnPreRender(EventArgs e)
        {
            EnableValidators();

            if (PageInfo != null)
            {
                String jScript = "\r\n";
                jScript += String.Format("var pageEditor_sectionName = {0};\r\n", JavaScriptHelper.GetJsValue(PageInfo.SectionName));
                jScript += String.Format("var pageEditor_masterPageName = {0};\r\n", JavaScriptHelper.GetJsValue(MasterPage.Value));
                jScript += String.Format("var pageEditor_masterPageThemeName = {0};\r\n", JavaScriptHelper.GetJsValue(MasterTheme.Value));
                jScript += String.Format("var pageEditor_headerPosition = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderPosition.ValueAsInt32));
                jScript += String.Format("var pageEditor_headerLayout = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderLayout.ValueAsInt32));
                jScript += String.Format("var pageEditor_headerTitle = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderTitle.Text, "''"));
                jScript += String.Format("var pageEditor_showHeaderLink = {0};\r\n", JavaScriptHelper.GetJsValue(ShowHeaderLink.Checked));
                jScript += String.Format("var pageEditor_headerLinkText = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderLinkText.Text, "''"));
                jScript += String.Format("var pageEditor_headerTemplateHtml = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderTemplateEditor.Content, "''"));
                jScript += String.Format("var pageEditor_contentLayout = {0};\r\n", JavaScriptHelper.GetJsValue(ContentLayout.ValueAsInt32, "''"));
                jScript += String.Format("var pageEditor_sideColumnSize = {0};\r\n", JavaScriptHelper.GetJsValue(SideColumnSize.ValueAsInt32, "''"));
                jScript += String.Format("var pageEditor_contentColumnPosition = {0};\r\n", JavaScriptHelper.GetJsValue(PageContentColumnPosition.ValueAsInt32, "''"));
                jScript += String.Format("var pageEditor_showColumnSeparator = {0};\r\n", JavaScriptHelper.GetJsValue(ShowColumnSeparator.Checked));
                jScript += String.Format("var pageEditor_menuPosition = {0};\r\n", JavaScriptHelper.GetJsValue(MenuPosition.ValueAsInt32));
                jScript += String.Format("var pageEditor_contentColumnHtml = {0};\r\n", JavaScriptHelper.GetJsValue(ContentColumnEditor.Content, "''"));
                jScript += String.Format("var pageEditor_sideColumnHtml = {0};\r\n", JavaScriptHelper.GetJsValue(SideContentColumnEditor.Content, "''"));

                ScriptManager.RegisterStartupScript(Page, GetType(), "jscript", jScript, true);
            }

            base.OnPreRender(e);
        }

        #endregion

        #region Helpers

        private void EnableValidators()
        {
            HeaderTitleValidator.Enabled = HeaderLayout.ValueAsInt32 == (Int32)ContentHeaderLayoutType.Text;
            HeaderLinkUrlValidator.Enabled = HeaderLayout.ValueAsInt32 == (Int32)ContentHeaderLayoutType.Text && ShowHeaderLink.Checked;
            HeaderLinkTextValidator.Enabled = HeaderLayout.ValueAsInt32 == (Int32)ContentHeaderLayoutType.Text && ShowHeaderLink.Checked;
        }

        private void ShowMessage(String msg)
        {
            if (String.IsNullOrEmpty(msg))
                return;

            ScriptManager.RegisterStartupScript(Page, GetType(), "message_script", JavaScriptHelper.GetShowMessageScript(msg), true);
        }

        private void JsRedirect(String url)
        {
            if (String.IsNullOrEmpty(url))
                return;

            ScriptManager.RegisterStartupScript(Page, GetType(), "redirect_script", JavaScriptHelper.GetRedirectScript(url), true);
        }

        #endregion
    }
}