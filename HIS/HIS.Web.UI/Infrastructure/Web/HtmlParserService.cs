using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HIS.Web.UI 
{
    public static class HtmlParserService
    {
        /// <summary>
        /// Takes HTML markup locates any table definition held within it and returns that
        /// markup with the table HTML pivotted
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string PivotHtmlTableMarkup(string html)
        {
            html = ReplaceShorthandTableTags(html);

            int openingTableTagIndex;
            string openingTableTagText;
            int closingTableTagIndex;
            string tableContentText;

            tableContentText = GetTagContentText("table", html, out openingTableTagIndex, out openingTableTagText, out closingTableTagIndex);

            MatchCollection rows = GetTagMatches("tr", tableContentText);
            if (rows.Count > 0)
            {
                MatchCollection columns = GetTagMatches("(td|th)", rows[0].Value);

                StringBuilder pivottedTableMarkup = new StringBuilder();

                for (int i = 0; i < columns.Count; i++)
                {
                    pivottedTableMarkup.Append("<tr>");
                    foreach (Match row in rows)
                    {
                        if (row.Value.Length > 0)
                        {
                            columns = GetTagMatches("(td|th)", row.Value);

                            if (columns.Count > i)
                            {
                                pivottedTableMarkup.Append(columns[i].Value);
                            }
                        }
                    }
                    pivottedTableMarkup.Append("</tr>");
                }

                string preTableText = "";
                if (openingTableTagIndex > 1)
                {
                    preTableText = html.Substring(1, openingTableTagIndex);
                }

                string postTableText;
                postTableText = html.Substring(closingTableTagIndex, html.Length - closingTableTagIndex);

                string newHtmlWithPivottedTable;
                newHtmlWithPivottedTable = preTableText + openingTableTagText + pivottedTableMarkup.ToString() + postTableText;

                return newHtmlWithPivottedTable;
            }
            else
            {
                return html;
            }

        }

        /// <summary>
        /// Gets the content between the specified tag.
        /// </summary>
        /// <param name="tag">The tag excluding any markup (e.g. "table" not "<table>"</param>
        /// <param name="text">The xml text string to extract content from</param>
        /// <param name="openingTagIndex">Outputs the indexed position of the opening tag</param>
        /// <param name="openingTagText">Outputs the definition of the tag, e.g. it's attributes etc</param>
        /// <param name="closingTagIndex">Outputs the indexed position of the closing tag</param>
        /// <returns></returns>
        public static string GetTagContentText(string tag, string text, out int openingTagIndex, out string openingTagText, out int closingTagIndex)
        {
            string contentText;

            if (String.IsNullOrEmpty(text))
            {

                openingTagIndex = 0;
                openingTagText = "";
                closingTagIndex = 0;

                return "";
            }

            openingTagIndex = text.ToLower().IndexOf("<" + tag);
            openingTagText = text.Substring(openingTagIndex, text.IndexOf(">", openingTagIndex) - openingTagIndex + 1);
            closingTagIndex = text.ToLower().LastIndexOf("</" + tag + ">");

            contentText = text.Substring(
                openingTagIndex + openingTagText.Length,
                closingTagIndex - (openingTagIndex + openingTagText.Length));

            return contentText;
        }

        /// <summary>
        /// Returns a collection of matches containing the content of each matched tag
        /// </summary>
        /// <param name="tag">HTML tag to match.  Exclude opening and close angled braces.
        /// Multiple tags can be matched by specifying them in the following format (tag1|tag2),
        /// e.g. (td|th)</param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MatchCollection GetTagMatches(string tag, string html)
        {
            Regex regexp = new Regex(@"<" + tag + @"\b[^>]*>(.*?)</" + tag + @">", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return regexp.Matches(html);
        }

        /// <summary>
        /// Ensures any shorthand XML tags are full expressed, e.g.
        /// <td/> is converted to <td></td>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ReplaceShorthandTableTags(string value)
        {
            value = value.Replace("<tr/>", "<tr></tr>");
            value = value.Replace("<td/>", "<td></td>");
            value = value.Replace("<th/>", "<th></th>");

            return value;
        }


    }
}