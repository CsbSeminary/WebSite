using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

using Csbs.Web;
using System.Xml;
using System.Text;
using Csbs.Web.UI;
using System.IO;

using Csbs.Data;
using Csbs.Data.Menu;
using Csbs.Data.Pages;
using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs
{
    public class MenuManager
    {
        #region Constants

        private const String CurrentApplicationKey = "MenuManager.Current";

        #endregion

        #region Fields

        private List<MenuInfo> _menus = null;
        private Hashtable _indexedItemsByName = null;
        private Hashtable _indexedItemsByID = null;
        private Int32 _nextItemID = 0;

        #endregion

        #region Public properties

        public static MenuManager Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new MenuManager();

                return (MenuManager)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        public List<MenuInfo> Menus
        {
            get
            {
                if (_menus == null)
                {
                    _menus = ParseXml();

                    RefreshIndexes();
                }

                return _menus;
            }
            set
            {
                if (value == null)
                    _menus = value;
            }
        }

        #endregion

        #region Private properties

        private static String XmlPath
        {
            get { return Settings.Menu.XmlFile; }
        }

        private Hashtable IndexedItemsByName
        {
            get
            {
                if (_indexedItemsByName == null)
                    _indexedItemsByName = new Hashtable();

                return _indexedItemsByName;
            }
            set
            {
                if (value == null)
                    _indexedItemsByName = value;
            }
        }

        private Hashtable IndexedItemsByID
        {
            get
            {
                if (_indexedItemsByID == null)
                    _indexedItemsByID = new Hashtable();

                return _indexedItemsByID;
            }
            set
            {
                if (value == null)
                    _indexedItemsByID = value;
            }
        }

        #endregion

        #region Loading

        private static List<MenuInfo> ParseXml()
        {
            List<MenuInfo> data = null;
            XmlDocument doc = FileManager.GetXmlDocument(XmlPath);

            if (doc != null)
            {
                data = new List<MenuInfo>();

                XmlNode root = doc.GetElementsByTagName("root")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "menu":
                            data.Add(new MenuInfo(node));
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
            IndexedItemsByID.Clear();
            IndexedItemsByName.Clear();

            foreach (MenuInfo menu in Menus)
            {
                if (menu.IsNew)
                    continue;

                if (menu.ID > _nextItemID)
                    _nextItemID = menu.ID;

                IndexedItemsByID.Add(menu.ID, menu);
                IndexedItemsByName.Add(menu.Name, menu);

                foreach (MenuSectionInfo section in menu.Sections)
                {
                    if (section.IsNew)
                        continue;

                    if (section.ID > _nextItemID)
                        _nextItemID = section.ID;

                    IndexedItemsByID.Add(section.ID, section);

                    if (!section.IsHtml)
                    {
                        foreach (MenuItemInfo item in section.Items)
                        {
                            if (item.IsNew)
                                continue;

                            if (item.ID > _nextItemID)
                                _nextItemID = item.ID;

                            IndexedItemsByID.Add(item.ID, item);
                        }
                    }
                }
            }
        }

        #endregion

        #region Finding

        public MenuInfo FindMenuByName(String name)
        {
            Object result = FindItemByName(name);

            return result is MenuInfo ? result as MenuInfo : null;
        }

        public MenuInfo FindMenuByID(Int32 id)
        {
            Object result = FindItemByID(id);

            return result is MenuInfo ? result as MenuInfo : null;
        }

        public MenuSectionInfo FindSectionByID(Int32 id)
        {
            Object result = FindItemByID(id);

            return result is MenuSectionInfo ? result as MenuSectionInfo : null;
        }

        public MenuItemInfo FindMenuItemByID(Int32 id)
        {
            Object result = FindItemByID(id);

            return result is MenuItemInfo ? result as MenuItemInfo : null;
        }

        private Object FindItemByName(String name)
        {
            Object result = null;

            if (Menus.Count > 0 && !String.IsNullOrEmpty(name) && IndexedItemsByName.Contains(name))
                result = IndexedItemsByName[name];

            return result;
        }

        private Object FindItemByID(Int32 id)
        {
            Object result = null;

            if (Menus.Count > 0 && IndexedItemsByID.Contains(id))
                result = IndexedItemsByID[id];

            return result;
        }

        #endregion

        #region Data operations

        public void Save()
        {
            if (_menus == null)
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

                        foreach (MenuInfo menu in _menus)
                        {
                            menu.Write(writer);
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

        public Boolean MoveUp(Int32 itemID)
        {
            Boolean isOk = false;

            Object item = FindItemByID(itemID);

            if (item is MenuSectionInfo)
                isOk = (item as MenuSectionInfo).MoveUp();
            else if (item is MenuItemInfo)
                isOk = (item as MenuItemInfo).MoveUp();

            return isOk;
        }

        public Boolean MoveDown(Int32 itemID)
        {
            Boolean isOk = false;

            Object item = FindItemByID(itemID);

            if (item is MenuSectionInfo)
                isOk = (item as MenuSectionInfo).MoveDown();
            else if (item is MenuItemInfo)
                isOk = (item as MenuItemInfo).MoveDown();

            return isOk;
        }

        public Boolean Delete(Int32 itemID)
        {
            Boolean isOk = false;

            Object item = FindItemByID(itemID);

            if (item != null)
            {
                if (item is MenuInfo)
                    isOk = (item as MenuInfo).TryDelete();
                else if (item is MenuSectionInfo)
                    isOk = (item as MenuSectionInfo).TryDelete();
                else if (item is MenuItemInfo)
                    isOk = (item as MenuItemInfo).TryDelete();
            }

            return isOk;
        }

        public Boolean AddMenu(MenuInfo menu)
        {
            Boolean isOk = false;

            if (menu.IsNew && !String.IsNullOrEmpty(menu.Name))
            {
                menu.ID = ++_nextItemID;

                _menus.Add(menu);

                isOk = true;
            }

            return isOk;
        }

        public Boolean AddSection(Int32 menuID, MenuSectionInfo section)
        {
            Boolean isOk = false;

            if (section.IsNew)
            {
                MenuInfo menu = FindMenuByID(menuID);

                if (menu != null && !menu.IsNew)
                {
                    section.ID = ++_nextItemID;

                    isOk = menu.AddSection(section);
                }
            }

            return isOk;
        }

        public Boolean AddMenuItem(Int32 sectionID, MenuItemInfo menuItem)
        {
            Boolean isOk = false;

            if (menuItem.IsNew)
            {
                MenuSectionInfo section = FindSectionByID(sectionID);

                if (section != null && !section.IsNew)
                {
                    menuItem.ID = ++_nextItemID;

                    isOk = section.AddMenuItem(menuItem);
                }
            }

            return isOk;
        }

        #endregion

        #region Data binding

        public DataTable GetSelectorDataSource()
        {
            DataTable t = new DataTable("Menu");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("ParentID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Name", typeof(String)));
            t.Columns.Add(new DataColumn("FullName", typeof(String)));
            t.Columns.Add(new DataColumn("ItemType", typeof(String)));

            foreach (MenuInfo menu in Menus)
            {
                AddRow(t, menu.ID, null, menu.Name, menu.Name, "Menu");

                for (Int32 sectionIndex = 0; sectionIndex < menu.Sections.Count; sectionIndex++)
                {
                    MenuSectionInfo menuSection = menu.Sections[sectionIndex];

                    String sectionName = String.Format("Section {0} {1}"
                        , sectionIndex + 1
                        , String.IsNullOrEmpty(menuSection.Title)
                            ? String.Empty
                            : "(" + menuSection.Title + ")"
                    );

                    AddRow(t, menuSection.ID, menu.ID, sectionName, null, "MenuSection");

                    if (!menuSection.IsHtml)
                    {
                        foreach (MenuItemInfo menuItem in menuSection.Items)
                        {
                            AddRow(t, menuItem.ID, menuSection.ID, menuItem.Title, null, "MenuItem");
                        }
                    }
                }
            }

            return t;
        }

        public DataTable GetFinderDataSource()
        {
            DataTable t = new DataTable("Menu");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Name", typeof(String)));
            t.Columns.Add(new DataColumn("BulletImageUrl", typeof(String)));
            t.Columns.Add(new DataColumn("HorizontalSeparator", typeof(String)));
            t.Columns.Add(new DataColumn("HtmlContent", typeof(String)));

            foreach (MenuInfo menu in Menus)
            {
                String menuHtmlContent = null;

                foreach (MenuSectionInfo menuSection  in menu.Sections)
                {
                    String innerHtml = String.Empty;

                    if (!menuSection.IsHtml)
                    {
                        foreach (MenuItemInfo menuItem in menuSection.Items)
                        {
                            innerHtml += String.Format("<li>{0}</li>", menuItem.Title);
                        }

                        if (!String.IsNullOrEmpty(innerHtml))
                            innerHtml = String.Format("<ul>{0}</ul>", innerHtml);
                    }
                    else
                    {
                        innerHtml = menuSection.InnerHtml;
                    }

                    menuHtmlContent += 
                        String.Format(
                            "<li>{0}{2}{1}</li>"
                          , menuSection.Title
                          , innerHtml
                          , !String.IsNullOrEmpty(innerHtml)
                              ? "<br/>" 
                              : String.Empty
                    );
                }

                menuHtmlContent =
                    String.IsNullOrEmpty(menuHtmlContent)
                        ? "<strong>No items to display.</strong>"
                        : String.Format("<ol>{0}</ol>", menuHtmlContent);

                String bulletImageUrl = 
                    menu.Bullet == VerticalMenuBulletType.Sun
                        ? LinkUtils.ResolveClientUrl("~/Media/Images/menu_bullet_sun.gif")
                        : menu.Bullet == VerticalMenuBulletType.Triangle
                            ? LinkUtils.ResolveClientUrl("~/Media/Images/menu_bullet_triangle.gif")
                            : String.Empty;

                AddRow(t, menu.ID, menu.Name, bulletImageUrl, menu.HorizontalSeparator, menuHtmlContent);
            }

            return t;
        }

        public DataTable GetSectionsDataSource(Int32 menuID)
        {
            DataTable t = new DataTable("Sections");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Title", typeof(String)));
            t.Columns.Add(new DataColumn("IsHeaderVisible", typeof(Boolean)));
            t.Columns.Add(new DataColumn("IsHtmlContent", typeof(Boolean)));
            t.Columns.Add(new DataColumn("HtmlContent", typeof(String)));

            MenuInfo menu = FindMenuByID(menuID);

            if (menu != null)
            {
                foreach (MenuSectionInfo section in menu.Sections)
                {
                    String innerHtml = String.Empty;

                    if (!section.IsHtml)
                    {
                        foreach (MenuItemInfo menuItem in section.Items)
                        {
                            innerHtml += String.Format("<li>{0}</li>", menuItem.Title);
                        }

                        if (!String.IsNullOrEmpty(innerHtml))
                            innerHtml = String.Format("<ul>{0}</ul>", innerHtml);
                    }
                    else
                    {
                        innerHtml = section.InnerHtml;
                    }

                    AddRow(t, section.ID, section.Title, section.ShowHeader, section.IsHtml, innerHtml);
                }
            }

            return t;
        }

        public DataTable GetItemsDataSource(Int32 sectionID)
        {
            DataTable t = new DataTable("MenuItems");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Title", typeof(String)));
            t.Columns.Add(new DataColumn("Text", typeof(String)));
            t.Columns.Add(new DataColumn("Target", typeof(String)));
            t.Columns.Add(new DataColumn("AdminTitle", typeof(String)));
            t.Columns.Add(new DataColumn("AdminOnly", typeof(Boolean)));

            MenuSectionInfo section = FindSectionByID(sectionID);

            if (section != null && !section.IsHtml)
            {
                foreach (MenuItemInfo item in section.Items)
                {
                    AddRow(t, item.ID, item.Title, item.Text, item.Target, item.AdminTitle, item.AdminOnly);
                }
            }

            return t;
        }

        #endregion

        #region Rendering

        public void Render(HtmlTextWriter writer, IEnumerable menuItems, PageInfo page, BlogArticle article, MenuRenderingType direction)
        {
            StringBuilder sb = new StringBuilder();

            Render(sb, menuItems, page, article, direction);

            writer.Write(sb.ToString());
        }

        public void Render(StringBuilder sb, IEnumerable menuItems, PageInfo page, BlogArticle article, MenuRenderingType direction)
        {
            if (direction != MenuRenderingType.None)
            {
                sb.AppendFormat("<div class='page-navigation-menu {0}'>", direction == MenuRenderingType.Horizontal ? "horizontal" : "vertical");

                Boolean isFirst = true;

                foreach (Object menuItem in menuItems)
                {
                    MenuInfo menu = null;

                    if (menuItem is String)
                    {
                        String menuItemName = menuItem as String;

                        if (String.IsNullOrEmpty(menuItemName))
                            continue;

                        menu = FindMenuByName(menuItemName);
                    }
                    else if (menuItem is MenuInfo)
                    {
                        menu = menuItem as MenuInfo;
                    }
                    else
                        throw new Exception(String.Format("Unknown type {0}.", menuItem.GetType().Name));

                    if (menu == null)
                        continue;

                    if (!isFirst)
                        sb.Append("<div class='menu-separator'>&nbsp;</div>");

                    menu.Render(sb, page, article, direction);

                    isFirst = false;
                }

                sb.Append("</div>");
            }
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            Menus = null;
            IndexedItemsByName = null;
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