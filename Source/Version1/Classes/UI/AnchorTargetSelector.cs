using System;
using System.Data;

namespace Csbs.Web.UI
{
    public class AnchorTargetSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(String));

            AddDataRow(table, "Same Window", "_self");
            AddDataRow(table, "New Window", "_blank");

            return table;
        }

        #endregion
    }
}