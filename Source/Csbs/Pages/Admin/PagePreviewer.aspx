<%@ Page Title="Page Previewer" Language="C#"
    MasterPageFile="~/Masters/CSBS.Master"
    AutoEventWireup="true"
    CodeBehind="PagePreviewer.aspx.cs"
    Inherits="Csbs.Pages.PagePreviewer"
    ValidateRequest="false"
%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <asp:PlaceHolder runat="server" ID="ContentHeader" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    <asp:PlaceHolder runat="server" ID="ContentContainer" />
</asp:Content>