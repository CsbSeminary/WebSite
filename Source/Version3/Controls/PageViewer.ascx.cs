using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Kernel;
using Kernel.Text;
using Iris.Web.UI;

namespace Seminary.Controls
{
    public partial class PageViewer : UserControl
    {
        #region Loading

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (!LoadPageFromTextFile())
                    LoadPageFromHtmlFile();
            }
        }

        public static String CreatePageTitleText(String name)
        {
            String text = name
                .Replace("-"," ")
                .Replace(".html", String.Empty)
                .Replace(".txt", String.Empty)
                ;

            return Inflector.ToTitleCase(text);
        }

        private Boolean LoadPageFromTextFile()
        {
            String relativePath = String.Format("~/Pages/{0}", Request["name"].Replace(".html",".txt"));
            String physicalPath = Server.MapPath(relativePath);
            if ( File.Exists(physicalPath) )
            {
                FileInfo file = new FileInfo(physicalPath);
                String name = CreatePageTitleText(file.Name);

                String title = name;
                Page.Title = "Shift iQ | " + title;

                String text = File.ReadAllText(physicalPath);
                Markdown markdown = new Markdown();
                
                PageBody.Text = markdown.Transform(text);
                ((HtmlGenericControl)Page.Master.FindControl("BannerTitle")).InnerHtml = title;

                String description = ControlHelper.TruncateString(ControlHelper.GetDescriptionFromHtml(PageBody.Text), 245);
                if (PageBody.Text != null && PageBody.Text.Length > 245)
                    description = ControlHelper.AppendWithEllipsis(description);

                ControlHelper.AddOpenGraphMetaData(
                    Page,
                    "website",
                    Page.Request.RawUrl,
                    "/app_themes/insite/images/header-logo-shift.png",
                    title,
                    description
                    );

                return true;
            }
            return false;
        }

        private Boolean LoadPageFromHtmlFile()
        {
            String relativePath = String.Format("~/Pages/{0}", Request["name"]);
            String physicalPath = Server.MapPath(relativePath);
            if (File.Exists(physicalPath))
            {
                FileInfo file = new FileInfo(physicalPath);
                String name = CreatePageTitleText(file.Name);

                String title = name;
                Page.Title = "Shift iQ | " + title;

                String html = File.ReadAllText(physicalPath);
                
                PageBody.Text = html;
                ((HtmlGenericControl)Page.Master.FindControl("BannerTitle")).InnerHtml = title;

                String description = ControlHelper.TruncateString(ControlHelper.GetDescriptionFromHtml(PageBody.Text), 245);
                if (PageBody.Text != null && PageBody.Text.Length > 245)
                    description = ControlHelper.AppendWithEllipsis(description);

                ControlHelper.AddOpenGraphMetaData(
                    Page,
                    "website",
                    Page.Request.RawUrl,
                    "/app_themes/insite/images/header-logo-shift.png",
                    title,
                    description
                    );

                return true;
            }
            return false;
        }

        #endregion
    }
}