using System;
using System.Data;
using System.Web.UI;

using Csbs.Web.UI;
using System.Collections.Specialized;

using Telerik.Web.UI;

using Csbs.Data.Pages;
using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public partial class PageFinder : UserControl
    {
        #region Constants

        public const String ActionName = "find-page";

        private const String JsScriptViewStateKey = "JsScript";

        private const String DefaultSelectedImageSrc = "~/Media/Icons/symbol-check.gif";

        #endregion

        #region Properties

        private String JsScript
        {
            get
            {
                if (ViewState[JsScriptViewStateKey] == null)
                    ViewState[JsScriptViewStateKey] = GetJsScript();

                return (String)ViewState[JsScriptViewStateKey];
            }
        }

        #endregion

        #region Fields

        private DataTable _dataTable = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ResetCommand.Click += ResetCommand_Click;

            SiteMapTreeView.DataBinding += SiteMapTreeView_DataBinding;
            SiteMapTreeView.NodeDataBound += SiteMapTreeView_NodeDataBound;

            RequestPanel.AjaxRequest += RequestPanel_AjaxRequest;
        }

        #endregion

        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
                SiteMapTreeView.DataBind();
        }

        #endregion

        #region Event handlers

        private void ResetCommand_Click(Object sender, EventArgs e)
        {
            PageManager.Current.Reset();

            SiteMapTreeView.DataBind();
        }

        private void SiteMapTreeView_NodeDataBound(Object sender, RadTreeNodeEventArgs e)
        {
            RadTreeNode node = e.Node;
            DataRowView data = (DataRowView)node.DataItem;

            if (node.Level == 0)
                node.ImageUrl = "~/Media/Icons/world-wide-web.gif";
            else if (node.Level == 1 || node.Level == 2)
                node.ImageUrl = "~/Media/Icons/folder.gif";
            else
                node.ImageUrl = ((Boolean)data["IsVisible"])
                    ? "~/Media/Icons/web-page.gif"
                    : "~/Media/Icons/web-page-invisible.gif";

            node.Attributes.Add("tInfo", String.Format("{0}||{1}||{2}", ToAttributeValue(data["PageTitle"]), ToAttributeValue(data["Url"]), ToAttributeValue(data["IsVisible"])));
            node.Attributes.Add("itemName", ToAttributeValue(data["Name"]));
            node.Attributes.Add("itemID", ToAttributeValue(data["ID"]));
            node.Attributes.Add("itemType", ToAttributeValue(data["Type"]));

            if (data["MasterPage"] != DBNull.Value)
                node.Attributes.Add("masterPage", ToAttributeValue(data["MasterPage"]));

            if (data["ThemeName"] != DBNull.Value)
                node.Attributes.Add("themeName", ToAttributeValue(data["ThemeName"]));
        }

        private void SiteMapTreeView_DataBinding(Object sender, EventArgs e)
        {
            BindTreeView();
        }

        private void RequestPanel_AjaxRequest(Object sender, AjaxRequestEventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                CommandsQuery.Value = "message:The user is not authenticated.";
                return;
            }

            try
            {
                Boolean isCommandHandled = false;

                if (!String.IsNullOrEmpty(e.Argument) && e.Argument.IndexOf(":") > 0)
                {
                    String command = e.Argument.Substring(0, e.Argument.IndexOf(":"));
                    String[] values = GetValues(e.Argument);

                    switch (command)
                    {
                        case "view":
                            isCommandHandled = true;
                            CommandsQuery.Value = "redirect:" + Page.ResolveUrl(LinkUtils.GetPageUrl(values[0]));
                            break;
                        case "edit":
                            isCommandHandled = true;
                            CommandsQuery.Value = EditItem(values);
                            break;
                        case "rename":
                            isCommandHandled = true;
                            CommandsQuery.Value = RenameItem(values);
                            break;
                        case "set_master":
                            isCommandHandled = true;
                            CommandsQuery.Value = SetMaster(values);
                            break;
                        case "set_theme":
                            isCommandHandled = true;
                            CommandsQuery.Value = SetTheme(values);
                            break;
                        case "addTo":
                            isCommandHandled = true;
                            CommandsQuery.Value = AddNewItem(values);
                            break;
                        case "delete":
                            isCommandHandled = true;
                            CommandsQuery.Value = DeleteItems(values);
                            break;
                        case "move_over":
                            isCommandHandled = true;
                            CommandsQuery.Value = MoveItems(values);
                            break;
                    }
                }

                if (!isCommandHandled)
                    CommandsQuery.Value = "message:Command not handled.";
                else
                    SiteMapTreeView.DataBind();
            }
            catch (Exception ex)
            {
                CommandsQuery.Value = "message:Exception: " + ex.Message + "\r\nStack trace:\r\n" + ex.StackTrace;
            }
        }

        #endregion

        #region Data binding

        private void BindTreeView()
        {
            SiteMapTreeView.DataTextField = "Text";
            SiteMapTreeView.DataFieldID = "ID";
            SiteMapTreeView.DataFieldParentID = "ParentID";

            if (_dataTable == null)
                _dataTable = PageManager.Current.GetFinderDataSource(Request.Url.Host);

            SiteMapTreeView.DataSource = _dataTable;
        }

        #endregion

        #region Command handlers

        private static String MoveItems(String[] values)
        {
            String command = null;
            Int32 destinationID = StringHelper.ToInt32(values[0]);
            String destinationType = FromAttributeValue(values[1]);
            String destinationName = FromAttributeValue(values[2]);

            if (String.Compare(destinationType, "section", true) != 0)
                command = "message:Operation is not implemented.";

            if (String.IsNullOrEmpty(destinationName))
                command = "message:Destination item is not defined";

            if (command == null)
            {
                try
                {
                    Int32 movedItemsCount = 0;

                    for (Int32 i = 3; i < values.Length; i++)
                    {
                        Int32? pageID = StringHelper.ToInt32Nullable(values[i]);

                        if (pageID.HasValue)
                        {
                            if (!PageManager.Current.MovePage(pageID.Value, destinationName))
                            {
                                movedItemsCount = -1;
                                command = "message:Item(s) cannot be moved.";
                                break;
                            }

                            movedItemsCount++;
                        }
                    }

                    if (movedItemsCount > 0)
                    {
                        PageManager.Current.Save();

                        command = String.Format("move_over_ok:0'$'message:{0} item(s) have been successfully moved.", movedItemsCount);
                    }
                }
                finally
                {
                    PageManager.Current.Reset();
                }
            }

            return command;
        }

        private static String EditItem(String[] values)
        {
            String command = null;

            Int32? id = StringHelper.ToInt32Nullable(values[0]);
            String type = FromAttributeValue(values[1]);
            String name = FromAttributeValue(values[2]);

            if (id.HasValue && String.Compare(type, "page", true) == 0)
            {
                command = "redirect:" + LinkUtils.ResolveClientUrl(LinkUtils.GetAdminPageUrl(PageEditor.ActionName, id));
            }
            else
            {
                command = "message:Operation is not implemented.";
            }

            return command;
        }

        private static String DeleteItems(String[] values)
        {
            String command = null;

            if (values.Length > 0 && values.Length % 3 == 0)
            {
                try
                {
                    Boolean isOk = true;

                    for (Int32 i = 0; i < values.Length; i += 3)
                    {
                        Int32? id = StringHelper.ToInt32Nullable(values[i]);
                        String type = FromAttributeValue(values[i + 1]);
                        String name = FromAttributeValue(values[i + 2]);

                        Boolean isDeleted = false;

                        if (String.Compare(type, "group", true) == 0)
                            isDeleted = PageManager.Current.DeleteGroup(name);
                        else if (String.Compare(type, "section", true) == 0)
                            isDeleted = PageManager.Current.DeleteSection(name);
                        else if (String.Compare(type, "page", true) == 0)
                            isDeleted = PageManager.Current.DeletePage(id.Value);
                        else
                            command = "message:Operation is not implemented.";

                        if (!isDeleted)
                        {
                            isOk = false;

                            if (command == null)
                                command = "message:The item(s) has not been removed.";

                            break;
                        }
                    }

                    if (isOk && command == null)
                    {
                        PageManager.Current.Save();

                        command = "delete_ok:0'$'message:The item(s) is successfully deleted.";
                    }
                }
                finally
                {
                    PageManager.Current.Reset();
                }
            }
            else
            {
                command = "message:Nothing to delete.";
            }

            return command;
        }

        private static String AddNewItem(String[] values)
        {
            String command = null;

            Int32? id = StringHelper.ToInt32Nullable(values[0]);
            String type = FromAttributeValue(values[1]);
            String name = FromAttributeValue(values[2]);
            String param1 = FromAttributeValue(values[3]);
            String param2 = FromAttributeValue(values[4]);
            String param3 = FromAttributeValue(values[5]);

            if (String.IsNullOrEmpty(type))
                command = "message:Type is undefined.";

            if (command == null)
            {
                if (String.Compare(type, "group", true) == 0)
                {
                    if (String.IsNullOrEmpty(param1))
                        command = "message:Group name is not specified.";

                    if (String.IsNullOrEmpty(param2))
                        command = "message:Master page is not specified.";

                    if (command == null)
                    {
                        if (PageManager.Current.CreateGroup(param1, param2))
                            command = "add_ok:" + name + "'$'message:Changes successfully saved.";
                        else
                            command = String.Format("message:Group named '{0}' already created.", param1, type);
                    }
                }
                else if (String.Compare(type, "section", true) == 0)
                {
                    if (String.IsNullOrEmpty(param1))
                        command = "message:Group name is undefined.";

                    if (String.IsNullOrEmpty(param2))
                        command = "message:Section name is not specified.";

                    if (command == null)
                    {
                        if (PageManager.Current.CreateSection(param1, param2, param3))
                            command = "add_ok:" + name + "'$'message:Changes successfully saved.";
                        else
                            command = String.Format("message:Section named '{0}' already created.", param2, type);
                    }
                }
                else if (String.Compare(type, "page", true) == 0)
                {
                    if (String.IsNullOrEmpty(param1))
                        command = "message:Section name is undefined.";

                    if (command == null)
                    {
                        PageSectionInfo section = PageManager.Current.FindSection(param1);

                        if (section != null)
                            command = "redirect:" + LinkUtils.ResolveClientUrl(LinkUtils.GetAdminPageUrl(LinkUtils.ActionNameQueryStringKey, PageCreator.ActionName, PageCreator.SectionNameQueryStringKey, section.FullName));
                        else
                            command = String.Format("message:Section named '{0}' does not exists.", param1);
                    }
                }
                else
                    command = "message:Operation is not implemented.";
            }

            return command;
        }

        private static String SetTheme(String[] values)
        {
            String command = null;

            String type = FromAttributeValue(values[0]);
            String name = FromAttributeValue(values[1]);
            String themeName = FromAttributeValue(values[2]);

            if (String.IsNullOrEmpty(type))
                command = "message:Type is undefined.";

            if (String.IsNullOrEmpty(name) || String.Compare(type, "Root", true) == 0)
                command = "message:You cannot operate with the Root item.";

            if (String.IsNullOrEmpty(themeName))
                command = "message:Theme name is null.";

            if (String.Compare(type, "section", true) != 0)
                command = "message:Operation is not implemented.";

            if (command == null)
            {
                if (PageManager.Current.SetSectionTheme(name, themeName))
                    command = "set_master_ok:" + name + "'$'message:Changes successfully saved.";
                else
                    command = String.Format("message:Theme named '{0}' is not found.", themeName, type);
            }

            return command;
        }

        private static String SetMaster(String[] values)
        {
            String command = null;

            String type = FromAttributeValue(values[0]);
            String name = FromAttributeValue(values[1]);
            String masterName = FromAttributeValue(values[2]);

            if (String.IsNullOrEmpty(type))
                command = "message:Type is undefined.";

            if (String.IsNullOrEmpty(name) || String.Compare(type, "Root", true) == 0)
                command = "message:You cannot operate with the Root item.";

            if (String.IsNullOrEmpty(masterName))
                command = "message:Master name is null.";

            if (String.Compare(type, "group", true) != 0)
                command = "message:Operation is not implemented.";

            if (command == null)
            {
                if (PageManager.Current.SetGroupMaster(name, masterName))
                    command = "set_master_ok:" + name + "'$'message:Changes successfully saved.";
                else
                    command = String.Format("message:Master page named '{0}' is not found.", masterName, type);
            }

            return command;
        }

        private static String RenameItem(String[] values)
        {
            String command = null;

            Int32? id = StringHelper.ToInt32Nullable(values[0]);
            String type = FromAttributeValue(values[1]);
            String name = FromAttributeValue(values[2]);
            String newName = FromAttributeValue(values[3]);

            if (String.IsNullOrEmpty(type))
                command = "message:Type is undefined.";

            if (!id.HasValue || id == -1 || String.IsNullOrEmpty(name) || String.Compare(type, "Root", true) == 0)
                command = "message:You cannot operate with the Root item.";

            if (String.IsNullOrEmpty(newName))
                command = "message:Name cannot be empty.";

            if (command == null)
            {
                Boolean isSaved = false;

                if (String.Compare(type, "page", true) == 0)
                    isSaved = PageManager.Current.RenamePage(name, newName);
                else if (String.Compare(type, "section", true) == 0)
                    isSaved = PageManager.Current.RenameSection(name, newName);
                else if (String.Compare(type, "group", true) == 0)
                    isSaved = PageManager.Current.RenameGroup(name, newName);
                else
                    command = "message:Operation is not implemented.";

                if (command == null)
                {
                    if (isSaved)
                        command = "rename_ok:" + id + "||" + name + String.Format("'$'message:The {0} is successfully renamed.", type.ToLower());
                    else
                        command = String.Format("message:{1} named '{0}' already exists.", newName, type);
                }
            }

            return command;
        }

        #endregion

        #region PreRender

        protected override void OnPreRender(EventArgs e)
        {
            if (!String.IsNullOrEmpty(JsScript))
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "masters_array", JsScript, true);
            }

            base.OnPreRender(e);
        }

        #endregion

        #region Helper methods

        private String GetJsScript()
        {
            String mastersArray = null;

            StringCollection masters = FileManager.GetAvailableMasterPages();

            foreach (String masterName in masters)
            {
                String arrayItem = null;

                StringCollection themes = FileManager.GetAvailableThemes(masterName);

                foreach (String theme in themes)
                {
                    arrayItem += String.Format(",'{0}'", theme);
                }

                mastersArray += String.Format(",['{0}', {1}]", masterName, arrayItem == null ? "null" : "[" + arrayItem.Substring(1) + "]");
            }

            String jsScript = String.Format("\r\nvar _pageFinder_mastersArray = {0};\r\n", mastersArray == null ? "null" : "[" + mastersArray.Substring(1) + "]");
            jsScript += String.Format("\r\nvar _pageFinder_defaultSelectedImageSrc = '{0}';\r\n", ResolveClientUrl(DefaultSelectedImageSrc));

            return jsScript;
        }

        private static String FromAttributeValue(String value)
        {
            return value == null ? null : value.Replace("!|", "|").Replace("&amp;", "&");
        }

        private static String[] GetValues(String command)
        {
            return command.Substring(command.IndexOf(":") + 1).Split(new String[] { "||" }, StringSplitOptions.None);
        }

        private static String ToAttributeValue(Object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            if (value is Boolean)
                return (Boolean)value ? "Yes" : "No";

            return value.ToString().Replace("|", "!|").Replace("&", "&amp;");
        }

        #endregion
    }
}