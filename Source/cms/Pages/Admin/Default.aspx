<%@ Page Title="" Language="C#"
    MasterPageFile="~/Masters/CSBS.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Csbs.Pages.Default"
    ValidateRequest="false"
%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel="Stylesheet" type="text/css" href="/cms/styles/admin.css" />
    <link rel="Stylesheet" type="text/css" href="/cms/styles/csbs/default.css" />
    <link rel="Stylesheet" type="text/css" href="/cms/styles/csbs/themes/csb-gray.css" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    <div class="header-title-page">
        <asp:Literal runat="server" ID="HeaderTitle" />
        <div class="separator-black" style="margin-top:9px;"></div>
    </div>
    <div id="editor-content">
        <telerik:RadScriptManager runat="server" ID="rsm"></telerik:RadScriptManager>
        <asp:PlaceHolder runat="server" ID="Container" />
    </div>
</asp:Content>
