using System;
using System.Xml;
using System.Collections.Generic;

using CSBS.Utilities;

namespace CSBS.Data.Menu
{
    public class MenuGroupInfo
    {
        #region Private fields

        private Int32? _id = null;
        private String _name = null;

        private List<MenuInfo> _menus = new List<MenuInfo>();

        private MenuThemeGroupInfo _parent = null;

        #endregion

        #region Properties

        public Int32? ID
        {
            get { return _id.Value; }
        }

        public String Name
        {
            get { return _name; }
        }

        public List<MenuInfo> Menus
        {
            get { return _menus; }
        }

        #endregion

        #region Computed properties

        public String FullName
        {
            get { return String.Format("{0}.{1}", _parent.Name, _name); }
        }

        public String HorizontalMenuSeparator
        {
            get { return _parent.DefaultHorizontalMenuSeparator; }
        }

        public VerticalMenuBulletType VerticalMenuBullet
        {
            get { return _parent.DefaultVertiacalMenuBulletType; }
        }

        #endregion

        #region Constructor

        public MenuGroupInfo(XmlNode xmlNode, MenuThemeGroupInfo parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;

            ReadXmlAttributes(xmlNode);
            ReadChilds(xmlNode);
        }

        #endregion

        #region Private methods

        private void ReadChilds(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "menu":
                        _menus.Add(new MenuInfo(childNode, this));
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
                    case "name":
                        _name = attr.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        #endregion
    }
}