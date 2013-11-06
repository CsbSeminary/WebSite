﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Csbs.Classes.Data;

namespace Csbs.Controls
{
    public partial class ChapelViewer : System.Web.UI.UserControl
    {
        #region Constants

        public const String ActionName = "view-chapel";

        #endregion

        #region Fields

        private String _lastHeading;

        private String CurrentSemester
        {
            get
            {
                if (Request.QueryString["semester"] != null)
                    return HttpUtility.UrlDecode(Request.QueryString["semester"]);

                return null;
            }
        }

        #endregion

        #region Methods (loading)

        protected void Page_Init(Object sender, EventArgs e)
        {
            SermonRepeater.ItemDataBound += SermonRepeater_ItemDataBound;
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            const String relativePath = "/Media/Audios/Chapel/";
            String physicalPath = Server.MapPath(relativePath);

            List<Sermon> sermons = SermonHelper.GetSermons(relativePath,physicalPath);

            SermonRepeater.DataSource = SermonHelper.FilterSermons(CurrentSemester, sermons);
            SermonRepeater.Comparer = new SermonComparer();
            SermonRepeater.DataBind();

            SemesterRepeater.DataSource = SermonHelper.CreateSemesters(sermons,CurrentSemester);
            SemesterRepeater.DataBind();
        }

        #endregion

        #region Methods (binding)

        void SermonRepeater_ItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            HtmlGenericControl photo = (HtmlGenericControl)e.Item.FindControl("SermonPhoto");
            HtmlTableRow heading = (HtmlTableRow)e.Item.FindControl("SermonHeading");
            HtmlTableCell detail = (HtmlTableCell) e.Item.FindControl("SermonDetail");

            Sermon sermon = (Sermon)e.Item.DataItem;

            Boolean showHeading = !String.IsNullOrEmpty(sermon.Heading) && _lastHeading != sermon.Heading;
            
            photo.Visible = !String.IsNullOrEmpty(sermon.PhotoUrl);
            heading.Visible = showHeading;
            
            if (!String.IsNullOrEmpty(sermon.Heading))
                detail.Style["padding-left"] = "50px;";

            _lastHeading = sermon.Heading;
        }

        #endregion
    }
}