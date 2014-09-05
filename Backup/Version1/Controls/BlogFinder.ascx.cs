using System;
using System.Data;
using System.Web.UI;

using Csbs.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Telerik.Web.UI;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public partial class BlogFinder : UserControl
    {
        #region Constants

        public const String ActionName = "find-blog";

        #endregion

        #region Fields

        private DataTable _dataSource = null;
        private String _gridMessage = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ResetCommand.Click += ResetCommand_Click;

            BlogGrid.DataBinding += BlogGrid_DataBinding;
            BlogGrid.NeedDataSource += BlogGrid_NeedDataSource;
            BlogGrid.ItemDataBound += BlogGrid_ItemDataBound;
            BlogGrid.ItemCommand += BlogGrid_ItemCommand;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                BlogGrid.DataBind();
            }
        }

        #endregion

        #region Data binding

        private void BlogGrid_DataBinding(Object sender, EventArgs e)
        {
            BlogGrid.DataSource = GetDataSource();
        }

        private void BlogGrid_NeedDataSource(Object source, GridNeedDataSourceEventArgs e)
        {
            BlogGrid.DataSource = GetDataSource();
        }

        #endregion

        #region Event handlers

        private void ResetCommand_Click(Object sender, EventArgs e)
        {
            BlogManager.Current.Reset();
        }

        private void BlogGrid_ItemDataBound(Object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            {
                DataRowView dataItem = (DataRowView)e.Item.DataItem;
                RadToolTip tip = (RadToolTip)e.Item.FindControl("PreviewToolTip");

                tip.Text = GetToolTipText(dataItem);
            }
        }

        protected void Grid_DataBound(Object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_gridMessage))
            {
                BlogGrid.Controls.Add(new LiteralControl(String.Format("<span style='color:red'>{0}</span>", _gridMessage)));
            }
        }

        protected void BlogGrid_ItemCommand(Object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Delete")
                {
                    GridEditableItem item = (GridEditableItem)e.Item;
                    Object id = item.GetDataKeyValue("ID");

                    try
                    {
                        BlogManager.Current.Delete(Convert.ToInt32(id));
                        BlogManager.Current.Save();
                    }
                    finally
                    {
                        BlogManager.Current.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                
                SetMessage("Article cannot be deleted. Reason: " + ex.Message);
                e.Canceled = true;
            }
            finally
            {
                BlogGrid.Rebind();
            }
        }

        #endregion

        #region Helper methods

        private void SetMessage(String message) { _gridMessage += message; }

        private DataTable GetDataSource()
        {
            if (_dataSource == null)
                _dataSource = BlogManager.Current.GetItemsDataSource();

            return _dataSource;
        }

        private static String GetToolTipText(DataRowView dataItem)
        {
            StringBuilder html = new StringBuilder();

            html.Append("<div class='articles-list'>");
            html.Append("<div class='item'>");
            html.AppendFormat("<div class='photo-overflow'><div class='photo-container'><img src='{0}' alt=''></div></div>", ValueHelper.ToString(dataItem["ImageSource"]));
            html.Append("<div class='description'>");
            html.AppendFormat("<div class='header'>{0}</div>", ValueHelper.ToString(dataItem["Title"]));
            html.AppendFormat("<div class='date'>{0:dddd, MMMM dd, yyyy}</div>", ValueHelper.ToDateTimeNullable(dataItem["Date"]));
            html.AppendFormat("<div class='text'>{0}</div>", ValueHelper.ToString(dataItem["Description"]));
            html.Append("</div>");
            html.Append("</div>");
            html.Append("</div>");

            return html.ToString();
        }
        
        #endregion
    }
}