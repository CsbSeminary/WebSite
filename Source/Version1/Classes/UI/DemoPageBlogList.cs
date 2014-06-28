using System;
using System.Collections.Generic;
using System.Web.UI;

using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public class DemoPageBlogList : Control
    {
        #region Constants

        private const String NoteCssClass = "note";
        private const String NoteWrapperCssClass = "note-wrapper";
        private const String NoteHeaderCssClass = "note-header";
        private const String NoteDescriptionCssClass = "note-description";

        private const String HeaderImageUrlViewStateKey = "HeaderImageUrl";

        #endregion

        #region Properties

        public String HeaderImageUrl
        {
            get { return (String) ViewState[HeaderImageUrlViewStateKey]; }
            set { ViewState[HeaderImageUrlViewStateKey] = value; }
        }

        #endregion

        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
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
        }

        #endregion
    }
}