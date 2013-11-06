<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSignIn.ascx.cs" Inherits="Csbs.Web.UI.UserSignIn" %>

<table align="left">
	<tr>
		<td class="label">Name</td>
		<td class="value">
		    <asp:TextBox runat="server" ID="UserName" Width="156px" />
		    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" ErrorMessage="Required" />
		</td>
	</tr>
	<tr>
		<td class="label">Password</td>
		<td class="value">
		    <asp:TextBox runat="server" ID="UserPassword" TextMode="Password" Width="156px" />
		    <asp:RequiredFieldValidator runat="server" ControlToValidate="UserPassword" ErrorMessage="Required" />
		</td>
	</tr>
	<tr>
		<td class="value"></td>
		<td class="value"><asp:Button runat="server" ID="SubmitCommand" Text="Sign In" /></td>
	</tr>
</table>

<div style="clear: both;"></div>

<table align="left">
<tr><td>
    <div id="ErrorMessage" runat="server" style="color: #f00;" />
</td></tr>
</table>