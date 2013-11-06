using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Csbs.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Data;
using Csbs.Data.Pages;
using Csbs.Utilities;

using Telerik.Web.UI;

namespace Csbs.Web.UI
{
    public partial class PageCreator : UserControl
    {
        #region Constants

        public const String ActionName = "create-page";

        public const String SectionNameQueryStringKey = "section";

        #endregion

        #region Proeprties

        private String SectionName
        {
            get { return Request[SectionNameQueryStringKey] == null ? String.Empty : Request[SectionNameQueryStringKey].ToLowerInvariant(); }
        }

        private PageSectionInfo Section
        {
            get
            {
                if (_section == null && !String.IsNullOrEmpty(SectionName))
                    _section = PageManager.Current.FindSection(SectionName);

                return _section;
            }
        }

        #endregion

        #region Fields

        private PageSectionInfo _section = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CreatorWizard.NextButtonClick += CreatorWizard_NextButtonClick;
            CreatorWizard.FinishButtonClick += CreatorWizard_FinishButtonClick;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (Section == null)
                    throw new ArgumentNullException("section");

                DataBind();
                SetInputValues();
            }
        }

        #endregion

        #region Event handlers

        private void CreatorWizard_NextButtonClick(Object sender, WizardNavigationEventArgs e)
        {
            EnableValidators();

            if (!Page.IsValid)
            {
                e.Cancel = true;
                return;
            }

            if (e.CurrentStepIndex == CreatorWizard.WizardSteps.IndexOf(Step1))
            {
                PageInfo page = new PageInfo(Section);

                if (!page.SetName(PageName.Text))
                {
                    e.Cancel = true;
                    ShowMessage(String.Format("Page named '{0}' already created.", PageName.Text));
                }
            }
            else if (e.CurrentStepIndex == CreatorWizard.WizardSteps.IndexOf(Step2))
            {

            }
            else if (e.CurrentStepIndex == CreatorWizard.WizardSteps.IndexOf(Step3))
            {
                SideContentColumnHolder.Visible = (ContentLayoutType)ContentLayout.ValueAsInt32.Value == ContentLayoutType.TwoColumn;
            }
        }

        private void CreatorWizard_FinishButtonClick(Object sender, WizardNavigationEventArgs e)
        {
            if (!Page.IsValid)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                PageInfo page = PageManager.Current.CreateNewPage(SectionName, PageName.Text);

                if (page != null)
                {
                    // Step 1
                    page.Title = PageTitle.Text;
                    page.Theme = MasterTheme.Value;

                    // Step 2
                    page.HeaderLayout = (ContentHeaderLayoutType)HeaderLayout.ValueAsInt32.Value;
                    page.HeaderPosition = (ContentHeaderPositionType)HeaderPosition.ValueAsInt32.Value;
                    page.HeaderTitle = HeaderTitle.Text;
                    page.HeaderLinkVisible = ShowHeaderLink.Checked;
                    page.HeaderLinkUrl =
                        !String.IsNullOrEmpty(HeaderLinkUrl.Value)
                            ? HeaderLinkUrl.Value
                            : HeaderLinkUrl.Text;
                    page.HeaderLinkText = HeaderLinkText.Text;
                    page.HeaderLinkTarget = HeaderLinkTarget.Value;

                    // Step 3
                    page.ContentLayout = (ContentLayoutType)ContentLayout.ValueAsInt32.Value;
                    page.ContentPostion = (ContentPositionType)PageContentColumnPosition.ValueAsInt32.Value;
                    page.SideColumnSize = (ContentLayoutSideColumnSize)SideColumnSize.ValueAsInt32.Value;
                    page.ShowColumnsSeparator = ShowColumnSeparator.Checked;
                    page.MenuPosition = (MenuPositionType)MenuPosition.ValueAsInt32.Value;

                    AddMenuItems(page, MenuItemsCache.Value);

                    // Step 4
                    page.AdminOnly = AdminOnly.ValueAsBoolean.Value;
                    page.MetaDescription = MetaDescription.Text;
                    page.MetaKeywords = MetaKeywords.Text;

                    if (page.SetContentHtml(ContentColumnEditor.Content, SideContentColumnEditor.Content, HeaderTemplateEditor.Content))
                    {
                        PageManager.Current.Save();

                        LinkUtils.RedirectToAdminPage(PageFinder.ActionName);
                    }
                    else
                    {
                        e.Cancel = true;
                        ShowMessage(String.Format("An error has occured when HTML file saving.", PageName.Text));
                    }
                }
                else
                {
                    e.Cancel = true;
                    ShowMessage(String.Format("Page named '{0}' already created.", PageName.Text));
                }
            }
            finally
            {
                PageManager.Current.Reset();
            }
        }

        #endregion

        #region Setting and getting input values

        private void SetInputValues()
        {
            AdminOnly.ValueAsBoolean = true;

            if (Section != null)
            {
                MasterPage.Value = Section.MasterPage;

                MasterTheme.DataBind();
                MasterTheme.Value = Section.DefaultTheme;
            }
        }

        #endregion

        #region PreRender

        protected override void OnPreRender(EventArgs e)
        {
            EnableValidators();

            String jScript = "\r\n";
            jScript += String.Format("var pageCreator_sectionName = {0};\r\n", JavaScriptHelper.GetJsValue(SectionName, "''"));
            jScript += String.Format("var pageCreator_masterPageName = {0};\r\n", JavaScriptHelper.GetJsValue(MasterPage.Value));
            jScript += String.Format("var pageCreator_masterPageThemeName = {0};\r\n", JavaScriptHelper.GetJsValue(MasterTheme.Value));
            jScript += String.Format("var pageCreator_currentStepIndex = {0};\r\n", JavaScriptHelper.GetJsValue(CreatorWizard.ActiveStepIndex));
            jScript += String.Format("var pageCreator_headerPosition = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderPosition.ValueAsInt32));
            jScript += String.Format("var pageCreator_headerLayout = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderLayout.ValueAsInt32));
            jScript += String.Format("var pageCreator_headerTitle = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderTitle.Text, "''"));
            jScript += String.Format("var pageCreator_showHeaderLink = {0};\r\n", JavaScriptHelper.GetJsValue(ShowHeaderLink.Checked));
            jScript += String.Format("var pageCreator_headerLinkText = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderLinkText.Text, "''"));
            jScript += String.Format("var pageCreator_headerTemplateHtml = {0};\r\n", JavaScriptHelper.GetJsValue(HeaderTemplateEditor.Content, "''"));
            jScript += String.Format("var pageCreator_contentLayout = {0};\r\n", JavaScriptHelper.GetJsValue(ContentLayout.ValueAsInt32, "''"));
            jScript += String.Format("var pageCreator_sideColumnSize = {0};\r\n", JavaScriptHelper.GetJsValue(SideColumnSize.ValueAsInt32, "''"));
            jScript += String.Format("var pageCreator_contentColumnPosition = {0};\r\n", JavaScriptHelper.GetJsValue(PageContentColumnPosition.ValueAsInt32, "''"));
            jScript += String.Format("var pageCreator_showColumnSeparator = {0};\r\n", JavaScriptHelper.GetJsValue(ShowColumnSeparator.Checked));
            jScript += String.Format("var pageCreator_menuPosition = {0};\r\n", JavaScriptHelper.GetJsValue(MenuPosition.ValueAsInt32));
            jScript += String.Format("var pageCreator_contentColumnHtml = {0};\r\n", JavaScriptHelper.GetJsValue(ContentColumnEditor.Content, "''"));
            jScript += String.Format("var pageCreator_sideColumnHtml = {0};\r\n", JavaScriptHelper.GetJsValue(SideContentColumnEditor.Content, "''"));
            
            ScriptManager.RegisterStartupScript(Page, GetType(), "jscript", jScript, true);

            base.OnPreRender(e);
        }

        #endregion

        #region Helpers

        private static void AddMenuItems(PageInfo page, String menuItems)
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



        #endregion
    }
}