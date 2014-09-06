using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class HeaderPositionSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, "Top of page", (Int32)ContentHeaderPositionType.TopOfPage);
            AddDataRow(table, "Top of content column", (Int32)ContentHeaderPositionType.TopOfContent);

            return table;
        }

        #endregion
    }
}