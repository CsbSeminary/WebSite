<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuEditor.ascx.cs" Inherits="Csbs.Web.UI.MenuEditor" %>

<div class="commands top">
    <asp:Button runat="server" ID="SaveButton" Text="Save Menu" CausesValidation="true" />
    <asp:Button runat="server" ID="CloseButton" Text="Close" CausesValidation="false" />
</div>

<div class="editor">
    <div class="field">
        <csbs:Label runat="server" Text="Name" />
        <telerik:RadTextBox runat="server" ID="MenuName" Width="200px" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="MenuName" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Bullet" />
        <csbs:MenuBulletSelector runat="server" ID="BulletSelector" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Horiz. Separator" />
        <telerik:RadTextBox runat="server" ID="HorizontalSeparator" Width="200px" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="HorizontalSeparator" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <asp:PlaceHolder runat="server" ID="SectionGridHolder">
        <div class="separator-dotted"></div>
        <div class="field-header">Sections</div>
        <div class="field">
            <a href='<%# Page.ResolveUrl(Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Utilities.LinkUtils.ActionNameQueryStringKey, Csbs.Web.UI.MenuSectionEditor.ActionName, "menu_id", Page.Request["id"])) %>'>Add a new section</a>
            <telerik:RadAjaxPanel runat="server" LoadingPanelID="LoadingPanel">
                <telerik:RadGrid runat="server" ID="SectionGrid" GridLines="None" AutoGenerateColumns="false">
                    <MasterTableView DataKeyNames="ID" AllowSorting="false" AllowPaging="false" CommandItemDisplay="None" EditMode="EditForms">
                        <Columns>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Width="15px" />
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" ID="EditorLink" NavigateUrl='<%# Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Utilities.LinkUtils.ActionNameQueryStringKey, Csbs.Web.UI.MenuSectionEditor.ActionName, "id", (Int32?)Eval("ID"), "menu_id", Page.Request["id"]) %>'>
                                        <asp:Image runat="server" ImageUrl="~/Media/Icons/symbol-edit.gif" AlternateText="Edit" />
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Width="15px" />
                                <ItemTemplate>
                                    <asp:Image runat="server" ID="PreviewImage" ImageUrl="~/Media/Icons/zoom-in.gif" CssClass="show-preview" />
                                    <telerik:RadToolTip runat="server" ID="PreviewToolTip" TargetControlID="PreviewImage" Title="Content Preview" Position="TopRight" Animation="None" ShowEvent="OnClick" ManualClose="true" Width="360px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
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
                            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" />
                            <telerik:GridTemplateColumn HeaderText="Header Visible">
                                <ItemTemplate>
                                    <%# (Boolean)Eval("IsHeaderVisible") ? "Yes" : "No" %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Html">
                                <ItemTemplate>
                                    <%# (Boolean)Eval("IsHtmlContent") ? "Yes" : "No"%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this item?" ConfirmDialogType="RadWindow" ImageUrl="~/Media/Icons/symbol-delete.gif"
                                ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                                <ItemStyle Width="15px" />
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </div>
    </asp:PlaceHolder>
</div>

<telerik:RadAjaxLoadingPanel runat="server" ID="LoadingPanel" Skin="Default" EnableSkinTransparency="true" />