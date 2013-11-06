using System;
using System.Data;

using Telerik.Web.UI;

namespace Csbs.Web.UI
{
    public class PageMenuSelector : RadTreeView
    {
        #region Constructor

        public PageMenuSelector()
        {
            CheckBoxes = true;
        }

        #endregion

        #region Data binding

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            DataFieldID = "ID";
            DataValueField = "FullName";
            DataFieldParentID = "ParentID";
            DataTextField = "Name";

            DataSource = MenuManager.Current.GetSelectorDataSource();
        }

        protected override void OnNodeDataBound(RadTreeNodeEventArgs e)
        {
            base.OnNodeDataBound(e);

            RadTreeNode node = e.Node;
            DataRowView data = (DataRowView)node.DataItem;

            String itemType = data["ItemType"] == DBNull.Value ? null : (String)data["ItemType"];

            if (String.Compare(itemType, "Menu", true) == 0)
            {
                node.ImageUrl = "~/Media/Icons/menu.gif";
                node.Checkable = true;
                node.Expanded = false;
            }
            else if (String.Compare(itemType, "MenuSection", true) == 0)
            {
                node.ImageUrl = "~/Media/Icons/point-triangle.gif";
                node.Checkable = false;
                node.Expanded = true;
            }
            else if (String.Compare(itemType, "MenuItem", true) == 0)
            {
                node.ImageUrl = "~/Media/Icons/point-circle.gif";
                node.Checkable = false;
            }
        }

        #endregion
    }
}