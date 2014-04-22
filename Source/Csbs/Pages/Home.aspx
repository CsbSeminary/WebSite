<%@ Page Title="CSBS&amp;C" 
    Language="C#"
    MasterPageFile="~/Masters/Home.Master" 
%>

<asp:Content runat="server" ContentPlaceHolderID="cb">

    <csbs:HomePageBlogList runat="server" HeaderImageUrl="~/Media/Images/header_school_blog.png" />

    <div style="margin: 22px 0 0 34px; width:425px; float: left;">
        <div style="width:425px; height: 170px; float:left;">
            <div id="virtual-tour">
                <img alt="" src="/Media/Images/header_get_involved.png" /><div class="separator-black" style="margin-top:3px;"></div>
                <div class="content">
                    <a href="http://csbs.ca/pages/csbs/splash/history-celebration-project.html"><img alt="" src="http://csbs.ca/Media/Images/get_involved_history_project.jpg" /></a>
                </div>
            </div>

            <div id="take-note">
                <img alt="" src="/Media/Images/header_online_news.png" /><div class="separator-black" style="margin-top:3px;"></div>
                <div class="content">
                Apart from information in the above <a href="http://csbs.ca/articles/">School<b>Blog</b></a> and our <br />bi-annual <a href="http://csbs.ca/pages/csbs/current/seminarynews.html">Seminary<b>News</b></a> publication, follow us online through our <a href="http://www.facebook.com/CSBSeminary">Facebook</a> and <a href="http://www.twitter.com/CSBSeminary">Twitter</a> pages for latest community pictures and news!</div>
            </div>
        </div>

        <div id="partners">
            <div class="images">
                <a href="/pages/csbs/about/accreditation.html"><img alt="" src="/Media/Images/logo_abhe.png" /></a>
                <a href="/pages/csbs/about/accreditation.html"><img alt="" src="/Media/Images/logo_ats.png" style="margin-left:13px;" /></a>
                <a href="/pages/csbs/about/accreditation.html"><img alt="" src="/Media/Images/logo_governament_ab.png" style="margin-left:13px;" /></a>
            </div>
            <div class="text">
                Recognized by the Province of Alberta. Accredited by the <a href="/pages/csbs/about/accreditation.html" target="_blank">Association of Theological Schools (ATS)</a> and the <a href="/pages/csbs/about/accreditation.html" target="_blank">Association for Biblical Higher Education (ABHE)</a>.
            </div>
        </div>
    </div>

    <div id="news">
        <div class="note"> <a href="/pages/csbs/current/seminarynews.html" target="_self"><img alt="" src="/Media/Images/header_seminary_news.png" style="margin-left: 4px;" /></a>
          <div class="separator-black" style="margin-top:1px;"></div>
            <div style="width: 103px; padding: 5px 0 0 7px;">
                <a href="/pages/csbs/current/seminarynews.html">Click Here</a> to sign up for email updates and  download the latest and prevous editions of our school's newsletter!<br />
          </div>
            <a href="/pages/csbs/current/seminarynews.html" target="_self"><img alt="" src="/Media/Images/image_seminary_news.png" style="position: relative; top: -113px; right: -104px;" /></a>
        </div>
        <div class="note cnb-links"> <a href="http://www.cnbc.ca" target="new"><img alt="" src="/Media/Images/header_cnbc_links.png" style="margin: -3px 0 0 4px;" /></a>
          <div class="separator-black" style="margin-top:4px;"></div>
            <div style="font-size: 8px; padding: 3px 0 0 13px;">Canadian National Baptist Convention</div>
            <div class="page-navigation-menu vertical">
                <div class="menu-list-container">
                    <ul class="triangle-bullet-list">
                        <li><a href="http://www.cnbc.ca/" target="_blank">CNBC.ca</a></li>
                        <li><a href="http://e-quip.net/" target="_blank">E-quip</a></li>
                        <li><a href="http://www.cnbc.ca/horizon" target="_blank">Baptist Horizon</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
