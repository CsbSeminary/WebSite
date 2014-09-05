<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogFinder.ascx.cs" Inherits="Csbs.Web.UI.BlogFinder" %>

<div>
    <a href='<%= Page.ResolveUrl(Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Web.UI.BlogEditor.ActionName, -1)) %>'>Add a new article</a>
    <div style="float:right;">
        <asp:LinkButton runat="server" ID="ResetCommand" Text="Reset Cache" />
    </div>
</div>

<telerik:RadGrid runat="server" ID="BlogGrid" GridLines="None" AutoGenerateColumns="false" EnableViewState="true">
    <MasterTableView DataKeyNames="ID" AllowSorting="false" AllowPaging="false" CommandItemDisplay="None" EditMode="EditForms" GroupLoadMode="Client">
        <Columns>
            <telerik:GridTemplateColumn>
                <ItemStyle Width="15px" />
                <ItemTemplate>
                    <asp:HyperLink runat="server" ID="EditorLink" NavigateUrl='<%# Csbs.Utilities.LinkUtils.GetAdminPageUrl(Csbs.Web.UI.BlogEditor.ActionName, (Int32?)Eval("ID")) %>'>
                        <asp:Image runat="server" ImageUrl="~/Media/Icons/symbol-edit.gif" AlternateText="Edit" />
                    </asp:HyperLink>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridTemplateColumn>
                <ItemStyle Width="15px" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="PreviewImage" ImageUrl="~/Media/Icons/zoom-in.gif" CssClass="show-preview" />
                    <telerik:RadToolTip runat="server" ID="PreviewToolTip" TargetControlID="PreviewImage" Title="Article Preview" Position="TopRight" Animation="None" ShowEvent="OnClick" ManualClose="true" />
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridBoundColumn DataField="Title" HeaderText="Title" />
            <telerik:GridDateTimeColumn DataField="Date" HeaderText="Date" DataFormatString="{0:ddd, MMM dd, yyyy}" />
            <telerik:GridTemplateColumn HeaderText="Visible">
                <ItemTemplate>
                    <%# (Boolean)Eval("IsVisible") ? "Yes" : "No" %>
                </ItemTemplate>
            </telerik:GridTemplateColumn>
            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete this article?" ConfirmDialogType="RadWindow" ImageUrl="~/Media/Icons/symbol-delete.gif"
                ConfirmTitle="Delete" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn">
                <ItemStyle Width="15px" />
            </telerik:GridButtonColumn>
        </Columns>
        <GroupByExpressions>
            <telerik:GridGroupByExpression>
                <GroupByFields>
                    <telerik:GridGroupByField FieldName="Year" HeaderText="Year" SortOrder="Descending" />
                </GroupByFields>
                <SelectFields>
                    <telerik:GridGroupByField FieldName="Year" HeaderText="Year" />
                </SelectFields>
            </telerik:GridGroupByExpression>
        </GroupByExpressions>
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Year" SortOrder="Descending" />
            <telerik:GridSortExpression FieldName="ID" SortOrder="Descending" />
        </SortExpressions>
    </MasterTableView>
</telerik:RadGrid>