using System;
using System.Data;
using System.Web.UI;

using Csbs.Data;
using Csbs.Data.Menu;
using Csbs.Utilities;

using Telerik.Web.UI;

namespace Csbs.Web.UI
{
    public partial class MenuEditor : UserControl
    {
        #region Constants

        public const String ActionName = "edit-menu";

        #endregion

        #region Fields

        private MenuInfo _menu = null;
        private DataTable _gridDataSource = null;

        #endregion

        #region Properties

        private Int32? MenuID
        {
            get { return StringHelper.ToInt32Nullable(Request["id"]); }
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
                            : new MenuInfo();
                }

                return _menu;
            }
        }

        private DataTable SectionGridDataSource
        {
            get
            {
                if (_gridDataSource == null && MenuID.HasValue)
                    _gridDataSource = MenuManager.Current.GetSectionsDataSource(MenuID.Value);

                return _gridDataSource;
            }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SectionGrid.DataBinding += SectionGrid_DataBinding;
            SectionGrid.NeedDataSource += SectionGrid_NeedDataSource;
            SectionGrid.ItemDataBound += SectionGrid_ItemDataBound;
            SectionGrid.ItemCommand += SectionGrid_ItemCommand;

            SaveButton.Click += SaveButton_Click;
            CloseButton.OnClientClick = String.Format("window.location = '{0}'; return false;", ResolveUrl(LinkUtils.GetAdminPageUrl(MenuFinder.ActionName)));
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (MenuID.HasValue && Menu == null)
                {
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
            Boolean isCancel = false;

            try
            {
                if (Menu != null)
                {
                    if (String.Compare(MenuName.Text, Menu.Name, true) != 0)
                    {
                        if (!Menu.SetName(MenuName.Text))
                        {
                            isCancel = true;
                            ShowMessage(String.Format("Menu named '{0}' already exists.", MenuName.Text));
                        }
                    }

                    Menu.Bullet = (VerticalMenuBulletType)BulletSelector.ValueAsInt32.Value;
                    Menu.HorizontalSeparator = HorizontalSeparator.Text;

                    if (!isCancel)
                    {
                        if (Menu.IsNew)
                            isCancel = !MenuManager.Current.AddMenu(Menu);

                        if (!isCancel)
                        {
                            MenuManager.Current.Save();

                            ShowMessage("Your changes have been saved.");

                            if (!Menu.IsNew)
                                JsRedirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(MenuFinder.ActionName)));
                            else
                                JsRedirect(LinkUtils.ResolveAbsoluteUrl(LinkUtils.GetAdminPageUrl(MenuEditor.ActionName, Menu.ID)));
                        }
                    }
                }
            }
            finally
            {
                MenuManager.Current.Reset();
            }
        }

        private void SectionGrid_DataBinding(Object sender, EventArgs e)
        {
            SectionGrid.DataSource = SectionGridDataSource;
        }

        private void SectionGrid_NeedDataSource(Object source, GridNeedDataSourceEventArgs e)
        {
            SectionGrid.DataSource = SectionGridDataSource;
        }

        private void SectionGrid_ItemDataBound(Object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                RadToolTip tip = (RadToolTip)e.Item.FindControl("PreviewToolTip");

                tip.Text = dataItem["HtmlContent"] == DBNull.Value ? null : (String)dataItem["HtmlContent"];
            }
        }

        protected void SectionGrid_ItemCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                Int32 id = (Int32)item.GetDataKeyValue("ID");

                if (e.CommandName == "MoveUp")
                {
                    try
                    {
                        if (MenuManager.Current.MoveUp(id))
                            MenuManager.Current.Save();
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                    }
                }
                else if (e.CommandName == "MoveDown")
                {
                    try
                    {
                        if (MenuManager.Current.MoveDown(id))
                            MenuManager.Current.Save();
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                    }
                }
                else if (e.CommandName == "Delete")
                {
                    try
                    {
                        if (MenuManager.Current.Delete(id))
                            MenuManager.Current.Save();
                        else
                            ShowMessage("Item cannot be removed: section contains a child items.");
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Item cannot be deleted. Reason: " + ex.Message);
                e.Canceled = true;
            }
            finally
            {
                SectionGrid.Rebind();
            }
        }

        #endregion

        #region Setting input values

        private void SetInputValues()
        {
            MenuName.Text = Menu.Name;
            BulletSelector.ValueAsInt32 = (Int32)Menu.Bullet;
            HorizontalSeparator.Text = Menu.HorizontalSeparator;

            SectionGridHolder.Visible = !Menu.IsNew;
        }

        #endregion

        #region Helper methods

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