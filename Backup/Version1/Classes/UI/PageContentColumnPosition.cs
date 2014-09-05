using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class PageContentColumnPosition : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, ContentPositionType.Right.ToString(), (Int32)ContentPositionType.Right);
            AddDataRow(table, ContentPositionType.Left.ToString(), (Int32)ContentPositionType.Left);

            return table;
        }

        #endregion
    }
}