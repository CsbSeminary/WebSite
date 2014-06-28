using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

using Csbs.Web.UI;
using System.Xml;

using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public partial class SettingsEditor : UserControl
    {
        #region SettingItem class

        private class SettingItem
        {
            public String Name { get; set; }
            public String Value { get; set; }
        }

        #endregion

        #region Constants

        public const String ActionName = "edit-settings";

        #endregion

        #region Properties

        private String SettingsFilePath
        {
            get { return MapPath("~/App_Data/AppSettings.config"); }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
                LoadData();

            SaveButton.Click += SaveButton_Click;
        }

        #endregion

        #region Event handlers

        private void SaveButton_Click(Object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                SaveData();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.ToString().Replace("\r\n", "<br />");
            }
        }

        #endregion

        #region Load & save data

        private void LoadData()
        {
            List<SettingItem> items = ReadSettingItems();
            SettingItem chapelOnline = items.Find(i => i.Name == "LinkNames.ChapelOnline");

            ChapelOnline.Text = chapelOnline != null ? chapelOnline.Value : null;
        }

        private void SaveData()
        {
            List<SettingItem> items = ReadSettingItems();
            SettingItem chapelOnline = items.Find(i => i.Name == "LinkNames.ChapelOnline");

            if (chapelOnline == null)
            {
                chapelOnline = new SettingItem() { Name = "LinkNames.ChapelOnline" };
                items.Add(chapelOnline);
            }

            chapelOnline.Value = ChapelOnline.Text;

            WriteSettingItems(items);

            Response.Redirect("~/Pages/admin/default.aspx");
        }

        private List<SettingItem> ReadSettingItems()
        {
            List<SettingItem> list = new List<SettingItem>();

            using (XmlReader reader = XmlReader.Create(SettingsFilePath))
            {
                reader.ReadStartElement("appSettings");

                while (reader.IsStartElement("add"))
                {
                    SettingItem item = new SettingItem();
                    item.Name = reader.GetAttribute("key");
                    item.Value = reader.GetAttribute("value");

                    list.Add(item);

                    reader.Read();
                }

                reader.ReadEndElement();
            }

            return list;
        }

        private void WriteSettingItems(List<SettingItem> items)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(SettingsFilePath, settings))
            {
                writer.WriteStartElement("appSettings");

                foreach (SettingItem item in items)
                {
                    writer.WriteStartElement("add");
                    writer.WriteAttributeString("key", item.Name);
                    writer.WriteAttributeString("value", item.Value);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        #endregion
    }
}