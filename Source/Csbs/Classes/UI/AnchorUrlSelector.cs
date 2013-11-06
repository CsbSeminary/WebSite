using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Csbs.Web.UI
{
    public class AnchorUrlSelector : BaseComboBox
    {
        #region Constructor

        public AnchorUrlSelector()
        {
            AllowCustomText = true;
            DropDownWidth = Unit.Pixel(500);
        }

        #endregion

        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable t = new DataTable("Pages");

            t.Columns.Add("Text", typeof(String));
            t.Columns.Add("Value", typeof(String));

            PageManager.Current.AddDataToSelectorDataSource(t);
            BlogManager.Current.AddDataToSelectorDataSource(t);

            return t;
        }

        #endregion
    }
}