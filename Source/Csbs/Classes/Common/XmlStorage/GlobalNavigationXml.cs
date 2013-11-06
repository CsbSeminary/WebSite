using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

using CSBS.Utilities;

namespace CSBS.XmlStorage
{
    public class GlobalNavigationXml
    {
        #region Classes

        [Serializable]
        public class GlobalNavigationMenuItem
        {
            #region Private fields

            private String _title = null;
            private String _href = null;
            private String[] _visibleOn = null;
            private Boolean _isAdminAccessOnly = false;

            #endregion

            #region Public properties

            public Boolean IsAdminAccessOnly
            {
                get { return _isAdminAccessOnly; }
            }

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

            public GlobalNavigationMenuItem(XmlNode xmlNode)
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
                        case "visibleOn":
                            if (!String.IsNullOrEmpty(attr.Value))
                                _visibleOn = attr.Value.ToLower().Split(';');
                            break;
                        case "adminAccessOnly":
                            _isAdminAccessOnly = StringHelper.ToBoolean(attr.Value);
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                    }
                }
            }

            #endregion

            #region Public methods

            public Boolean IsVisible(String pagePath, Boolean isAuthenticated)
            {
                Boolean isVisible = true;

                if (_isAdminAccessOnly && !isAuthenticated)
                {
                    isVisible = false;
                }
                else if (_visibleOn != null && !String.IsNullOrEmpty(pagePath))
                {
                    isVisible = false;

                    foreach (String pathTemplate in _visibleOn)
                    {
                        if (pagePath.ToLower().StartsWith(pathTemplate))
                            isVisible = true;
                    }
                }

                return isVisible;
            }

            #endregion
        }

        #endregion

        #region Constants

        private const String CurrentApplicationKey = "GlobalNavigationXml.Current";

        #endregion

        #region Fields

        private DateTime? _fileUpdateDate = null;
        private List<GlobalNavigationMenuItem> _items = null;

        #endregion

        #region Public properties

        public static GlobalNavigationXml Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new GlobalNavigationXml();

                return (GlobalNavigationXml)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        public List<GlobalNavigationMenuItem> MenuItems
        {
            get
            {
                if (IsNeedRefresh || _items == null)
                {
                    _items = ParseXml();
                    _fileUpdateDate = _items != null && _items.Count > 0 ? FileManager.GetFileUpdateDate(XmlPath) : null;
                }

                return _items;
            }
            set
            {
                if (value == null)
                    _items = value;
            }
        }

        #endregion

        #region Private properties

        private static String XmlPath
        {
            get { return Settings.Menu.GlobalNavigationXml; }
        }

        private Boolean IsNeedRefresh
        {
            get { return !_fileUpdateDate.HasValue || _fileUpdateDate != FileManager.GetFileUpdateDate(XmlPath); }
        }

        #endregion

        #region Static methods

        private static List<GlobalNavigationMenuItem> ParseXml()
        {
            List<GlobalNavigationMenuItem> data = null;
            XmlDocument doc = FileManager.GetXmlDocument(XmlPath);

            if (doc != null)
            {
                data = new List<GlobalNavigationMenuItem>();

                XmlNode root = doc.GetElementsByTagName("menu")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "item":
                            data.Add(new GlobalNavigationMenuItem(node));
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
            MenuItems = null;
            _fileUpdateDate = null;
        }

        #endregion
    }
}