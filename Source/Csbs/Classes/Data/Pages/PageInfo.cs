using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Xml;
using Csbs.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Data.Menu;
using Csbs.Utilities;

namespace Csbs.Data.Pages
{
    [Serializable]
    public class PageInfo
    {
        #region Constants

        private const String FileNameHeaderPostfix = "header";
        private const String FileNameSideColumnPostfix = "side";

        #endregion

        #region Private fields

        private Int32? _id = null;
        private String _name = null;
        private String _fileName = null;
        private String _title = null;
        private Boolean _adminOnly = false;
        private String _theme = null;
        private String _headerTitle = null;
        private Boolean _showColumnsSeparator = false;
        private String _headerLinkUrl = null;
        private String _headerLinkText = null;
        private String _headerLinkTarget = null;
        private Boolean _headerLinkVisible = false;
        private String _metaKeywords = null;
        private String _metaDescription = null;
        private MenuPositionType _menuPosition = MenuPositionType.None;
        private ContentLayoutType _contentLayout = ContentLayoutType.OneColumn;
        private ContentHeaderLayoutType _headerLayout = ContentHeaderLayoutType.None;
        private ContentLayoutSideColumnSize _sideColumnSize = ContentLayoutSideColumnSize.Small;
        private ContentPositionType _contentPostion = ContentPositionType.Right;
        private ContentHeaderPositionType _headerPosition = ContentHeaderPositionType.TopOfPage;

        private Boolean _isDeleted = false;
        private Boolean _isNew = true;

        private List<String> _menuItems = new List<String>();

        private PageSectionInfo _parent = null;

        // Cache

        private String _contentCache = null;
        private String _headCache = null;

        #endregion

        #region Public properties

        public Int32 ID
        {
            get { return _id.Value; }
        }

        public String Name
        {
            get { return _name; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String HeaderTitle
        {
            get { return _headerTitle; }
            set { _headerTitle = value; }
        }

        public String HeaderLinkUrl
        {
            get { return _headerLinkUrl; }
            set { _headerLinkUrl = value; }
        }

        public String HeaderLinkText
        {
            get { return _headerLinkText; }
            set { _headerLinkText = value; }
        }

        public String MetaKeywords
        {
            get { return _metaKeywords; }
            set { _metaKeywords = value; }
        }

        public String MetaDescription
        {
            get { return _metaDescription; }
            set { _metaDescription = value; }
        }

        public String Theme
        {
            get { return String.IsNullOrEmpty(_theme) ? _parent.DefaultTheme : _theme; }
            set { _theme = value; }
        }

        public ContentHeaderPositionType HeaderPosition
        {
            get { return _headerPosition; }
            set { _headerPosition = value; }
        }

        public ContentPositionType ContentPostion
        {
            get { return _contentPostion; }
            set { _contentPostion = value; }
        }

        public Boolean HeaderLinkVisible
        {
            get { return _headerLinkVisible; }
            set { _headerLinkVisible = value; }
        }

        public String HeaderLinkTarget
        {
            get { return _headerLinkTarget; }
            set { _headerLinkTarget = value; }
        }

        public Boolean ShowColumnsSeparator
        {
            get { return _showColumnsSeparator; }
            set { _showColumnsSeparator = value; }
        }

        public ContentLayoutSideColumnSize SideColumnSize
        {
            get { return _sideColumnSize; }
            set { _sideColumnSize = value; }
        }

        public ContentHeaderLayoutType HeaderLayout
        {
            get { return _headerLayout; }
            set { _headerLayout = value; }
        }

        public ContentLayoutType ContentLayout
        {
            get { return _contentLayout; }
            set { _contentLayout = value; }
        }

        public MenuPositionType MenuPosition
        {
            get { return _menuPosition; }
            set { _menuPosition = value; }
        }

        public Boolean AdminOnly
        {
            get { return _adminOnly; }
            set { _adminOnly = value; }
        }

        #endregion

        #region Computed properties

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        public String FullTitle
        {
            get { return String.IsNullOrEmpty(_title) ? null : "CSBS :: " + _title; }
        }

        public String MasterPage
        {
            get { return _parent.MasterPage; }
        }

        public String MasterPageFilePath
        {
            get { return _parent.MasterPageFilePath; }
        }

        public String FullName
        {
            get { return GetFullName(_parent, _name); }
        }

        public Boolean IsDeleted
        {
            get { return _isDeleted || (_parent != null && _parent.IsDeleted); }
        }

        public String ContentColumnHtml
        {
            get { return FileManager.GetPageContent(_fileName, null); }
        }

        public String HeaderTemplateHtml
        {
            get { return FileManager.GetPageContent(_fileName, FileNameHeaderPostfix); }
        }

        public String SideColumnHtml
        {
            get { return FileManager.GetPageContent(_fileName, FileNameSideColumnPostfix); }
        }

        public String SectionName
        {
            get { return _parent.FullName; }
        }

        public PageSectionInfo Section
        {
            get { return _parent; }
        }

        public String GroupName
        {
            get { return _parent.GroupName; }
        }

        #endregion

        #region Constructor

        public PageInfo(PageSectionInfo parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;
        }

        public PageInfo(Int32 id, PageSectionInfo parent)
            : this(parent)
        {
            _id = id;
            _isNew = true;
        }

        public PageInfo(XmlNode xmlNode, PageSectionInfo parent)
            : this(parent)
        {
            ReadXmlAttributes(xmlNode);
            ReadChilds(xmlNode);

            _isNew = false;
        }

        #endregion

        #region Loading

        private void ReadChilds(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "metaDescription":
                        _metaDescription = ReadInnerText(childNode);
                        break;
                    case "metaKeywords":
                        _metaKeywords = ReadInnerText(childNode);
                        break;
                    case "menuItems":
                        ReadMenuItems(childNode);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }
        }

        private void ReadMenuItems(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "item":
                        _menuItems.Add(childNode.Attributes["name"].Value);
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
                        _id = StringHelper.ToInt32(attr.Value);
                        break;
                    case "pageTitle":
                        _title = attr.Value;
                        break;
                    case "name":
                        _name = attr.Value;
                        break;
                    case "adminOnly":
                        _adminOnly = StringHelper.ToBoolean(attr.Value);
                        break;
                    case "theme":
                        _theme = attr.Value;
                        break;
                    case "menuPosition":
                        _menuPosition = (MenuPositionType)StringHelper.ToInt32(attr.Value);
                        break;
                    case "contentLayout":
                        _contentLayout = (ContentLayoutType)StringHelper.ToInt32(attr.Value);
                        break;
                    case "headerLayout":
                        _headerLayout = (ContentHeaderLayoutType)StringHelper.ToInt32(attr.Value);
                        break;
                    case "sideColumnSize":
                        _sideColumnSize = (ContentLayoutSideColumnSize)StringHelper.ToInt32(attr.Value);
                        break;
                    case "headerTitle":
                        _headerTitle = attr.Value;
                        break;
                    case "showColumnsSeparator":
                        _showColumnsSeparator = StringHelper.ToBoolean(attr.Value);
                        break;
                    case "headerLinkText":
                        _headerLinkText = attr.Value;
                        break;
                    case "headerLinkUrl":
                        _headerLinkUrl = attr.Value;
                        break;
                    case "headerLinkTarget":
                        _headerLinkTarget = attr.Value;
                        break;
                    case "headerLinkVisible":
                        _headerLinkVisible = StringHelper.ToBoolean(attr.Value);
                        break;
                    case "contentPosition":
                        _contentPostion = (ContentPositionType)StringHelper.ToInt32(attr.Value);
                        break;
                    case "headerPosition":
                        _headerPosition = (ContentHeaderPositionType)StringHelper.ToInt32(attr.Value);
                        break;
                    case "fileName":
                        _fileName = attr.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        private static String ReadInnerText(XmlNode xmlNode)
        {
            String result = null;

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        result += childNode.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }

            return result;
        }

        #endregion

        #region Saving

        public void Write(XmlTextWriter writer)
        {
            if (IsDeleted)
            {
                FileManager.TryToDeletePageContent(_fileName, null, FileNameHeaderPostfix, FileNameSideColumnPostfix);
                return;
            }

            writer.WriteStartElement("page");

            writer.WriteAttributeString("id", ValueHelper.ToString(_id.Value));
            writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("fileName", _fileName);
            writer.WriteAttributeString("pageTitle", _title);
            writer.WriteAttributeString("adminOnly", ValueHelper.ToString(_adminOnly));

            if (!String.IsNullOrEmpty(_theme) && String.Compare(_parent.DefaultTheme, _theme, true) != 0) 
                writer.WriteAttributeString("theme", _theme);

            writer.WriteAttributeString("menuPosition", ValueHelper.ToString(_menuPosition));
            writer.WriteAttributeString("contentLayout", ValueHelper.ToString(_contentLayout));
            writer.WriteAttributeString("headerLayout", ValueHelper.ToString(_headerLayout));
            writer.WriteAttributeString("sideColumnSize", ValueHelper.ToString(_sideColumnSize));

            if (!String.IsNullOrEmpty(_headerTitle))
                writer.WriteAttributeString("headerTitle", _headerTitle);

            writer.WriteAttributeString("showColumnsSeparator", ValueHelper.ToString(_showColumnsSeparator));

            if (!String.IsNullOrEmpty(_headerLinkText))
                writer.WriteAttributeString("headerLinkText", _headerLinkText);

            if (!String.IsNullOrEmpty(_headerLinkUrl))
                writer.WriteAttributeString("headerLinkUrl", _headerLinkUrl);

            if (!String.IsNullOrEmpty(_headerLinkTarget))
                writer.WriteAttributeString("headerLinkTarget", _headerLinkTarget);

            writer.WriteAttributeString("headerLinkVisible", ValueHelper.ToString(_headerLinkVisible));
            writer.WriteAttributeString("contentPosition", ValueHelper.ToString(_contentPostion));
            writer.WriteAttributeString("headerPosition", ValueHelper.ToString(_headerPosition));

            writer.WriteStartElement("metaDescription");
            writer.WriteCData(_metaDescription);
            writer.WriteEndElement();

            writer.WriteStartElement("metaKeywords");
            writer.WriteCData(_metaKeywords);
            writer.WriteEndElement();

            writer.WriteStartElement("menuItems");

            foreach (String menuItemName in _menuItems)
            {
                writer.WriteStartElement("item");
                writer.WriteAttributeString("name", menuItemName);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region Rendering

        public void RenderHead(PlaceHolder ph)
        {
            if (ph == null)
                throw new ArgumentNullException("PlaceHolder");

            if (String.IsNullOrEmpty(_headCache))
            {
                StringBuilder sb = new StringBuilder();

                RenderHelper.WritePageHeadContent(sb, MasterPage, Theme, _metaDescription, _metaKeywords);

                _headCache = sb.ToString();
            }

            ph.Controls.Add(new LiteralControl(_headCache));
        }

        public void RenderContent(PlaceHolder ph)
        {
            if (ph == null)
                throw new ArgumentNullException("PlaceHolder");

            if (String.IsNullOrEmpty(_contentCache))
            {
                StringBuilder sb = new StringBuilder();

                RenderHelper.WritePageHtmlContent(sb, this);

                _contentCache = sb.ToString();
            }

            ph.Controls.Add(new LiteralControl(_contentCache));
        }

        #endregion

        #region Public methods

        public Boolean CanMoveTo(PageSectionInfo section)
        {
            PageInfo existsPage = PageManager.Current.FindPage(GetFullName(section, _name));

            return !IsNew && (existsPage == null || this.ID == existsPage.ID);
        }

        public Boolean SetName(String value)
        {
            Boolean isOk = false;

            if (!String.IsNullOrEmpty(value))
            {
                String name = StringHelper.ReplaceNonAlphabetCharacters(value.ToLower());

                PageInfo existsPage = PageManager.Current.FindPage(GetFullName(_parent, name));

                if (existsPage == null || !IsNew && this.ID == existsPage.ID)
                {
                    _name = name;

                    isOk = true;
                }
            }

            return isOk;
        }

        public Boolean TryDelete()
        {
            _isDeleted = true;

            return _isDeleted;
        }

        public StringCollection GetMenuItems()
        {
            StringCollection result = new StringCollection();

            foreach (String menuItemsName in _menuItems)
            {
                result.Add(menuItemsName);
            }

            return result;
        }

        public Boolean AddMenuItem(String name)
        {
            Boolean isAdded = false;

            if (!_menuItems.Contains(name))
            {
                _menuItems.Add(name);

                isAdded = true;
            }

            return isAdded;
        }

        public void ClearMenuItems()
        {
            _menuItems.Clear();
        }

        public Boolean SetContentHtml(String contentColumn, String sideColumn, String headerHtml)
        {
            Boolean isOk = false;

            if (IsNew)
                _fileName = FileManager.CreatePageHtmlFile(FullName, contentColumn);
            else
                FileManager.SavePageHtmlFile(_fileName, null, contentColumn);

            if (!String.IsNullOrEmpty(_fileName))
            {
                FileManager.SavePageHtmlFile(_fileName, FileNameSideColumnPostfix, sideColumn);
                FileManager.SavePageHtmlFile(_fileName, FileNameHeaderPostfix, headerHtml);

                isOk = true;
            }

            return isOk;
        }

        #endregion

        #region Helpers

        private static String GetFullName(PageSectionInfo section, String name)
        {
            return section.FullName + "." + name;
        }

        #endregion
    }
}