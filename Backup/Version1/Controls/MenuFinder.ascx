<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuFinder.ascx.cs" Inherits="Csbs.Web.UI.MenuFinder" %>

<div>
    <a href='<%= Page.ResolveUrl(Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Web.UI.MenuEditor.ActionName)) %>'>Add a new menu</a>
    <div style="float:right;">
        <asp:LinkButton runat="server" ID="ResetCommand" Text="Reset Cache" />
    </div>
</div>

<telerik:RadGrid runat="server" ID="MenuGrid" GridLines="None" AutoGenerateColumns="false" EnableViewState="true">
    <MasterTableView DataKeyNames="ID" AllowSorting="false" AllowPaging="false" CommandItemDisplay="None" EditMode="EditForms">
        <Columns>
            <telerik:GridTemplateColumn>
                <ItemStyle Width="15px" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="EditorLink" NavigateUrl='<%# Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Web.UI.MenuEditor.ActionName, (Int32?)Eval("ID")) %>'>
                        <asp:Image runat="server" ImageUrl="~/Media/Icons/symbol-edit.gif" AlternateText="Edit" />
                    </asp:HyperLink>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn>
                <ItemStyle Width="15px" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="PreviewImage" ImageUrl="~/Media/Icons/zoom-in.gif" CssClass="show-preview" />
                    <telerik:RadToolTip runat="server" ID="PreviewToolTip" TargetControlID="PreviewImage" Title="Menu Preview" Position="TopRight" Animation="None" ShowEvent="OnClick" ManualClose="true" Width="360px" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="Name" HeaderText="Name" />
            <telerik:GridTemplateColumn HeaderText="Bullet" Visible="false">
                <ItemTemplate>
                    <img alt='' src='<%# Eval("BulletImageUrl") %>' />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="HorizontalSeparator" HeaderText="Horizontal Separator" Visible="false" />
            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this menu?" ConfirmDialogType="RadWindow" ImageUrl="~/Media/Icons/symbol-delete.gif"
                ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                <ItemStyle Width="15px" />
            </telerik:GridButtonColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>