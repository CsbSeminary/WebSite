using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;

using Csbs.Classes.Data;

namespace Csbs.Handlers
{
    /// <summary>
    ///     This is an RSS feed for Chapel services.
    /// </summary>
    public class RssChapel : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            const String relativePath = "/Media/Audios/Chapel/";
            String physicalPath = context.Server.MapPath(relativePath);

            List<Sermon> sermons = SermonHelper.GetSermons(relativePath, physicalPath);

            Byte[] data = CreateRssFeed(sermons);
            WriteFeedToResponse(context, data);
        }

        public Boolean IsReusable
        {
            get { return false; }
        }

        private void WriteFeedToResponse(HttpContext context, Byte[] data)
        {
            HttpResponse response = context.Response;
            response.Buffer = false;
            response.Cache.SetExpires(DateTime.Now.AddMinutes(0));
            response.Cache.SetCacheability(HttpCacheability.Public);

            response.Clear();
            response.ClearHeaders();
            response.ClearContent();

            response.AddHeader("Content-Length", data.Length.ToString(CultureInfo.InvariantCulture));
            response.ContentType = "application/rss+xml";

            response.OutputStream.Write(data, 0, data.Length);
            response.Flush();
            response.End();
        }

        #region Methods (helper)

        public static Byte[] CreateRssFeed(List<Sermon> sermons)
        {
            var xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

            var settings = new XmlWriterSettings {Encoding = new UTF8Encoding(false), Indent = true, OmitXmlDeclaration = true};

            using (XmlWriter writer = XmlWriter.Create(xml, settings))
            {
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version", "2.0");

                writer.WriteStartElement("channel");

                writer.WriteElementString("title", "CSBS Online Chapel");
                writer.WriteElementString("link", "http://www.csbs.ca/chapel");
                writer.WriteElementString("description",
                    "This is the RSS feed for chapel services at the Canadian Southern Baptist Seminary and College. Here you'll find recordings from most of our recent chapel services, which you can listen to on your computer.");
                writer.WriteElementString("language", "en-CA");

                AddSermons(writer, sermons);

                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            return settings.Encoding.GetBytes(xml.ToString());
        }

        private static void AddSermons(XmlWriter writer, IEnumerable<Sermon> sermons)
        {
            foreach (Sermon sermon in sermons)
            {
                writer.WriteStartElement("item");

                writer.WriteElementString("title", String.Format("{0}: {1}", sermon.Semester, sermon.Speaker));
                writer.WriteElementString("link", String.Format("http://www.csbs.ca{0}", sermon.AudioUrl));

                WriteDescription(writer, sermon);

                String pubDate = String.Format(CultureInfo.InvariantCulture, "{0:ddd, dd MMM yyyy}", sermon.Date);
                writer.WriteElementString("pubDate", pubDate);

                writer.WriteEndElement();
            }
        }

        private static void WriteDescription(XmlWriter writer, Sermon sermon)
        {
            String description = sermon.Synopsis;
            description = String.IsNullOrEmpty(description) ? "" : description.Replace("\n", "<br/>");

            String rssItemDescription = String.Format("<p><strong>{0}</strong></p>{1}", sermon.Heading, description);

            writer.WriteStartElement("description");
            writer.WriteCData(rssItemDescription);
            writer.WriteEndElement();
        }

        #endregion
    }
}