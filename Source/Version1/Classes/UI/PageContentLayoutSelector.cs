using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class PageContentLayoutSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, "One Column", (Int32)ContentLayoutType.OneColumn);
            AddDataRow(table, "Two Column", (Int32)ContentLayoutType.TwoColumn);

            return table;
        }

        #endregion
    }
}