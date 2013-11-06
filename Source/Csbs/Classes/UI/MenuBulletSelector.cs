using System;
using System.Data;

using Csbs.Data;

namespace Csbs.Web.UI
{
    public class MenuBulletSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Text", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            AddDataRow(table, "None", (Int32)VerticalMenuBulletType.None);
            AddDataRow(table, "Circle", (Int32)VerticalMenuBulletType.Sun);
            AddDataRow(table, "Triangle", (Int32)VerticalMenuBulletType.Triangle);

            return table;
        }

        #endregion
    }
}