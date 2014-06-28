using System;
using System.Data;
using System.Web.UI;

using Csbs.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using Csbs.Data.Menu;
using Csbs.Utilities;

using Telerik.Web.UI;

namespace Csbs.Web.UI
{
    public partial class MenuSectionEditor : UserControl
    {
        #region Constants

        public const String ActionName = "edit-menu-section";

        private const String SectionIdViewStateKey = "SectionID";

        #endregion

        #region Fields

        private MenuSectionInfo _section = null;
        private MenuInfo _menu = null;
        private DataTable _gridDataSource = null;

        #endregion

        #region Properties

        private Int32? MenuID
        {
            get { return StringHelper.ToInt32Nullable(Request["menu_id"]); }
        }

        private Int32? SectionID
        {
            get 
            {
                if (ViewState[SectionIdViewStateKey] == null)
                    return StringHelper.ToInt32Nullable(Request["id"]);

                return (Int32)ViewState[SectionIdViewStateKey];
            }
            set
            {
                ViewState[SectionIdViewStateKey] = value;
            }
        }

        private MenuInfo Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu =
                        MenuID.HasValue
                            ? MenuManager.Current.FindMenuByID(MenuID.Value)
                            : null;
                }

                return _menu;
            }
        }

        private MenuSectionInfo Section
        {
            get
            {
                if (_section == null)
                {
                    _section =
                        SectionID.HasValue
                            ? MenuManager.Current.FindSectionByID(SectionID.Value)
                            : new MenuSectionInfo(Menu);
                }

                return _section;
            }
        }

        private DataTable ItemsGridDataSource
        {
            get
            {
                if (_gridDataSource == null && SectionID.HasValue)
                    _gridDataSource = MenuManager.Current.GetItemsDataSource(SectionID.Value);

                return _gridDataSource;
            }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ItemsGrid.DataBinding += ItemsGrid_DataBinding;
            ItemsGrid.NeedDataSource += ItemsGrid_NeedDataSource;
            ItemsGrid.ItemDataBound += ItemsGrid_ItemDataBound;
            ItemsGrid.ItemCommand += ItemsGrid_ItemCommand;
            ItemsGrid.DeleteCommand += ItemsGrid_DeleteCommand;
            ItemsGrid.InsertCommand += ItemsGrid_InsertCommand;
            ItemsGrid.UpdateCommand += ItemsGrid_UpdateCommand;

            SaveButton.Click += SaveButton_Click;

            if (Menu != null && !Menu.IsNew)
                CloseButton.OnClientClick = String.Format("window.location = '{0}'; return false;", ResolveUrl(LinkUtils.GetAdminPageUrl(MenuEditor.ActionName, Menu.ID)));
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (!MenuID.HasValue || Menu == null || SectionID.HasValue && Section == null || !Section.IsNew && Section.MenuID != Menu.ID)
                {
                    if (Menu != null)
                        LinkUtils.RedirectToAdminPage(MenuEditor.ActionName, Menu.ID);
                    else
                        LinkUtils.RedirectToAdminPage(MenuFinder.ActionName);
                }
                else
                {
                    DataBind();
                    SetInputValues();
                }
            }
        }

        #endregion

        #region Event handlers

        private void SaveButton_Click(Object sender, EventArgs e)
        {
            Boolean isNewSection = Section.IsNew;

            if (SaveSection())
            {
                if (!isNewSection)
                    Page.Response.Redirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(MenuEditor.ActionName, Menu.ID)));
                else
                    Page.Response.Redirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(LinkUtils.ActionNameQueryStringKey, MenuSectionEditor.ActionName, "id", Section.ID, "menu_id", Menu.ID)));
            }
        }

        private void ItemsGrid_DataBinding(Object sender, EventArgs e)
        {
            ItemsGrid.DataSource = ItemsGridDataSource;
        }

        private void ItemsGrid_NeedDataSource(Object source, GridNeedDataSourceEventArgs e)
        {
            ItemsGrid.DataSource = ItemsGridDataSource;
        }

        private void ItemsGrid_DeleteCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                DeleteComponent(e);
                ItemsGrid.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Menu item cannot be deleted. Reason: " + ex.Message);
                e.Canceled = true;
                ItemsGrid.Rebind();
            }
        }

        private void ItemsGrid_UpdateCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                UpdateComponent(e);
                ItemsGrid.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Menu item cannot be update. Reason: " + ex.Message);
                e.Canceled = true;
                ItemsGrid.Rebind();
            }
        }

        private void ItemsGrid_InsertCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                InsertComponent(e);
                ItemsGrid.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Menu item cannot be inserted. Reason: " + ex.Message);
                e.Canceled = true;
                ItemsGrid.Rebind();
            }
        }

        private void ItemsGrid_ItemDataBound(Object sender, GridItemEventArgs e)
        {
            if (e.Item.IsInEditMode && e.Item.ItemType == GridItemType.EditFormItem)
            {
                if (e.Item.ItemIndex >= 0)
                {
                    DataRowView dataItem = (DataRowView)e.Item.DataItem;

                    Int32? id = dataItem["ID"] == DBNull.Value ? null : (Int32?)dataItem["ID"];

                    if (id.HasValue)
                    {
                        MenuItemInfo menuItem = MenuManager.Current.FindMenuItemByID(id.Value);

                        if (menuItem != null)
                        {
                            RadTextBox ItemTitle = (RadTextBox)e.Item.FindControl("ItemTitle");
                            RadTextBox ItemText = (RadTextBox)e.Item.FindControl("ItemText");
                            AnchorUrlSelector ItemHref = (AnchorUrlSelector)e.Item.FindControl("ItemHref");
                            AnchorTargetSelector ItemLinkTarget = (AnchorTargetSelector)e.Item.FindControl("ItemLinkTarget");
                            RadTextBox ItemAdminTitle = (RadTextBox)e.Item.FindControl("ItemAdminTitle");
                            AnchorUrlSelector ItemAdminHref = (AnchorUrlSelector)e.Item.FindControl("ItemAdminHref");
                            CheckBox ItemAdminOnly = (CheckBox)e.Item.FindControl("ItemAdminOnly");
                            RadTextBox ItemVisibleURL = (RadTextBox)e.Item.FindControl("ItemVisibleURL");

                            ItemTitle.Text = menuItem.Title;
                            ItemText.Text = menuItem.Text;
                            ItemLinkTarget.Value = menuItem.Target;
                            ItemAdminTitle.Text = menuItem.AdminTitle;
                            ItemAdminOnly.Checked = menuItem.AdminOnly;
                            ItemVisibleURL.Text = menuItem.VisibeForUrl;

                            RadComboBoxItem hrefItem = ItemHref.FindItemByValue(menuItem.Href);
                            if (hrefItem != null)
                                ItemHref.Value = menuItem.Href;
                            else
                                ItemHref.Text = menuItem.Href;

                            RadComboBoxItem adminHrefItem = ItemAdminHref.FindItemByValue(menuItem.AdminHref);
                            if (adminHrefItem != null)
                                ItemAdminHref.Value = menuItem.AdminHref;
                            else
                                ItemAdminHref.Text = menuItem.AdminHref;
                        }
                    }
                }
            }
        }

        protected void ItemsGrid_ItemCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "MoveUp")
                {
                    GridEditableItem item = e.Item as GridEditableItem;
                    Int32 id = (Int32)item.GetDataKeyValue("ID");
                    try
                    {
                        if (MenuManager.Current.MoveUp(id))
                            MenuManager.Current.Save();
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                        ItemsGrid.Rebind();
                    }
                }
                else if (e.CommandName == "MoveDown")
                {
                    GridEditableItem item = e.Item as GridEditableItem;
                    Int32 id = (Int32)item.GetDataKeyValue("ID");
                    try
                    {
                        if (MenuManager.Current.MoveDown(id))
                            MenuManager.Current.Save();
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                        ItemsGrid.Rebind();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Menu item cannot be deleted. Reason: " + ex.Message);
                e.Canceled = true;
            }
        }

        #endregion

        #region Data operations

        private Boolean SaveSection()
        {
            Boolean isCancel = false;

            try
            {
                if (Section != null)
                {
                    Section.Title = SectionTitle.Text;
                    Section.ShowHeader = ShowHeader.Checked;
                    Section.IsHtml = IsHtmlContent.Checked;

                    if (Section.IsHtml)
                        Section.InnerHtml = HtmlContentEditor.Content;

                    if (!isCancel)
                    {
                        if (Section.IsNew)
                            isCancel = !MenuManager.Current.AddSection(Menu.ID, Section);

                        if (!isCancel)
                        {
                            SectionID = _section.ID;

                            MenuManager.Current.Save();

                            _section = null;
                        }
                    }
                }
            }
            finally
            {
                MenuManager.Current.Reset();
            }

            return !isCancel;
        }

        private void DeleteComponent(GridCommandEventArgs e)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            Int32 id = (Int32)item.GetDataKeyValue("ID");

            try
            {
                if (MenuManager.Current.Delete(id))
                    MenuManager.Current.Save();
            }
            finally
            {
                MenuManager.Current.Reset();
            }
        }

        private void InsertComponent(GridCommandEventArgs e)
        {
            SaveMenuItem(null, e);
        }

        private void UpdateComponent(GridCommandEventArgs e)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            Int32 id = (Int32)item.GetDataKeyValue("ID");

            SaveMenuItem(id, e);
        }

        private Boolean SaveMenuItem(Int32? id, GridCommandEventArgs e)
        {
            Boolean isCancel = false;

            if (SaveSection())
            {
                if (!Section.IsHtml)
                {
                    try
                    {
                        MenuItemInfo menuItem = id.HasValue ? MenuManager.Current.FindMenuItemByID(id.Value) : new MenuItemInfo();

                        GridEditableItem editedItem = (GridEditableItem)e.Item;

                        RadTextBox ItemTitle = (RadTextBox)e.Item.FindControl("ItemTitle");
                        RadTextBox ItemText = (RadTextBox)e.Item.FindControl("ItemText");
                        AnchorUrlSelector ItemHref = (AnchorUrlSelector)e.Item.FindControl("ItemHref");
                        AnchorTargetSelector ItemLinkTarget = (AnchorTargetSelector)e.Item.FindControl("ItemLinkTarget");
                        RadTextBox ItemAdminTitle = (RadTextBox)e.Item.FindControl("ItemAdminTitle");
                        AnchorUrlSelector ItemAdminHref = (AnchorUrlSelector)e.Item.FindControl("ItemAdminHref");
                        CheckBox ItemAdminOnly = (CheckBox)e.Item.FindControl("ItemAdminOnly");
                        RadTextBox ItemVisibleURL = (RadTextBox)e.Item.FindControl("ItemVisibleURL");

                        menuItem.Title = ItemTitle.Text;
                        menuItem.Text = ItemText.Text;
                        menuItem.Href = String.IsNullOrEmpty(ItemHref.Value) ? ItemHref.Text : ItemHref.Value;
                        menuItem.Target = ItemLinkTarget.Value;
                        menuItem.AdminTitle = ItemAdminTitle.Text;
                        menuItem.AdminHref = ItemAdminHref.Text;
                        menuItem.AdminOnly = ItemAdminOnly.Checked;
                        menuItem.VisibeForUrl = ItemVisibleURL.Text;

                        if (!IsRegularExpressionValid(menuItem.VisibeForUrl))
                        {
                            ShowMessage("Invalid regular expression.");
                            isCancel = true;
                            e.Canceled = isCancel;
                        }

                        if (!isCancel)
                        {
                            if (menuItem.IsNew)
                                isCancel = !MenuManager.Current.AddMenuItem(Section.ID, menuItem);

                            if (!isCancel)
                                MenuManager.Current.Save();
                        }
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                    }
                }
                else
                {
                    isCancel = true;
                }
            }
            else
            {
                isCancel = true;
            }

            return !isCancel;
        }

        #endregion

        #region Setting input values

        private void SetInputValues()
        {
            SectionTitle.Text = Section.Title;
            ShowHeader.Checked = Section.ShowHeader;
            IsHtmlContent.Checked = Section.IsHtml;

            if (!Section.IsNew && Section.IsHtml)
                HtmlContentEditor.Content = Section.InnerHtml;

            InnerContent.Visible = !Section.IsNew;
        }

        #endregion

        #region Helper methods

        private Boolean IsRegularExpressionValid(String regexp)
        {
            Boolean isValid = true;

            if (!String.IsNullOrEmpty(regexp))
            {
                try
                {
                    Regex.IsMatch("/test/inputpath.html", regexp);
                }
                catch
                {
                    isValid = false;
                }
            }

            return isValid;
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