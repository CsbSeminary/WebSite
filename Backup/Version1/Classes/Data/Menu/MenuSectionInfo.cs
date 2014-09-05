using System;
using System.Collections.Generic;
using System.Xml;

using Csbs.Utilities;

namespace Csbs.Data.Menu
{
    public class MenuSectionInfo
    {
        #region Private fields

        private Int32? _id = null;
        private String _title = null;       
        private Boolean _showHeader = true;
        private Boolean _isHtml = false;
        private String _innerHtml = null;

        private List<MenuItemInfo> _items = null;

        private Boolean _isNew = true;
        private Boolean _isDeleted = false;

        private MenuInfo _parent = null;

        #endregion

        #region Public properties

        public Int32 ID
        {
            get { return _id.Value; }
            set { if (IsNew) _id = value; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public Boolean ShowHeader
        {
            get { return _showHeader; }
            set { _showHeader = value; }
        }

        public Boolean IsHtml
        {
            get { return _isHtml; }
            set { _isHtml = value; }
        }

        public String InnerHtml
        {
            get { return _innerHtml; }
            set { _innerHtml = value; }
        }

        public List<MenuItemInfo> Items
        {
            get { return _items; }
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region Computed properties

        public Int32? MenuID
        {
            get { return _parent != null ? _parent.ID : (Int32?)null; }
        }

        public VerticalMenuBulletType Bullet
        {
            get { return _parent.Bullet; }
        }

        public String HorizontalSeparator
        {
            get { return _parent.HorizontalSeparator; }
        }

        public Boolean AllowDelete
        {
            get { return true; }
        }

        #endregion

        #region Constructor

        public MenuSectionInfo(MenuInfo parent)
        {
            _isNew = true;
            _items = new List<MenuItemInfo>();
            _parent = parent;
        }

        public MenuSectionInfo(XmlNode xmlNode, MenuInfo parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;

            ReadXmlAttributes(xmlNode);

            if (!_isHtml)
                _items = new List<MenuItemInfo>();

            ReadChilds(xmlNode);

            _isNew = false;
        }

        #endregion

        #region Private methods

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

        private void ReadChilds(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "items":
                        ReadItems(childNode);
                        break;
                    case "html":
                        ReadHtmlContent(childNode);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }
        }

        private void ReadItems(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "item":
                        _items.Add(new MenuItemInfo(node, this));
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
                    case "id":
                        _id = StringHelper.ToInt32(attr.Value);
                        break;
                    case "html":
                        _isHtml = StringHelper.ToBoolean(attr.Value);
                        break;
                    case "title":
                        _title = attr.Value;
                        break;
                    case "showHeader":
                        _showHeader = StringHelper.ToBoolean(attr.Value);
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
            if (_isDeleted)
                return;

            writer.WriteStartElement("section");
            writer.WriteAttributeString("id", _id.Value.ToString());
            writer.WriteAttributeString("showHeader", _showHeader ? "1" : "0");
            writer.WriteAttributeString("html", _isHtml ? "1" : "0");

            if (!String.IsNullOrEmpty(_title))
                writer.WriteAttributeString("title", _title);

            if (_isHtml)
            {
                writer.WriteStartElement("html");

                writer.WriteCData(_innerHtml);

                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement("items");

                if (_items != null)
                {
                    foreach (MenuItemInfo item in _items)
                    {
                        item.Write(writer);
                    }
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        #endregion

        #region Public methods

        public Boolean MoveUp()
        {
            Boolean isOk = false;

            if (!IsNew)
                isOk = _parent.MoveUp(this);

            return isOk;
        }

        public Boolean MoveDown()
        {
            Boolean isOk = false;

            if (!IsNew)
                isOk = _parent.MoveDown(this);

            return isOk;
        }

        public Boolean MoveUp(MenuItemInfo menuItem)
        {
            Boolean isOk = false;

            Int32 menuItemIndex = _items.IndexOf(menuItem);

            if (menuItemIndex > 0)
            {
                _items.RemoveAt(menuItemIndex);
                _items.Insert(menuItemIndex - 1, menuItem);

                isOk = true;
            }

            return isOk;
        }

        public Boolean MoveDown(MenuItemInfo menuItem)
        {
            Boolean isOk = false;

            Int32 menuItemIndex = _items.IndexOf(menuItem);

            if (menuItemIndex >= 0 && menuItemIndex < (_items.Count - 1))
            {
                _items.RemoveAt(menuItemIndex);
                _items.Insert(menuItemIndex + 1, menuItem);

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

        public Boolean AddMenuItem(MenuItemInfo menuItem)
        {
            Boolean isOk = false;

            if (menuItem.IsNew)
            {
                _items.Add(menuItem);

                isOk = true;
            }

            return isOk;

        }

        #endregion
    }
}