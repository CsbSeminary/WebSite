<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsEditor.ascx.cs" Inherits="Csbs.Web.UI.SettingsEditor" %>

<style type="text/css">
    #editor-content .editor .field .label { width:150px; }
</style>

<div class="error"><asp:Literal runat="server" ID="ErrorMessage" /></div>

<div class="commands top">
    <asp:Button runat="server" ID="SaveButton" Text="Save Settings" ValidationGroup="Settings" />
</div>

<div class="editor">

    <div class="field">
        <csbs:Label runat="server" Text="Chapel Online Link Name" />
        <telerik:RadTextBox runat="server" ID="ChapelOnline" Width="430px" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ChapelOnline" ErrorMessage="Required" ToolTip="Required field" ValidationGroup="Settings" />
    </div>

</div>