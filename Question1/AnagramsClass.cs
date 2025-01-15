using System.Linq;
using System.Text.RegularExpressions;
namespace Question1
{
    public static class AnagramsClass
    {
        public static bool Anagrams(string s1, string s2)
        {
            var charPattern = @"[a-zA-Z]+";
            var matches1 = Regex.Matches(s1, charPattern);
            var matches2 = Regex.Matches(s2, charPattern);
            var s1Chars = matches1?.OrderBy(x => x.Value)?.Select(x => x.Value)?.Aggregate("", (carry, next) => carry+ next) ?? string.Empty;
            var s2Chars = matches2?.OrderBy(x => x.Value).Select(x => x.Value)?.Aggregate("", (carry, next) => carry + next) ?? string.Empty;
            return s1Chars.Count() == s2Chars.Count() && s1Chars.Select(x => s2Chars.Contains(x, StringComparison.OrdinalIgnoreCase)).Aggregate(true, (carry, next) => carry && next );
           
        }

    }
}
