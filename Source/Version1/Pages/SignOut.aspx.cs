using System;
using System.Web.Security;
using System.Web.UI;

namespace Csbs
{
    public partial class SignOut : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}