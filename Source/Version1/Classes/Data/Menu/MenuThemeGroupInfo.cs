using System;
using System.Xml;
using System.Collections.Generic;

using CSBS.Utilities;

namespace CSBS.Data.Menu
{
    public class MenuThemeGroupInfo
    {
        #region Private fields

        private Int32? _id = null;
        private String _name = null;
        private String _defaultHorizontalMenuSeparator = null;
        private VerticalMenuBulletType _defaultVertiacalMenuBulletType = VerticalMenuBulletType.None;

        private Boolean _isNew = true;
        private Boolean _isDeleted = false;

        private List<MenuGroupInfo> _menuGroups = new List<MenuGroupInfo>();

        #endregion

        #region Properties

        public Int32 ID
        {
            get { return _id.Value; }
        }

        public String Name
        {
            get { return _name; }           
        }

        public List<MenuGroupInfo> MenuGroups
        {
            get { return _menuGroups; }
        }

        public VerticalMenuBulletType DefaultVertiacalMenuBulletType
        {
            get { return _defaultVertiacalMenuBulletType; }
        }

        public String DefaultHorizontalMenuSeparator
        {
            get { return _defaultHorizontalMenuSeparator; }           
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region Constructor

        public MenuThemeGroupInfo(XmlNode xmlNode)
        {
            ReadXmlAttributes(xmlNode);
            ReadChilds(xmlNode);

            _isNew = false;
        }

        #endregion

        #region Private methods

        private void ReadChilds(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "menuGroup":
                        _menuGroups.Add(new MenuGroupInfo(node, this));
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
                    case "name":
                        _name = attr.Value;
                        break;
                    case "defaultHorizontalMenuSeparator":
                        _defaultHorizontalMenuSeparator = attr.Value;
                        break;
                    case "defaultVertiacalMenuBulletType":
                        _defaultVertiacalMenuBulletType = (VerticalMenuBulletType)StringHelper.ToInt32(attr.Value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        #endregion

        #region Public methods

        public Boolean SetName(String value)
        {
            Boolean isOk = false;

            if (!String.IsNullOrEmpty(value))
            {
                String name = StringHelper.ReplaceNonAlphabetCharacters(value.ToLower());
                
                MenuThemeGroupInfo existsGroup = MenuManager.Current.FindThemeGroupByName(name);

                if (existsGroup == null || !_isNew && _name == existsGroup.Name)
                {
                    _name = name;

                    isOk = true;
                }
            }

            return isOk;
        }

        #endregion

        #region Saving

        public void Write(XmlTextWriter writer)
        {
            if (_isDeleted)
                return;

            writer.WriteStartElement("group");
            writer.WriteAttributeString("id", _id.Value.ToString());
            writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("defaultHorizontalMenuSeparator", _defaultHorizontalMenuSeparator);
            writer.WriteAttributeString("defaultVertiacalMenuBulletType", ((Int32)_defaultVertiacalMenuBulletType).ToString());

            foreach (MenuGroupInfo section in _menuGroups)
            {
                section.Write(writer);
            }

            writer.WriteEndElement();
        }

        #endregion
    }
}