<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChapelViewer.ascx.cs" Inherits="Csbs.Controls.ChapelViewer" %>

    <style type="text/css">
        
        div.chapel .sermon .heading { font-size: 1.4em; }
        div.chapel .sermon .date { padding-bottom: 5px; font-size: 1.2em; color: #480B06; }
        div.chapel .sermon .speaker { font-weight: bold; }
        div.chapel .audio-player { float:left; margin-top:5px; }
        div.chapel .audio-link { float:left; padding-top:5px; padding-left: 10px; }
        
        div.chapel .sermon td { vertical-align: top; padding-right: 20px; padding-bottom: 20px; }
        div.chapel .sermon .photo-outer {
            width: 100px;
            height: 100px;
            -webkit-box-shadow: 0px 0px 10px #4d4d4d;
            -moz-box-shadow: 0px 0px 10px #4d4d4d;
            box-shadow: 0px 0px 10px #4d4d4d;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            -khtml-border-radius: 8px;  
            border-radius: 8px;
            border:solid white 3px;
            overflow: hidden;
            padding: 5px;
        }
        div.chapel .sermon .photo-inner {
            background: white;
            padding: 0;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            -khtml-border-radius: 8px;  
            border-radius: 8px;
            width: 100px;
            height: 100px;
            overflow: hidden;
        }
    </style>

    <div>
    
        <div class="chapel">
            
        <h1>CSBS Online Chapel</h1>
        <p>
            Welcome to the CSBS Chapel page. Here you'll find most of our recent chapel services, which you can hear from your computer. You must have the Flash Player  to hear this content. Please <a href="http://get.adobe.com/flashplayer/" style="text-decoration: none" target="_blank" >click here</a> to download the latest Flash Player if you can't hear any of the content. Please contact <a href="mailto:Steve.Parsons@csbs.ca" style="text-decoration: none">CSBS</a> if you have any questions or problems with the site.
        </p>
        <p>
            <img style="border:solid 1px #555;" src="/Media/Images/inside-chapel.jpg" />
            <br /><br />
        </p>
        
        <table class="sermon">
        
        <csbs:GroupingRepeater runat="server" ID="SermonRepeater">
            
            <GroupTemplate>
                <tr>
                    <td colspan="2">
				        <h2><%# Eval("Semester") %></h2>
                    </td>
                </tr>
			</GroupTemplate>

            <ItemTemplate>
                
                <tr runat="server" id="SermonHeading">
                    <td>
                        
                    </td>
                    <td class="heading">
                        <%# Eval("Heading") %>
                    </td>
                </tr>

                <tr>
                    
                    <td>
                        <div runat="server" ID="SermonPhoto" class="photo-outer">
                            <div class="photo-inner">
                                <img src='<%# Eval("PhotoUrl") %>' width="100" height="100" />
                            </div>
                        </div>
                    </td>
                        
                    <td runat="server" id="SermonDetail">
                        
                        <div class="date"><%# Eval("Date","{0:MMMM d, yyyy}") %></div>
                        <div class="speaker"><%# Eval("Speaker") %></div>
                        <div class="synopsis"><%# Eval("Synopsis") %></div>
                
                        <div>
                    
                            <div class="audio-player">
                            <object type="application/x-shockwave-flash" data="/Media/Audios/Chapel/player_mp3.swf" width="200" height="20">
                                    <param name="movie" value="/Media/Audios/Chapel/player_mp3.swf" />
                                    <param name="FlashVars" value="mp3=<%# Eval("AudioUrl") %>" />
                            </object>
                            </div>
                    
                            <div class="audio-link">
                            <a href="<%# Eval("AudioUrl") %>" target="_blank" title="Right-click and select 'Save As'">
                                Download
                            </a>
                            </div>

                        </div>
                
                        <div style="clear:both; padding-bottom:20px;"></div>
                            
                    </td>

                </tr>

            </ItemTemplate>

        </csbs:GroupingRepeater>
            
        </table>
           
        <hr/> 
        <h3>More Chapel Services:</h3>
        <ul>
            <asp:Repeater runat="server" ID="SemesterRepeater">
                <ItemTemplate>
                    <li>
                        <a href="<%# Eval("NavigateUrl") %>"><%# Eval("Name") %></a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>

        </div>

    </div>