using System;
using System.Web.UI;

using Csbs.Web.UI;

using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs.Pages
{
    public partial class Entries : Page
    {
        #region Properties

        private Int32? ArticleID
        {
            get { return StringHelper.ToInt32Nullable(Page.Request["id"]); }
        }

        private String ArticleName
        {
            get { return Page.Request["name"]; }
        }

        public BlogArticle CurrentArticle
        {
            get
            {
                if (_article == null)
                {
                    if (ArticleID.HasValue)
                        _article = BlogManager.Current.FindArticleByID(ArticleID.Value);
                    else if (!String.IsNullOrEmpty(ArticleName))
                        _article = BlogManager.Current.FindArticleByIndexName(ArticleName);
                }

                return _article;
            }
        }

        #endregion

        #region Fields

        private BlogArticle _article = null;

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (CurrentArticle == null || !InitPage(CurrentArticle))
                Response.Redirect(Settings.Forms.Articles.IndexPageUrl);
        }

        private Boolean InitPage(BlogArticle article)
        {
            Boolean loaded = false;

            try
            {
                article.RenderHead(HeadContent);
                article.RenderContent(ArticleContent);

                loaded = true;
            }
            catch
            {
                
            }

            return loaded;
        }

        #endregion
    }
}
