using System;
using System.Collections.Generic;
using System.Xml;

using Csbs.Utilities;

namespace Csbs.Data.Pages
{
    public class PageGroupInfo
    {
        #region Private fields

        private String _name = null;
        private String _masterPage = null;
        private Boolean _isNew = false;
        private Boolean _isDeleted = false;
        private List<PageSectionInfo> _sections = new List<PageSectionInfo>();

        #endregion

        #region Public properties

        public String Name
        {
            get { return _name; }
        }

        public String MasterPage
        {
            get { return _masterPage; }
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        public Boolean IsDeleted
        {
            get { return _isDeleted; }
        }

        public List<PageSectionInfo> Sections
        {
            get { return _sections; }
        }

        #endregion

        #region Computed properties

        public String MasterPageFilePath
        {
            get { return LinkUtils.GetMasterPageFilePath(MasterPage); }
        }

        public Boolean AllowDelete
        {
            get { return _sections.Count <= 0; }
        }

        #endregion

        #region Constructor

        public PageGroupInfo()
        {
            _isNew = true;
        }

        public PageGroupInfo(XmlNode xmlNode)
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
                    case "sections":
                        ReadSections(childNode);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
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
                        _sections.Add(new PageSectionInfo(node, this));
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
                    case "masterPage":
                        _masterPage = attr.Value;
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

            writer.WriteStartElement("group");
            writer.WriteAttributeString("name", _name);
            writer.WriteAttributeString("masterPage", _masterPage);

            writer.WriteStartElement("sections");

            foreach (PageSectionInfo section in _sections)
            {
                section.Write(writer);
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

                PageGroupInfo existsGroup = PageManager.Current.FindGroup(name);

                if (existsGroup == null || !_isNew && _name == existsGroup.Name)
                {
                    _name = name;

                    isOk = true;
                }
            }

            return isOk;
        }

        public Boolean SetMasterPage(String value)
        {
            Boolean isOk = false;

            if (FileManager.IsMasterPageExists(value))
            {
                _masterPage = value;

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

        public Boolean CreateSection(String name, String theme)
        {
            Boolean isCreated = false;

            if (!IsNew)
            {
                PageSectionInfo section = new PageSectionInfo(this);

                if (section.SetName(name) && (String.IsNullOrEmpty(theme) || section.SetTheme(theme)))
                {
                    _sections.Add(section);

                    isCreated = true;
                }
            }

            return isCreated;
        }

        #endregion
    }
}