using System;
using System.Collections.Generic;
using System.Web;

using Csbs.Web;
using System.Xml;
using System.Text;

using Csbs.Utilities;
using Csbs.Data.Menu;
using Csbs.Data;

namespace Csbs.XmlStorage
{
    public class ContactFormXml
    {
        #region Classes

        [Serializable]
        public class ContactFormData
        {
            #region Private fields

            private String _id = null;
            private Boolean _displayInMenu = true;
            private String _name = null;
            private String _title = null;
            private String _description = null;
            private String _email = null;

            #endregion

            #region Public properties

            public String ID
            {
                get { return _id; }
            }

            public Boolean IsDisplayInMenu
            {
                get { return _displayInMenu; }
            }

            public String Name
            {
                get { return _name; }
            }

            public String Title
            {
                get { return _title; }
            }

            public String Description
            {
                get { return _description; }
            }

            public String Email
            {
                get { return _email; }
            }

            #endregion

            #region Constructor

            public ContactFormData(XmlNode xmlNode)
            {
                ReadXmlAttributes(xmlNode);
                ReadChildNodes(xmlNode);
            }

            #endregion

            #region Private methods

            private void ReadChildNodes(XmlNode xmlNode)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "title":
                            _title = childNode.InnerText;
                            break;
                        case "description":
                            _description = childNode.InnerText;
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                    }
                }
            }

            private void ReadXmlAttributes(XmlNode xmlNode)
            {
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "id":
                            _id = attr.Value;
                            break;
                        case "name":
                            _name = attr.Value;
                            break;
                        case "email":
                            _email = attr.Value;
                            break;
                        case "displayInMenu":
                            _displayInMenu = StringHelper.ToBoolean(attr.Value);
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Constants

        private const String CurrentApplicationKey = "ContactFormData.Current";

        #endregion

        #region Fields

        private List<ContactFormData> _data = null;
        private DateTime? _fileUpdateDate = null;

        #endregion

        #region Public properties

        public static ContactFormXml Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new ContactFormXml();

                return (ContactFormXml)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        #endregion

        #region Private properties

        private static String XmlPath
        {
            get { return Settings.Menu.ContactFormsXml; }
        }

        private List<ContactFormData> FormsData
        {
            get
            {
                if (IsNeedRefresh || _data == null)
                {
                    _data = ParseXml();
                    _fileUpdateDate = _data != null && _data.Count > 0 ? FileManager.GetFileUpdateDate(XmlPath) : null;
                }

                return _data;
            }
            set
            {
                if (value == null)
                    _data = value;
            }
        }

        private Boolean IsNeedRefresh
        {
            get { return !_fileUpdateDate.HasValue || _fileUpdateDate != FileManager.GetFileUpdateDate(XmlPath); }
        }

        #endregion

        #region Static methods

        private static List<ContactFormData> ParseXml()
        {
            List<ContactFormData> data = null;
            XmlDocument doc = FileManager.GetXmlDocument(XmlPath);

            if (doc != null)
            {
                data = new List<ContactFormData>();

                XmlNode root = doc.GetElementsByTagName("forms")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "form":
                            data.Add(new ContactFormData(node));
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                    }
                }
            }

            return data;
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            FormsData = null;
            _fileUpdateDate = null;
        }

        public ContactFormData GetItemByID(String id)
        {
            foreach (ContactFormData item in FormsData)
            {
                if (String.Compare(item.ID, id, true) == 0)
                    return item;
            }

            return null;
        }

        #endregion

        #region Helper methods
        
        public MenuInfo GetMenuInfo()
        {
            MenuInfo menu = new MenuInfo();
            menu.Bullet = VerticalMenuBulletType.Sun;

            MenuSectionInfo section = new MenuSectionInfo(menu);
            section.Title = "Other Contacts";

            foreach (ContactFormData formData in FormsData)
            {
                if (formData.IsDisplayInMenu)
                {
                    MenuItemInfo menuItem = new MenuItemInfo();

                    menuItem.Title = formData.Name;
                    menuItem.Href = LinkUtils.ResolveClientUrl(String.Format(Settings.Forms.ContactUs.UrlTemplate, formData.ID));

                    section.AddMenuItem(menuItem);
                }
            }

            menu.AddSection(section);

            return menu;
        }

        #endregion
    }
}