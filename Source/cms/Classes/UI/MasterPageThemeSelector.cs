using System;
using System.Data;
using System.Collections.Specialized;

using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class MasterPageThemeSelector : BaseComboBox
    {
        #region Constants

        private const String MasterPageControlViewStateKey = "MasterPageControl";

        #endregion

        #region Properties

        public String MasterPageControl
        {
            get { return (String)ViewState[MasterPageControlViewStateKey]; }
            set { ViewState[MasterPageControlViewStateKey] = value; }
        }

        #endregion

        #region Fields

        private MasterPageSelector _masterInput;

        #endregion

        #region Constructor

        public MasterPageThemeSelector()
        {
            
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (MasterPageControl != null)
            {
                _masterInput = (MasterPageSelector)Parent.FindControl(MasterPageControl);

                if (_masterInput != null)
                {
                    _masterInput.AutoPostBack = true;
                    _masterInput.SelectedIndexChanged += MasterInput_SelectedIndexChanged;
                }
            }

            if (_masterInput == null)
                throw new ArgumentNullException("MasterPageControl");
        }

        #endregion

        #region Data binding

        protected override DataTable SelectData(String text)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Value", typeof(String));
            table.Columns.Add("Text", typeof(String));

            if (!String.IsNullOrEmpty(_masterInput.Value))
            {
                StringCollection availThemes = FileManager.GetAvailableThemes(_masterInput.Value);

                if (availThemes != null && availThemes.Count > 0)
                {
                    foreach (String themeName in availThemes)
                    {
                        AddDataRow(table, themeName, themeName);
                    }
                }
            }

            Enabled = table.Rows.Count > 0;

            return table;
        }

        #endregion

        #region Event handlers

        private void MasterInput_SelectedIndexChanged(Object sender, EventArgs e)
        {
            DataBind();
        }

        #endregion
    }
}