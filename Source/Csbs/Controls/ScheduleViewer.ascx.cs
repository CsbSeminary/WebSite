using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

using Csbs.Classes.Data;

using Telerik.Web.UI;

namespace Csbs.Controls
{
    public partial class ScheduleViewer : System.Web.UI.UserControl
    {
        #region Constants

        public const String ActionName = "view-schedule";

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
            Scheduler.Provider = new XmlSchedulerProvider(Server.MapPath("~/App_Data/Appointments.xml"), true);
            Scheduler.AppointmentDataBound += Scheduler_AppointmentDataBound;
        }

        void Scheduler_AppointmentDataBound(Object sender, SchedulerEventArgs e)
        {
            if (Scheduler.SelectedView == SchedulerViewType.AgendaView)
                return;

            Resource resource = e.Appointment.Resources.GetResourceByType("Color");
            if (resource != null)
                e.Appointment.CssClass = "rsCategory" + resource.Text;
            else
            {
                e.Appointment.CssClass = null;
            }
        }

        protected void Page_Load(Object sender, EventArgs e)
        {
            
        }
        
        #endregion

        #region Methods (binding)

        

        #endregion
    }
}