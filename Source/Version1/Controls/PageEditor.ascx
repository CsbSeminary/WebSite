<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageEditor.ascx.cs" Inherits="Csbs.Web.UI.PageEditor" %>

<div class="error"><asp:Literal runat="server" ID="ErrorMessage" /></div>

<div class="commands top">
    <asp:Button runat="server" ID="SaveButton" Text="Save Page" CausesValidation="true" />
    <asp:Button runat="server" ID="CancelButton" Text="Cancel" CausesValidation="false" />
</div>

<asp:HiddenField runat="server" ID="MenuItemsCache" />

<telerik:RadTabStrip runat="server" ID="TabStrip" SelectedIndex="0" MultiPageID="mp" OnClientTabSelected="tabStrip_onClientTabSelected">
    <Tabs>
        <telerik:RadTab Text="Design" Value="tabDesign" />
        <telerik:RadTab Text="Header" Value="tabHeader" />
        <telerik:RadTab Text="Settings" Value="tabSettings" />
        <telerik:RadTab Text="Content" Value="tabContent" />
        <telerik:RadTab Text="Previewer" Value="tabPreviewer" />
    </Tabs>
</telerik:RadTabStrip>

<telerik:RadMultiPage ID="mp" runat="server" SelectedIndex="0">
        
    <telerik:RadPageView runat="server" ID="page1" CssClass="editor">
        <div class="field-header">Page Design</div>
        <div class="field">
            <csbs:Label runat="server" Text="Page Name" />
            <telerik:RadTextBox runat="server" ID="PageName" Width="200px"  />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PageName" ErrorMessage="*" ValidationGroup="PageEditor" />
        </div>
        <div class="field">
            <csbs:Label runat="server" Text="Page Title" />
            <telerik:RadTextBox runat="server" ID="PageTitle" Width="200px"  />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="PageTitle" ErrorMessage="*" ValidationGroup="PageEditor" />
        </div>
        <div class="field">
            <csbs:Label runat="server" Text="Master Page" />
            <csbs:MasterPageSelector runat="server" ID="MasterPage" Enabled="false" OnClientSelectedIndexChanged="masterPage_selectedIndexChanged" />
        </div>
        <div class="field">
            <csbs:Label runat="server" Text="Theme" />
            <csbs:MasterPageThemeSelector runat="server" ID="MasterTheme" MasterPageControl="MasterPage" OnClientSelectedIndexChanged="masterTheme_selectedIndexChanged" />
        </div>
    </telerik:RadPageView>

    <telerik:RadPageView runat="server" ID="page2" CssClass="editor">
        <div class="field-header">Page Header</div>
        <div class="field">
            <csbs:Label runat="server" Text="Layout" />
            <csbs:HeaderLayoutSelector runat="server" ID="HeaderLayout" OnClientSelectedIndexChanged="headerLayout_selectedIndexChanged" />
        </div>
        <div id="headerLayoutTextHolder" style="display:none;">
            <div class="field">
                <csbs:Label runat="server" Text="Position" />
                <csbs:HeaderPositionSelector runat="server" ID="HeaderPosition" OnClientSelectedIndexChanged="headerPosition_selectedIndexChanged" />                       
            </div>
            <div class="field">
                <csbs:Label runat="server" Text="Title" />
                <telerik:RadTextBox runat="server" ID="HeaderTitle" Width="200px" ClientEvents-OnValueChanged="headerTitle_onValueChanged" />
                <asp:RequiredFieldValidator runat="server" ID="HeaderTitleValidator" ControlToValidate="HeaderTitle" ErrorMessage="*" ValidationGroup="PageEditor" />                        
            </div>
            <div class="field">
                <csbs:Label runat="server" Text="Link" />
                <asp:CheckBox runat="server" ID="ShowHeaderLink" Text="Show link in header" />
            </div>
            <div id="headerLayoutHeaderLinkHolder" style="display:none;">
                <div class="field-header">Header Link Settings</div>
                <div class="field">
                    <csbs:Label runat="server" Text="Url" />
                    <csbs:AnchorUrlSelector runat="server" ID="HeaderLinkUrl" />
                    <asp:RequiredFieldValidator runat="server" ID="HeaderLinkUrlValidator" ControlToValidate="HeaderLinkUrl" ErrorMessage="*" ValidationGroup="PageEditor" />                        
                </div>
                <div class="field">
                    <csbs:Label runat="server" Text="Text" />
                    <telerik:RadTextBox runat="server" ID="HeaderLinkText" Width="200px" ClientEvents-OnValueChanged="headerLinkText_onValueChanged" />
                    <asp:RequiredFieldValidator runat="server" ID="HeaderLinkTextValidator" ControlToValidate="HeaderLinkText" ErrorMessage="*" ValidationGroup="PageEditor" />                        
                </div>
                <div class="field">
                    <csbs:Label runat="server" Text="Target" />
                    <csbs:AnchorTargetSelector runat="server" ID="HeaderLinkTarget" />
                </div>
            </div>
        </div>
        <div id="headerLayoutTemplateHolder" style="display:none;">
            <div class="field-header">Header Template</div>
            <div class="field">
                <csbs:HtmlEditor runat="server" ID="HeaderTemplateEditor" Width="777px" OnClientSelectionChange="headerTemplateEditor_onChange" OnClientLoad="headerTemplateEditor_onLoad" />
            </div>
        </div>        
    </telerik:RadPageView>

    <telerik:RadPageView runat="server" ID="page3" CssClass="editor">
        <div class="field-header">Page Settings</div>
        <div class="field">
            <csbs:Label runat="server" Text="Layout" />
            <csbs:PageContentLayoutSelector runat="server" ID="ContentLayout" OnClientSelectedIndexChanged="contentLayout_selectedIndexChanged" />
        </div>
        <div id="pageSettingsTwoColumnHolder" style="display:none;">
            <div class="field">
                <csbs:Label runat="server" Text="Position" />
                <csbs:PageContentColumnPosition runat="server" ID="PageContentColumnPosition" OnClientSelectedIndexChanged="pageContentColumnPosition_selectedIndexChanged" />                       
            </div>
            <div class="field">
                <csbs:Label runat="server" Text="Column Size" />
                <csbs:PageContentSizeSelector runat="server" ID="SideColumnSize" OnClientSelectedIndexChanged="sideColumnSize_selectedIndexChanged" />                       
            </div>
            <div class="field">
                <csbs:Label runat="server" Text="Separator" />
                <asp:CheckBox runat="server" ID="ShowColumnSeparator" Text="Show separator between columns" />
            </div>
        </div>
        <div class="field-header">Menu Settings</div>
        <div class="field">
            <csbs:Label runat="server" Text="Position" />
            <csbs:MenuPositionTypeSelector runat="server" ID="MenuPosition" OnClientSelectedIndexChanged="menuPosition_selectedIndexChanged" />
        </div>
        <div id="menuItemsHolder" class="field" style="display: none;">
            <csbs:Label runat="server" Text="Items" />
            <csbs:PageMenuSelector runat="server" ID="PageMenuSelector" DragAndDrop="false" ExpandDelay="0" Style="float:left;" OnClientNodeChecked="pageMenuSelector_onNodeChecked" OnClientLoad="pageMenuSelector_onLoad">
                <DataBindings>
                    <telerik:RadTreeNodeBinding Expanded="true" />
                </DataBindings>
            </csbs:PageMenuSelector>
        </div>
    </telerik:RadPageView>

    <telerik:RadPageView runat="server" ID="page4" CssClass="editor">
        <div class="field-header">Page Content</div>
        <div class="field">
            <csbs:Label runat="server" Text="Visibility" />
            <csbs:PageVisibilitySelector runat="server" ID="AdminOnly" />
        </div>
        <div class="field">
            <csbs:Label runat="server" Text="Meta Description" />
            <telerik:RadTextBox runat="server" ID="MetaDescription" Width="650px" />
        </div>
        <div class="field">
            <csbs:Label runat="server" Text="Meta Keywords" />
            <telerik:RadTextBox runat="server" ID="MetaKeywords" Width="650px" />
        </div>
        <div class="field-header">Content Column</div>
        <div class="field">
            <csbs:HtmlEditor runat="server" ID="ContentColumnEditor" Width="777px" OnClientSelectionChange="contentColumnEditor_onChange" OnClientLoad="contentColumnEditor_onLoad" />
        </div>
        <div id="pageContentSideColumnHolder" style="display:none;">
            <div class="field-header">Side Column</div>
            <div class="field">
                <csbs:HtmlEditor runat="server" ID="SideContentColumnEditor" Width="777px" OnClientSelectionChange="sideContentColumnEditor_onChange" OnClientLoad="sideContentColumnEditor_onLoad" />
            </div>
        </div>
    </telerik:RadPageView>

    <telerik:RadPageView runat="server" ID="page5" CssClass="editor">
        <div id="previewerHolder" style="display: none; width:866px; height:500px; position:relative; margin:8px 0 0 -55px; padding:8px; border:solid 2px #797979; background-color:#ffffff; overflow:hidden;">
            <div style="font-size: 18px; font-weight: bold; margin: 0  0 8px 0;">Page Previewer</div>
            <div id="previewerOverflowHolder" style="width: 866px; height:469px; overflow:hidden;">
                <iframe id="previewFrame" style="margin-left: -37px;" width="900px" height="600px" frameborder="0" scrolling="no"></iframe>
            </div>
            <div style="background-image:url(/Media/Icons/transparent.gif); width:100%; height:100%; position:absolute;top:0;left:0;">&nbsp;</div>
        </div>
    </telerik:RadPageView>

</telerik:RadMultiPage>

<telerik:RadCodeBlock runat="server">
<script type="text/javascript">

    /* Tab 1 */

    // Event handlers

    function masterPage_selectedIndexChanged(sender, args) {
        pageEditor_masterPageName = sender.get_value();
    }

    function masterTheme_selectedIndexChanged(sender, args) {
        pageEditor_masterPageThemeName = sender.get_value();
    }

    /* Tab 2 */

    // Event handlers

    function headerTemplateEditor_onLoad(editor) {
        clearCssLinks(editor);

        var doc = editor.get_document();
        var head = doc.getElementsByTagName("HEAD")[0];

        if (doc.createStyleSheet) {
            doc.createStyleSheet('/styles/' + pageEditor_masterPageName + '/editor/editor-header.css');
        }
        else {
            head.appendChild(getCssLink(doc, '/styles/' + pageEditor_masterPageName + '/editor/editor-header.css'));
        }
    }

    function headerTemplateEditor_onChange(sender, args) {
        pageEditor_headerTemplateHtml = sender.get_html();

        headerTemplateEditor_onLoad(sender);
    }

    function headerTitle_onValueChanged(sender, args) {
        pageEditor_headerTitle = sender.get_value();
    }

    function headerLinkText_onValueChanged(sender, args) {
        pageEditor_headerLinkText = sender.get_value();
    }

    function headerPosition_selectedIndexChanged(sender, args) {
        pageEditor_headerPosition = sender.get_value();
    }

    function headerLayout_selectedIndexChanged() {
        var layoutSelector = $find('<%= HeaderLayout.ClientID %>');

        pageEditor_headerLayout = layoutSelector.get_value();

        initTextHolder(pageEditor_headerLayout == 1);
        initTemplateHolder(pageEditor_headerLayout == 2);
    }

    function showHeaderLink_onClick() {
        var chk = document.getElementById('<%= ShowHeaderLink.ClientID %>');

        pageEditor_showHeaderLink = chk && chk.checked;

        initHeaderLinkSettings(chk && chk.checked);
    }

    // Methods

    function initTextHolder(isVisible) {
        var textHolder = document.getElementById('headerLayoutTextHolder');
        var chkShowHeaderLink = document.getElementById('<%= ShowHeaderLink.ClientID %>');

        if (isVisible && isVisible == true) {
            textHolder.style.display = 'block';
        } else {
            textHolder.style.display = 'none';
            chkShowHeaderLink.checked = false;
            pageEditor_showHeaderLink = false;
        }

        ValidatorEnable(document.getElementById('<%= HeaderTitleValidator.ClientID %>'), isVisible);

        showHeaderLink_onClick();
    }

    function initTemplateHolder(isVisible) {
        var templateHolder = document.getElementById('headerLayoutTemplateHolder');

        if (isVisible && isVisible == true) {
            templateHolder.style.display = 'block';
        } else {
            templateHolder.style.display = 'none';
        }
    }

    function initHeaderLinkSettings(isVisible) {
        var holder = document.getElementById('headerLayoutHeaderLinkHolder');

        if (holder) {
            if (isVisible) {
                holder.style.display = 'block';
            } else {
                holder.style.display = 'none';
            }

            ValidatorEnable(document.getElementById('<%= HeaderLinkUrlValidator.ClientID %>'), isVisible);
            ValidatorEnable(document.getElementById('<%= HeaderLinkTextValidator.ClientID %>'), isVisible);
        } else {
            alert('Error: headerLayoutHeaderLinkHolder is null.');
        }
    }

    /* Tab 3 */

    // Event handlers

    function menuPosition_selectedIndexChanged() {
        var menuPosition = $find('<%= MenuPosition.ClientID %>');
        pageEditor_menuPosition = menuPosition.get_value();

        initMenuSelectorHolder(pageEditor_menuPosition != 0);
    }

    function sideColumnSize_selectedIndexChanged(sender, args) {
        pageEditor_sideColumnSize = sender.get_value();
    }

    function pageContentColumnPosition_selectedIndexChanged(sender, args) {
        pageEditor_contentColumnPosition = sender.get_value();
    }

    function contentLayout_selectedIndexChanged() {
        var layoutSelector = $find('<%= ContentLayout.ClientID %>');

        pageEditor_contentLayout = layoutSelector.get_value();

        initTwoColumnHolder(pageEditor_contentLayout == 2);
    }

    function showColumnSeparator_onClick() {
        var chk = document.getElementById('<%= ShowColumnSeparator.ClientID %>');

        pageEditor_showColumnSeparator = chk && chk.checked;
    }

    function pageMenuSelector_onNodeChecked(sender, args) {
        var node = args.get_node();
        var nodeValue = node.get_value();
        var nodeChecked = node.get_checked();
        var existsIndex = null;

        var menuItemsArray = getMenuItems();

        for (var i = 0; i < menuItemsArray.length; i++) {
            if (menuItemsArray[i] == nodeValue) {
                existsIndex = i;
                break;
            }
        }

        if (nodeChecked && existsIndex == null) {
            menuItemsArray.push(nodeValue);
        } else if (!nodeChecked && existsIndex != null) {
            menuItemsArray.splice(existsIndex, 1);
        }

        setMenuItems(menuItemsArray);
    }

    function pageMenuSelector_onLoad(sender, args) {
        sender.uncheckAllNodes();

        var menuItems = getMenuItems();

        if (menuItems && menuItems.length > 0) {
            for (var i = 0; i < menuItems.length; i++) {
                var menuItemName = menuItems[i];

                if (menuItemName && menuItemName.length > 0) {
                    var tvNode = sender.findNodeByValue(menuItemName);

                    if (tvNode) {
                        tvNode.set_checked(true);
                    }
                }
            }
        }
    }

    // Methods

    function initTwoColumnHolder(isVisible) {
        var settingsHolder = document.getElementById('pageSettingsTwoColumnHolder');
        var contentHolder = document.getElementById('pageContentSideColumnHolder');

        if (isVisible && isVisible == true) {
            settingsHolder.style.display = 'block';
            contentHolder.style.display = 'block';
        } else {
            settingsHolder.style.display = 'none';
            contentHolder.style.display = 'none';
        }
    }

    function initMenuSelectorHolder(isVisible) {
        var holder = document.getElementById('menuItemsHolder');

        if (isVisible && isVisible == true) {
            holder.style.display = 'block';
        } else {
            holder.style.display = 'none';
        }
    }

    /* Tab 4 */

    // Event handlers

    function contentColumnEditor_onLoad(editor) {
        clearCssLinks(editor);

        var doc = editor.get_document();
        var head = doc.getElementsByTagName("HEAD")[0];

        if (doc.createStyleSheet) {
            doc.createStyleSheet('/styles/' + pageEditor_masterPageName + '/editor/editor-content.css');
        }
        else {
            head.appendChild(getCssLink(doc, '/styles/' + pageEditor_masterPageName + '/editor/editor-content.css'));
        }
    }

    function contentColumnEditor_onChange(sender, args) {
        pageEditor_contentColumnHtml = sender.get_html();

        contentColumnEditor_onLoad(sender);
    }

    function sideContentColumnEditor_onLoad(editor) {
        clearCssLinks(editor);

        var doc = editor.get_document();
        var head = doc.getElementsByTagName("HEAD")[0];

        if (doc.createStyleSheet) {
            doc.createStyleSheet('/styles/' + pageEditor_masterPageName + '/editor/editor-side.css');
        }
        else {
            head.appendChild(getCssLink(doc, '/styles/' + pageEditor_masterPageName + '/editor/editor-side.css'));
        }
    }

    function sideContentColumnEditor_onChange(sender, args) {
        pageEditor_sideColumnHtml = sender.get_html();

        sideContentColumnEditor_onLoad(sender);
    }

    /* Tab 5 */

    // Fields

    var pageEditor_previewerUrlTemplate = "/Pages/admin/pagepreviewer.aspx?";

    // Event handlers

    function previewer_onLoad() {
        setPreviewerValues();

        var pHolder = document.getElementById('previewerHolder');
        var pOverflow = document.getElementById('previewerOverflowHolder');
        var pFrame = document.getElementById('previewFrame');

        if (pFrame && pHolder && pOverflow) {
            var fHeight = null;

            if (pFrame.contentWindow && pFrame.contentWindow.document.body.scrollHeight) {
                fHeight = pFrame.contentWindow.document.body.scrollHeight;
            } else if (pFrame.contentDocument && pFrame.contentDocument.body.scrollHeight) {
                fHeight = pFrame.contentDocument.body.scrollHeight;
            }

            if (fHeight != null) {
                pFrame.height = fHeight + "px";

                fHeight += 3;
                pOverflow.style.height = fHeight + "px";

                fHeight += 31;
                pHolder.style.height = fHeight + "px";
            }
        }
    }

    function tabStrip_onClientTabSelected(sender, args) {
        if (args.get_tab().get_value() == 'tabPreviewer') {
            initPreviewer();
        }
    }

    // Methods

    function initPreviewer() {
        var previewerHolderDiv = document.getElementById('previewerHolder');
        var previewerFrame = document.getElementById('previewFrame');

        var qParams = '';

        if (pageEditor_masterPageName && pageEditor_masterPageName != '') {
            qParams = 'section=' + pageEditor_sectionName;
            qParams += '&master=' + pageEditor_masterPageName;
            qParams += '&theme=' + pageEditor_masterPageThemeName;
            qParams += '&showHeaderLink=' + pageEditor_showHeaderLink;
            qParams += '&contentLayout=' + pageEditor_contentLayout;
            qParams += '&sideColumnSize=' + pageEditor_sideColumnSize;
            qParams += '&contentPosition=' + pageEditor_contentColumnPosition;
            qParams += '&showColumnSeparator=' + pageEditor_showColumnSeparator;
            qParams += '&menuItems=' + getMenuItemsField().value;
            qParams += '&menuPosition=' + pageEditor_menuPosition;

            if (pageEditor_headerPosition)
                qParams += '&headerPosition=' + pageEditor_headerPosition;

            if (pageEditor_headerPosition)
                qParams += '&headerLayout=' + pageEditor_headerLayout;
        }

        if (qParams == null || qParams == '') {
            previewerHolderDiv.style.display = 'none';
            previewerFrame.src = '';
        }
        else {
            previewerHolderDiv.style.display = 'block';
            previewerFrame.src = pageEditor_previewerUrlTemplate + qParams;
        }
    }

    function setPreviewerValues() {
        var previewerHolder = document.getElementById('previewFrame');

        if (previewerHolder) {
            var headerTitle = getFrameElement(previewerHolder, 'csbs_pagePreviewer_HeaderTitle');
            var headerLink = getFrameElement(previewerHolder, 'csbs_contentHeaderLink');
            var headerTemplateContainer = getFrameElement(previewerHolder, 'csbs_contentHeaderTitle');
            var contentColumnContainer = getFrameElement(previewerHolder, 'csbs_pageContent');
            var sideColumnContainer = getFrameElement(previewerHolder, 'csbs_sidePanel');

            if (contentColumnContainer) {
                contentColumnContainer.innerHTML = pageEditor_contentColumnHtml;
            }

            if (sideColumnContainer) {
                var sideContainer = getFrameElement(previewerHolder, 'previewer_sidePanelContainer');

                if (!sideContainer) {
                    sideColumnContainer.innerHTML = sideColumnContainer.innerHTML + '<span id="previewer_sidePanelContainer"></span>';
                    sideContainer = getFrameElement(previewerHolder, 'previewer_sidePanelContainer');
                }

                if (sideContainer) {
                    sideContainer.innerHTML = pageEditor_sideColumnHtml;
                }
            }

            if (pageEditor_headerLayout == 1) {
                if (headerTitle) {
                    headerTitle.innerHTML = pageEditor_headerTitle;
                }

                if (headerLink) {
                    headerLink.href = '#';
                    headerLink.target = '_self';
                    headerLink.innerHTML = pageEditor_headerLinkText;
                }
            } else if (pageEditor_headerLayout == 2) {
                if (headerTemplateContainer) {
                    headerTemplateContainer.innerHTML = pageEditor_headerTemplateHtml;
                }
            }
        }
    }

    /* Helpers */

    function getFrameElement(frame, id) {
        var element = null;

        if (frame) {
            if (frame.contentWindow) {
                element = frame.contentWindow.document.getElementById(id);
            } else if (frame.contentDocument) {
                element = frame.contentDocument.getElementById(id);
            }
        }

        return element;
    }

    function getCssLink(doc, stylesheetUrl) {
        link = doc.createElement("LINK");

        link.setAttribute("href", stylesheetUrl);
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("type", "text/css");

        return link;
    }

    function getMenuItemsField() {
        return document.getElementById('<%= MenuItemsCache.ClientID %>');
    }

    function getMenuItems() {
        var hfValue = getMenuItemsField().value;
        var itemsArray = [];

        if (hfValue && hfValue.length > 0) {
            var values = hfValue.split(';');

            if (values && values.length > 0) {
                for (var i = 0; i < values.length; i++) {
                    var menuItemsValue = values[i];

                    if (menuItemsValue && menuItemsValue.length > 0) {
                        itemsArray.push(menuItemsValue);
                    }
                }
            }
        }

        return itemsArray;
    }

    function setMenuItems(itemsArray) {
        var strValue = '';

        if (itemsArray) {
            for (var i = 0; i < itemsArray.length; i++) {
                if (strValue.length > 0) {
                    strValue += ';';
                }

                strValue += itemsArray[i];
            }
        }

        getMenuItemsField().value = strValue;
    }

    function clearCssLinks(editor) {
        var doc = editor.get_document();
        doc.body.style.backgroundColor = "White";

        var head = doc.getElementsByTagName("HEAD")[0];
        var links = doc.getElementsByTagName("LINK");

        for (var i = 0; i < links.length; i++) {
            var curLink = doc.getElementById(links[i].id);
            if (curLink != null && curLink.rel == 'stylesheet') {
                head.removeChild(curLink);
                i--;
            }
        }
    }

    /* Initialization */

    function initEditor() {
        var chkShowHeaderLink = document.getElementById('<%= ShowHeaderLink.ClientID %>');
        var chkShowColumnSeparator = document.getElementById('<%= ShowColumnSeparator.ClientID %>');
        var previewHolder = document.getElementById('previewFrame');

        headerLayout_selectedIndexChanged();
        contentLayout_selectedIndexChanged();
        menuPosition_selectedIndexChanged();

        // attach events

        if (window.addEventListener) {
            previewHolder.addEventListener('load', previewer_onLoad, false);
            chkShowHeaderLink.addEventListener('click', showHeaderLink_onClick, false);
            chkShowColumnSeparator.addEventListener('click', showColumnSeparator_onClick, false);
        } else if (window.attachEvent) {
            previewHolder.attachEvent('onload', previewer_onLoad);
            chkShowHeaderLink.attachEvent('onclick', showHeaderLink_onClick);
            chkShowColumnSeparator.attachEvent('onclick', showColumnSeparator_onClick);
        } else {
            previewHolder.onload = previewer_onLoad;
            chkShowPreviewer.onclick = showHeaderLink_onClick;
            chkShowColumnSeparator.onclick = showColumnSeparator_onClick;
        }
    }

    Sys.Application.add_load(initEditor);

</script>
</telerik:RadCodeBlock>