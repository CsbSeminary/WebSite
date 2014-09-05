using System;
using System.Data;
using System.Web.UI.WebControls;

using Telerik.Web.UI;

namespace Csbs.Web.UI
{
    public abstract class BaseComboBox : RadComboBox
    {
        #region Constants

        private const String AllowDisableItemsViewStateKey = "AllowDisableItems";
        private const String AllowBlankSelectionViewState = "AllowBlankSelection";
        private const String EmptyValueTextViewStateKey = "EmptyValueText";
        private const String AutoSelectViewStateKey = "AutoSelect";

        #endregion

        #region Properties

        public virtual Boolean AllowDisableItems
        {
            get { return ViewState[AllowDisableItemsViewStateKey] != null && (Boolean)ViewState[AllowDisableItemsViewStateKey]; }
            set { ViewState[AllowDisableItemsViewStateKey] = value; }
        }

        public virtual String EmptyValueText
        {
            get { return (String)ViewState[EmptyValueTextViewStateKey]; }
            set { ViewState[EmptyValueTextViewStateKey] = value; }
        }

        public Boolean AllowBlankSelection
        {
            get { return ViewState[AllowBlankSelectionViewState] != null && (Boolean)ViewState[AllowBlankSelectionViewState]; }
            set { ViewState[AllowBlankSelectionViewState] = value; }
        }

        public String Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public Boolean HasValue
        {
            get { return !String.IsNullOrEmpty(Value); }
        }

        public Int32? ValueAsInt32
        {
            get
            {
                Int32 val;

                if (Int32.TryParse(Value, out val))
                    return val;

                return null;
            }
            set { Value = value.HasValue ? value.Value.ToString() : null; }
        }

        public Boolean? ValueAsBoolean
        {
            get { return String.IsNullOrEmpty(Value) ? (Boolean?)null : Boolean.Parse(Value); }
            set { Value = value.HasValue ? value.Value.ToString() : Value = null; }
        }

        public Boolean AutoSelect
        {
            get { return ViewState[AutoSelectViewStateKey] != null && (Boolean)ViewState[AutoSelectViewStateKey]; }
            set { ViewState[AutoSelectViewStateKey] = value; }
        }

        public Int32 ManualWidth { get; set; }

        #endregion

        #region Fields

        private Boolean _disableAutoSelect = false;

        #endregion

        #region Construction

        public BaseComboBox()
        {
            DataTextField = "Text";
            DataValueField = "Value";

            AllowCustomText = false;
            MarkFirstMatch = false;

            CausesValidation = false;

            EnableLoadOnDemand = false;
            ItemRequestTimeout = 250;
            LoadingMessage = "Please wait...";

            ExpandAnimation.Type = AnimationType.None;
            CollapseAnimation.Type = AnimationType.None;

            Skin = "Default";

            MaxHeight = Unit.Pixel(155);
            Width = Unit.Pixel(204);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ItemsRequested += BaseComboBox_ItemsRequested;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // If the control width has not been changed from the default, the override it.

            if (ManualWidth > 0)
                Width = ManualWidth;
        }

        #endregion

        #region Data binding

        protected abstract DataTable SelectData(String text);

        private void BaseComboBox_ItemsRequested(Object o, RadComboBoxItemsRequestedEventArgs e)
        {
            try
            {
                Object data = SelectData(e.Text);

                DataSource = data;
                DataBind();

                if (data == null || (data is DataTable && (data as DataTable).Rows.Count == 0))
                    e.Message = "No matches";
            }
            catch (Exception x)
            {
                e.Message = String.Format("Error: {0}", x);
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            if (!EnableLoadOnDemand)
            {
                _disableAutoSelect = false;

                Items.Clear();

                Object data = SelectData(null);

                if (AllowBlankSelection && data != null && data is DataTable)
                    InsertBlankValue(data as DataTable, DataTextField, EmptyValueText);

                DataSource = data;
            }

            base.OnDataBinding(e);
        }

        protected override void OnDataBound(EventArgs e)
        {
            Boolean isNeedClearText = true;

            foreach (RadComboBoxItem item in Items)
            {
                if (Text == item.Text && item.Enabled)
                {
                    base.SelectedValue = item.Value;
                    isNeedClearText = false;
                    break;
                }
            }

            if (isNeedClearText)
                Text = null;

            if (AutoSelect && !_disableAutoSelect)
            {
                RadComboBoxItem autoSelectItem = null;

                foreach (RadComboBoxItem item in Items)
                {
                    if (!String.IsNullOrEmpty(item.Value) && item.Enabled)
                    {
                        if (autoSelectItem != null)
                        {
                            autoSelectItem = null;
                            break;
                        }

                        autoSelectItem = item;
                    }
                }

                if (autoSelectItem != null)
                    SelectedIndex = autoSelectItem.Index;
            }

            base.OnDataBound(e);
        }

        protected override void OnItemDataBound(RadComboBoxItemEventArgs e)
        {
            base.OnItemDataBound(e);

            if (AllowDisableItems)
            {
                RadComboBoxItem item = e.Item;
                DataRowView data = (DataRowView)e.Item.DataItem;

                if (data != null)
                {
                    Object isEnabled = data["IsEnabled"];
                    item.Enabled = isEnabled == DBNull.Value || (Boolean)isEnabled;
                }
            }
        }

        #endregion

        #region Setting and getting input value

        protected virtual String GetValue()
        {
            return SelectedValue;
        }

        protected virtual void SetValue(String value)
        {
            if (!String.IsNullOrEmpty(value))
                SelectedValue = value;
            else
                ClearSelection();
        }

        #endregion

        #region Event handlers

        protected override void OnSelectedIndexChanged()
        {
            if (String.IsNullOrEmpty(SelectedValue))
                _disableAutoSelect = true;

            base.OnSelectedIndexChanged();
        }

        #endregion

        #region Helper methods

        protected static void InsertBlankValue(DataTable table, String textColumn, String text)
        {
            DataRow row = table.NewRow();

            for (Int32 i = 0; i < table.Columns.Count; i++)
            {
                row[i] = !String.IsNullOrEmpty(text) && String.Compare(table.Columns[i].ColumnName, textColumn, true) == 0
                             ? (Object)text
                             : DBNull.Value;
            }

            table.Rows.InsertAt(row, 0);
        }

        protected static void AddDataRow(DataTable table, params Object[] values)
        {
            DataRow dataRow = table.NewRow();

            for (Int32 i = 0; i < table.Columns.Count && i < values.Length; i++)
                dataRow[i] = values[i];

            table.Rows.Add(dataRow);
        }

        #endregion
    }
}