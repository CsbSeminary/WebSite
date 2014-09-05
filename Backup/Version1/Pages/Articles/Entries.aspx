<%@ Page Title="CSBS :: Articles" Language="C#"
    MasterPageFile="~/Masters/CSBS.Master"
    AutoEventWireup="true"
    CodeBehind="Entries.aspx.cs"
    Inherits="Csbs.Pages.Entries"
%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <asp:PlaceHolder runat="server" ID="HeadContent" />
    <link rel="Stylesheet" type="text/css" href="/styles/csbs/default.css" />
    <link rel="Stylesheet" type="text/css" href="/styles/csbs/themes/csb-gray.css" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    <asp:PlaceHolder runat="server" ID="ArticleContent" />
</asp:Content>