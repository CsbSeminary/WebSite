<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="Seminary.Pages.DefaultPage"
    MasterPageFile="~/Default.master"
    Title="Default"
%>

<asp:Content runat="server" ID="ch" ContentPlaceHolderID="HtmlHead">
    
</asp:Content>

<asp:Content ID="cf" runat="server" ContentPlaceHolderID="BodyContent">
    <asp:PlaceHolder runat="server" ID="ActionControlContainer" />
</asp:Content>