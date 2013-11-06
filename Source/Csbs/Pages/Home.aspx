<%@ Page Title="CSBS&amp;C" 
    Language="C#"
    MasterPageFile="~/Masters/Home.Master" 
%>

<asp:Content runat="server" ContentPlaceHolderID="cb">

    <csbs:HomePageBlogList runat="server" HeaderImageUrl="~/Media/Images/header_school_blog.png" />

    <div style="margin: 22px 0 0 34px; width:425px; float: left;">
        <div style="width:425px; height: 170px; float:left;">
            <div id="virtual-tour">
                <img alt="" src="/Media/Images/header_virtual_tour.png" /><div class="separator-black" style="margin-top:3px;"></div>
                <div class="content">
                    <a href="/pages/csbs/home/school-photos.html"><img alt="" src="/Media/Images/virtual_tour.png" /></a>
                </div>
            </div>

            <div id="take-note">
                <img alt="" src="/Media/Images/header_take_note.png" /><div class="separator-black" style="margin-top:3px;"></div>
                <div class="content">
                    Check the playground progress photo album for the "Kids Helping Kids" Program.<br /><a href="/pages/csbs/pr/kids-helping-kids-photos.html">Click here &gt;</a>
                </div>
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
        <div class="note">
            <img alt="" src="/Media/Images/header_seminary_news.png" style="margin-left: 4px;" /><div class="separator-black" style="margin-top:1px;"></div>
            <div style="width: 103px; padding: 5px 0 0 7px;">
                Download the last issue of our school's newsletter<br /><a href="/Media/Documents/SN22.pdf">Click Here &gt;</a> (PDF reader required)
            </div>
            <img alt="" src="/Media/Images/image_seminary_news.png" style="position: relative; top: -113px; right: -104px;" />
        </div>
        <div class="note cnb-links">
            <img alt="" src="/Media/Images/header_cnbc_links.png" style="margin: -3px 0 0 4px;" /><div class="separator-black" style="margin-top:4px;"></div>
            <div style="font-size: 8px; padding: 3px 0 0 13px;">Canadian National Baptist Convention</div>
            <div class="page-navigation-menu vertical">
                <div class="menu-list-container">
                    <ul class="triangle-bullet-list">
                        <li><a href="http://www.cnbc.ca/" target="_blank">CNBC.ca</a></li>
                        <li><a href="http://e-quip.net/" target="_blank">E-quip</a></li>
                        <li><a href="http://encountercanada.com/" target="_blank">Encounter Canada</a></li>
                        <li><a href="http://www.cnbc.ca/national-ministries/horizon/archive-of-the-baptist-horizon-2007" target="_blank">Baptist Horizon</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
