using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web;

using Csbs.Web;
using System.Xml;
using System.IO;
using System.Text;

using Csbs.Data.Pages;
using Csbs.Utilities;

using Telerik.Web.UI;

namespace Csbs
{
    public class PageManager
    {
        #region Constants

        private const String CurrentApplicationKey = "PageManager.Current";

        #endregion

        #region Fields

        private List<PageGroupInfo> _groups = null;
        private Hashtable _indexedByName = null;
        private Hashtable _indexedByPageID = null;
        private Int32 _nextID = 0;

        #endregion

        #region Public properties

        public static PageManager Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new PageManager();

                return (PageManager)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        public List<PageGroupInfo> Groups
        {
            get
            {
                if (_groups == null)
                {
                    _groups = ParseXml();

                    RefreshIndexes();
                }

                return _groups;
            }
            set
            {
                if (value == null)
                    _groups = value;
            }
        }

        #endregion

        #region Private properties

        private static String XmlPath
        {
            get { return Settings.Pages.InfoFile; }
        }

        private Hashtable IndexedByPageID
        {
            get
            {
                if (_indexedByPageID == null)
                    _indexedByPageID = new Hashtable();

                return _indexedByPageID;
            }
            set
            {
                if (value == null)
                    _indexedByPageID = value;
            }
        }

        private Hashtable IndexedByName
        {
            get
            {
                if (_indexedByName == null)
                    _indexedByName = new Hashtable();

                return _indexedByName;
            }
            set
            {
                if (value == null)
                    _indexedByName = value;
            }
        }

        #endregion

        #region Loading

        private static List<PageGroupInfo> ParseXml()
        {
            List<PageGroupInfo> data = null;
            XmlDocument doc = FileManager.GetXmlDocument(XmlPath);

            if (doc != null)
            {
                data = new List<PageGroupInfo>();

                XmlNode root = doc.GetElementsByTagName("root")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "group":
                            data.Add(new PageGroupInfo(node));
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                    }
                }
            }

            return data;
        }

        private void RefreshIndexes()
        {
            IndexedByName.Clear();
            IndexedByPageID.Clear();

            if (_groups != null)
            {
                foreach (PageGroupInfo group in _groups)
                {
                    if (group.IsNew)
                        continue;

                    IndexedByName.Add(group.Name.ToLower(), group);

                    foreach (PageSectionInfo section in group.Sections)
                    {
                        if (section.IsNew)
                            continue;

                        IndexedByName.Add(section.FullName.ToLower(), section);

                        foreach (PageInfo page in section.Pages)
                        {
                            if (page.IsNew)
                                continue;

                            if (_nextID < page.ID)
                                _nextID = page.ID;

                            IndexedByName.Add(page.FullName.ToLower(), page);
                            IndexedByPageID.Add(page.ID, page);
                        }
                    }
                }
            }
        }

        #endregion

        #region Data operations

        public void Save()
        {
            if (_groups == null)
                return;

            FileManager.CreateBackupFile(XmlPath);

            try
            {
                String physPath = null;
                FileManager.TryGetPhysicalPath(XmlPath, out physPath);

                using (FileStream fs = File.Open(physPath, FileMode.Create))
                {
                    using (XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;

                        writer.WriteStartDocument(false);
                        writer.WriteStartElement("root");

                        foreach (PageGroupInfo group in _groups)
                        {
                            group.Write(writer);
                        }

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.RestoreFileFromBackup(XmlPath);

                throw ex;
            }
        }

        public PageInfo CreateNewPage(String sectionName, String pageName)
        {
            PageInfo page = null;
            PageSectionInfo section = FindSection(sectionName);

            if (section != null)
            {
                page = section.CreatePage(++_nextID, pageName);
            }

            return page;
        }

        public Boolean DeleteGroup(String name)
        {
            PageGroupInfo group = FindGroup(name);

            return group != null && group.TryDelete();
        }

        public Boolean DeleteSection(String name)
        {
            PageSectionInfo section = FindSection(name);

            return section != null && section.TryDelete();
        }

        public Boolean DeletePage(Int32 id)
        {
            PageInfo page = FindPage(id);

            return page != null && page.TryDelete();
        }

        public Boolean RenamePage(String name, String newName)
        {
            Boolean isSaved = false;

            try
            {
                PageInfo page = FindPage(name);

                if (page != null && page.SetName(newName))
                {
                    Save();
                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean RenameGroup(String name, String newName)
        {
            Boolean isSaved = false;

            try
            {
                PageGroupInfo group = FindGroup(name);

                if (group != null && group.SetName(newName))
                {
                    Save();
                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean RenameSection(String name, String newName)
        {
            Boolean isSaved = false;

            try
            {
                PageSectionInfo section = FindSection(name);

                if (section != null && section.SetName(newName))
                {
                    Save();
                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean SetGroupMaster(String name, String masterName)
        {
            Boolean isSaved = false;

            try
            {
                PageGroupInfo group = FindGroup(name);

                if (group != null && group.SetMasterPage(masterName))
                {
                    Save();
                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean SetSectionTheme(String name, String themeName)
        {
            Boolean isSaved = false;

            try
            {
                PageSectionInfo section = FindSection(name);

                if (section != null && section.SetTheme(themeName))
                {
                    Save();
                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean CreateGroup(String name, String masterPage)
        {
            Boolean isSaved = false;

            try
            {
                PageGroupInfo group = new PageGroupInfo();

                if (group.SetName(name) && group.SetMasterPage(masterPage))
                {
                    Groups.Add(group);

                    Save();

                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean CreateSection(String groupName, String sectionName, String theme)
        {
            Boolean isSaved = false;

            try
            {
                PageGroupInfo group = FindGroup(groupName);

                if (group != null && group.CreateSection(sectionName, theme))
                {
                    Save();

                    isSaved = true;
                }
            }
            finally
            {
                Reset();
            }

            return isSaved;
        }

        public Boolean MovePage(Int32 pageID, String destinationName)
        {
            Boolean isOk = false;

            PageSectionInfo section = FindSection(destinationName);
            if (section != null)
                isOk = section.MovePage(pageID);

            return isOk;
        }

        #endregion

        #region Finding

        public PageGroupInfo FindGroup(String name)
        {
            Object obj = FindByName(name);

            return obj != null && obj is PageGroupInfo ? obj as PageGroupInfo : null;
        }

        public PageSectionInfo FindSection(String name)
        {
            Object obj = FindByName(name);

            return obj != null && obj is PageSectionInfo ? obj as PageSectionInfo : null;
        }

        public PageInfo FindPage(String name)
        {
            Object obj = FindByName(name);

            return obj != null && obj is PageInfo ? obj as PageInfo : null;
        }

        public PageInfo FindPage(Int32 id)
        {
            Object obj = FindByID(id);

            return obj != null && obj is PageInfo ? obj as PageInfo : null;
        }

        private Object FindByID(Int32 id)
        {
            Object result = null;

            if (Groups.Count > 0)
            {
                if (IndexedByPageID.Contains(id))
                    result = IndexedByPageID[id];
            }

            return result;
        }

        private Object FindByName(String name)
        {
            Object result = null;

            if (Groups.Count > 0 && !String.IsNullOrEmpty(name))
            {
                name = name.ToLower();

                if (IndexedByName.Contains(name))
                    result = IndexedByName[name];
            }

            return result;
        }

        #endregion

        #region Data binding

        public void AddEditorPageLinks(EditorLinkCollection links)
        {
            EditorLink pageLink = new EditorLink("Pages", null);
            
            foreach (PageGroupInfo group in Groups)
            {
                EditorLink groupLink = new EditorLink(group.Name, null);

                foreach (PageSectionInfo section in group.Sections)
                {
                    EditorLink sectionLink = new EditorLink(section.Name, null);

                    foreach (PageInfo page in section.Pages)
                    {
                        String href = LinkUtils.ResolveClientUrl(LinkUtils.GetPageUrl(page.FullName));

                        sectionLink.ChildLinks.Add(new EditorLink(page.Name, href));
                    }

                    if (sectionLink.ChildLinks.Count > 0)
                        groupLink.ChildLinks.Add(sectionLink);
                }

                if (groupLink.ChildLinks.Count > 0)
                    pageLink.ChildLinks.Add(groupLink);
            }

            links.Add(pageLink);
        }

        public void AddDataToSelectorDataSource(DataTable table)
        {
            foreach (PageGroupInfo group in Groups)
            {
                foreach (PageSectionInfo section in group.Sections)
                {
                    foreach (PageInfo page in section.Pages)
                    {
                        String url = LinkUtils.ResolveClientUrl(LinkUtils.GetPageUrl(page.FullName));
                        AddRow(table, url, url);
                    }
                }

            }
        }

        public DataTable GetFinderDataSource(String siteName)
        {
            DataTable t = new DataTable("Pages");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Name", typeof(String)));
            t.Columns.Add(new DataColumn("ParentID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Text", typeof(String)));
            t.Columns.Add(new DataColumn("PageTitle", typeof(String)));
            t.Columns.Add(new DataColumn("Url", typeof(String)));
            t.Columns.Add(new DataColumn("IsVisible", typeof(Boolean)));
            t.Columns.Add(new DataColumn("Type", typeof(String)));
            t.Columns.Add(new DataColumn("MasterPage", typeof(String)));
            t.Columns.Add(new DataColumn("ThemeName", typeof(String)));

            Int32 parentEntityID = -1;

            AddRow(t, parentEntityID, null, null, siteName == null ? "Root" : siteName, null, null, true, "Root", null, null);
            AddGroups(t, Groups, parentEntityID);

            return t;
        }

        private static void AddGroups(DataTable t, IEnumerable groups, Int32 parentID)
        {
            Int32 entityID = parentID;

            foreach (PageGroupInfo group in groups)
            {
                entityID--;

                AddRow(t, entityID, group.Name.ToLower(), parentID, group.Name, null, null, true, "Group", group.MasterPage, null);
                entityID = AddSections(t, group.Sections, entityID);
            }
        }

        private static Int32 AddSections(DataTable t, IEnumerable sections, Int32 parentID)
        {
            Int32 entityID = parentID;

            foreach (PageSectionInfo section in sections)
            {
                entityID--;

                AddRow(t, entityID, section.FullName.ToLower(), parentID, section.Name, null, null, true, "Section", section.MasterPage, section.DefaultTheme);

                foreach (PageInfo page in section.Pages)
                {
                    AddRow(t, page.ID, page.FullName, entityID, page.Name, page.Title, LinkUtils.ResolveClientUrl(LinkUtils.GetPageUrl(page.FullName)), !page.AdminOnly, "Page", page.MasterPage, page.Theme);
                }
            }

            return entityID;
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            Groups = null;
            IndexedByPageID = null;
            IndexedByName = null;
        }

        #endregion

        #region Helper methods

        private static void AddRow(DataTable t, params Object[] args)
        {
            DataRow row = t.NewRow();

            if (args != null)
            {
                for (Int32 i = 0; i < t.Columns.Count && i < args.Length; i++)
                    row[i] = args[i] == null ? DBNull.Value : args[i];
            }

            t.Rows.Add(row);
        }

        #endregion
    }
}