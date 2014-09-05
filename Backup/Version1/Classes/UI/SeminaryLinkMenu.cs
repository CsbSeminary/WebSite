using System;
using System.Collections.Generic;
using System.Web.UI;

using CSBS.XmlStorage;

using SeminaryLinkMenuSection = CSBS.XmlStorage.SeminaryLinkXml.SeminaryLinkMenuSection;
using SeminaryLinkMenuItem = CSBS.XmlStorage.SeminaryLinkXml.SeminaryLinkMenuItem;

namespace CSBS.Web.UI
{   
    public class SeminaryLinkMenu : Control
    {
        #region Constants

        private const String CssClassViewStateKey = "CssClass";
        private const String MenuNameViewStateKey = "MenuName";
        private const String IsIncludeContactsViewStateKey = "IsIncludeContacts";
        private const String ContactsMenuTitleViewStateKey = "ContactsMenuTitle";

        private const String DefaultCssClass = "left notop";
        private const String DefaultHeaderCssClass = "side-title";
        private const String DefaultItemsContainerCssClass = "side";

        #endregion

        #region Properties

        public String ContactsMenuTitle
        {
            get { return ViewState[ContactsMenuTitleViewStateKey] == null ? "Contact Us" : (String)ViewState[ContactsMenuTitleViewStateKey]; }
            set { ViewState[ContactsMenuTitleViewStateKey] = value; }
        }

        public Boolean IsIncludeContacts
        {
            get { return ViewState[IsIncludeContactsViewStateKey] != null && (Boolean)ViewState[IsIncludeContactsViewStateKey]; }
            set { ViewState[IsIncludeContactsViewStateKey] = value; }
        }

        public String CssClass
        {
            get
            {
                if (ViewState[CssClassViewStateKey] == null)
                    return DefaultCssClass;

                return (String)ViewState[CssClassViewStateKey];
            }
            set
            {
                ViewState[CssClassViewStateKey] = value;
            }
        }

        public String MenuName
        {
            get { return (String)ViewState[MenuNameViewStateKey]; }
            set { ViewState[MenuNameViewStateKey] = value; }
        }

        #endregion

        #region Rendering

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "sidebar");

            if (!String.IsNullOrEmpty(CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);

            writer.RenderBeginTag("div");

            if (!String.IsNullOrEmpty(MenuName))
            {
                String[] namesArray = MenuName.Split(';');

                foreach (String name in namesArray)
                {
                    RenderMenu(writer, name);
                }
            }

            if (IsIncludeContacts)
                RenderContactsMenu(writer, ContactsMenuTitle);

            writer.RenderEndTag();
        }

        private static void RenderMenu(HtmlTextWriter writer, String menuName)
        {
            List<SeminaryLinkMenuSection> sections = SeminaryLinkXml.Current.GetMenuItems(menuName);

            if (sections != null)
            {
                foreach (SeminaryLinkMenuSection section in sections)
                {
                    RenderMenuSection(writer, section);
                }
            }
        }

        private static void RenderMenuSection(HtmlTextWriter writer, SeminaryLinkMenuSection section)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, DefaultHeaderCssClass);
            writer.RenderBeginTag("div");
            writer.Write(section.Title);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, DefaultItemsContainerCssClass);
            writer.RenderBeginTag("div");

            if (!section.IsHtml)
            {
                writer.RenderBeginTag("ul");

                foreach (SeminaryLinkMenuItem item in section.Items)
                {
                    RenderMenuItem(writer, item);
                }

                writer.RenderEndTag(); // ul
            }
            else
            {
                writer.Write(section.InnerHtml);
            }

            writer.RenderEndTag(); // div
        }

        private static void RenderMenuItem(HtmlTextWriter writer, SeminaryLinkMenuItem item)
        {
            writer.RenderBeginTag("li");

            if (!String.IsNullOrEmpty(item.Href))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, item.Href);
                writer.AddAttribute(HtmlTextWriterAttribute.Title, item.Title);
                writer.RenderBeginTag("a");
                writer.Write(item.Title);
                writer.RenderEndTag(); // a
            }
            else
            {
                writer.Write(item.Title);
            }

            writer.RenderEndTag(); // li
        }

        private static void RenderContactsMenu(HtmlTextWriter writer, String title)
        {
            String menuItems = ContactFormXml.Current.GetMenuHtml();

            if (String.IsNullOrEmpty(menuItems))
                return;

            writer.AddAttribute(HtmlTextWriterAttribute.Class, DefaultHeaderCssClass);
            writer.RenderBeginTag("div");
            writer.Write(title);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, DefaultItemsContainerCssClass);
            writer.RenderBeginTag("div");
            writer.Write(menuItems);
            writer.RenderEndTag(); // div
        }

        #endregion
    }
}