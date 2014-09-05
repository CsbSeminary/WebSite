<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuSectionEditor.ascx.cs" Inherits="Csbs.Web.UI.MenuSectionEditor" %>

<div class="commands top">
    <asp:Button runat="server" ID="SaveButton" Text="Save" CausesValidation="true" />
    <asp:Button runat="server" ID="CloseButton" Text="Close" CausesValidation="false" />
</div>

<div class="editor">
    <div class="field">
        <csbs:Label runat="server" Text="Title" />
        <telerik:RadTextBox runat="server" ID="SectionTitle" Width="200px" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Show Header" />
        <asp:CheckBox runat="server" ID="ShowHeader" Checked="false" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Html Content" />
        <asp:CheckBox runat="server" ID="IsHtmlContent" Checked="false" />
    </div>
    <asp:PlaceHolder runat="server" ID="InnerContent" Visible="false">
        <div id="htmlContentPlaceHolder" style="display:none;">
            <div class="separator-dotted"></div>
            <div class="field-header">HTML</div>
            <div class="field">
                <csbs:HtmlEditor runat="server" ID="HtmlContentEditor" Width="777px" OnClientLoad="htmlContentEditor_OnClientLoad" />
            </div>
        </div>
        <div id="menuItemsPlaceHolder" style="display:none;">
            <div class="separator-dotted"></div>
            <div class="field-header">Items</div>
            <div class="field">
                <telerik:RadAjaxPanel runat="server" LoadingPanelID="LoadingPanel">
                    <telerik:RadGrid runat="server" ID="ItemsGrid" GridLines="None" AutoGenerateColumns="false">
                        <MasterTableView DataKeyNames="ID" AllowSorting="false" AllowPaging="false" CommandItemDisplay="Top" EditMode="EditForms">
                            <Columns>

                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn" EditImageUrl="~/Media/Icons/symbol-edit.gif">
                                    <ItemStyle Width="15px" />
                                </telerik:GridEditCommandColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Width="15px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="MoveUp">
                                            <asp:Image runat="server" ImageUrl="~/Media/Icons/arrow-up.gif" AlternateText="Move Up" ToolTip="Move Up" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Width="15px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="MoveDown">
                                            <asp:Image runat="server" ImageUrl="~/Media/Icons/arrow-down.gif" AlternateText="Move Down" ToolTip="Move Down" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="Title" HeaderText="Title" ReadOnly="true" />
                                <telerik:GridBoundColumn DataField="Text" HeaderText="Text" ReadOnly="true" />
                                <telerik:GridTemplateColumn HeaderText="Target" ReadOnly="true">
                                    <ItemTemplate>
                                        <%# Eval("Target") as String == "_blank" ? "New Window" : "Same Window"%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="AdminTitle" HeaderText="Admin Title" ReadOnly="true" />
                                <telerik:GridTemplateColumn HeaderText="Admin Only" ReadOnly="true">
                                    <ItemTemplate>
                                        <%# (Boolean)Eval("AdminOnly") ? "Yes" : "No"%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Link Title" Visible="false">
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="ItemTitle" Width="200px" />                                       
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Text" Visible="false">
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="ItemText" Width="200px" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Href" Visible="false">
                                    <EditItemTemplate>
                                        <csbs:AnchorUrlSelector runat="server" ID="ItemHref" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Target" Visible="false">
                                    <EditItemTemplate>
                                        <csbs:AnchorTargetSelector runat="server" ID="ItemLinkTarget" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Admin Link Text" Visible="false">
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="ItemAdminTitle" Width="200px" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Admin Href" Visible="false">
                                    <EditItemTemplate>
                                        <csbs:AnchorUrlSelector runat="server" ID="ItemAdminHref" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Visible when URL like" Visible="false">
                                    <EditItemTemplate>
                                        <telerik:RadTextBox runat="server" ID="ItemVisibleURL" Width="200px" /> <span class="small-text">(regular expression)</span>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Admin Only" Visible="false">
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="ItemAdminOnly" Text="Is menu item visible to administrator only" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridButtonColumn ConfirmText="Delete this menu item?" ConfirmDialogType="RadWindow" ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                    <ItemStyle Width="15px" HorizontalAlign="Right" />
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings ColumnNumber="2" CaptionFormatString="Edit properties of menu item">
                                <FormTableItemStyle Wrap="False" VerticalAlign="Top"></FormTableItemStyle>
                                <FormTableAlternatingItemStyle Wrap="False" VerticalAlign="Top"></FormTableAlternatingItemStyle>
                                <FormCaptionStyle VerticalAlign="Top" ForeColor="Blue"></FormCaptionStyle>
                                <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="3" BackColor="White" Width="100%" />
                                <FormTableStyle CellSpacing="0" CellPadding="2" Height="50px" BackColor="White" />
                                <EditColumn ButtonType="ImageButton" InsertText="Insert menu item" UpdateText="Update menu item"
                                    UniqueName="EditCommandColumn1" CancelText="Cancel edit">
                                </EditColumn>
                                <FormTableButtonRowStyle HorizontalAlign="Right"></FormTableButtonRowStyle>
                            </EditFormSettings>
                        </MasterTableView>
                    </telerik:RadGrid>
                </telerik:RadAjaxPanel>
            </div>
        </div>
    </asp:PlaceHolder>
</div>

<telerik:RadAjaxLoadingPanel runat="server" ID="LoadingPanel" Skin="Default" EnableSkinTransparency="true" />

<script type="text/javascript">

    // Event handlers

    function htmlContentEditor_OnClientLoad(sender, args) {
        clearCssLinks(sender);
    }

    function isHtmlContent_onClick() {
        var chkHtmlContent = document.getElementById('<%= IsHtmlContent.ClientID %>');
        var divHtmlContent = document.getElementById('htmlContentPlaceHolder');
        var divMenuItems = document.getElementById('menuItemsPlaceHolder');

        if (divHtmlContent && divMenuItems) {
            if (chkHtmlContent.checked) {
                divHtmlContent.style.display = 'block';
                divMenuItems.style.display = 'none';
            } else {
                divHtmlContent.style.display = 'none';
                divMenuItems.style.display = 'block';
            }
        }
    }

    // Helpers

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

    // Initialization

    function initEditor() {
        var chkHtmlContent = document.getElementById('<%= IsHtmlContent.ClientID %>');

        if (window.addEventListener) {
            chkHtmlContent.addEventListener('click', isHtmlContent_onClick, false);
        } else if (window.attachEvent) {
            chkHtmlContent.attachEvent('onclick', isHtmlContent_onClick);
        } else {
            chkHtmlContent.onclick = isHtmlContent_onClick;
        }

        isHtmlContent_onClick();
    }

    Sys.Application.add_load(initEditor);

</script>
