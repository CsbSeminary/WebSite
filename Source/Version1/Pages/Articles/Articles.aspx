<%@ Page Title="School Blog"
    Language="C#"
    MasterPageFile="~/Masters/CSBS.Master"
%>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel="Stylesheet" type="text/css" href="/styles/csbs/default.css" />
    <link rel="Stylesheet" type="text/css" href="/styles/csbs/themes/csb-gray.css" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    <div class="header-title-page">
        <span style="color:#000000;">School</span>Blog
        <div style="margin-top: 2px;" class="separator-black"></div>
    </div>
    <div class="page-content" style="padding: 35px 108px 0 111px;">
        <csbs:BlogArticlesList runat="server" />    
    </div>
</asp:Content>