using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class PageContentSizeSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, "Small", (Int32)ContentLayoutSideColumnSize.Large);
            AddDataRow(table, "Medium", (Int32)ContentLayoutSideColumnSize.Medium);
            AddDataRow(table, "Large", (Int32)ContentLayoutSideColumnSize.Small);

            return table;
        }

        #endregion
    }
}