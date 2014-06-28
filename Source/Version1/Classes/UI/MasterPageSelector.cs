using System;
using System.Data;
using System.Collections.Specialized;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class MasterPageSelector : BaseComboBox
    {
        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Value", typeof(String));
            table.Columns.Add("Text", typeof(String));

            StringCollection availMasters = FileManager.GetAvailableMasterPages();

            if (availMasters != null && availMasters.Count > 0)
            {
                foreach (String masterName in availMasters)
                {
                    AddDataRow(table, masterName, masterName);
                }
            }

            return table;
        }

        #endregion

    }
}