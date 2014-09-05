using System;
using System.Text;
using System.Web.UI;

using Csbs.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Utilities;

using Csbs.Data;
using Csbs.Data.Pages;

namespace Csbs.Pages
{
    public partial class PagePreviewer : Page
    {
        #region Constatnts

        private const String MasterPageNameQueryStringKey = "master";
        private const String ThemeNameQueryStringKey = "theme";
        private const String HeaderLayoutQueryStringKey = "headerLayout";
        private const String HeaderPositionQueryStringKey = "headerPosition";
        private const String ShowHeaderLinkQueryStringKey = "showHeaderLink";
        private const String SectionNameQueryStringKey = "section";
        private const String ContentLayoutQueryStringKey = "contentLayout";
        private const String SideColumnSizeQueryStringKey = "sideColumnSize";
        private const String ContentPositionQueryStringKey = "contentPosition";
        private const String ShowColumnSeparatorQueryStringKey = "showColumnSeparator";
        private const String MenuItemsQueryStringKey = "menuItems";
        private const String MenuPositionQueryStringKey = "menuPosition";

        #endregion

        #region Properties

        private String MasterPageName
        {
            get { return String.IsNullOrEmpty(Request[MasterPageNameQueryStringKey]) ? "CSBS" : Request[MasterPageNameQueryStringKey]; }
        }

        private String MasterPageThemeName
        {
            get { return Request[ThemeNameQueryStringKey]; }
        }

        private String SectionName
        {
            get { return Request[SectionNameQueryStringKey]; }
        }

        private ContentHeaderLayoutType HeaderLayout
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[HeaderLayoutQueryStringKey]);

                return value.HasValue ? (ContentHeaderLayoutType)value.Value : ContentHeaderLayoutType.None;
            }
        }

        private ContentHeaderPositionType HeaderPosition
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[HeaderPositionQueryStringKey]);

                return value.HasValue ? (ContentHeaderPositionType)value.Value : ContentHeaderPositionType.TopOfPage;
            }
        }

        private ContentLayoutType ContentLayout
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[ContentLayoutQueryStringKey]);

                return value.HasValue ? (ContentLayoutType)value.Value : ContentLayoutType.OneColumn;
            }
        }

        private ContentLayoutSideColumnSize SideColumnSize
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[SideColumnSizeQueryStringKey]);

                return value.HasValue ? (ContentLayoutSideColumnSize)value.Value : ContentLayoutSideColumnSize.Medium;
            }
        }

        private ContentPositionType ContentPosition
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[ContentPositionQueryStringKey]);

                return value.HasValue ? (ContentPositionType)value.Value : ContentPositionType.Right;
            }
        }

        private MenuPositionType MenuPosition
        {
            get
            {
                Int32? value = StringHelper.ToInt32Nullable(Request[MenuPositionQueryStringKey]);

                return value.HasValue ? (MenuPositionType)value.Value : MenuPositionType.None;
            }
        }

        private Boolean ShowHeaderLink
        {
            get { return StringHelper.ToBoolean(Request[ShowHeaderLinkQueryStringKey]); }
        }

        private Boolean ShowColumnSeparator
        {
            get { return StringHelper.ToBoolean(Request[ShowColumnSeparatorQueryStringKey]); }
        }

        private String MenuItems
        {
            get { return Request[MenuItemsQueryStringKey]; }
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

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            MasterPageFile = LinkUtils.GetMasterPageFilePath(MasterPageName);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Control widgets = Master.FindControl("Widgets");
            Control globalNavigation = Master.FindControl("GlobalNavigationMenu");

            if (widgets != null)
                widgets.Visible = false;

            if (globalNavigation != null)
                globalNavigation.Visible = false;

            ContentHeader.Controls.Add(new LiteralControl(GetHeadContent()));
            ContentContainer.Controls.Add(new LiteralControl(GetHtmlContent()));
        }

        #endregion

        #region Content rendering

        private String GetHeadContent()
        {
            StringBuilder sb = new StringBuilder();

            RenderHelper.WritePageHeadContent(sb, MasterPageName, MasterPageThemeName);

            return sb.ToString();
        }

        private String GetHtmlContent()
        {
            StringBuilder sb = new StringBuilder();

            PageInfo page = new PageInfo(Section);

            page.HeaderLayout = HeaderLayout;
            page.HeaderPosition = HeaderPosition;
            page.HeaderLinkVisible = ShowHeaderLink;
            page.ContentLayout = ContentLayout;
            page.ContentPostion = ContentPosition;
            page.SideColumnSize = SideColumnSize;
            page.ShowColumnsSeparator = ShowColumnSeparator;
            page.MenuPosition = MenuPosition;

            page.ClearMenuItems();

            if (!String.IsNullOrEmpty(MenuItems))
            {
                String[] items = MenuItems.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (String item in items)
                {
                    page.AddMenuItem(item);
                }
            }

            if (page.HeaderLayout != ContentHeaderLayoutType.None)
                page.HeaderTitle = "<span id='csbs_pagePreviewer_HeaderTitle'></span>";

            RenderHelper.WritePageHtmlContent(sb, page);

            return sb.ToString();
        }

        #endregion
    }
}