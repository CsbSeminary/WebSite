using System;
using System.Data;

namespace Csbs.Web.UI
{
    public class PageVisibilitySelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Boolean));

            AddDataRow(table, "All Visitors", false);
            AddDataRow(table, "Administrator Only", true);

            return table;
        }

        #endregion
    }
}