using System;
using System.Web.UI;

using Csbs;

using Csbs.Web.UI;
using System.Collections.Generic;
using System.Text;

using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class BlogArticlesList : Control
    {
        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
            List<BlogArticle> articles = BlogManager.Current.BlogArticles;

            StringBuilder builder = new StringBuilder();

            builder.Append("<div class=\"articles-list\">");

            for (Int32 i = 0; i < articles.Count; i++)
            {
                BlogArticle article = articles[i];

                if (article.IsVisible)
                {
                    builder.AppendFormat("<div class=\"item{0}\">", i == articles.Count - 1 ? " last" : String.Empty);
                    builder.AppendFormat("<div class=\"photo-overflow\"><div class=\"photo-container\"><img alt=\"\" src=\"{0}\" /></div></div>", article.ImageSource);
                    builder.Append("<div class=\"description\">");
                    builder.AppendFormat("<div class=\"header\">{0}</div>", article.Title);
                    builder.AppendFormat("<div class=\"date\">{0:dddd, MMMM dd, yyyy}</div>", article.Date);
                    builder.AppendFormat("<div class=\"text\">{0}</div>", article.Description);
                    builder.AppendFormat("<div class=\"read-more\"><a href=\"{0}\">Read more...</a></div>", LinkUtils.ResolveClientUrl(LinkUtils.GetArticleUrl(article.IndexName)));
                    builder.Append("</div>");
                    builder.Append("</div>");
                }
            }

            builder.Append("</div>");

            writer.Write(builder.ToString());
        }

        #endregion
    }
}