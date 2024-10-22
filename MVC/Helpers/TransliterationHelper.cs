using System.Text;

namespace MVC.Helpers
{
    public static class TransliterationHelper
    {
        private static readonly Dictionary<char, string> CyrillicToLatin = new Dictionary<char, string>
    {
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
        {'е', "ye"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
        {'й', "j"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
        {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
        {'у', "u"}, {'ф', "f"}, {'х', "h"}, {'ц', "ts"}, {'ч', "ch"},
        {'ш', "sh"}, {'щ', "shch"}, {'ъ', ""}, {'ы', "y"}, {'ь', "_"},
        {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
    };

        private static readonly Dictionary<string, char> LatinToCyrillic = new Dictionary<string, char>
    {
        {"a", 'а'}, {"b", 'б'}, {"v", 'в'}, {"g", 'г'}, {"d", 'д'},
        {"ye", 'е'}, {"yo", 'ё'}, {"zh", 'ж'}, {"z", 'з'}, {"i", 'и'},
        {"j", 'й'}, {"k", 'к'}, {"l", 'л'}, {"m", 'м'}, {"n", 'н'},
        {"o", 'о'}, {"p", 'п'}, {"r", 'р'}, {"s", 'с'}, {"t", 'т'},
        {"u", 'у'}, {"f", 'ф'}, {"h", 'х'}, {"ts", 'ц'}, {"ch", 'ч'},
        {"sh", 'ш'}, {"shch", 'щ'}, {"y", 'ы'},{"_", 'ь'}, {"e", 'э'}, {"yu", 'ю'}, {"ya", 'я'}
    };

        public static string ToLatin(string input)
        {
            input = input.ToLower();
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = new StringBuilder();

            foreach (var ch in input)
            {
                if (CyrillicToLatin.TryGetValue(ch, out var translitChar))
                {
                    result.Append(translitChar);
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString().ToLowerInvariant();
        }

        public static string ToCyrillic(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (i < input.Length - 1 && LatinToCyrillic.ContainsKey(input.Substring(i, 2)))
                {
                    result.Append(LatinToCyrillic[input.Substring(i, 2)]);
                    i++;
                }
                else if (LatinToCyrillic.TryGetValue(input[i].ToString(), out var translitChar))
                {
                    result.Append(translitChar);
                }
                else
                {
                    result.Append(input[i]);
                }
            }

            return result.ToString();
        }
    }

}
