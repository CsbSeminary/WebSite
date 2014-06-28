using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Csbs.Web.Site.Masters
{
    public class HomeImage 
    { 
        public String ImageUrl { get; set; }

        public HomeImage(String url) { ImageUrl = url; }
    }

	public partial class Home : MasterPage
    {
        protected void Page_Load(Object sender, EventArgs args)
        {
            
        }
    }
}
