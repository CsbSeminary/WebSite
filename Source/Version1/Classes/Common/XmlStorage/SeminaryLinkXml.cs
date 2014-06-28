using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;
using System.Web.UI;

using CSBS.Utilities;

namespace CSBS.XmlStorage
{
    public class SeminaryLinkXml
    {
        #region Classes

        [Serializable]
        public class SeminaryLinkMenuItem
        {
            #region Private fields

            private String _title = null;
            private String _href = null;

            #endregion

            #region Public properties

            public String Title
            {
                get { return _title; }
            }

            public String Href
            {
                get { return _href; }
            }

            #endregion

            #region Constructor

            public SeminaryLinkMenuItem(XmlNode xmlNode)
            {
                ReadXmlAttributes(xmlNode);
            }

            #endregion

            #region Private methods

            private void ReadXmlAttributes(XmlNode xmlNode)
            {
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "title":
                            _title = attr.Value;
                            break;
                        case "href":
                            _href = attr.Value;
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                    }
                }
            }

            #endregion
        }

        [Serializable]
        public class SeminaryLinkMenuSection
        {
            #region Private fields

            private String _title = null;
            private String _innerHtml = null;
            private Boolean _isHtml = false;
            private List<SeminaryLinkMenuItem> _items = null;

            #endregion

            #region Public properties

            public String Title
            {
                get { return _title; }
            }

            public List<SeminaryLinkMenuItem> Items
            {
                get { return _items; }
            }

            public String InnerHtml
            {
                get { return _innerHtml; }
            }

            public Boolean IsHtml
            {
                get { return _isHtml; }
            }

            #endregion

            #region Constructor

            public SeminaryLinkMenuSection(XmlNode xmlNode, Boolean isHtmlContent)
            {
                ReadXmlAttributes(xmlNode);

                if (isHtmlContent)
                {
                    _isHtml = true;

                    ReadHtmlContent(xmlNode);
                }
                else
                {
                    _items = new List<SeminaryLinkMenuItem>();
                    ReadItems(xmlNode);
                }
            }

            #endregion

            #region Private methods

            private void ReadXmlAttributes(XmlNode xmlNode)
            {
                foreach (XmlAttribute attr in xmlNode.Attributes)
                {
                    switch (attr.Name)
                    {
                        case "title":
                            _title = attr.Value;
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                    }
                }
            }

            private void ReadHtmlContent(XmlNode xmlNode)
            {
                _innerHtml = String.Empty;

                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    switch (childNode.NodeType)
                    {
                        case XmlNodeType.Text:
                        case XmlNodeType.CDATA:
                            _innerHtml += childNode.Value;
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                    }
                }
            }

            private void ReadItems(XmlNode xmlNode)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    SeminaryLinkMenuItem item = new SeminaryLinkMenuItem(childNode);

                    _items.Add(item);
                }
            }

            #endregion
        }

        [Serializable]
        public class SeminaryLinkMenuFile
        {
            #region Private fields

            private String _menuName = null;
            private List<SeminaryLinkMenuSection> _items = null;
            private DateTime? _fileUpdateDate = null;

            #endregion

            #region Public properties

            public String MenuName
            {
                get { return _menuName; }
            }

            public List<SeminaryLinkMenuSection> Items
            {
                get { return _items; }
            }

            public Boolean IsNeedRefresh
            {
                get { return !_fileUpdateDate.HasValue || _fileUpdateDate != FileManager.GetFileUpdateDate(FilePath); }
            }

            public String FilePath
            {
                get { return String.Format(Settings.Menu.SeminaryLink.MenuPathTemplate, _menuName); }
            }

            #endregion

            #region Constructor

            public SeminaryLinkMenuFile(String menuName)
            {
                _menuName = menuName;               
            }

            #endregion

            #region Private methods

            public Boolean Refresh()
            {
                _items = null;

                XmlDocument doc = FileManager.GetXmlDocument(FilePath);

                if (doc == null)
                    return false;

                _items = new List<SeminaryLinkMenuSection>();

                XmlNode root = doc.GetElementsByTagName("menu")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "section":
                            _items.Add(new SeminaryLinkMenuSection(node, false));
                            break;
                        case "sectionHtml":
                            _items.Add(new SeminaryLinkMenuSection(node, true));
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                    }
                }

                _fileUpdateDate = FileManager.GetFileUpdateDate(FilePath);

                return true;
            }

            #endregion
        }

        #endregion

        #region Constants

        private const String CurrentApplicationKey = "SeminaryLinkXml.Current";

        #endregion

        #region Fields

        private Dictionary<String, SeminaryLinkMenuFile> _menuFiles = new Dictionary<String, SeminaryLinkMenuFile>();

        #endregion

        #region Properties

        public static SeminaryLinkXml Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new SeminaryLinkXml();

                return (SeminaryLinkXml)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            _menuFiles.Clear();
        }

        public List<SeminaryLinkMenuSection> GetMenuItems(String menuName)
        {
            List<SeminaryLinkMenuSection> result = null;

            if (String.IsNullOrEmpty(menuName))
                return result;

            String key = menuName.ToLower();
            SeminaryLinkMenuFile file = null;

            if (!_menuFiles.TryGetValue(key, out file))
            {
                file = new SeminaryLinkMenuFile(menuName);

                if (file.Refresh())
                    _menuFiles.Add(key, file);
                else
                    file = null;
            }

            if (file != null)
            {
                if (file.IsNeedRefresh && !file.Refresh())
                {
                    file = null;
                    _menuFiles.Remove(key);
                }
                else
                {
                    result = file.Items;
                }
            }

            return result;
        }

        #endregion
    }
}