using System;
using System.Web.Configuration;

namespace Csbs.Utilities
{
    public static class Settings
    {
        public static class LinkNames
        {
            public static String ChapelOnline
            {
                get { return WebConfigurationManager.AppSettings["LinkNames.ChapelOnline"]; }
            }
        }

        public static class FeedbackEmail
        {
            public static String RecipientEmail
            {
                get { return WebConfigurationManager.AppSettings["FeedbackEmail.Recipient"]; }
            }

            public static String SenderEmail
            {
                get { return WebConfigurationManager.AppSettings["FeedbackEmail.Sender"]; }
            }

            public static String Subject
            {
                get { return WebConfigurationManager.AppSettings["FeedbackEmail.Subject"]; }
            }
        }

        public static class Menu
        {
            public static String ContactFormsXml
            {
                get { return WebConfigurationManager.AppSettings["Menu.ContactFormsXml"]; }
            }

            public static String XmlFile
            {
                get { return WebConfigurationManager.AppSettings["Menu.XmlFile"]; }
            }
        }

        public static class Blog
        {
            public static String ArticlesListFile
            {
                get { return WebConfigurationManager.AppSettings["Blog.ArticlesListFile"]; }
            }

            public static String ArticlesPath
            {
                get { return WebConfigurationManager.AppSettings["Blog.ArticlesPath"]; }
            }
        }

        public static class Pages
        {
            public static String InfoFile
            {
                get { return WebConfigurationManager.AppSettings["Pages.InfoFile"]; }
            }

            public static String PagesPath
            {
                get { return WebConfigurationManager.AppSettings["Pages.PagesPath"]; }
            }

            public static class System
            {
                public static String PageNotFound
                {
                    get { return WebConfigurationManager.AppSettings["Pages.System.PageNotFound"]; }
                }

                public static String Maintenance
                {
                    get { return WebConfigurationManager.AppSettings["Pages.System.Maintenance"]; }
                }
            }
        }

        public static class HtmlEditor
        {
            public static class Downloads
            {
                public static String Images
                {
                    get { return WebConfigurationManager.AppSettings["HtmlEditor.Downloads.Images"]; }
                }

                public static String Documents
                {
                    get { return WebConfigurationManager.AppSettings["HtmlEditor.Downloads.Documents"]; }
                }

                public static String Flash
                {
                    get { return WebConfigurationManager.AppSettings["HtmlEditor.Downloads.Flash"]; }
                }

                public static String Media
                {
                    get { return WebConfigurationManager.AppSettings["HtmlEditor.Downloads.Media"]; }
                }

                public static String Templates
                {
                    get { return WebConfigurationManager.AppSettings["HtmlEditor.Downloads.Templates"]; }
                }
            }
        }

        public static Boolean EmailEnabled
        {
            get 
            {
                return StringHelper.ToBoolean(WebConfigurationManager.AppSettings["EmailEnabled"]);
            }
        }

        public static class Forms
        {
            public static class ContactUs
            {
                public static String[] MenuItems
                {
                    get
                    {
                        String value = WebConfigurationManager.AppSettings["Forms.ContactUs.MenuItems"];

                        return String.IsNullOrEmpty(value)
                            ? null
                            : value.Split(';');
                    }
                }

                public static String UrlTemplate
                {
                    get { return WebConfigurationManager.AppSettings["Forms.ContactUs.UrlTemplate"]; }
                }
            }

            public static class Articles
            {
                public static String IndexPageUrl
                {
                    get { return WebConfigurationManager.AppSettings["Forms.Articles.IndexPageUrl"]; }
                }
            }
        }

        public static class Services
        {
            public static class ProcessImage
            {
                public static String ImagesPath
                {
                    get { return WebConfigurationManager.AppSettings["Services.ProcessImage.ImagesPath"]; }
                }

                public static String ProcessedImagesPath
                {
                    get { return WebConfigurationManager.AppSettings["Services.ProcessImage.ProcessedImagesPath"]; }
                }
            }
        }
    }
}