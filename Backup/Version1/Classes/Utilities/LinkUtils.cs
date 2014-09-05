using System;
using System.Globalization;
using System.Web;

using Csbs.Web;

namespace Csbs.Utilities
{
    public static class LinkUtils
    {
        #region Constants

        public const String ActionNameQueryStringKey = "action";

        #endregion

        #region Public methods

        public static String GetThemeCssFilePath(String masterPageName)
        {
            return GetThemeCssFilePath(masterPageName, null);
        }

        public static String GetThemeCssFilePath(String masterPageName, String themeName)
        {
            String path = "~/Styles/" + masterPageName + "/";

            if (String.IsNullOrEmpty(themeName))
                path += "default";
            else
                path += "themes/" + themeName;

            return path + ".css";
        }

        public static String GetMasterPageFilePath(String masterPageName)
        {
            return String.Format("~/Masters/{0}.Master", masterPageName);
        }

        public static String ResolveAbsoluteUrl(String relativeUrl)
        {
            String serverUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            Int32 index = serverUrl.IndexOf("://", StringComparison.CurrentCulture);

            index = serverUrl.IndexOf('/', index + "://".Length);

            if (index >= 0)
                serverUrl = serverUrl.Substring(0, index);

            return String.Format(CultureInfo.CurrentCulture, "{0}{1}", serverUrl, ResolveClientUrl(relativeUrl));
        }

        public static String ResolveClientUrl(String relativeUrl)
        {
            relativeUrl = relativeUrl.Trim();

            if (relativeUrl.StartsWith("~", StringComparison.CurrentCulture))
                relativeUrl = relativeUrl.Substring(2);
            else if (relativeUrl.StartsWith("/", StringComparison.CurrentCulture))
                relativeUrl = relativeUrl.Substring(1);

            String applicationPath = HttpContext.Current.Request.ApplicationPath;

            if (!applicationPath.EndsWith("/", StringComparison.CurrentCulture))
                applicationPath += "/";

            return String.Format(CultureInfo.CurrentCulture, "{0}{1}", applicationPath, relativeUrl);
        }

        public static void RedirectToAdminPage()
        {
            RedirectToAdminPage(null, null);
        }

        public static void RedirectToAdminPage(String actionName)
        {
            RedirectToAdminPage(actionName, null);
        }

        public static void RedirectToAdminPage(String actionName, Int32? id)
        {
            HttpContext.Current.Response.Redirect(GetAdminPageUrl(actionName, id));
        }

        public static void AddParameter(ref String parameters, String key, Object value)
        {
            if (!String.IsNullOrEmpty(key) && (value != null || (value is String && !String.IsNullOrEmpty((String)value))))
            {
                parameters += String.IsNullOrEmpty(parameters) ? "?" : "&";
                parameters += key + "=" + value;
            }
        }

        public static String GetAdminPageUrl(String actionName)
        {
            return GetAdminPageUrl(actionName, null);
        }

        public static String GetAdminPageUrl(String actionName, Int32? id)
        {
            return GetAdminPageUrl(ActionNameQueryStringKey, actionName, "id", id);
        }

        public static String GetAdminPageUrl(params Object[] args)
        {
            String parameters = null;

            for (Int32 i = 0; i < args.Length && i < args.Length + 1; i += 2)
            {
                String key = args[i] != null ? args[i].ToString() : null;
                Object value = args[i + 1];

                AddParameter(ref parameters, key, value);
            }

            return "~/Pages/admin/default.aspx" + parameters;
        }

        public static String GetPageUrl(String pageName)
        {
            return GetPagePathByName("pages", pageName);
        }

        public static String GetArticleUrl(String indexName)
        {
            return GetPagePathByName("articles", indexName);
        }

        private static String GetPagePathByName(String dir, String name)
        {
            String virtualPath = "~/";

            if (!String.IsNullOrEmpty(name))
            {
                String[] nameParts = name.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                if (nameParts != null && nameParts.Length > 0)
                {
                    virtualPath += dir + "/";

                    foreach (String part in nameParts)
                    {
                        virtualPath += part + "/";
                    }

                    virtualPath = virtualPath.Remove(virtualPath.Length - 1, 1) + ".html";
                }
            }

            return virtualPath;            
        }

        #endregion
    }
}