<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Masters/SeminaryLink.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="Csbs.ContactUs" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <link href="/styles/seminarylink/default.css" type="text/css" rel="Stylesheet" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">

<telerik:RadScriptManager runat="server" ID="rsm"></telerik:RadScriptManager>

<div class="page-side-content content-large content-right">
    <asp:PlaceHolder runat="server" ID="MenuHolder" />
</div>

<div class="page-content content-large content-right">
		
    <div class="header-title-content"><asp:PlaceHolder runat="server" ID="PageTopTitle"></asp:PlaceHolder></div>
    
    <div class="content-title">
        <asp:PlaceHolder runat="server" ID="ContentTitle"></asp:PlaceHolder>
    </div>
    <div class="content-body">
        <asp:PlaceHolder runat="server" ID="RequestForm">
        <p><asp:PlaceHolder runat="server" ID="ContentDescription"></asp:PlaceHolder></p>
        <br />
        <table>
            <tbody>
                <tr>
                    <td class="msg-lbl">Your E-mail Address: </td>
                    <td>
                        <asp:TextBox runat="server" ID="EmailAddress" CssClass="msg-write" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="EmailAddress" ValidationGroup="ContactUs" ErrorMessage="*" ToolTip="Required field" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="EmailAddress" ErrorMessage="*" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ToolTip="Incorrect Format" ValidationGroup="ContactUs" />
                    </td>
                </tr>
                <tr>
                    <td>First Name:</td>
                    <td>
                        <asp:TextBox runat="server" ID="FirstName" CssClass="msg-write" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName" ValidationGroup="ContactUs" ErrorMessage="*" ToolTip="Required field" />
                    </td>
                </tr>
                <tr>
                    <td>Last Name:</td>
                    <td>
                        <asp:TextBox runat="server" ID="LastName" CssClass="msg-write" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName" ValidationGroup="ContactUs" ErrorMessage="*" ToolTip="Required field" />
                    </td>
                </tr>
                <tr>
                    <td>Subject:</td>
                    <td>
                        <asp:TextBox runat="server" ID="Subject" CssClass="msg-write" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Subject" ValidationGroup="ContactUs" ErrorMessage="*" ToolTip="Required field" />
                    </td>
                </tr>
                <tr>
                    <td class="msg-lbl">Message:</td>
                    <td>
                        <asp:TextBox runat="server" ID="Message" CssClass="msg-txt" TextMode="MultiLine" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Message" ValidationGroup="ContactUs" ErrorMessage="*" ToolTip="Required field" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td><telerik:RadCaptcha runat="server" ID="Captcha" ValidationGroup="ContactUs" ErrorMessage="Security code you entered is not valid" CaptchaTextBoxLabel="" Display="Dynamic" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td><asp:Button runat="server" ID="SubmitButton" Text="Send E-mail" ValidationGroup="ContactUs" /></td>
                </tr>
            </tbody>
        </table>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="ErrorMessage"></asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="ThanksMessage">
		    <div class="photo right200"><img src="/cms/Media/Images/image_forms_csbs_front_tall.jpg" alt="Canadian Southern Baptist Seminary photo"></div>
		    <p>Thank you for contacting us. We hope to be able to pray for you in your decisions regarding seminary.</p>
		    <p>You can return to our main website pages at left, or if you'd like to contact another seminary department, you can do so in the contacts menu at bottom left.</p>
		    <p>Please let us know about your experience with our website. We are still in the process of updating and optimizing the site, and would love to have your input. We invite your suggestions, problems, questions and feedback as we continue to make the site easier to use.</p>
		    <p>If you experience any problems with the site, please let us know. <a href="/contact-webmaster.html">Click here</a> to contact our webmaster with any suggestions, problems, questions or feedback.</p>
		    <p>Thank you so much for visiting our site.</p>
		</asp:PlaceHolder>
    </div>
</div>
</asp:Content>
