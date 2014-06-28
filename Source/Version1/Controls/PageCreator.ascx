<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageCreator.ascx.cs" Inherits="Csbs.Web.UI.PageCreator" %>

<div style="float:right;">
    <asp:CheckBox runat="server" ID="ShowPreviewer" Checked="false" Text="Show Previewer" />
</div>

<div class="editor">
    <asp:Wizard runat="server" ID="CreatorWizard" DisplaySideBar="false" Width="800px">
        <StartNavigationTemplate>
            <div style="padding: 4px 481px 0 0;">
                <asp:Button runat="server" CausesValidation="true" Text="Next >" CommandName="MoveNext" ValidationGroup="PageCreator" />
            </div>
        </StartNavigationTemplate>
        <StepNavigationTemplate>
            <div style="padding: 4px 481px 0 0;">
                <asp:Button runat="server" Text="< Previous" CommandName="MovePrevious" />
                <asp:Button runat="server" CausesValidation="true" Text="Next >" CommandName="MoveNext" ValidationGroup="PageCreator" />
            </div>
        </StepNavigationTemplate>
        <FinishNavigationTemplate>
            <div style="padding: 4px 481px 0 0;">
                <asp:Button runat="server" Text="< Previous" CommandName="MovePrevious" />
                <asp:Button runat="server" CausesValidation="true" Text="Finish" CommandName="MoveComplete" ValidationGroup="PageCreator" />
            </div>
        </FinishNavigationTemplate>
        <WizardSteps>
            <asp:WizardStep runat="server" ID="Step1" Title="Step 1">
                <div class="field-header">Page Design</div>
                <div class="field">
                    <csbs:Label runat="server" Text="Page Name" />
                    <telerik:RadTextBox runat="server" ID="PageName" Width="200px"  />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PageName" ErrorMessage="*" ValidationGroup="PageCreator" />
                </div>
                <div class="field">
                    <csbs:Label runat="server" Text="Page Title" />
                    <telerik:RadTextBox runat="server" ID="PageTitle" Width="200px"  />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="PageTitle" ErrorMessage="*" ValidationGroup="PageCreator" />
                </div>
                <div class="field">
                    <csbs:Label runat="server" Text="Master Page" />
                    <csbs:MasterPageSelector runat="server" ID="MasterPage" Enabled="false" OnClientSelectedIndexChanged="masterPage_selectedIndexChanged" />
                </div>
                <div class="field">
                    <csbs:Label runat="server" Text="Theme" />
                    <csbs:MasterPageThemeSelector runat="server" ID="MasterTheme" MasterPageControl="MasterPage" OnClientSelectedIndexChanged="masterTheme_selectedIndexChanged" />
                </div>
            </asp:WizardStep>
            <asp:WizardStep runat="server" ID="Step2" Title="Step 2">
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
                        <asp:RequiredFieldValidator runat="server" ID="HeaderTitleValidator" ControlToValidate="HeaderTitle" ErrorMessage="*" ValidationGroup="PageCreator" />                        
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
                            <asp:RequiredFieldValidator runat="server" ID="HeaderLinkUrlValidator" ControlToValidate="HeaderLinkUrl" ErrorMessage="*" ValidationGroup="PageCreator" />                        
                        </div>
                        <div class="field">
                            <csbs:Label runat="server" Text="Text" />
                            <telerik:RadTextBox runat="server" ID="HeaderLinkText" Width="200px" ClientEvents-OnValueChanged="headerLinkText_onValueChanged" />
                            <asp:RequiredFieldValidator runat="server" ID="HeaderLinkTextValidator" ControlToValidate="HeaderLinkText" ErrorMessage="*" ValidationGroup="PageCreator" />                        
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
            </asp:WizardStep>
            <asp:WizardStep runat="server" ID="Step3" Title="Step 3">
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
                        <asp:CheckBox runat="server" ID="ShowColumnSeparator" Text="Show separator between columns" Checked="true" />
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
            </asp:WizardStep>
            <asp:WizardStep runat="server" ID="Step4" Title="Step 4">
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
                <asp:PlaceHolder runat="server" ID="SideContentColumnHolder" Visible="false">
                    <div class="field-header">Side Column</div>
                    <div class="field">
                        <csbs:HtmlEditor runat="server" ID="SideContentColumnEditor" Width="777px" OnClientSelectionChange="sideContentColumnEditor_onChange" OnClientLoad="sideContentColumnEditor_onLoad" />
                    </div>
                </asp:PlaceHolder>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</div>

<asp:HiddenField runat="server" ID="MenuItemsCache" />

<div id="previewerHolder" style="display: none; width:866px; height:500px; position:relative; margin:16px 0 0 -55px; padding:8px; border:solid 2px #797979; background-color:#ffffff; overflow:hidden;">
    <div style="font-size: 18px; font-weight: bold; margin: 0  0 8px 0;">Page Previewer</div>
    <div id="previewerOverflowHolder" style="width: 866px; height:469px; overflow:hidden;">
        <iframe id="previewFrame" style="margin-left: -37px;" width="900px" height="600px" frameborder="0" scrolling="no"></iframe>
    </div>
    <div style="background-color:transparent; width:100%; height:100%; position:absolute;top:0;left:0;">&nbsp;</div>
</div>

<script type="text/javascript">

    /* Step 1 */

    // Event handlers

    function masterPage_selectedIndexChanged(sender, args) {
        if (pageCreator_currentStepIndex == 0) {
            pageCreator_masterPageName = sender.get_value();

            initPreviewer();
        }
    }

    function masterTheme_selectedIndexChanged(sender, args) {
        if (pageCreator_currentStepIndex == 0) {
            pageCreator_masterPageThemeName = sender.get_value();

            initPreviewer();
        }
    }

    /* Step 2 */

    // Event handlers

    function headerTemplateEditor_onLoad(editor) {
        if (pageCreator_currentStepIndex == 1) {
            clearCssLinks(editor);

            var doc = editor.get_document();
            var head = doc.getElementsByTagName("HEAD")[0];

            if (doc.createStyleSheet) {
                doc.createStyleSheet('/styles/' + pageCreator_masterPageName + '/editor/editor-header.css');
            }
            else {
                head.appendChild(getCssLink(doc, '/styles/' + pageCreator_masterPageName + '/editor/editor-header.css'));
            }
        }
    }

    function headerTemplateEditor_onChange(sender, args) {
        if (pageCreator_currentStepIndex == 1) {
            pageCreator_headerTemplateHtml = sender.get_html();

            setPreviewerValues();

            headerTemplateEditor_onLoad(sender);
        }
    }

    function headerTitle_onValueChanged(sender, args) {
        if (pageCreator_currentStepIndex == 1) {
            pageCreator_headerTitle = sender.get_value();

            setPreviewerValues();
        }
    }

    function headerLinkText_onValueChanged(sender, args) {
        if (pageCreator_currentStepIndex == 1) {
            pageCreator_headerLinkText = sender.get_value();

            setPreviewerValues();
        }
    }

    function headerPosition_selectedIndexChanged(sender, args) {
        if (pageCreator_currentStepIndex == 1) {
            pageCreator_headerPosition = sender.get_value();

            initPreviewer();
        }
    }

    function headerLayout_selectedIndexChanged() {
        if (pageCreator_currentStepIndex == 1) {
            var layoutSelector = $find('<%= HeaderLayout.ClientID %>');

            pageCreator_headerLayout = layoutSelector.get_value();

            initTextHolder(pageCreator_headerLayout == 1);
            initTemplateHolder(pageCreator_headerLayout == 2);
            initPreviewer();
        }
    }

    function showHeaderLink_onClick() {
        if (pageCreator_currentStepIndex == 1) {
            var chk = document.getElementById('<%= ShowHeaderLink.ClientID %>');

            pageCreator_showHeaderLink = chk && chk.checked;

            initHeaderLinkSettings(chk && chk.checked);
            initPreviewer();
        }
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
            pageCreator_showHeaderLink = false;
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

    /* Step 3 */

    // Event handlers

    function menuPosition_selectedIndexChanged() {
        if (pageCreator_currentStepIndex == 2) {
            var menuPosition = $find('<%= MenuPosition.ClientID %>');
            pageCreator_menuPosition = menuPosition.get_value();

            initMenuSelectorHolder(pageCreator_menuPosition != 0);
            initPreviewer();
        }
    }

    function sideColumnSize_selectedIndexChanged(sender, args) {
        if (pageCreator_currentStepIndex == 2) {
            pageCreator_sideColumnSize = sender.get_value();

            initPreviewer();
        }
    }

    function pageContentColumnPosition_selectedIndexChanged(sender, args) {
        if (pageCreator_currentStepIndex == 2) {
            pageCreator_contentColumnPosition = sender.get_value();

            initPreviewer();
        }
    }

    function contentLayout_selectedIndexChanged() {
        if (pageCreator_currentStepIndex == 2) {
            var layoutSelector = $find('<%= ContentLayout.ClientID %>');

            pageCreator_contentLayout = layoutSelector.get_value();

            initTwoColumnHolder(pageCreator_contentLayout == 2);
            initPreviewer();
        }
    }

    function showColumnSeparator_onClick() {
        if (pageCreator_currentStepIndex == 2) {
            var chk = document.getElementById('<%= ShowColumnSeparator.ClientID %>');

            pageCreator_showColumnSeparator = chk && chk.checked;

            initPreviewer();
        }
    }

    function pageMenuSelector_onNodeChecked(sender, args) {
        if (pageCreator_currentStepIndex == 2) {
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

            initPreviewer();
        }
    }

    function pageMenuSelector_onLoad(sender, args) {
        if (pageCreator_currentStepIndex == 2) {
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
    }

    // Methods

    function initTwoColumnHolder(isVisible) {
        var holder = document.getElementById('pageSettingsTwoColumnHolder');

        if (isVisible && isVisible == true) {
            holder.style.display = 'block';
        } else {
            holder.style.display = 'none';
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

    /* Step 4 */

    // Event handlers

    function contentColumnEditor_onLoad(editor) {
        if (pageCreator_currentStepIndex == 3) {
            clearCssLinks(editor);

            var doc = editor.get_document();
            var head = doc.getElementsByTagName("HEAD")[0];

            if (doc.createStyleSheet) {
                doc.createStyleSheet('/styles/' + pageCreator_masterPageName + '/editor/editor-content.css');
            }
            else {
                head.appendChild(getCssLink(doc, '/styles/' + pageCreator_masterPageName + '/editor/editor-content.css'));
            }
        }
    }

    function contentColumnEditor_onChange(sender, args) {
        if (pageCreator_currentStepIndex == 3) {
            pageCreator_contentColumnHtml = sender.get_html();

            setPreviewerValues();

            contentColumnEditor_onLoad(sender);
        }
    }

    function sideContentColumnEditor_onLoad(editor) {
        if (pageCreator_currentStepIndex == 3) {
            clearCssLinks(editor);

            var doc = editor.get_document();
            var head = doc.getElementsByTagName("HEAD")[0];

            if (doc.createStyleSheet) {
                doc.createStyleSheet('/styles/' + pageCreator_masterPageName + '/editor/editor-side.css');
            }
            else {
                head.appendChild(getCssLink(doc, '/styles/' + pageCreator_masterPageName + '/editor/editor-side.css'));
            }
        }
    }

    function sideContentColumnEditor_onChange(sender, args) {
        if (pageCreator_currentStepIndex == 3) {
            pageCreator_sideColumnHtml = sender.get_html();

            setPreviewerValues();

            sideContentColumnEditor_onLoad(sender);
        }
    }

    /* Previewer */

    // Fields

    var pageCreator_previewerUrlTemplate = "/Pages/admin/pagepreviewer.aspx?";

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
                //fHeight += 10;
                pFrame.height = fHeight + "px";

                fHeight += 3;
                pOverflow.style.height = fHeight + "px";

                fHeight += 31;
                pHolder.style.height = fHeight + "px";
            }
        }
    }

    // Methods

    function initPreviewer() {
        var chkShowPreviewer = document.getElementById('<%= ShowPreviewer.ClientID %>');
        var previewerHolderDiv = document.getElementById('previewerHolder');
        var previewerFrame = document.getElementById('previewFrame');

        var qParams = '';

        if (chkShowPreviewer.checked) {
            if (pageCreator_masterPageName && pageCreator_masterPageName != '') {
                qParams = 'section=' + pageCreator_sectionName;
                qParams += '&master=' + pageCreator_masterPageName;
                qParams += '&theme=' + pageCreator_masterPageThemeName;
                qParams += '&showHeaderLink=' + pageCreator_showHeaderLink;
                qParams += '&contentLayout=' + pageCreator_contentLayout;
                qParams += '&sideColumnSize=' + pageCreator_sideColumnSize;
                qParams += '&contentPosition=' + pageCreator_contentColumnPosition;
                qParams += '&showColumnSeparator=' + pageCreator_showColumnSeparator;
                qParams += '&menuItems=' + getMenuItemsField().value;
                qParams += '&menuPosition=' + pageCreator_menuPosition;
                
                if (pageCreator_headerPosition)
                    qParams += '&headerPosition=' + pageCreator_headerPosition;

                if (pageCreator_headerPosition)
                    qParams += '&headerLayout=' + pageCreator_headerLayout;
            }
        }

        if (qParams == null || qParams == '') {
            previewerHolderDiv.style.display = 'none';
            previewerFrame.src = '';
        }
        else {
            previewerHolderDiv.style.display = 'block';
            previewerFrame.src = pageCreator_previewerUrlTemplate + qParams;
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
                contentColumnContainer.innerHTML = pageCreator_contentColumnHtml;
            }

            if (sideColumnContainer) {
                var sideContainer = getFrameElement(previewerHolder, 'previewer_sidePanelContainer');

                if (!sideContainer) {
                    sideColumnContainer.innerHTML = sideColumnContainer.innerHTML + '<span id="previewer_sidePanelContainer"></span>';
                    sideContainer = getFrameElement(previewerHolder, 'previewer_sidePanelContainer');
                }

                if (sideContainer) {
                    sideContainer.innerHTML = pageCreator_sideColumnHtml;
                }
            }

            if (pageCreator_headerLayout == 1) {
                if (headerTitle) {
                    headerTitle.innerHTML = pageCreator_headerTitle;
                }

                if (headerLink) {
                    headerLink.href = '#';
                    headerLink.target = '_self';
                    headerLink.innerHTML = pageCreator_headerLinkText;
                }
            } else if (pageCreator_headerLayout == 2) {
                if (headerTemplateContainer) {
                    headerTemplateContainer.innerHTML = pageCreator_headerTemplateHtml;
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

    function initCreator() {
        var chkShowPreviewer = document.getElementById('<%= ShowPreviewer.ClientID %>');
        var chkShowHeaderLink = document.getElementById('<%= ShowHeaderLink.ClientID %>');
        var chkShowColumnSeparator = document.getElementById('<%= ShowColumnSeparator.ClientID %>');
        var previewHolder = document.getElementById('previewFrame');

        if (pageCreator_currentStepIndex == 1) {
            headerLayout_selectedIndexChanged();
        } else if (pageCreator_currentStepIndex == 2) {
            contentLayout_selectedIndexChanged();
            menuPosition_selectedIndexChanged();
        }

        // attach events

        if (window.addEventListener) {
            chkShowPreviewer.addEventListener('click', initPreviewer, false);
            previewHolder.addEventListener('load', previewer_onLoad, false);

            if (pageCreator_currentStepIndex == 1)
                chkShowHeaderLink.addEventListener('click', showHeaderLink_onClick, false);

            if (pageCreator_currentStepIndex == 2)
                chkShowColumnSeparator.addEventListener('click', showColumnSeparator_onClick, false);
        } else if (window.attachEvent) {
            chkShowPreviewer.attachEvent('onclick', initPreviewer);
            previewHolder.attachEvent('onload', previewer_onLoad);

            if (pageCreator_currentStepIndex == 1)
                chkShowHeaderLink.attachEvent('onclick', showHeaderLink_onClick);

            if (pageCreator_currentStepIndex == 2)
                chkShowColumnSeparator.attachEvent('onclick', showColumnSeparator_onClick);
        } else {
            chkShowPreviewer.onclick = initPreviewer;
            previewHolder.onload = previewer_onLoad;

            if (pageCreator_currentStepIndex == 1)
                chkShowPreviewer.onclick = showHeaderLink_onClick;

            if (pageCreator_currentStepIndex == 2)
                chkShowColumnSeparator.onclick = showColumnSeparator_onClick;
        }
    }

    Sys.Application.add_load(initCreator);
    Sys.Application.add_load(initPreviewer);

</script>