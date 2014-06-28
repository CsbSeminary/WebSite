using System;
using System.Web;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Csbs.Web;

using Csbs.Data.Articles;
using Csbs.Data.Pages;
using Csbs.Utilities;

namespace Csbs.Data.Menu
{
    public class MenuItemInfo
    {
        #region Private fields

        private Int32? _id = null;
        private String _title = null;
        private String _text = null;
        private String _href = null;       
        private String _target = null;
        private String _adminTitle = null;
        private String _adminHref = null;
        private Boolean _adminOnly = false;
        private String _visibleUrl = null;

        private Boolean _isNew = true;
        private Boolean _isDeleted = false;

        private MenuSectionInfo _parent = null;

        #endregion

        #region Public properties

        public Int32 ID
        {
            get { return _id.Value; }
            set { if (IsNew) _id = value; }
        }

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public String Href
        {
            get { return _href; }
            set { _href = value; }
        }

        public String Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public String AdminHref
        {
            get { return _adminHref; }
            set { _adminHref = value; }
        }

        public String AdminTitle
        {
            get { return _adminTitle; }
            set { _adminTitle = value; }
        }

        public Boolean AdminOnly
        {
            get { return _adminOnly; }
            set { _adminOnly = value; }
        }

        public String VisibeForUrl
        {
            get { return _visibleUrl; }
            set { _visibleUrl = value; }
        }

        public Boolean IsNew
        {
            get { return _isNew; }
        }

        #endregion

        #region Constructor

        public MenuItemInfo()
        {
            _isNew = true;
        }

        public MenuItemInfo(XmlNode xmlNode, MenuSectionInfo parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            _parent = parent;

            ReadXmlAttributes(xmlNode);

            _isNew = false;
        }

        #endregion

        #region Private methods

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
                    case "href":
                        _href = attr.Value;
                        break;
                    case "target":
                        _target = attr.Value;
                        break;
                    case "text":
                        _text = attr.Value;
                        break;
                    case "adminTitle":
                        _adminTitle = attr.Value;
                        break;
                    case "adminHref":
                        _adminHref = attr.Value;
                        break;
                    case "adminOnly":
                        _adminOnly = attr.Value == "1";
                        break;
                    case "visibleURL":
                        _visibleUrl = attr.Value;
                        break;
                    default:
                        throw new NotImplementedException(String.Format("Unknown attribute of XML menu item: {0}", attr.Name));
                }
            }
        }

        #endregion

        #region Saving

        public void Write(XmlTextWriter writer)
        {
            if (_isDeleted)
                return;

            writer.WriteStartElement("item");
            writer.WriteAttributeString("id", _id.Value.ToString());
            writer.WriteAttributeString("adminOnly", _adminOnly ? "1" : "0");

            if (!String.IsNullOrEmpty(_title))
                writer.WriteAttributeString("title", _title);

            if (!String.IsNullOrEmpty(_href))
                writer.WriteAttributeString("href", _href);

            if (!String.IsNullOrEmpty(_target))
                writer.WriteAttributeString("target", _target);

            if (!String.IsNullOrEmpty(_adminTitle))
                writer.WriteAttributeString("adminTitle", _adminTitle);

            if (!String.IsNullOrEmpty(_adminHref))
                writer.WriteAttributeString("adminHref", _adminHref);

            if (!String.IsNullOrEmpty(_text))
                writer.WriteAttributeString("text", _text);

            if (!String.IsNullOrEmpty(_visibleUrl))
                writer.WriteAttributeString("visibleURL", _visibleUrl);

            writer.WriteEndElement();
        }

        #endregion

        #region Rendering

        public void Render(StringBuilder sb, PageInfo page, BlogArticle article)
        {
            if (!IsVisible(page, article))
                return;

            Boolean isAdmin = HttpContext.Current.User.Identity.IsAuthenticated;

            String url =
                    isAdmin && !String.IsNullOrEmpty(AdminHref)
                        ? AdminHref
                        : Href;

            if (!String.IsNullOrEmpty(url))
            {
                if (url.IndexOf("{$id}") >= 0 || url.IndexOf("{$article_id}") >= 0)
                {
                    if (page != null)
                        url = url.Replace("{$id}", page.ID.ToString());

                    if (article != null)
                        url = url.Replace("{$article_id}", article.ID.ToString());
                }

                sb.AppendFormat("<a href='{0}'", url);

                if (!String.IsNullOrEmpty(Target))
                    sb.AppendFormat(" target='{0}'", Target);

                sb.AppendFormat(">{0}</a> ", isAdmin && !String.IsNullOrEmpty(AdminTitle) ? AdminTitle : Title);

                sb.Append(Text);
            }
            else
            {
                sb.Append(Title);
            }
        }

        #endregion

        #region Public methods

        public Boolean MoveUp()
        {
            Boolean isOk = false;

            if (!IsNew)
                isOk = _parent.MoveUp(this);

            return isOk;
        }

        public Boolean MoveDown()
        {
            Boolean isOk = false;

            if (!IsNew)
                isOk = _parent.MoveDown(this);

            return isOk;
        }

        public Boolean TryDelete()
        {
            _isDeleted = true;

            return _isDeleted;
        }

        #endregion

        #region Helper methods

        public Boolean IsVisible(PageInfo page, BlogArticle article) 
        {
            Boolean isVisible = (!AdminOnly || HttpContext.Current.User.Identity.IsAuthenticated) 
                && (String.IsNullOrEmpty(Href) || (Href.IndexOf("{$id}") < 0 || page != null) && (Href.IndexOf("{$article_id}") < 0 || article != null));

            if (isVisible && !String.IsNullOrEmpty(VisibeForUrl))
            {
                String rawUrl = HttpContext.Current.Request.RawUrl;

                isVisible = Regex.IsMatch(rawUrl, VisibeForUrl, RegexOptions.IgnoreCase);
            }

            return isVisible;
        }

        #endregion
    }
}