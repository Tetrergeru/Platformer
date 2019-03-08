using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Platformer.Files
{
    internal class ParseResults
    {
        private Dictionary<string, object> Variables;
    }

    internal static class Parser
    {
        public static ParseResults Parse(string fname)
        {
            Regex r = new Regex(@"([a-zA-Z0-1_]+)( )*[=|+=](.+)");
            var m = r.Match(fname);
            Console.WriteLine(m.Groups[0].ToString());
            return null;
        }
    }
}