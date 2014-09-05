using System;
using System.Web.UI;

using Csbs.Web.UI;
using System.Text;

using Csbs.Data.Articles;
using Csbs.Utilities;

namespace Csbs.Web.UI
{
    public partial class BlogEditor : UserControl
    {
        #region Constants

        public const String ActionName = "edit-article";

        #endregion

        #region Fields

        private BlogArticle _article = null;

        #endregion

        #region Properties

        private BlogArticle Article
        {
            get
            {
                if (_article == null)
                {
                    Int32 id = -1;

                    if (Int32.TryParse(Request["id"], out id))
                        _article = BlogManager.Current.FindArticleByID(id);

                    if (_article == null)
                        _article = new BlogArticle();
                }

                return _article;
            }
        }

        #endregion

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {
                if (Article.IsNew)
                    InitCreator();
                else
                    InitEditor(Article);
            }

            CloseButton.OnClientClick = String.Format("window.location = '{0}'; return false;", ResolveUrl(LinkUtils.GetAdminPageUrl(BlogFinder.ActionName)));
            SaveButton.Click += SaveButton_Click;
        }

        #endregion

        #region Event handlers

        private void SaveButton_Click(Object sender, EventArgs e)
        {
            try
            {
                SaveArticle();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.ToString().Replace("\r\n", "<br />");
            }
        }

        #endregion

        #region Loading

        private void InitCreator()
        {
            Title.Text = null;
            Date.SelectedDate = DateTime.Now;
            IsVisible.Checked = true;
            ImageSrc.Text = null;
            HomeImageSrc.Text = null;
            Description.Text = null;
            FileName.Text = null;
            PageName.Text = null;
            ContentEditor.Content = GetContentTemplate(DateTime.Now);
            PageLinkField.Visible = false;
        }

        private void InitEditor(BlogArticle article)
        {
            if (article != null)
            {
                Title.Text = article.Title;
                Date.SelectedDate = article.Date;
                IsVisible.Checked = article.IsVisible;
                ImageSrc.Text = article.ImageSource;
                HomeImageSrc.Text = article.HomePageImageSource;
                Description.Text = article.Description;
                MetaDescription.Text = article.MetaDescription;
                MetaKeywords.Text = article.MetaKeywords;
                FileName.Text = String.IsNullOrEmpty(article.FileName) ? null : article.FileName + ".html";
                ContentEditor.Content = FileManager.GetArticleContent(article.FileName);
                PageName.Text = article.Name;

                String url = LinkUtils.GetArticleUrl(article.IndexName);

                PageLink.NavigateUrl = url;
                PageLink.Text = LinkUtils.ResolveAbsoluteUrl(url);
                PageLinkField.Visible = true;
            }
            else
            {
                CloseEditor();
            }
        }

        #endregion

        #region Saving

        private void SaveArticle()
        {
            if (Page.IsValid)
            {
                try
                {
                    Article.Name = StringHelper.ReplaceNonAlphabetCharacters(PageName.Text.ToLower().Replace("'", String.Empty));
                    Article.Title = Title.Text;
                    Article.Date = Date.SelectedDate.Value;
                    Article.IsVisible = IsVisible.Checked;
                    Article.ImageSource = ImageSrc.Text;
                    Article.HomePageImageSource = HomeImageSrc.Text;
                    Article.Description = Description.Text;
                    Article.MetaDescription = MetaDescription.Text;
                    Article.MetaKeywords = MetaKeywords.Text;

                    if (!BlogManager.Current.IsDuplicate(Article))
                    {
                        if (Article.IsNew)
                            BlogManager.Current.AddArticle(Article);

                        FileManager.SaveArticleHtmlFile(Article, ContentEditor.Content);

                        BlogManager.Current.Save();

                        CloseEditor();
                    }
                    else
                        ErrorMessage.Text = String.Format("Error: Article named '{0}' already exists.", Article.Name);
                }
                finally
                {
                    BlogManager.Current.Reset();
                }
            }
        }

        #endregion

        #region Helper methods

        private static void CloseEditor()
        {
            LinkUtils.RedirectToAdminPage(BlogFinder.ActionName);
        }

        private static String GetContentTemplate(DateTime date)
        {
            StringBuilder html = new StringBuilder();

            html.AppendLine("<div id=\"article-content\">");

            html.AppendLine("<div id=\"photo-wall-left\">");
            html.AppendLine("<img alt=\"\" src=\"/Media/Images/image-template-200.gif\" />");
            html.AppendLine("<div class=\"description\">This is template.</div>");
            html.AppendLine("</div>");

            html.AppendLine("<div id=\"content-right\">");

            html.AppendLine("<div id=\"written-by\">");
            html.AppendLine("<div class=\"photo\">");
            html.AppendLine("<img alt=\"\" src=\"/Media/Images/image-template-50.gif\" />");
            html.AppendLine("<img class=\"reflected\" alt=\"\" src=\"/Handlers/processimage.ashx?fn=Media/Images/image-template-50.gif&tp=reflect\" />");
            html.AppendLine("</div>");
            html.AppendLine("<div class=\"description\">");
            html.AppendLine("Written by <b>First and Last Name</b>Template");
            html.AppendLine("</div>");
            html.AppendLine("</div>");

            html.AppendLine("<img alt=\"\" src=\"/Media/Images/header_school_news.png\" class=\"school-news-logo\" />");
            html.AppendFormat("<div class=\"date\">{0:dddd, MMMM dd, yyyy}</div>", date);
            html.AppendLine("<h1 style=\"color: #D81E00;\">Header</h1>");
            html.AppendLine("<p>Text.</p>");

            html.AppendLine("</div>");

            return html.ToString();
        }

        #endregion
    }
}