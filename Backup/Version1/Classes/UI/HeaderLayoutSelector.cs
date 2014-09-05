using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class HeaderLayoutSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, ContentHeaderLayoutType.None.ToString(), (Int32)ContentHeaderLayoutType.None);
            AddDataRow(table, ContentHeaderLayoutType.Text.ToString(), (Int32)ContentHeaderLayoutType.Text);
            AddDataRow(table, ContentHeaderLayoutType.Template.ToString(), (Int32)ContentHeaderLayoutType.Template);

            return table;
        }

        #endregion
    }
}