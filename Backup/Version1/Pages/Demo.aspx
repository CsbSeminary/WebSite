<%@ Page Title="CSBS&amp;C" 
    Language="C#"
    MasterPageFile="~/Masters/Demo.Master" 
%>

<%@ Register tagPrefix="csbs" tagName="TwitterFeed" src="~/Controls/TwitterFeed.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="cb">
    
    <link rel="Stylesheet" type="text/css" href="/styles/demo-page.css" />

    <div>
        <div class="bulletin-board" style="height: 530px; padding: 20px; background-color:rgb(204,204,204);">
            
            <table style="width:630px">
                <tr>
                    <td style="width:400px; vertical-align:top;">
                        
                        <h2>School Blog</h2>
                        <csbs:DemoPageBlogList runat="server" />

                        <h2>School Calendar</h2>

                    </td>
                    <td style="width:200px; vertical-align:top;">
                        <csbs:TwitterFeed runat="server" />
                    </td>
                </tr>
            </table>

        </div>
    </div>

</asp:Content>
