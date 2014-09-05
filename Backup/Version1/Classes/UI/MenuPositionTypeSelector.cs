using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class MenuPositionTypeSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, "None", (Int32)MenuPositionType.None);
            AddDataRow(table, "Side Column", (Int32)MenuPositionType.SidePanel);
            AddDataRow(table, "Top of page", (Int32)MenuPositionType.Top);

            return table;
        }

        #endregion
    }
}