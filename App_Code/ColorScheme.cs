using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace net.UyghurDev.Text
{
    /// <summary>
    /// كود رەڭلەش
    /// <para>ئەسلى مەنبەسى:http://www.codeplex.com/codecolorer</para>
    /// <para>ئۆزگەرتىپ تۈزگۈچى: سارۋان</para>
    /// <para>http://lab.uyghurdev.net/CodeColorer/</para>
    /// </summary>
   public  class CodeColorer
    {
     

        #region properties
       private struct Group
       {
           public Color color;
           public Color backColor;
           public int indexColor;
           public int indexBackColor;
           public String name;
           public List<String> expressions;
       };

        private List<Group> groups;
        private String name = "";
        public String Name
        {
            get
            {
                return name;
            }
        }
        private String extensions = "";
        public String Extensions
        {
            get
            {
                return extensions;
            }
        }

        private Color defaultColor = Color.Black;
        private Color defaultBackColor = Color.White;
        private int indexColor;
        private int indexBackColor;
        private String expression = "";
        private List<Color> colorTable;

        #endregion

        #region private methods
        private static String ReplaceChars(String text)
        {
            StringBuilder result = new StringBuilder(text);
            result = result.Replace("\\", "\\\\");
            result = result.Replace("*", "\\*");
            result = result.Replace("\'", "\\\'");
            result = result.Replace("\"", "\\\"");
            result = result.Replace("&", "\\&");
            result = result.Replace(".", "\\.");
            result = result.Replace("$", "\\$");
            result = result.Replace("^", "\\^");
            result = result.Replace("(", "\\(");
            result = result.Replace(")", "\\)");
            result = result.Replace("{", "\\{");
            result = result.Replace("}", "\\}");
            result = result.Replace("[", "\\[");
            result = result.Replace("]", "\\]");
            result = result.Replace("|", "\\|");
            result = result.Replace("+", "\\+");
            result = result.Replace("?", "\\?");
            result = result.Replace("#", "\\#");
            return result.ToString();
        }
        public static String MatchReplaceHtml(Match m)
        {
            if (m.Value == "&") return "&amp;";
            if (m.Value == "\'") return "&apos;";
            if (m.Value == "\"") return "&quot;";
            if (m.Value == "<") return "&lt;";
            if (m.Value == ">") return "&gt";
            if (m.Value == " ") return "&nbsp;";
            if (m.Value == "\t") return "&nbsp;&nbsp;&nbsp;&nbsp;";
            return "";
        }
        public static String MatchReplaceRtf(Match m)
        {
            if (m.Value == "\\") return "\\\\";
            if (m.Value == "{") return "\\{";
            if (m.Value == "}") return "\\}";
            if (m.Value == "\n") return "\\par\n";
            if (m.Value == "\t") return "\\tab ";
            return "";
        }
        private int AddColor(Color cl)
        {
            for (int i = 0; i < colorTable.Count; i++)
                if (colorTable[i] == cl) return i;
            colorTable.Add(cl);
            return (colorTable.Count - 1);
        }

        private static String ColorName(Color color)
        {
            String result = color.Name;
            if ((color.Name.Length == 8) && (color.Name[0] == 'f') && (color.Name[1] == 'f'))
                result = "#" + color.Name.Substring(2);
            return result;
        }
        #endregion

        public CodeColorer(String fileName)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(fileName, settings);
            reader.Read();
            groups = new List<Group>();
            name = reader.GetAttribute("name");
            extensions = reader.GetAttribute("extensions");
            colorTable = new List<Color>();
            Group currentGroup = new Group();
            currentGroup.name = "name";
            currentGroup.color = Color.Black;
            currentGroup.backColor = Color.White;
            currentGroup.expressions = new List<String>();
            while (reader.Read())
            {
                //read scheme attributes
                if ((reader.Name == "scheme") && (reader.IsStartElement()))
                {
                    name = reader.GetAttribute("name");
                    extensions = reader.GetAttribute("extension");
                }
                else
                    //read new group
                    if ((reader.Name == "group") && (reader.IsStartElement()))
                    {
                        String color = "";
                        String backColor = "";
                        if (reader.GetAttribute("name") == "Default")
                        {
                            color = reader.GetAttribute("color");
                            if (color[0] == '#')
                            {
                                int r = Convert.ToInt32(color.Substring(1, 2), 16);
                                int g = Convert.ToInt32(color.Substring(3, 2), 16);
                                int b = Convert.ToInt32(color.Substring(5, 2), 16);
                                defaultColor = Color.FromArgb(r, g, b);
                            }
                            else
                                defaultColor = Color.FromName(color);
                            indexColor = AddColor(defaultColor);
                            backColor = reader.GetAttribute("bgcolor");
                            if (backColor[0] == '#')
                            {
                                int r = Convert.ToInt32(backColor.Substring(1, 2), 16);
                                int g = Convert.ToInt32(backColor.Substring(3, 2), 16);
                                int b = Convert.ToInt32(backColor.Substring(5, 2), 16);
                                defaultBackColor = Color.FromArgb(r, g, b);
                            }
                            else
                                defaultBackColor = Color.FromName(backColor);
                            indexBackColor = AddColor(defaultBackColor);
                            continue;
                        }
                        currentGroup = new Group();
                        currentGroup.name = reader.GetAttribute("name");
                        color = reader.GetAttribute("color");
                        if (color[0] == '#')
                        {
                            int r = Convert.ToInt32(color.Substring(1, 2), 16);
                            int g = Convert.ToInt32(color.Substring(3, 2), 16);
                            int b = Convert.ToInt32(color.Substring(5, 2), 16);
                            currentGroup.color = Color.FromArgb(r, g, b);
                        }
                        else
                            currentGroup.color = Color.FromName(color);
                        backColor = reader.GetAttribute("bgcolor");
                        if (backColor[0] == '#')
                        {
                            int r = Convert.ToInt32(backColor.Substring(1, 2), 16);
                            int g = Convert.ToInt32(backColor.Substring(3, 2), 16);
                            int b = Convert.ToInt32(backColor.Substring(5, 2), 16);
                            currentGroup.backColor = Color.FromArgb(r, g, b);
                        }
                        else
                            currentGroup.backColor = Color.FromName(backColor);
                        currentGroup.indexColor = AddColor(currentGroup.color);
                        currentGroup.indexBackColor = AddColor(currentGroup.backColor);
                        currentGroup.expressions = new List<string>();
                        groups.Add(currentGroup);
                    }
                    //read regex for currentGroup    
                    else
                        if ((reader.Name == "regex") && (reader.IsStartElement()))
                        {
                            currentGroup.expressions.Add(reader.GetAttribute("value"));
                            String exp = reader.ReadString();
                            expression += "|" + exp;
                        }
                        //read word for currentGroup
                        else
                            if ((reader.Name == "word") && (reader.IsStartElement()))
                            {
                                bool anySize = Convert.ToBoolean(reader.GetAttribute("anysize"), CultureInfo.CurrentCulture);
                                String exp = ReplaceChars(reader.ReadString());
                                if (anySize)
                                    exp = "(?i:" + exp + ")";
                                currentGroup.expressions.Add(exp);
                                exp = "(?<=(\\W))" + "(" + exp + "(?=(\\W)))";
                                expression += "|" + exp;
                            }
                            //read bracket for currentGroup
                            else
                                if (reader.Name == "bracket")
                                {
                                    String start = ReplaceChars(reader.GetAttribute("start"));
                                    String end = ReplaceChars(reader.GetAttribute("end"));
                                    String carry = ReplaceChars(reader.GetAttribute("carry"));
                                    bool onlyBegin = Convert.ToBoolean(reader.GetAttribute("onlybegin"), CultureInfo.CurrentCulture);
                                    String exp = "";
                                    if ((end == "\\$none") && (carry == "\\$none"))
                                    {
                                        exp = start + ".*";
                                        exp = "(" + exp + ")";
                                        if (onlyBegin)
                                            exp = "(^(\\s)*?" + exp + ")";
                                    }
                                    else
                                        if ((end != "\\$none") && (carry == "\\$any"))
                                        {
                                            exp = start + "((.|\n)*?)" + end;
                                            exp = "(" + exp + ")";
                                            if (onlyBegin)
                                                exp = "(^(\\s)*?" + exp + ")";
                                        }
                                        else
                                            if ((end != "\\$none") && (carry != "\\$any") && (carry != "\\$none"))
                                            {
                                                exp = start + "((.|(" + carry + "\n))*?)" + end;
                                                exp = "(" + exp + ")";
                                                if (onlyBegin)
                                                    exp = "(^(\\s)*?" + exp + ")";
                                            }
                                            else
                                                if ((end == "\\$none") && (carry != "\\$any") && (carry != "\\$none"))
                                                {
                                                    exp = start + "((.|(" + carry + "\n))*)";
                                                    exp = "(" + exp + ")";
                                                    if (onlyBegin)
                                                        exp = "(^(\\s)*?" + exp + ")";
                                                }
                                                else continue;//TODO: Error

                                    currentGroup.expressions.Add(exp);
                                    expression += "|" + exp;
                                }
                                else
                                {
                                    //TODO: Error adnako!!!!
                                }
            }
            expression = expression.Substring(1, expression.Length - 1);
        }
        ~CodeColorer()
        {
            groups.Clear();
            name = "";
            extensions = "";
            expression = "";
        }

        #region public methods

        /// <summary>
        /// RTF كە ئايلاندۇرۇش
        /// </summary>
        /// <param name="strCode">كود</param>
        /// <returns>RTF تېكست</returns>
        public string ConvertRtf(string strCode)
        {
            MatchEvaluator myEvaluatorRtf = new MatchEvaluator(MatchReplaceRtf);
            string result = strCode;
            StringBuilder resultRtf = new StringBuilder(result.Length * 2);
            //start filling rtf header
            resultRtf.Append("{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1049{\\fonttbl{\\f0\\fnil\\fcharset204{\\*\\fname Courier New;}Courier New CYR;}}\n");
            //fills rtf color table
            resultRtf.Append("{\\colortbl ");
            for (int i = 0; i < colorTable.Count; i++)
            {
                resultRtf.Append("\\red");
                resultRtf.Append(colorTable[i].R);
                resultRtf.Append("\\green");
                resultRtf.Append(colorTable[i].G);
                resultRtf.Append("\\blue");
                resultRtf.Append(colorTable[i].B);
                resultRtf.Append(";");
            }
            resultRtf.Append("}\n");
            //starts
            resultRtf.Append("\\viewkind4\\uc1\\pard\\f0\\fs20 ");
            int index = 0;
            MatchCollection matches = Regex.Matches("\n" + result, expression);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                bool final = false;
                if (!match.Success) continue;
                int currentColor = indexColor;
                int currentBackColor = indexBackColor;
                foreach (Group group in groups)
                {
                    foreach (String pattern in group.expressions)
                    {
                        Match resultMatch = Regex.Match(match.Value, pattern);
                        if (resultMatch.Success)
                            if (resultMatch.Index == 0)
                            {
                                currentColor = group.indexColor;
                                currentBackColor = group.indexBackColor;
                                final = true;
                                break;
                            }
                        if (final) break;
                    }
                    if (final) break;
                }
                if (!final)
                {
                    //Error adnako!!!
                }
                if (match.Index > index)
                {
                    resultRtf.Append("\\cf");
                    resultRtf.Append(indexColor);
                    resultRtf.Append("\\highlight");
                    resultRtf.Append(indexBackColor);
                    resultRtf.Append(" ");
                    resultRtf.Append(Regex.Replace((result.Substring(index, match.Index - index - 1)), "\\\\|\\{|\\}|\\n|\\t", myEvaluatorRtf));
                }
                resultRtf.Append("\\cf");
                resultRtf.Append(currentColor);
                resultRtf.Append("\\highlight");
                resultRtf.Append(currentBackColor);
                resultRtf.Append(" ");
                resultRtf.Append(Regex.Replace((result.Substring(match.Index - 1, match.Length)), "\\\\|\\{|\\}|\\n|\\t", myEvaluatorRtf));
                index = match.Index + match.Length - 1;
            }
            if (result.Length > index)
            {
                resultRtf.Append("\\cf");
                resultRtf.Append(indexColor);
                resultRtf.Append("\\highlight");
                resultRtf.Append(indexBackColor);
                resultRtf.Append(" ");
                resultRtf.Append(Regex.Replace((result.Substring(index, result.Length - index)), "\\\\|\\{|\\}|\\n|\\t", myEvaluatorRtf));
            }
            resultRtf.Append("}");
            return resultRtf.ToString();
        }

        /// <summary>
        /// HTML گە ئايلاندۇرۇش
        /// </summary>
        /// <param name="codeText">كود تېكىستى</param>
        /// <returns>HTML تېكست</returns>
        public String ConvertHtml( String codeText)
        {
            MatchEvaluator myEvaluatorHtml = new MatchEvaluator(MatchReplaceHtml);
            //fills begin
            String result = codeText;
            StringBuilder resultHtml = new StringBuilder(result.Length * 2);
            //convert to html
            resultHtml.Append("<html><head><title>");
            resultHtml.Append("");
            resultHtml.Append(" - Generated by Raven`s CodeColorer.NET");
            resultHtml.Append("</title></head><body><table width = \"100%\"><tr><td><pre><code>");
            resultHtml.Append("<span style=");
            resultHtml.Append("\"color: ");
            resultHtml.Append(ColorName(defaultColor));
            resultHtml.Append("; background-color: ");
            resultHtml.Append(ColorName(defaultBackColor));
            resultHtml.Append("\"");
            resultHtml.Append(">");
            //
            int index = 0;
            MatchCollection matches = Regex.Matches("\n" + result, expression);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                bool final = false;
                if (!match.Success) continue;
                int currentColor = indexColor;
                int currentBackColor = indexBackColor;
                foreach (Group group in groups)
                {
                    foreach (String pattern in group.expressions)
                    {
                        Match resultMatch = Regex.Match(match.Value, pattern);
                        if (resultMatch.Success)
                            if (resultMatch.Index == 0)
                            {
                                currentColor = group.indexColor;
                                currentBackColor = group.indexBackColor;
                                final = true;
                                break;
                            }
                        if (final) break;
                    }
                    if (final) break;
                }
                if (!final)
                {
                    //Error adnako!!!
                }
                if (match.Index > index)
                {
                    resultHtml.Append((Regex.Replace((result.Substring(index, match.Index - index - 1)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
                }
                resultHtml.Append("<span style=");
                resultHtml.Append("\"color: ");
                resultHtml.Append(ColorName(colorTable[currentColor]));
                resultHtml.Append("; background-color: ");
                resultHtml.Append(ColorName(colorTable[currentBackColor]));
                resultHtml.Append("\"");
                resultHtml.Append(">");
                resultHtml.Append((Regex.Replace((result.Substring(match.Index - 1, match.Length)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
                resultHtml.Append("</span>");
                index = match.Index + match.Length - 1;
            }
            if (result.Length > index)
            {
                resultHtml.Append((Regex.Replace((result.Substring(index, result.Length - index)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
            }
            resultHtml.Append("</span></code></pre></td></tr></table></body></html>");
            return resultHtml.ToString();
        }


        /// <summary>
        /// BB كودقا ئايلاندۇرۇش
        /// </summary>
        /// <param name="codeText">كود تېكىستى</param>
        /// <returns>BB تېكىستى</returns>
        public String ConvertBbCodes(string codeText)
        {
            //fills begin
            String result = codeText;
            StringBuilder resultHtml = new StringBuilder(result.Length * 2);
            //convert to html
            resultHtml.Append("[code]");
            resultHtml.Append("[color=");
            resultHtml.Append(ColorName(defaultColor));
            resultHtml.Append("]");
            //
            int index = 0;
            MatchCollection matches = Regex.Matches("\n" + result, expression);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                bool final = false;
                if (!match.Success) continue;
                int currentColor = indexColor;
                int currentBackColor = indexBackColor;
                foreach (Group group in groups)
                {
                    foreach (String pattern in group.expressions)
                    {
                        Match resultMatch = Regex.Match(match.Value, pattern);
                        if (resultMatch.Success)
                            if (resultMatch.Index == 0)
                            {
                                currentColor = group.indexColor;
                                currentBackColor = group.indexBackColor;
                                final = true;
                                break;
                            }
                        if (final) break;
                    }
                    if (final) break;
                }
                if (!final)
                {
                    //Error adnako!!!
                }
                if (match.Index > index)
                {
                    resultHtml.Append(result.Substring(index, match.Index - index - 1));
                }
                resultHtml.Append("[color=");
                resultHtml.Append(ColorName(colorTable[currentColor]));
                resultHtml.Append("]");
                resultHtml.Append(result.Substring(match.Index - 1, match.Length));
                resultHtml.Append("[/color]");
                index = match.Index + match.Length - 1;
            }
            if (result.Length > index)
            {
                resultHtml.Append(result.Substring(index, result.Length - index));
            }
            resultHtml.Append("[/color][/code]");
            return resultHtml.ToString();
        }

       /// <summary>
       /// ئاددى HTMLگە ئايلاندۇرۇش
       /// </summary>
       /// <param name="codeText">كود تېكىستى</param>
        /// <returns>HTML تېكىستى</returns>
        public string ConvertHtmlSimple(string codeText)
        {
            MatchEvaluator myEvaluatorHtml = new MatchEvaluator(MatchReplaceHtml);
            //fills begin
            String result = codeText;
            StringBuilder resultHtml = new StringBuilder(result.Length * 2);
            //convert to html
            
            resultHtml.Append("<table width = \"100%\"><tr><td><pre><code>");
            resultHtml.Append("<span style=");
            resultHtml.Append("\"color: ");
            resultHtml.Append(ColorName(defaultColor));
            resultHtml.Append("; background-color: ");
            resultHtml.Append(ColorName(defaultBackColor));
            resultHtml.Append("\"");
            resultHtml.Append(">");
            //
            int index = 0;
            MatchCollection matches = Regex.Matches("\n" + result, expression);
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                bool final = false;
                if (!match.Success) continue;
                int currentColor = indexColor;
                int currentBackColor = indexBackColor;
                foreach (Group group in groups)
                {
                    foreach (String pattern in group.expressions)
                    {
                        Match resultMatch = Regex.Match(match.Value, pattern);
                        if (resultMatch.Success)
                            if (resultMatch.Index == 0)
                            {
                                currentColor = group.indexColor;
                                currentBackColor = group.indexBackColor;
                                final = true;
                                break;
                            }
                        if (final) break;
                    }
                    if (final) break;
                }
                if (!final)
                {
                    //Error adnako!!!
                }
                if (match.Index > index)
                {
                    resultHtml.Append((Regex.Replace((result.Substring(index, match.Index - index - 1)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
                }
                resultHtml.Append("<span style=");
                resultHtml.Append("\"color: ");
                resultHtml.Append(ColorName(colorTable[currentColor]));
                resultHtml.Append("; background-color: ");
                resultHtml.Append(ColorName(colorTable[currentBackColor]));
                resultHtml.Append("\"");
                resultHtml.Append(">");
                resultHtml.Append((Regex.Replace((result.Substring(match.Index - 1, match.Length)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
                resultHtml.Append("</span>");
                index = match.Index + match.Length - 1;
            }
            if (result.Length > index)
            {
                resultHtml.Append((Regex.Replace((result.Substring(index, result.Length - index)), "\\&|\\\'|\\\"|<|>| |\\t", myEvaluatorHtml)));
            }
            resultHtml.Append("</span></code></pre></td></tr></table>");
            return resultHtml.ToString();
        }

        #endregion


    }
}
