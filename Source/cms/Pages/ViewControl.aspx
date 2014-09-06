<%@ Page Title="" Language="C#"
    MasterPageFile="~/Masters/CSBS.Master"
    AutoEventWireup="true"
    CodeBehind="ViewControl.aspx.cs"
    Inherits="Csbs.Pages.ViewControl"
    ValidateRequest="false"
%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel='Stylesheet' type='text/css' href='/cms/styles/csbs/default.css' />
    <link rel='Stylesheet' type='text/css' href='/cms/styles/csbs/themes/csb-red.css' />
    <style type="text/css">
        #control-wrapper { padding-left: 40px; padding-right: 40px; }
    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    <telerik:RadScriptManager runat="server" ID="rsm"></telerik:RadScriptManager>
    <div id="control-wrapper">
        <asp:PlaceHolder runat="server" ID="Container" />
    </div>
</asp:Content>