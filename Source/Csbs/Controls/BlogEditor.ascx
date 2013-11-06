<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogEditor.ascx.cs" Inherits="Csbs.Web.UI.BlogEditor" %>

<div class="error"><asp:Literal runat="server" ID="ErrorMessage" /></div>

<div class="commands top">
    <asp:Button runat="server" ID="SaveButton" Text="Save Article" CausesValidation="true" />
    <asp:Button runat="server" ID="CloseButton" Text="Close" CausesValidation="false" />
</div>

<div class="editor">

    <div class="field">
        <csbs:Label runat="server" Text="Filename" />
        <telerik:RadTextBox runat="server" ID="FileName" Width="143px" Enabled="false" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Page Name" />
        <telerik:RadTextBox runat="server" ID="PageName" Width="430px" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="PageName" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div runat="server" id="PageLinkField" class="field">
        <csbs:Label runat="server" Text="Page Link" />
        <asp:HyperLink runat="server" ID="PageLink" Target="_blank" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Date" />
        <telerik:RadDatePicker runat="server" ID="Date" DateInput-DateFormat="MMM d, yyyy" DateInput-EnabledStyle-HorizontalAlign="Center" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="Date" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Title" />
        <telerik:RadTextBox runat="server" ID="Title" Width="430px" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="Title" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Description" />
        <telerik:RadTextBox runat="server" ID="Description" Width="430px" TextMode="MultiLine" Rows="3" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="Description" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Image" />
        <telerik:RadTextBox runat="server" ID="ImageSrc" Width="430px" />
        <img alt="" src="/Media/Icons/symbol-edit.gif" onclick="OpenImageManager('<%= ImageSrc.ClientID %>');" style="cursor: pointer;" />
        <img alt="" src="/Media/Icons/view.gif" onclick="ViewImage('<%= ImageSrc.ClientID %>');" style="cursor: pointer;" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ImageSrc" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Home Page Image" />
        <telerik:RadTextBox runat="server" ID="HomeImageSrc" Width="430px" />
        <img alt="" src="/Media/Icons/symbol-edit.gif" onclick="OpenImageManager('<%= HomeImageSrc.ClientID %>');" style="cursor: pointer;" />
        <img alt="" src="/Media/Icons/view.gif" onclick="ViewImage('<%= HomeImageSrc.ClientID %>');" style="cursor: pointer;" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="HomeImageSrc" ErrorMessage="Required" ToolTip="Required field" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Meta Description" />
        <telerik:RadTextBox runat="server" ID="MetaDescription" Width="650px" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Meta Keywords" />
        <telerik:RadTextBox runat="server" ID="MetaKeywords" Width="650px" />
    </div>
    <div class="field">
        <csbs:Label runat="server" Text="Is Visible" />
        <asp:CheckBox runat="server" ID="IsVisible" />
    </div>
    <div class="separator-dotted"></div>
    <div class="field editor">
        <csbs:HtmlEditor runat="server" ID="ContentEditor" Width="851px" OnClientLoad="contentEditor_OnClientLoad" />
    </div>
</div>

<csbs:ManagerOpener runat="server" ID="DialogOpener" />
<telerik:RadToolTip runat="server" ID="ToolTipImageViewer" Title="Image Preview" Position="Center" Animation="None" ManualClose="true">
    <div style="min-height: 220px; min-width: 220px; text-align: center;">
        <img alt="" id="toolTipImageViewer_previewImage" src="" />
    </div>
</telerik:RadToolTip>

<script type="text/javascript">

    var currentImageSelectorID = null;

    function contentEditor_OnClientLoad(editor) {
        var doc = editor.get_document();
        doc.body.style.backgroundColor = "White";

        var head = doc.getElementsByTagName("HEAD")[0];
        var links = doc.getElementsByTagName("LINK");

        for (var i = 0; i < links.length; i++) {
            var curLink = doc.getElementById(links[i].id);
            if (curLink != null && curLink.rel == 'stylesheet') {
                head.removeChild(curLink);
                i--;
            }
        }

        if (doc.createStyleSheet) {
            doc.createStyleSheet('/styles/csbs/default.css');
            doc.createStyleSheet('/styles/articles.css');
        }
        else {
            head.appendChild(getCssLink(doc, '/styles/csbs/default.css'));
            head.appendChild(getCssLink(doc, '/styles/articles.css'));           
        }
    }

    function getCssLink(doc, stylesheetUrl) {
        link = doc.createElement("LINK");

        link.setAttribute("href", stylesheetUrl);
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("type", "text/css");

        return link;
    }

    function OpenImageManager(textBoxID) {
        var dlgOpener = $find('<%= DialogOpener.ClientID %>');
        dlgOpener.open('ImageManager', { CssClasses: [] });

        currentImageSelectorID = textBoxID;
    }

    function ImageManagerCallback(sender, args) {
        if (!args) {
            alert('No file was selected!');
            return false;
        }

        var selectedItem = args.get_value();
        var txt = $find(currentImageSelectorID);

        txt.set_value(args.value.getAttribute("src", 2));
    }

    function ViewImage(controlID) {
        var txt = $find(controlID);
        var toolTip = $find('<%= ToolTipImageViewer.ClientID %>');
        var image = document.getElementById('toolTipImageViewer_previewImage');

        toolTip.hide();
        toolTip.set_targetControlID(txt.id);
        image.src = txt.get_value();
        toolTip.show()
    }

</script>   