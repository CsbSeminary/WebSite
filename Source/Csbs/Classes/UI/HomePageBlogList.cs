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
    public class HomePageBlogList : Control
    {
        #region Constants

        private const String WrapperCssClass = "school-blog";
        private const String WrapperHeaderCssClass = "header";
        private const String WrapperHeaderSeparatorCssClass = "separator-black";

        private const String NoteCssClass = "note";
        private const String NoteWrapperCssClass = "note-wrapper";
        private const String NoteHeaderCssClass = "note-header";
        private const String NoteDescriptionCssClass = "note-description";

        private const String AdditionalInfoCssClass = "additional-info";
        private const String AdditionalInfoArticlesCssClass = "articles";
        private const String AdditionalInfoChapelCssClass = "chapel-online";

        private const String HeaderImageUrlViewStateKey = "HeaderImageUrl";

        #endregion

        #region Properties

        public String HeaderImageUrl
        {
            get { return (String)ViewState[HeaderImageUrlViewStateKey]; }
            set { ViewState[HeaderImageUrlViewStateKey] = value; }
        }

        #endregion

        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, WrapperCssClass);
            writer.RenderBeginTag("div"); 

            writer.AddAttribute(HtmlTextWriterAttribute.Class, WrapperHeaderCssClass);
            writer.RenderBeginTag("div"); 

            if (!String.IsNullOrEmpty(HeaderImageUrl))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Alt, String.Empty);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, ResolveClientUrl(HeaderImageUrl));
                writer.RenderBeginTag("img");
                writer.RenderEndTag();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, WrapperHeaderSeparatorCssClass);
            writer.RenderBeginTag("div");
            writer.RenderEndTag();

            writer.RenderEndTag(); 

            List<BlogArticle> articles = BlogManager.Current.BlogArticles;
            Int32 renderedItemsCount = 0;

            foreach (BlogArticle article in articles)
            {
                if (article.IsVisible)
                {
                    // note
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, NoteCssClass); 
                    writer.RenderBeginTag("div");

                    // note-img
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, String.Empty);
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, ResolveClientUrl(article.HomePageImageSource));
                    writer.RenderBeginTag("img");
                    writer.RenderEndTag();

                    // note-wrapper
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, NoteWrapperCssClass);
                    writer.RenderBeginTag("div");

                    // note-header
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, NoteHeaderCssClass);
                    writer.RenderBeginTag("div");

                    writer.AddAttribute(HtmlTextWriterAttribute.Href, ResolveClientUrl(LinkUtils.GetArticleUrl(article.IndexName)));
                    writer.RenderBeginTag("a");
                    writer.Write(article.Title);
                    writer.RenderEndTag();

                    writer.RenderEndTag(); // note-header

                    // note-description
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, NoteDescriptionCssClass);
                    writer.RenderBeginTag("div");
                    writer.Write(article.Description);
                    writer.RenderEndTag(); // note-description

                    writer.RenderEndTag(); // note-wrapper

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "clear");
                    writer.RenderBeginTag("div");
                    writer.RenderEndTag();

                    writer.RenderEndTag(); // note

                    renderedItemsCount++;
                }

                if (renderedItemsCount >= 2)
                    break;
            }

            // additional-info
            writer.AddAttribute(HtmlTextWriterAttribute.Class, AdditionalInfoCssClass);
            writer.RenderBeginTag("div");

            // articles
            writer.AddAttribute(HtmlTextWriterAttribute.Class, AdditionalInfoArticlesCssClass);
            writer.RenderBeginTag("div");

            writer.AddAttribute(HtmlTextWriterAttribute.Href, ResolveClientUrl(Settings.Forms.Articles.IndexPageUrl));
            writer.RenderBeginTag("a");
            writer.Write("Article's List &gt;");
            writer.RenderEndTag();

            writer.RenderEndTag(); // articles

            // chapel-online
            writer.AddAttribute(HtmlTextWriterAttribute.Class, AdditionalInfoChapelCssClass);
            writer.RenderBeginTag("div");

            writer.AddAttribute(HtmlTextWriterAttribute.Href, ResolveClientUrl(LinkUtils.GetPageUrl("seminarylink.chapel.default")));
            writer.RenderBeginTag("a");
            writer.Write(Settings.LinkNames.ChapelOnline);
            writer.RenderEndTag();

            writer.RenderEndTag(); // chapel-online

            writer.RenderEndTag(); // additional-info

            writer.RenderEndTag(); 
        }

        #endregion
    }
}