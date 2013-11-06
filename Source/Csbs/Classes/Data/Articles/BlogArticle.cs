using System;
using System.Text;
using System.Web.UI;
using System.Xml;
using Csbs.Web.UI;
using System.Web.UI.WebControls;

using Csbs.Utilities;

namespace Csbs.Data.Articles
{
    [Serializable]
    public class BlogArticle : IComparable
    {
        #region Private fields

        private Int32? _id = null;
        private DateTime _date = DateTime.MinValue;
        private String _title = null;
        private Boolean _isVisible = false;
        private String _description = null;
        private String _imageSource = null;
        private String _homeImageSource = null;
        private String _filename = null;
        private String _metaKeywords = null;
        private String _metaDescription = null;
        private String _name = null;

        private String _contentCache = null;
        private String _headCache = null;

        #endregion

        #region Public properties

        public Int32 ID
        {
            get { return _id.Value; }
            set { _id = value; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public Boolean IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; }
        }

        public String HomePageImageSource
        {
            get { return _homeImageSource; }
            set { _homeImageSource = value; }
        }

        public String FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public String MetaKeywords
        {
            get { return _metaKeywords; }
            set { _metaKeywords = value; }
        }

        public String MetaDescription
        {
            get { return _metaDescription; }
            set { _metaDescription = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public String IndexName
        {
            get { return GetIndexName(Date, Name); }
        }

        public Boolean IsNew
        {
            get { return !_id.HasValue; }
        }

        #endregion

        #region Constructor

        public BlogArticle()
        {
        }

        public BlogArticle(XmlNode xmlNode)
        {
            ReadXmlAttributes(xmlNode);
            ReadChilds(xmlNode);
        }

        #endregion

        #region Private methods

        private void ReadChilds(XmlNode xmlNode)
        {
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "description":
                        _description = ReadInnerText(childNode);
                        break;
                    case "metaDescription":
                        _metaDescription = ReadInnerText(childNode);
                        break;
                    case "metaKeywords":
                        _metaKeywords = ReadInnerText(childNode);
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }
        }

        private void ReadXmlAttributes(XmlNode xmlNode)
        {
            foreach (XmlAttribute attr in xmlNode.Attributes)
            {
                switch (attr.Name)
                {
                    case "id":
                        _id = StringHelper.ToInt32(attr.Value);
                        break;
                    case "title":
                        _title = attr.Value;
                        break;
                    case "date":
                        _date = StringHelper.ToDateTime(attr.Value);
                        break;
                    case "isVisible":
                        _isVisible = StringHelper.ToBoolean(attr.Value);
                        break;
                    case "imgSrc":
                        _imageSource = attr.Value;
                        break;
                    case "homeImgSrc":
                        _homeImageSource = attr.Value;
                        break;
                    case "filename":
                        _filename = attr.Value;
                        break;
                    case "name":
                        _name = attr.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        private static String ReadInnerText(XmlNode xmlNode)
        {
            String result = null;

            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        result += childNode.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown XML tag: {0}", childNode.Name));
                }
            }

            return result;
        }

        #endregion

        #region IComparable

        public Int32 CompareTo(Object obj)
        {
            Int32 result = 1;

            if (obj != null && obj is BlogArticle)
            {
                BlogArticle compareBlog = obj as BlogArticle;

                result = compareBlog.Date.CompareTo(Date);

                if (result == 0)
                    result = compareBlog.ID.CompareTo(ID);
            }

            return result;
        }

        #endregion

        #region Rendering

        public void RenderHead(PlaceHolder ph)
        {
            if (ph == null)
                throw new ArgumentNullException("PlaceHolder");

            if (String.IsNullOrEmpty(_headCache))
                _headCache = GetHeadContent();

            ph.Controls.Add(new LiteralControl(_headCache));
        }

        public void RenderContent(PlaceHolder ph)
        {
            if (ph == null)
                throw new ArgumentNullException("PlaceHolder");

            if (String.IsNullOrEmpty(_contentCache))
                _contentCache = FileManager.GetArticleContent(_filename);

            ph.Controls.Add(new LiteralControl(_contentCache));
        }

        private String GetHeadContent()
        {
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(_metaDescription))
                RenderHelper.WriteMetaDeclaration(sb, "description", _metaDescription);

            if (!String.IsNullOrEmpty(_metaKeywords))
                RenderHelper.WriteMetaDeclaration(sb, "keywords", _metaKeywords);

            return sb.ToString();
        }

        #endregion

        #region Helpers

        private static String GetIndexName(DateTime date, String pageName)
        {
            return String.Format("{0:yyyy-MM-dd}.{1}", date, pageName);
        }

        #endregion
    }
}