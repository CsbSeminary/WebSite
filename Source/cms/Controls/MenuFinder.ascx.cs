using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

using Csbs.Web.UI;

using Telerik.Web.UI;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public partial class MenuFinder : UserControl
    {
        #region Constants

        public const String ActionName = "find-menu";

        #endregion

        #region Properties

        private DataTable DataSource
        {
            get
            {
                if (_dataSource == null)
                    _dataSource = MenuManager.Current.GetFinderDataSource();

                return _dataSource;
            }
        }

        #endregion

        #region Fields

        private DataTable _dataSource = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ResetCommand.Click += ResetCommand_Click;

            MenuGrid.DataBinding += MenuGrid_DataBinding;
            MenuGrid.NeedDataSource += MenuGrid_NeedDataSource;
            MenuGrid.ItemDataBound += MenuGrid_ItemDataBound;
            MenuGrid.ItemCommand += MenuGrid_ItemCommand;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                MenuGrid.DataBind();
            }
        }

        #endregion

        #region Data binding

        private void MenuGrid_DataBinding(Object sender, EventArgs e)
        {
            MenuGrid.DataSource = DataSource;
        }

        private void MenuGrid_NeedDataSource(Object source, GridNeedDataSourceEventArgs e)
        {
            MenuGrid.DataSource = DataSource;
        }

        #endregion

        #region Event handlers

        private void ResetCommand_Click(Object sender, EventArgs e)
        {
            MenuManager.Current.Reset();
        }

        private void MenuGrid_ItemDataBound(Object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                RadToolTip tip = (RadToolTip)e.Item.FindControl("PreviewToolTip");

                tip.Text = dataItem["HtmlContent"] == DBNull.Value ? null : (String)dataItem["HtmlContent"];
            }
        }

        protected void MenuGrid_ItemCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                Int32 id = (Int32)item.GetDataKeyValue("ID");

                if (e.CommandName == "Delete")
                {
                    try
                    {
                        if (MenuManager.Current.Delete(id))
                            MenuManager.Current.Save();
                        else
                            ShowMessage("Item cannot be removed: menu contains a child items.");
                    }
                    finally
                    {
                        MenuManager.Current.Reset();
                    }
                }
            }
            catch (Exception ex)
            {

                ShowMessage("Activity cannot be deleted. Reason: " + ex.Message);
                e.Canceled = true;
            }
            finally
            {
                MenuGrid.Rebind();
            }
        }

        #endregion

        #region Helper methods

        private void ShowMessage(String msg)
        {
            if (String.IsNullOrEmpty(msg))
                return;

            ScriptManager.RegisterStartupScript(Page, GetType(), "message_script", JavaScriptHelper.GetShowMessageScript(msg), true);
        }

        #endregion
    }
}