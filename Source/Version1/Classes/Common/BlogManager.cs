using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Web;

using Csbs.Web;
using System.Xml;
using System.IO;
using System.Text;

using Csbs.Data.Articles;
using Csbs.Utilities;

using Telerik.Web.UI;

namespace Csbs
{
    public class BlogManager
    {
        #region Constants

        private const String CurrentApplicationKey = "BlogManager.Current";

        #endregion

        #region Fields

        private List<BlogArticle> _articles = null;
        private Hashtable _indexedByIdArticles = null;
        private Hashtable _indexedByNameArticles = null;
        private Int32 _nextArticleID = 1;

        #endregion

        #region Public properties

        public static BlogManager Current
        {
            get
            {
                if (HttpContext.Current.Application[CurrentApplicationKey] == null)
                    HttpContext.Current.Application[CurrentApplicationKey] = new BlogManager();

                return (BlogManager)HttpContext.Current.Application[CurrentApplicationKey];
            }
        }

        public List<BlogArticle> BlogArticles
        {
            get
            {
                if (_articles == null)
                {
                    _articles = ParseXml(out _nextArticleID);

                    RefreshIndexes();
                }

                return _articles;
            }
            set
            {
                if (value == null)
                    _articles = value;
            }
        }

        #endregion

        #region Private properties

        private static String XmlPath
        {
            get { return Settings.Blog.ArticlesListFile; }
        }

        private Hashtable IndexedByIdArticles
        {
            get
            {
                if (_indexedByIdArticles == null)
                    _indexedByIdArticles = new Hashtable();

                return _indexedByIdArticles;
            }
            set
            {
                if (value == null)
                    _indexedByIdArticles = value;
            }
        }

        private Hashtable IndexedByNameArticles
        {
            get
            {
                if (_indexedByNameArticles == null)
                    _indexedByNameArticles = new Hashtable();

                return _indexedByNameArticles;
            }
            set
            {
                if (value == null)
                    _indexedByNameArticles = value;
            }
        }

        #endregion

        #region Loading

        private static List<BlogArticle> ParseXml(out Int32 nextItemID)
        {
            nextItemID = 1;
            List<BlogArticle> data = null;
            XmlDocument doc = FileManager.GetXmlDocument(XmlPath);

            if (doc != null)
            {
                data = new List<BlogArticle>();

                XmlNode root = doc.GetElementsByTagName("blog")[0];

                foreach (XmlNode node in root.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "article":
                            BlogArticle article = new BlogArticle(node);
                            data.Add(article);

                            if (article.ID > nextItemID)
                                nextItemID = article.ID;

                            break;
                        default:
                            throw new NotImplementedException(String.Format("Unknown XML tag: {0}", node.Name));
                    }
                }

                if (data.Count > 0)
                    nextItemID++;

                data.Sort();
            }

            return data;
        }

        private void RefreshIndexes()
        {
            IndexedByIdArticles.Clear();
            IndexedByNameArticles.Clear();

            if (_articles != null)
            {
                foreach (BlogArticle article in _articles)
                {
                    IndexedByIdArticles.Add(article.ID, article);
                    IndexedByNameArticles.Add(article.IndexName, article);
                }
            }
        }

        #endregion

        #region Data operations

        public void Save()
        {
            if (_articles == null)
                return;

            FileManager.CreateBackupFile(XmlPath);

            try
            {
                String physPath = null;
                FileManager.TryGetPhysicalPath(XmlPath, out physPath);

                using (FileStream fs = File.Open(physPath, FileMode.Create))
                {
                    using (XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;

                        writer.WriteStartDocument(false);
                        writer.WriteStartElement("blog");

                        foreach (BlogArticle article in _articles)
                        {
                            if (article.IsNew || article.Date == DateTime.MinValue || String.IsNullOrEmpty(article.FileName) || String.IsNullOrEmpty(article.Name))
                                throw new Exception("The item is not initialized.");

                            writer.WriteStartElement("article");
                            writer.WriteAttributeString("id", article.ID.ToString());
                            writer.WriteAttributeString("filename", article.FileName);
                            writer.WriteAttributeString("name", article.Name);
                            writer.WriteAttributeString("title", article.Title);
                            writer.WriteAttributeString("date", article.Date.ToShortDateString());
                            writer.WriteAttributeString("isVisible", article.IsVisible ? "1" : "0");
                            writer.WriteAttributeString("imgSrc", article.ImageSource);
                            writer.WriteAttributeString("homeImgSrc", article.HomePageImageSource);

                            writer.WriteStartElement("description");
                            writer.WriteCData(article.Description);
                            writer.WriteEndElement();

                            writer.WriteStartElement("metaDescription");
                            writer.WriteCData(article.MetaDescription);
                            writer.WriteEndElement();

                            writer.WriteStartElement("metaKeywords");
                            writer.WriteCData(article.MetaKeywords);
                            writer.WriteEndElement();

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }
            }
            catch (Exception ex)
            {
                FileManager.RestoreFileFromBackup(XmlPath);

                throw ex;
            }
        }

        public void AddArticle(BlogArticle article)
        {
            if (!article.IsNew)
                return;

            article.ID = _nextArticleID++;

            _articles.Add(article);
        }

        public Boolean Delete(Int32 id)
        {
            if (BlogArticles == null || BlogArticles.Count <= 0)
                return false;

            BlogArticle article = FindArticleByID(id);

            if (article != null)
            {
                FileManager.TryToDeleteFile(FileManager.GetArticleFileRelativePath(article.FileName));
                BlogArticles.Remove(article);

                article = null;
            }

            return true;
        }

        #endregion

        #region Data binding

        public void AddEditorPageLinks(EditorLinkCollection links)
        {
            EditorLink articlesLink = new EditorLink("Articles", null);

            List<BlogArticle> articles = BlogArticles;

            if (articles != null)
            {
                foreach (BlogArticle article in articles)
                {
                    String href = LinkUtils.ResolveClientUrl(LinkUtils.GetArticleUrl(article.IndexName));

                    articlesLink.ChildLinks.Add(new EditorLink(article.Title, href));
                }
            }

            links.Add(articlesLink);
        }

        public void AddDataToSelectorDataSource(DataTable table)
        {
            List<BlogArticle> articles = BlogArticles;

            if (articles != null)
            {
                foreach (BlogArticle article in articles)
                {
                    String url = LinkUtils.ResolveClientUrl(LinkUtils.GetArticleUrl(article.IndexName));

                    AddRow(table, url, url);
                }
            }
        }

        public DataTable GetItemsDataSource()
        {
            DataTable t = new DataTable("BlogItems");

            t.Columns.Add(new DataColumn("ID", typeof(Int32)));
            t.Columns.Add(new DataColumn("Title", typeof(String)));
            t.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            t.Columns.Add(new DataColumn("IsVisible", typeof(Boolean)));
            t.Columns.Add(new DataColumn("Year", typeof(Int32)));
            t.Columns.Add(new DataColumn("ImageSource", typeof(String)));
            t.Columns.Add(new DataColumn("Description", typeof(String)));

            List<BlogArticle> articles = BlogArticles;

            if (articles != null)
            {
                foreach (BlogArticle article in articles)
                {
                    AddRow(t, article.ID, article.Title, article.Date, article.IsVisible, article.Date.Year, article.ImageSource, article.Description);
                }
            }

            return t;
        }

        #endregion

        #region Public methods

        public void Reset()
        {
            BlogArticles = null;
            IndexedByIdArticles = null;
            IndexedByNameArticles = null;
        }

        public BlogArticle FindArticleByID(Object id)
        {
            BlogArticle result = null;

            if (BlogArticles.Count > 0 && id != null && IndexedByIdArticles.Contains(id))
                result = (BlogArticle)IndexedByIdArticles[id];

            return result;
        }

        public BlogArticle FindArticleByIndexName(String indexName)
        {
            BlogArticle result = null;

            if (!String.IsNullOrEmpty(indexName))
            {
                String key = indexName.ToLower();

                if (BlogArticles.Count > 0 && IndexedByNameArticles.Contains(key))
                    result = (BlogArticle)IndexedByNameArticles[key];
            }

            return result;
        }

        public Boolean IsDuplicate(BlogArticle article)
        {
            BlogArticle existsArticle = FindArticleByIndexName(article.IndexName);

            return article.IsNew && existsArticle != null || !article.IsNew && article.ID != existsArticle.ID;
        }

        #endregion

        #region Helper methods

        private static void AddRow(DataTable t, params Object[] args)
        {
            DataRow row = t.NewRow();

            if (args != null)
            {
                for (Int32 i = 0; i < t.Columns.Count && i < args.Length; i++)
                {
                    row[i] = args[i] == null ? DBNull.Value : args[i];
                }
            }

            t.Rows.Add(row);
        }

        #endregion
    }
}