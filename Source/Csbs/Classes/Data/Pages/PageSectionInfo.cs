using System;
using System.Collections.Generic;
using System.Xml;

using Csbs.Utilities;

namespace Csbs.Data.Pages
{
    [Serializable]
    public class PageSectionInfo
    {
        #region Private fields

        private String _name = null;
        private String _defaultTheme = null;
        private Boolean _isNew = false;
        private Boolean _isDeleted = false;
        private List<PageInfo> _pages = new List<PageInfo>();
        private PageGroupInfo _parent = null;

        #endregion

        #region Public properties

        public String Name
        {
            get { return _name; }
        }

        public String DefaultTheme
        {
            get { return _defaultTheme; }
        }

        public PageGroupInfo Parent
        {
            get { return _parent; }
        }

        public List<PageInfo> Pages
        {
            get { return _pages; }
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region Computed properties

        public String FullName
        {
            get { return GetFullName(_parent, _name); }
        }

        public String MasterPage
        {
            get { return _parent.MasterPage; }
        }

        public String MasterPageFilePath
        {
            get { return _parent.MasterPageFilePath; }
        }

        public Boolean AllowDelete
        {
            get { return _pages.Count <= 0; }
        }

        public Boolean IsDeleted
        {
            get { return _isDeleted || (_parent != null && _parent.IsDeleted); }
        }

        public String GroupName
        {
            get { return _parent.Name; }
        }

        #endregion

        #region Constructor

        public PageSectionInfo(PageGroupInfo parentGroup)
        {
            _parent = parentGroup;
            _isNew = true;
        }

        public PageSectionInfo(XmlNode xmlNode, PageGroupInfo parentGroup)
        {
            if (parentGroup == null)
                throw new ArgumentNullException("parentGroup");

            _parent = parentGroup;

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
                    case "pages":
                        ReadPages(childNode);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }
        }

        private void ReadPages(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "page":
                        _pages.Add(new PageInfo(node, this));
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                }
            }
        }

        private void ReadXmlAttributes(XmlNode xmlNode)
        {
            foreach (XmlAttribute attr in xmlNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "name":
                        _name = attr.Value;
                        break;
                    case "defaultTheme":
                        _defaultTheme = attr.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        #endregion

        #region Saving

        public void Write(XmlTextWriter writer)
        {
            if (IsDeleted)
                return;

            writer.WriteStartElement("section");
            writer.WriteAttributeString("name", _name);

            if (!String.IsNullOrEmpty(_defaultTheme))
                writer.WriteAttributeString("defaultTheme", _defaultTheme);

            writer.WriteStartElement("pages");

            foreach (PageInfo pageInfo in _pages)
            {
                pageInfo.Write(writer);
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region Public methods

        public Boolean SetName(String value)
        {
            Boolean isOk = false;

            if (!String.IsNullOrEmpty(value))
            {
                String name = StringHelper.ReplaceNonAlphabetCharacters(value.ToLower());

                PageSectionInfo existsSection = PageManager.Current.FindSection(GetFullName(_parent, name));

                if (existsSection == null || !IsNew && this.Name == existsSection.Name)
                {
                    _name = name;

                    isOk = true;
                }
            }

            return isOk;
        }

        public Boolean SetTheme(String value)
        {
            Boolean isOk = false;

            if (FileManager.IsMasterThemeExists(MasterPage, value))
            {
                _defaultTheme = value;

                isOk = true;
            }

            return isOk;
        }

        public Boolean TryDelete()
        {
            if (AllowDelete)
                _isDeleted = true;

            return _isDeleted;
        }

        public PageInfo CreatePage(Int32 pageID, String pageName)
        {
            PageInfo page = new PageInfo(pageID, this);

            if (page.SetName(pageName))
                _pages.Add(page);
            else
                page = null;

            return page;
        }

        public Boolean MovePage(Int32 pageID)
        {
            Boolean isOk = false;

            PageInfo page = PageManager.Current.FindPage(pageID);

            if (page != null && !page.IsNew)
            {
                Int32 index = page.Section._pages.IndexOf(page);

                if (index >= 0 && page.CanMoveTo(this))
                {
                    page.Section._pages.RemoveAt(index);

                    _pages.Add(page);

                    isOk = true;
                }
            }

            return isOk;
        }

        #endregion

        #region Helpers

        private static String GetFullName(PageGroupInfo group, String pageName)
        {
            return group.Name + "." + pageName;
        }

        #endregion
    }
}