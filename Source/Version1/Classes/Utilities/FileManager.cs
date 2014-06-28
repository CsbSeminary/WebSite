using System;
using System.Web;

using Csbs.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading;
using System.Collections.Specialized;

using Csbs.Data;
using Csbs.Data.Articles;
using Csbs.Data.Pages;

namespace Csbs.Utilities
{
    public static class FileManager
    {
        #region Constants

        private const String MastersDirVirtualPath = "~/Masters/";
        private const String MasterStylesDirVirtualPathTemplate = "~/Styles/{0}/";
        private const String MasterThemesDirVirtualPathTemplate = "~/Styles/{0}/themes/";

        #endregion

        #region Email

        public static String GetEmailTemplateRelativePath(String fileName)
        {
            return String.Format("~/Snippets/{0}", fileName);
        }

        public static String ReadEmailTemplate(String fileName)
        {
            String path = Server.MapPath(GetEmailTemplateRelativePath(fileName));

            return File.ReadAllText(path, _defaultEncoding);
        }

        #endregion

        #region Files

        public static void TryToDeleteFile(String relativePath)
        {
            String physPath = null;

            if (TryGetPhysicalPath(relativePath, out physPath))
            {
                Exception lastException = null;

                for (Int32 i = 0; i <= 4; i++)
                {
                    try
                    {
                        File.Delete(physPath);

                        lastException = null;

                        break;
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;

                        Thread.Sleep(1000);
                    }
                }

                if (lastException != null)
                    throw lastException;
            }
        }

        public static Boolean TryGetPhysicalPath(String virtualPath, out String physicalPath)
        {
            physicalPath = null;

            if (!String.IsNullOrEmpty(virtualPath))
            {
                physicalPath = Server.MapPath(virtualPath);

                if (File.Exists(physicalPath))
                    return true;
            }

            return false;
        }

        public static DateTime? GetFileUpdateDate(String virtualPath)
        {
            String physPath = null;

            if (TryGetPhysicalPath(virtualPath, out physPath))
                return File.GetLastWriteTimeUtc(physPath);

            return null;
        }

        public static XmlDocument GetXmlDocument(String xmlVirtualPath)
        {
            String physPath = null;

            if (TryGetPhysicalPath(xmlVirtualPath, out physPath))
            {
                XmlDocument doc = new XmlDocument();

                doc.Load(physPath);

                return doc;
            }

            return null;
        }

        public static String CheckPath(String path)
        {
            if (!path.EndsWith("/", StringComparison.CurrentCultureIgnoreCase))
                path += "/";

            return path;
        }

        public static void CreateBackupFile(String virtualPath)
        {
            String physPath = Server.MapPath(virtualPath);
            String backupPath = GetBackupFileName(physPath);

            File.Copy(physPath, backupPath, true);
        }

        public static void RestoreFileFromBackup(String virtualPath)
        {
            String physPath = Server.MapPath(virtualPath);
            String backupPath = GetBackupFileName(physPath);

            File.Copy(backupPath, physPath, true);
        }
        
        public static String GetBackupFileName(String physicalPath)
        {
            String result = null;

            if (!String.IsNullOrEmpty(physicalPath))
            {
                String extension = Path.GetExtension(physicalPath);

                result = physicalPath.Substring(0, physicalPath.Length - extension.Length) + ".backup" + extension;
            }

            return result;
        }

        #endregion

        #region Articles

        public static String GetArticleContent(String fileName)
        {
            String virtualPath = GetArticleFileRelativePath(fileName);
            String physicalPath = null;

            if (TryGetPhysicalPath(virtualPath, out physicalPath))
            {
                return File.ReadAllText(physicalPath, _defaultEncoding);
            }

            return null;
        }

        public static String GetArticleFileRelativePath(String fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return null;

            String path = CheckPath(Settings.Blog.ArticlesPath);
            path += fileName + ".html";

            return path;
        }

        public static String GenerateArticleFileName(DateTime date)
        {
            String filename = String.Format("article-{0:dd}-{0:MM}-{0:yyyy}", date);

            Int32? index = GetFileIndex(filename);

            if (index.HasValue)
                filename = AddIndex(filename, index.Value);

            return filename;
        }

        public static void SaveArticleHtmlFile(BlogArticle article, String content)
        {
            if (String.IsNullOrEmpty(article.FileName))
                article.FileName = GenerateArticleFileName(article.Date);

            String virtualPath = FileManager.GetArticleFileRelativePath(article.FileName);
            TryToDeleteFile(virtualPath);

            String physicalPath = Server.MapPath(virtualPath);
            File.WriteAllText(physicalPath, content, _defaultEncoding);
        }

        #endregion

        #region Pages

        public static String CreatePageHtmlFile(String pageFullName, String content)
        {
            if (String.IsNullOrEmpty(pageFullName))
                throw new ArgumentNullException("pageFullName");

            String filename = pageFullName;

            String virtualPath = FileManager.GetPageFileRelativePath(pageFullName, null);
            String physicalPath;

            if (TryGetPhysicalPath(virtualPath, out physicalPath))
                filename = GeneratePageFileName(pageFullName);

            SavePageHtmlFile(filename, null, content);

            return filename;
        }

        public static void SavePageHtmlFile(String filename, String postfix, String content)
        {
            if (String.IsNullOrEmpty(filename))
                return;

            String virtualPath = FileManager.GetPageFileRelativePath(filename, postfix);
            TryToDeleteFile(virtualPath);

            String physicalPath = Server.MapPath(virtualPath);
            File.WriteAllText(physicalPath, content, _defaultEncoding);
        }

        public static String GetPageContent(String fileName, String postfix)
        {
            String result = null;

            if (!String.IsNullOrEmpty(fileName))
            {
                String virtualPath = GetPageFileRelativePath(fileName, postfix);
                String physicalPath = null;

                if (TryGetPhysicalPath(virtualPath, out physicalPath))
                {
                    result = File.ReadAllText(physicalPath, _defaultEncoding);
                }
            }

            return result;
        }

        public static String GetPageFileRelativePath(String filename, String postfix)
        {
            if (String.IsNullOrEmpty(filename))
                return null;

            String fullFileName =
                String.IsNullOrEmpty(postfix)
                    ? filename
                    : String.Format("{0}.{1}", filename, postfix);

            String path = CheckPath(Settings.Pages.PagesPath);
            path += fullFileName + ".html";

            return path;
        }

        public static String GeneratePageFileName(String fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("fullName");

            String filename = StringHelper.ReplaceNonAlphabetCharacters(fullName);
            Int32? index = GetFileIndex(filename);

            if (index.HasValue)
                filename = AddIndex(filename, index.Value);

            return filename;
        }

        public static void TryToDeletePageContent(String filename, params String[] postfix)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentNullException("FileName");

            if (postfix == null || postfix.Length <= 0)
                throw new ArgumentNullException("postfix");

            foreach (String pf in postfix)
            {
                String virtualPath = GetPageFileRelativePath(filename, pf);

                TryToDeleteFile(virtualPath);
            }
        }

        #endregion

        #region Theming

        public static Boolean IsMasterThemeExists(String masterName, String themeName)
        {
            Boolean exists = false;

            if (!String.IsNullOrEmpty(masterName) && !String.IsNullOrEmpty(themeName))
            {
                String thVirtualPath = LinkUtils.GetThemeCssFilePath(masterName, themeName);
                String thPhysicalPath = HttpContext.Current.Server.MapPath(thVirtualPath);

                exists = File.Exists(thPhysicalPath);
            }

            return exists;
        }

        public static Boolean IsMasterPageExists(String name)
        {
            Boolean exists = false;

            if (!String.IsNullOrEmpty(name))
            {
                String mpVirtualPath = LinkUtils.GetMasterPageFilePath(name);
                String mpPhysicalPath = HttpContext.Current.Server.MapPath(mpVirtualPath);

                exists = File.Exists(mpPhysicalPath);
            }

            return exists;
        }

        public static StringCollection GetAvailableMasterPages()
        {
            StringCollection result = new StringCollection();

            String mastersDirPhysicalPath = HttpContext.Current.Server.MapPath(MastersDirVirtualPath);
            DirectoryInfo mastersDirInfo = new DirectoryInfo(mastersDirPhysicalPath);

            if (mastersDirInfo.Exists)
            {
                foreach (FileInfo fileInfo in mastersDirInfo.EnumerateFiles("*.master"))
                {
                    String masterName = Path.GetFileNameWithoutExtension(fileInfo.Name);

                    String stylesDirVirtualPath = String.Format(MasterStylesDirVirtualPathTemplate, masterName);
                    String stylesDirPhysicalPath = HttpContext.Current.Server.MapPath(stylesDirVirtualPath);
                    DirectoryInfo themesDirInfo = new DirectoryInfo(stylesDirPhysicalPath);

                    if (themesDirInfo.Exists)
                        result.Add(masterName);
                }
            }

            return result;
        }

        public static StringCollection GetAvailableThemes(String masterName)
        {
            StringCollection result = new StringCollection();

            String themesDirVirtualPath = String.Format(MasterThemesDirVirtualPathTemplate, masterName);
            String themesDirPhysicalPath = HttpContext.Current.Server.MapPath(themesDirVirtualPath);
            DirectoryInfo themesDirInfo = new DirectoryInfo(themesDirPhysicalPath);

            if (themesDirInfo.Exists)
            {
                foreach (FileInfo fileInfo in themesDirInfo.EnumerateFiles("*.css"))
                {
                    String themeName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    result.Add(themeName);
                }
            }

            return result;
        }

        #endregion

        #region Helpers
        
        private static Encoding _defaultEncoding = new UTF8Encoding(false);

        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        private static String AddIndex(String filename, Int32 index)
        {
            return String.Format("{0}-{1}", filename, index);
        }

        private static Int32? GetFileIndex(String filename)
        {
            String physPath = Server.MapPath(GetArticleFileRelativePath(filename));

            if (!File.Exists(physPath))
                return null;

            Int32 index = 0;

            while (index <= 1000000)
            {
                index++;
                physPath = Server.MapPath(GetArticleFileRelativePath(AddIndex(filename, index)));

                if (!File.Exists(physPath))
                    break;
            }

            return index;
        }

        #endregion
    }
}