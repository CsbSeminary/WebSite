<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageFinder.ascx.cs" Inherits="Csbs.Web.UI.PageFinder" %>

<div id="content">

<div style="float:right;">
    <asp:LinkButton runat="server" ID="ResetCommand" Text="Reset Cache" />
</div>

<telerik:RadAjaxPanel runat="server" ID="RequestPanel" ClientEvents-OnResponseEnd="RequestPanel_OnEndAjaxResponse" EnableAJAX="true">

    <telerik:RadTreeView runat="server" 
        ID="SiteMapTreeView" 
        AllowNodeEditing="true"
        MultipleSelect="true"
        EnableDragAndDrop="true"   
        OnClientContextMenuItemClicking="SiteMapTreeView_OnContextMenuItemClicking"
        OnClientContextMenuShowing="SiteMapTreeView_OnContextMenuShowing"
        OnClientLoad="SiteMapTreeView_OnLoad"
        OnClientNodeEdited="SiteMapTreeView_OnNodeEdited"
        OnClientNodeDropping="SiteMapTreeView_OnNodeDropping"
    >
        <DataBindings>
            <telerik:RadTreeNodeBinding Expanded="true" />
        </DataBindings>
        <ContextMenus>
            <telerik:RadTreeViewContextMenu runat="server" ID="ContextMenu">
                <Items>
                    <telerik:RadMenuItem runat="server" Text="Add..." commandName="add" />
                    <telerik:RadMenuItem runat="server" Text="Edit..." commandName="edit" />
                    <telerik:RadMenuItem runat="server" Text="Rename..." commandName="rename" />
                    <telerik:RadMenuItem runat="server" Text="Delete..." commandName="delete" />
                    <telerik:RadMenuItem IsSeparator="true" />
                    <telerik:RadMenuItem runat="server" Text="Info..." commandName="info" />
                    <telerik:RadMenuItem runat="server" Text="View..." commandName="view" />
                    <telerik:RadMenuItem runat="server" Text="Master Page" commandName="masterPageSelector" />
                    <telerik:RadMenuItem runat="server" Text="Theme" commandName="themeSelector" />
                </Items>
                <CollapseAnimation Type="None" />
            </telerik:RadTreeViewContextMenu>
        </ContextMenus>
    </telerik:RadTreeView>

    <asp:HiddenField runat="server" ID="CommandsQuery" />
</telerik:RadAjaxPanel>

<telerik:RadToolTip runat="server" ID="NodeInfoToolTip" OnClientHide="NodeInfoToolTip_OnHide" ManualClose="true">
</telerik:RadToolTip>

<telerik:RadDock runat="server" ID="GroupCreatorWindow" Title="Group Creator" Width="300px" Height="150px" EnableRoundedCorners="true" Resizable="false" Closed="true" Skin="Default" Style="z-index: 10002;" Pinned="true">
    <Commands>
        <telerik:DockCloseCommand AutoPostBack="false" OnClientCommand="hideGroupCreator" />
    </Commands>
    <ContentTemplate>
        <table border="0" cellpadding="0" cellspacing="0" style="margin-top: 16px; width: 100%;">
            <tr>
                <td valign="top">Name:</td>
                <td>
                    <telerik:RadTextBox runat="server" ID="GroupCreatorWindow_GroupName" Width="174px" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="GroupCreatorWindow_GroupName" ErrorMessage="*" ValidationGroup="GroupCreatorWindow" />
                </td>
            </tr>
            <tr>
                <td valign="top" style="padding-right:15px;">Master Page:</td>
                <td><div id="groupCreatorWindow_MasterPlaceHolder"></div></td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td style="text-align: right; padding-top:8px;">
                    <asp:Button runat="server" Text="Save" OnClientClick="return saveGroupCreator();" ViewStateMode="Disabled" CausesValidation="true" ValidationGroup="GroupCreatorWindow" />
                    <asp:Button runat="server" Text="Cancel" OnClientClick="return hideGroupCreator(true);" ViewStateMode="Disabled" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</telerik:RadDock>

<telerik:RadDock runat="server" ID="SectionCreatorWindow" Title="Section Creator" Width="300px" EnableRoundedCorners="true" Resizable="false" Closed="true" Skin="Default" Style="z-index: 10002;" Pinned="true">
    <Commands>
        <telerik:DockCloseCommand AutoPostBack="false" OnClientCommand="hideSectionCreator" />
    </Commands>
    <ContentTemplate>
        <input type="hidden" id="SectionCreatorWindow_GroupName" />
        <table border="0" cellpadding="0" cellspacing="0" style="margin-top: 16px; width: 100%;">
            <tr>
                <td valign="top">Name:</td>
                <td>
                    <telerik:RadTextBox runat="server" ID="SectionCreatorWindow_SectionName" Width="174px" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="SectionCreatorWindow_SectionName" ErrorMessage="*" ValidationGroup="SectionCreatorWindow" />
                </td>
            </tr>
            <tr id="sectionCreatorWindow_ThemesField">
                <td valign="top" style="padding-right:15px;">Theme:</td>
                <td><div id="sectionCreatorWindow_ThemesPlaceHolder"></div></td>
            </tr>
            <tr>
                <td valign="top"></td>
                <td style="text-align: right; padding-top:8px;">
                    <asp:Button runat="server" Text="Save" OnClientClick="return saveSectionCreator();" ViewStateMode="Disabled" CausesValidation="true" ValidationGroup="SectionCreatorWindow" />
                    <asp:Button runat="server" Text="Cancel" OnClientClick="return hideSectionCreator(true);" ViewStateMode="Disabled" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</telerik:RadDock>

<telerik:RadCodeBlock runat="server">

<script type="text/javascript">

    function SiteMapTreeView_OnNodeDropping(sender, args) {
        var destinationNode = args.get_destNode();

        if (!destinationNode)
            return;

        var itemsIdToMove = "";
        var sourceNodes = args.get_sourceNodes();

        for (var i = 0; i < sourceNodes.length; i++) {
            itemsIdToMove += "||" + sourceNodes[i].get_attributes().getAttribute("itemID");
            sourceNodes[i].get_textElement().style.color = "#0f0";
        }

        var move_command = "move_" + args.get_dropPosition() + ":" + destinationNode.get_attributes().getAttribute("itemID") + "||" + destinationNode.get_attributes().getAttribute("itemType") + "||" + destinationNode.get_attributes().getAttribute("itemName") + itemsIdToMove;
        SiteMapTreeView_DoRequest(move_command);

        destinationNode.get_treeView()._clearSelectedNodes();
    }

    function RequestPanel_OnEndAjaxResponse() {
        var treeView = $find("<%= SiteMapTreeView.ClientID %>");
        var query = document.getElementById('<%= CommandsQuery.ClientID %>');

        if (query.value == null || query.value == "")
            return;

        var commands = query.value.split("'$'");

        for (var i = 0; i < commands.length; i++) {
            var cname = commands[i].substring(0, commands[i].indexOf(':'));
            var cvalue = commands[i].substring(commands[i].indexOf(':') + 1);
            var values = cvalue.split("||");

            switch (cname) {
                case "message":
                    alert(cvalue);
                    break;
                case "rename_ok":
                    break;
                case "delete_ok":
                    break;
                case "move_over_ok":
                    break;
                case "set_master_ok":
                    break;
                case "add_ok":
                    break;
                case "redirect":
                    document.location = values[0];
                    break;
                default:
                    alert("Unknown command: " + cname + ".");
                    break;
            }
        }

        treeView._clearSelectedNodes();
    }

    function SiteMapTreeView_GetItemsByID(childNodes, itemsId, outArray) {
        if (childNodes != null && childNodes.get_count() > 0 && itemsId != null && itemsId.length != 0) {
            for (var x = 0; x < childNodes.get_count(); x++) {
                var item = childNodes.getItem(x);

                for (var y = 0; y < itemsId.length; y++) {
                    if (itemsId[y] != null && item.get_attributes().getAttribute('itemID') == itemsId[y]) {
                        outArray[outArray.length] = item;
                        itemsId.splice(y);
                        y--;
                        break;
                    }
                }

                if (itemsId == null || itemsId.length == 0)
                    return;

                SiteMapTreeView_GetItemsByID(item.get_nodes(), itemsId, outArray);
            }
        }
    }

    function SiteMapTreeView_OnNodeEdited(sender, args) {
        var treeNode = args.get_node();

        if (treeNode._originalText != treeNode._text) {
            if (treeNode._text == "" || treeNode._text == null) {
                treeNode.set_text(treeNode._originalText);
                alert('The item name cannot be empty.');

                return;
            }

            treeNode.get_textElement().style.color = "#275e9e";

            SiteMapTreeView_DoRequest("rename:" + treeNode.get_attributes().getAttribute("itemID") + "||" + treeNode.get_attributes().getAttribute("itemType") + "||" + treeNode.get_attributes().getAttribute("itemName") + "||" + treeNode._text.replace(/\|/g, "!|"));

            treeNode.set_text(treeNode._originalText);
        }
    }

    function SiteMapTreeView_DoRequest(commandLine) {
        var requestPanel = $find('<%= RequestPanel.ClientID %>');
        requestPanel.ajaxRequest(commandLine);
    }

    function NodeInfoToolTip_OnHide(sender, args) {
        sender.set_targetControl(null);
        sender.set_text(null);
        sender.set_title(null);
    }

    function showItemInfo(treeNode) {
        var toolTip = $find('<%= NodeInfoToolTip.ClientID %>');

        if (toolTip.isVisible()) {
            toolTip.set_targetControl(null);
            toolTip.set_text(null);
            toolTip.set_title(null);
            toolTip.hide();
        }

        toolTip.set_targetControl(treeNode.get_textElement());
        toolTip.set_position(Telerik.Web.UI.ToolTipPosition.MiddleRight);
        toolTip.set_relativeTo(Telerik.Web.UI.ToolTipRelativeDisplay.Element);
        toolTip.set_text(getInfoTooltipText(treeNode.get_attributes().getAttribute('tInfo')));
        toolTip.set_title(treeNode.get_text());

        setTimeout("$find('<%= NodeInfoToolTip.ClientID %>').show();", 0);
    }

    function getInfoTooltipText(data) {
        var info = "";
        var dataParts = data.split("||");

        info += "<table style='margin:3px;'>";
        info += "<tr><th align='left'>Title:</th><td>" + (dataParts[0] == null ? "" : dataParts[0].replace(/!\|/g, "|")) + "</td></tr>";
        info += "<tr><th align='left'>Url:</th><td>" + (dataParts[1] == null ? "" : dataParts[1].replace(/!\|/g, "|")) + "</td></tr>";
        info += "<tr><th align='left'>Visible:</th><td>" + (dataParts[2] == null ? "" : dataParts[2].replace(/!\|/g, "|")) + "</td></tr>";
        info += "</table>";

        return info;
    }

    function SiteMapTreeView_OnContextMenuShowing(sender, args) {
        var menuItems = args.get_menu().get_items();
        var treeNode = args.get_node();
        var selectedNodes = treeNode.get_treeView().get_selectedNodes();
        var itemType = treeNode.get_attributes().getAttribute("itemType");

        if (!treeNode.get_selected()) {
            treeNode.get_treeView()._clearSelectedNodes();
            treeNode.set_selected(true);
        }

        var isMultipleSelect = false;
        var allowAdd = false;
        var allowEdit = false;
        var allowRename = false;
        var allowView = false;
        var allowDelete = false;
        var allowSetMasterPage = false;
        var allowSetMasterPageTheme = false;

        if (selectedNodes != null && selectedNodes.length > 1) {
            isMultipleSelect = true;
            allowDelete = true;
            for (var i = 0; i < selectedNodes.length; i++) {
                if (selectedNodes[i].get_level() == 0) {
                    allowDelete = false;
                    break;
                }
            }
        } else {
            var level = treeNode.get_level();

            allowAdd = level >= 0 && level <= 2;
            allowEdit = level == 3;
            allowRename = level > 0;
            allowView = allowEdit;
            allowDelete = level > 0;
            allowSetMasterPage = itemType && itemType == 'Group';
            allowSetMasterPageTheme = itemType && itemType == 'Section';
        }

        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            var cmd = menuItem.get_attributes().getAttribute("commandName");
            if (cmd) {
                switch (cmd) {
                    case "add":
                        menuItem.set_enabled(!isMultipleSelect && allowAdd);
                        break;
                    case "edit":
                        menuItem.set_enabled(!isMultipleSelect && allowEdit);
                        break;
                    case "rename":
                        menuItem.set_enabled(!isMultipleSelect && allowRename);
                        break;
                    case "delete":
                        menuItem.set_enabled(allowDelete);
                        break;
                    case "info":
                        menuItem.set_enabled(!isMultipleSelect && allowView);
                        menuItem.set_visible(isMultipleSelect || !itemType || itemType == 'Page' || itemType == 'Root');
                        break;
                    case "view":
                        menuItem.set_enabled(!isMultipleSelect && allowView);
                        menuItem.set_visible(isMultipleSelect || !itemType || itemType == 'Page' || itemType == 'Root');
                        break;
                    case "masterPageSelector":
                        menuItem.set_visible(!isMultipleSelect && allowSetMasterPage);
                        if (menuItem.get_visible()) {
                            bindMasterPageContextMenu(menuItem, treeNode.get_attributes().getAttribute("masterPage"));
                        }
                        break;
                    case "themeSelector":
                        menuItem.set_visible(!isMultipleSelect && allowSetMasterPageTheme);
                        if (menuItem.get_visible()) {
                            bindMasterPageThemeContextMenu(menuItem, treeNode.get_attributes().getAttribute("masterPage"), treeNode.get_attributes().getAttribute("themeName"));
                        }
                        break;
                }
            }
        }
    }

    function SiteMapTreeView_OnContextMenuItemClicking(sender, args) {
        var menuItem = args.get_menuItem();
        var cmd = menuItem.get_attributes().getAttribute("commandName");
        var treeNode = args.get_node();
        var treeView = treeNode.get_treeView();

        if (cmd) {
            menuItem.get_menu().hide();

            switch (cmd) {
                case "add":
                    var itemType = treeNode.get_attributes().getAttribute("itemType");

                    if (itemType == "Root") {
                        showGroupCreator();
                    } else if (itemType == "Group") {
                        showSectionCreator(treeNode.get_attributes().getAttribute("itemName"), treeNode.get_attributes().getAttribute("masterPage"));
                    } else if (itemType == "Section") {
                        SiteMapTreeView_DoRequest("addTo:||Page||||" + treeNode.get_attributes().getAttribute("itemName") + "||||");
                    }

                    treeView._clearSelectedNodes();
                    break;
                case "edit":
                    SiteMapTreeView_DoRequest("edit:" + treeNode.get_attributes().getAttribute("itemID") + "||" + treeNode.get_attributes().getAttribute("itemType") + "||" + treeNode.get_attributes().getAttribute("itemName"));
                    treeView._clearSelectedNodes();
                    break;
                case "rename":
                    treeNode.startEdit();
                    args.set_cancel(true);
                    treeView._clearSelectedNodes();
                    break;
                case "delete":
                    var result = deleteTreeViewItem(treeNode);
                    args.set_cancel(!result);
                    if (result)
                        treeView._clearSelectedNodes();
                    break;
                case "info":
                    showItemInfo(treeNode);
                    break;
                case "view":
                    var itemName = treeNode.get_attributes().getAttribute("itemName");
                    SiteMapTreeView_DoRequest("view:" + itemName);
                    treeView._clearSelectedNodes();
                case "setGroupMasterPage":
                    var masterName = menuItem.get_value();
                    if (treeNode.get_attributes().getAttribute("masterPage") != masterName) {
                        SiteMapTreeView_DoRequest("set_master:" + treeNode.get_attributes().getAttribute("itemType") + "||" + treeNode.get_attributes().getAttribute("itemName") + "||" + masterName.replace(/\|/g, "!|"));
                        treeView._clearSelectedNodes();
                        treeNode.get_textElement().style.color = "#00f";
                    }
                    break;
                case "setSectionTheme":
                    var themeName = menuItem.get_value();
                    if (treeNode.get_attributes().getAttribute("themeName") != themeName) {
                        SiteMapTreeView_DoRequest("set_theme:" + treeNode.get_attributes().getAttribute("itemType") + "||" + treeNode.get_attributes().getAttribute("itemName") + "||" + themeName.replace(/\|/g, "!|"));
                        treeView._clearSelectedNodes();
                        treeNode.get_textElement().style.color = "#00f";
                    }
                    break;
            }
        }
    }

    function deleteTreeViewItem(treeNode) {
        var result = false;
        var selectedNodes = treeNode.get_treeView().get_selectedNodes();

        if (selectedNodes != null && selectedNodes.length > 1) {
            var noEmptyItems = "";
            var excludeList = [];

            for (var i = 0; i < selectedNodes.length; i++) {
                var childNodes = selectedNodes[i].get_nodes();

                if (childNodes != null && childNodes.get_count() > 0) {
                    var childsToDelete = 0;

                    for (var x = 0; x < childNodes.get_count(); x++) {
                        for (var y = 0; y < selectedNodes.length; y++) {
                            if (childNodes.getItem(x) == selectedNodes[y]) {
                                childsToDelete++;
                                excludeList[excludeList.length] = y;
                                break;
                            }
                        }
                    }

                    if (childNodes.get_count() > childsToDelete)
                        noEmptyItems += "   - " + selectedNodes[i].get_text() + "\r\n";
                }
            }

            if (noEmptyItems == null || noEmptyItems == '') {
                var itemsInfoToDelete = '';
                var itemsToDelete = '';
                var itemsToDeletePrefix = '   ';
                
                for (var i = 0; i < selectedNodes.length; i++) {
                    itemsInfoToDelete += 
                        (itemsInfoToDelete == "" ? "" : "||") 
                      + selectedNodes[i].get_attributes().getAttribute("itemID") 
                      + "||" 
                      + selectedNodes[i].get_attributes().getAttribute("itemType")
                      + "||" 
                      + selectedNodes[i].get_attributes().getAttribute("itemName");

                    var isExclude = false;

                    for (var x = 0; x < excludeList.length; x++) {
                        if (excludeList[x] == i) {
                            isExclude = true;
                            break;
                        }
                    }

                    if (isExclude)
                        continue;

                    itemsToDelete += itemsToDeletePrefix + "- " + selectedNodes[i].get_text() + "\r\n";
                    itemsToDelete += buildDeleteMessage(selectedNodes[i].get_nodes(), itemsToDeletePrefix);
                }

                result = confirm("The following items will be deleted:\r\n" + itemsToDelete + "\r\nAre you sure you want to delete this items?");

                if (result) {
                    SiteMapTreeView_DoRequest("delete:" + itemsInfoToDelete);
                    markNodes(selectedNodes, "#f00", false);
                }
            } else {
                alert("The selected item(s) cannot be removed because the following items contains a child items:\r\n" + noEmptyItems);
            }
        } else {
            if (treeNode.get_childListElement() != null && treeNode.get_childListElement().children.length) {
                alert("'" + treeNode.get_text() + "' cannot be removed: item contains a child items.");
            } else {
                result = confirm("Are you sure you want to delete '" + treeNode.get_text() + "' item?");

                if (result) {
                    SiteMapTreeView_DoRequest("delete:" + treeNode.get_attributes().getAttribute("itemID") + "||" + treeNode.get_attributes().getAttribute("itemType") + "||" + treeNode.get_attributes().getAttribute("itemName"));
                    treeNode.get_textElement().style.color = "#f00";
                }
            }
        }

        return result;
    }

    function markNodes(nodes, colorValue, isMarkChilds) {
        for (var i = 0; i < nodes.length; i++) {
            nodes[i].get_textElement().style.color = colorValue;

            if (isMarkChilds)
                markNodes(nodes[i].get_nodes(), colorValue, isMarkChilds);
        }
    }

    function buildDeleteMessage(childNodes, itemsToDeletePrefix) {
        var itemsToDelete = "";

        itemsToDeletePrefix += "   ";

        if (childNodes != null && childNodes.get_count() > 0) {
            for (var x = 0; x < childNodes.get_count(); x++) {
                var item = childNodes.getItem(x);

                itemsToDelete += itemsToDeletePrefix + "- " + item.get_text() + "\r\n";
                itemsToDelete += buildDeleteMessage(item.get_nodes(), itemsToDeletePrefix);
            }
        }

        return itemsToDelete;
    }

    function SiteMapTreeView_OnLoad(sender, args) {

    }

    function bindMasterPageContextMenu(menuItem, masterPage) {
        menuItem.get_parent()._selectedItemIndex = null;

        var menu_items = menuItem.get_items();

        menu_items.clear();

        if (_pageFinder_mastersArray) {
            for (var i = 0; i < _pageFinder_mastersArray.length; i++) {
                var value = _pageFinder_mastersArray[i][0];
                var item = new Telerik.Web.UI.RadMenuItem();

                item.set_selectedImageUrl(_pageFinder_defaultSelectedImageSrc);
                item.set_text(value);
                item.get_attributes().setAttribute("commandName", "setGroupMasterPage");
                item.set_value(value);

                menu_items.add(item);

                item.set_selected(masterPage && value == masterPage);
            }
        }
    }

    function bindMasterPageThemeContextMenu(menuItem, masterPage, theme) {
        menuItem.get_parent()._selectedItemIndex = null;

        var menu_items = menuItem.get_items();

        menu_items.clear();

        if (_pageFinder_mastersArray) {
            for (var x = 0; x < _pageFinder_mastersArray.length; x++) {
                if (_pageFinder_mastersArray[x][1] && _pageFinder_mastersArray[x][0] == masterPage) {
                    for (var y = 0; y < _pageFinder_mastersArray[x][1].length; y++) {
                        var value = _pageFinder_mastersArray[x][1][y];
                        var item = new Telerik.Web.UI.RadMenuItem();

                        item.set_selectedImageUrl(_pageFinder_defaultSelectedImageSrc);
                        item.set_text(value);
                        item.get_attributes().setAttribute("commandName", "setSectionTheme");
                        item.set_value(value);

                        menu_items.add(item);

                        item.set_selected(theme && theme == value);
                    }
                }
            }
        }

        menuItem.set_enabled(menu_items.get_count() > 0);
    }

    function showGroupCreator() {
        var dock = $find('<%= GroupCreatorWindow.ClientID %>');
        var txtGroupName = $find('<%= GroupCreatorWindow_GroupName.ClientID %>');
        var mDiv = document.getElementById('groupCreatorWindow_MasterPlaceHolder');

        if (mDiv) {
            var mInnerHtml = '';

            if (_pageFinder_mastersArray) {
                for (var i = 0; i < _pageFinder_mastersArray.length; i++) {
                    mInnerHtml += "<input type='radio' value='" + _pageFinder_mastersArray[i][0] + "' name='groupCreatorWindow_MasterSelector' id='groupCreatorWindow_MasterSelector_" + i + "'";
                    if (i == 0) { mInnerHtml += "checked='true'"; }
                    mInnerHtml += " />"
                    mInnerHtml += "<label for='groupCreatorWindow_MasterSelector_" + i + "'>" + _pageFinder_mastersArray[i][0] + "</label>";
                    mInnerHtml += "<br/>";
                }
            }

            mDiv.innerHTML = mInnerHtml;
        }

        txtGroupName.set_value('');

        dock.center();
        dock.showModal();
        dock.set_closed(false);

        return false;
    }

    function hideGroupCreator(close) {
        var dock = $find('<%= GroupCreatorWindow.ClientID %>');

        if (close && close == true)
            dock.set_closed(true);

        if (dock.get_closed())
            dock.hideModal();

        return false;
    }

    function saveGroupCreator() {
        if (Page_ClientValidate("GroupCreatorWindow")) {
            var masterName = '';
            var groupName = $find('<%= GroupCreatorWindow_GroupName.ClientID %>').get_value();

            for (var i = 0; true; i++) {
                var rb = document.getElementById("groupCreatorWindow_MasterSelector_" + i);

                if (!rb)
                    break;

                if (rb.checked){
                    masterName = rb.value;
                    break;
                }
            }

            hideGroupCreator(true);
            SiteMapTreeView_DoRequest("addTo:||Group||||" + groupName.replace(/\|/g, "!|") + "||" + masterName + "||");
        }

        return false;
    }

    function showSectionCreator(groupName, masterPage) {
        var dock = $find('<%= SectionCreatorWindow.ClientID %>');
        var txtSectionName = $find('<%= SectionCreatorWindow_SectionName.ClientID %>');
        var hdGroupName = document.getElementById('SectionCreatorWindow_GroupName');
        var tDiv = document.getElementById('sectionCreatorWindow_ThemesPlaceHolder');
        var tField = document.getElementById('sectionCreatorWindow_ThemesField');

        txtSectionName.set_value('');
        hdGroupName.value = groupName;

        if (tDiv) {
            var isThemesVisible = false;
            var mInnerHtml = '';

            if (_pageFinder_mastersArray) {
                for (var x = 0; x < _pageFinder_mastersArray.length; x++) {
                    if (_pageFinder_mastersArray[x][0] == masterPage) {
                        if (_pageFinder_mastersArray[x][1] != null && _pageFinder_mastersArray[x][1].length > 0) {
                            for (var y = 0; y < _pageFinder_mastersArray[x][1].length; y++) {
                                var radioID = "sectionCreatorWindow_ThemeSelector_" + y;

                                mInnerHtml += "<input type='radio' value='" + _pageFinder_mastersArray[x][1][y] + "' name='sectionCreatorWindow_ThemeSelector' id='" + radioID + "'";
                                if (y == 0) { mInnerHtml += "checked='true'"; }
                                mInnerHtml += " />"
                                mInnerHtml += "<label for='" + radioID + "'>" + _pageFinder_mastersArray[x][1][y] + "</label>";
                                mInnerHtml += "<br/>";
                            }

                            isThemesVisible = true;
                        }
                        break;
                    }
                }
            }

            tDiv.innerHTML = mInnerHtml;

            if (isThemesVisible == true)
                tField.style.display = 'table-row';
            else
                tField.style.display = 'none';
        }

        dock.center();
        dock.showModal();
        dock.set_closed(false);

        return false;
    }

    function hideSectionCreator(close) {
        var dock = $find('<%= SectionCreatorWindow.ClientID %>');

        if (close && close == true)
            dock.set_closed(true);

        if (dock.get_closed())
            dock.hideModal();

        return false;
    }

    function saveSectionCreator() {
        if (Page_ClientValidate("SectionCreatorWindow")) {
            var themeName = '';
            var hdGroupName = document.getElementById('SectionCreatorWindow_GroupName');
            var sectionName = $find('<%= SectionCreatorWindow_SectionName.ClientID %>').get_value();

            for (var i = 0; true; i++) {
                var rb = document.getElementById("sectionCreatorWindow_ThemeSelector_" + i);

                if (!rb)
                    break;

                if (rb.checked) {
                    themeName = rb.value;
                    break;
                }
            }

            hideSectionCreator(true);
            SiteMapTreeView_DoRequest("addTo:||Section||||" + hdGroupName.value + "||" + sectionName.replace(/\|/g, "!|") + "||" + themeName);
        }

        return false;
    }

</script>

<script type="text/javascript">

    Telerik.Web.UI.RadDock.prototype.showModal = function () {
        this.set_closed(false);
        if (!this._modalExtender) {
            this._modalExtender = new Telerik.Web.UI.ModalExtender(this.get_element());
        }
        this._modalExtender.show();
        this.repaint();
        this.updateClientState();

    };

    Telerik.Web.UI.RadDock.prototype.hideModal = function () {
        if (this._modalExtender) {
            this._modalExtender.hide();
        }
    };

    Telerik.Web.UI.RadDock.prototype.center = function () {
        if (window.screen) {
            var ah = screen.availHeight - (screen.availHeight / 4);
            var aw = screen.availWidth - 10;

            var ih = this.get_height() == null || this.get_height() == '' ? 200 : parseInt(this.get_height());
            var iw = this.get_width() == null || this.get_width() == '' ? 300 : parseInt(this.get_width());

            var xc = (aw - iw) / 2;
            var yc = (ah - ih) / 2;

            this.set_left(xc);
            this.set_top(yc);
        }
    };

    Telerik.Web.UI.RadDock.prototype.dispose = function () {
        if (this._modalExtender) {
            this._modalExtender.dispose();
            this._modalExtender = null;
        }
        Telerik.Web.UI.RadDock.callBaseMethod(this, 'dispose');
    };

</script>

</telerik:RadCodeBlock>
</div>