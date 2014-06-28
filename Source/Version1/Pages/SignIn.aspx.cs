using System;
using System.Collections.Generic;
using System.Linq;
using Csbs.Web;
using Csbs.Web.UI;
using System.Web.UI.WebControls;

namespace Csbs
{
    public partial class SignIn :System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Container.Controls.Clear();
            Container.Controls.Add(LoadControl("~/Controls/UserSignIn.ascx"));
        }
    }
}