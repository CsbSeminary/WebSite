using System;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

using Csbs.Data;
using Csbs.Data.Pages;

namespace Csbs.Utilities
{
    public static class RenderHelper
    {
        #region Head

        public static void WriteCssLinkDeclaration(StringBuilder sb, String relativePath)
        {
            sb.AppendFormat("<link rel='Stylesheet' type='text/css' href='{0}' />", LinkUtils.ResolveClientUrl(relativePath));
        }

        public static void WriteMetaDeclaration(StringBuilder sb, String name, String content)
        {
            sb.AppendFormat("<meta name='{0}' content='{1}' />", name, content);
        }

        #endregion

        #region CSS

        public static void WritePageHeadContent(StringBuilder sb, String masterPage, String themeName)
        {
            WritePageHeadContent(sb, masterPage, themeName, null, null);
        }

        public static void WritePageHeadContent(StringBuilder sb, String masterPage, String themeName, String metaDescription, String metaKeywords)
        {
            RenderHelper.WriteCssLinkDeclaration(sb, LinkUtils.GetThemeCssFilePath(masterPage));

            if (!String.IsNullOrEmpty(themeName))
                RenderHelper.WriteCssLinkDeclaration(sb, LinkUtils.GetThemeCssFilePath(masterPage, themeName));

            if (!String.IsNullOrEmpty(metaDescription))
                RenderHelper.WriteMetaDeclaration(sb, "description", metaDescription);

            if (!String.IsNullOrEmpty(metaKeywords))
                RenderHelper.WriteMetaDeclaration(sb, "keywords", metaKeywords);
        }

        #endregion

        #region HTML

        public static void WritePageHtmlContent(StringBuilder sb, PageInfo pageInfo)
        {
            if (pageInfo.MenuPosition == MenuPositionType.Top)
                MenuManager.Current.Render(sb, pageInfo.GetMenuItems(), pageInfo, null, MenuRenderingType.Horizontal);

            if (pageInfo.HeaderPosition == ContentHeaderPositionType.TopOfPage)
                RenderHeader(sb, pageInfo, true);

            String mainContent = pageInfo.ContentColumnHtml;

            if (pageInfo.ContentLayout == ContentLayoutType.OneColumn)
            {
                sb.Append("<div id='csbs_pageContent' class='page-content'>");
                sb.Append(mainContent);
                sb.Append("</div>");
            }
            else if (pageInfo.ContentLayout == ContentLayoutType.TwoColumn)
            {
                String contentColumnSize = pageInfo.SideColumnSize == ContentLayoutSideColumnSize.Large
                    ? " content-small"
                    : pageInfo.SideColumnSize == ContentLayoutSideColumnSize.Small
                        ? " content-large"
                        : " content-medium";
                String contentPositionClass = pageInfo.ContentPostion == ContentPositionType.Left
                    ? " content-left"
                    : " content-right";
                String separatorClass = pageInfo.ShowColumnsSeparator
                    ? " border-separator"
                    : String.Empty;

                sb.AppendFormat("<div id='csbs_sidePanel' class='page-side-content{0}{1}{2}'>", contentColumnSize, separatorClass, contentPositionClass);

                if (pageInfo.MenuPosition == MenuPositionType.SidePanel)
                    MenuManager.Current.Render(sb, pageInfo.GetMenuItems(), pageInfo, null, MenuRenderingType.Vertical);

                sb.Append(pageInfo.SideColumnHtml);
                sb.Append("</div>");

                sb.AppendFormat("<div id='csbs_pageContent' class='page-content{0}{1}'>", contentColumnSize, contentPositionClass);

                if (pageInfo.HeaderPosition == ContentHeaderPositionType.TopOfContent)
                    RenderHeader(sb, pageInfo, false);

                sb.Append(mainContent);
                sb.Append("</div>");
            }
        }

        private static void RenderHeader(StringBuilder sb, PageInfo pageInfo, Boolean topOfPage)
        {
            if (pageInfo.HeaderLayout == ContentHeaderLayoutType.Template)
            {
                sb.Append("<div id='csbs_contentHeaderTitle'>");
                sb.Append(pageInfo.HeaderTemplateHtml);
                sb.Append("</div>");
            }
            else if (pageInfo.HeaderLayout == ContentHeaderLayoutType.Text)
            {
                sb.AppendFormat("<div id='csbs_contentHeaderTitle' class='header-title-{0}'>", topOfPage ? "page" : "content");

                if (pageInfo.HeaderLinkVisible)
                {
                    sb.Append("<div class='link-right'>");
                    sb.AppendFormat("<a id='csbs_contentHeaderLink' href='{0}' target='{1}'>{2}</a>", pageInfo.HeaderLinkUrl, pageInfo.HeaderLinkTarget, pageInfo.HeaderLinkText);
                    sb.Append("</div>");
                }

                sb.Append(pageInfo.HeaderTitle);

                if (topOfPage)
                    sb.Append("<div style='margin-top:2px;' class='separator-black'></div>");

                sb.Append("</div>");
            }
        }

        #endregion
    }
}