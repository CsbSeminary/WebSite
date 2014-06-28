using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using Csbs.Web;

using Csbs.Utilities;
using Csbs.Data.Pages;
using Csbs.Data.Articles;

namespace Csbs.Data.Menu
{
    public class MenuInfo
    {
        #region Private fields

        private Int32? _id = null;
        private String _name = null;
        private String _horizontalSeparator = "|";
        private VerticalMenuBulletType _bullet = VerticalMenuBulletType.Triangle;

        private List<MenuSectionInfo> _sections = new List<MenuSectionInfo>();

        private Boolean _isNew = true;
        private Boolean _isDeleted = false;

        #endregion

        #region Properties

        public Int32 ID
        {
            get { return _id.Value; }
            set { if (IsNew) _id = value; }
        }

        public String Name
        {
            get { return _name; }
        }

        public List<MenuSectionInfo> Sections
        {
            get { return _sections; }
        }

        public VerticalMenuBulletType Bullet
        {
            get { return _bullet; }
            set { _bullet = value; }
        }

        public String HorizontalSeparator
        {
            get { return _horizontalSeparator; }
            set { _horizontalSeparator = value; }
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region Computed properties

        public Boolean AllowDelete
        {
            get { return _sections.Count <= 0; }
        }

        #endregion

        #region Constructor

        public MenuInfo()
        {
            _isNew = true;
        }

        public MenuInfo(XmlNode xmlNode)
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
                    case "sections":
                        ReadSections(node);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                }
            }
        }

        private void ReadSections(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "section":
                        _sections.Add(new MenuSectionInfo(node, this));
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
                    case "hSeparator":
                        _horizontalSeparator = attr.Value;
                        break;
                    case "bullet":
                        _bullet = (VerticalMenuBulletType)StringHelper.ToInt32(attr.Value);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        #endregion

        #region Rendering

        public void Render(StringBuilder sb, PageInfo page, BlogArticle article, MenuRenderingType direction)
        {
            foreach (MenuSectionInfo section in _sections)
            {
                if (section.ShowHeader && !String.IsNullOrEmpty(section.Title))
                    sb.AppendFormat("<div class='menu-section-title'>{0}</div>", section.Title);

                if (!section.IsHtml)
                {
                    if (direction == MenuRenderingType.Horizontal)
                        RenderSectionHorizontal(sb, page, article, section);
                    else if (direction == MenuRenderingType.Vertical)
                        RenderSectionVertical(sb, page, article, section);
                }
                else
                {
                    sb.AppendFormat("<div class='menu-list-container'>{0}</div>", section.InnerHtml);
                }
            }
        }

        private void RenderSectionHorizontal(StringBuilder sb, PageInfo page, BlogArticle article, MenuSectionInfo section)
        {
            foreach (MenuItemInfo item in section.Items)
            {
                if (item.IsVisible(page, article))
                {
                    sb.AppendFormat(" {0} ", section.HorizontalSeparator);

                    item.Render(sb, page, article);
                }
            }

            sb.AppendFormat(" {0} ", section.HorizontalSeparator);
        }

        private void RenderSectionVertical(StringBuilder sb, PageInfo page, BlogArticle article, MenuSectionInfo section)
        {
            String listClass = section.Bullet == VerticalMenuBulletType.Sun
                ? "sun-bullet-list"
                : section.Bullet == VerticalMenuBulletType.Triangle
                    ? "triangle-bullet-list"
                    : null;

            sb.AppendFormat("<div class='menu-list-container'><ul class='{0}'>", listClass);

            foreach (MenuItemInfo item in section.Items)
            {
                if (item.IsVisible(page, article))
                {
                    sb.Append("<li>");
                    item.Render(sb, page, article);
                    sb.Append("</li>");
                }
            }

            sb.Append("</ul></div>");
        }

        #endregion

        #region Saving

        public void Write(XmlTextWriter writer)
        {
            if (_isDeleted)
                return;

            writer.WriteStartElement("menu");
            writer.WriteAttributeString("id", _id.Value.ToString());
            writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("hSeparator", _horizontalSeparator);
            writer.WriteAttributeString("bullet", ((Int32)_bullet).ToString());

            writer.WriteStartElement("sections");

            foreach (MenuSectionInfo section in _sections)
            {
                section.Write(writer);
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region Public methods

        public Boolean TryDelete()
        {
            if (AllowDelete)
                _isDeleted = true;

            return _isDeleted;
        }

        public Boolean SetName(String value)
        {
            Boolean isOk = false;

            if (!String.IsNullOrEmpty(value))
            {
                String name = SanitizeName(value.ToLower());

                MenuInfo existsMenu = MenuManager.Current.FindMenuByName(name);

                if (existsMenu == null || !_isNew && _id == existsMenu.ID)
                {
                    _name = name;

                    isOk = true;
                }
            }

            return isOk;
        }

        public Boolean MoveUp(MenuSectionInfo section)
        {
            Boolean isOk = false;

            Int32 sectionIndex = _sections.IndexOf(section);

            if (sectionIndex > 0)
            {
                _sections.RemoveAt(sectionIndex);
                _sections.Insert(sectionIndex - 1, section);

                isOk = true;
            }

            return isOk;
        }

        public Boolean MoveDown(MenuSectionInfo section)
        {
            Boolean isOk = false;

            Int32 sectionIndex = _sections.IndexOf(section);

            if (sectionIndex >= 0 && sectionIndex < (_sections.Count - 1))
            {
                _sections.RemoveAt(sectionIndex);
                _sections.Insert(sectionIndex + 1, section);

                isOk = true;
            }

            return isOk;
        }

        public Boolean AddSection(MenuSectionInfo section)
        {
            Boolean isOk = false;

            if (section.IsNew)
            {
                _sections.Add(section);

                isOk = true;
            }

            return isOk;
        }

        #endregion

        #region Helpers

        private static String SanitizeName(String value)
        {
            String result = null;

            if (!String.IsNullOrEmpty(value))
            {
                String[] nameParts = value.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (String part in nameParts)
                    result += "." + StringHelper.ReplaceNonAlphabetCharacters(part);
            }

            return String.IsNullOrEmpty(result) ? result : result.Substring(1);
        }

        #endregion
    }
}