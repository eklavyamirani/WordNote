using System;
using System.Text.RegularExpressions;

public static class HTMLTagsRemover
{
    public static string StripTagsRegex(string source)
    {
        return Regex.Replace(source, "<*?>", string.Empty);
    }
    /// <summary>
    /// Retrieves the content from the body of an html webpage.
    /// </summary>
    /// <param name="source">The text from the source website</param>
    /// <returns>Content from the body of an html webpage</returns>
    public static string StripTagsRegexBody(string source)
    {
        Match test = Regex.Match(source, "body");
        char[] chararray = new char[source.Length];
        int ArrayIndex = 0;
        char let = new char();
        bool inside = false;
        bool body = false;
        for (int i = 0; i < source.Length; i++)
        {
            let = source[i];
            if (let == '<')
            {

                if (test.Index == i + 1)
                {
                    body = true;
                    test=test.NextMatch();
                }
                else if (test.Index == i + 2)
                {
                    body = false;
                    test = test.NextMatch();
                }
                inside = true;
                continue;
            }
            else if (let == '>')
            {
                inside = false;
                continue;
            }
            if (!inside&&body)
            {
                chararray[ArrayIndex++] = let;
            }
        }
        return new string(chararray,0,ArrayIndex);
    }
}
