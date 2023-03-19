using System.Text.RegularExpressions;

namespace CsvConverter
{
    public class RegexHelper
    {
        public static string[] RecognizeModelnumberPattern(string input)
        {
            Dictionary<int, Regex> regexMap = RegexMap();
            int type = 0;

            for (int i = 1; i <= regexMap.Count; i++)
            {

                if (regexMap[i].IsMatch(input))
                {
                    type = i;
                    break;
                }
            }

            return StringFormatter(input, type);
        }



        public static Dictionary<int, Regex> RegexMap()
        {
            /* RegEx for each type of format */
            Regex type1 = new("Nr:\\s*\\d{1,10}\\s*/\\s*\\d{1,10}\\s*-"); // Nr: 1234567890/1234567890 - Modelname
            Regex type2 = new("Nr:\\s*\\d{1,10}\\s*/\\s*\\d{1,10}\\s*"); // Nr: 1234567890/1234567890 Modelname
            Regex type3 = new("Nr:\\s*\\d{1,10}\\s*-"); // "Nr: 12345567890 - Modelname
            Regex type4 = new("Nr:\\s*\\d{1,10}"); // Nr: 1234567890 Modelname
            Regex type5 = new("Nr:\\s*\\d{1,10}\\w+"); // Nr: 1234567890a Modelname
            Regex type6 = new("\\A\\d{1,10}[^-][^stk]\\s*[\\w]+"); // 1234567890 Modelname UDEN 'stk'
            Regex type7 = new("Årgang"); // Årgang
            Regex type8 = new("År\\s*[:]?\\s*\\d{1,10}"); // År 1958 eller År: 1958


            return new() {
            { 1, type1 }, { 2, type2 }, { 3, type3 }, { 4, type4 },
            { 5, type5 }, {6, type6}, { 7, type7 }, { 8, type8 }
        };
        }


        public static string[] StringFormatter(string input, int type)
        {
            List<string> result = new();
            switch (type)
            {
                case 1: // Nr: 1234567890/1234567890 - Modelname
                    {
                        string trimInput = input[(input.IndexOf("Nr:") + 3)..];
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, trimInput.IndexOf('-')));
                        string name = trimInput.Substring(trimInput.IndexOf('-') + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }
                case 2: // Nr: 1234567890/1234567890 Modelname
                    {
                        string trimInput = input[(input.IndexOf("Nr:") + 3)..].Trim();
                        int indexOfSeparator = trimInput.IndexOf(' ');
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, indexOfSeparator));
                        string name = trimInput.Substring(indexOfSeparator + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }
                case 3: // "Nr: 12345567890 - Modelname
                    {
                        string trimInput = input[(input.IndexOf("Nr:") + 3)..];
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, trimInput.IndexOf('-')));
                        string name = trimInput.Substring(trimInput.IndexOf('-') + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }

                case 4: // Nr: 1234567890 Modelnames
                    {
                        string trimInput = input[(input.IndexOf("Nr:") + 3)..].Trim();
                        int indexOfSeparator = trimInput.IndexOf(' ');
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, indexOfSeparator));
                        string name = trimInput.Substring(indexOfSeparator + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }
                case 5: // Nr: 1234567890a Modelname
                    {
                        string trimInput = input[(input.IndexOf("Nr:") + 3)..].Trim();
                        int indexOfSeparator = trimInput.IndexOf(' ');
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, indexOfSeparator));
                        string name = trimInput.Substring(indexOfSeparator + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }
                case 6: // 1234567890 Modelname UDEN 'stk'
                    {
                        string trimInput = input.Trim();
                        int indexOfSeparator = trimInput.IndexOf(' ');
                        string modelNumber = TrimAllWithInplaceCharArray(trimInput.Substring(0, indexOfSeparator));
                        string name = trimInput.Substring(indexOfSeparator + 1).Trim();
                        result.Add(modelNumber);
                        result.Add(name);
                        break;
                    }
                case 7: // Årgang
                    {
                        // TODO
                        result.Add("");
                        result.Add("");
                        break;
                    }
                case 8: // År 1958 eller År: 1958
                    {
                        // TODO
                        result.Add("");
                        result.Add("");
                        break;
                    }
                default:
                    result.Add("");
                    result.Add(input);
                    break;
            }
            return result.ToArray();
        }

        public static string TrimAllWithInplaceCharArray(string str)
        {

            var len = str.Length;
            var src = str.ToCharArray();
            int dstIdx = 0;

            for (int i = 0; i < len; i++)
            {
                var ch = src[i];

                switch (ch)
                {

                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':

                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':

                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':

                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':

                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;

                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }
            return new string(src, 0, dstIdx);
        }


        public static void TestRegexFilter(string[] inputs, Dictionary<int, Regex> regexMap)
        {
            int matchedInputcount = 0;
            List<string> failedMatches = new();
            Dictionary<int, List<string>> matches = new();

            Dictionary<int, int> typeCount = new();
            for (int i = 1; i <= regexMap.Count; i++)
            {
                typeCount.Add(i, 0);
                matches.Add(i, new List<string>());

            }

            foreach (string input in inputs)
            {
                bool match = false;
                for (int i = 1; i <= regexMap.Count; i++)
                {

                    if (regexMap[i].IsMatch(input))
                    {
                        matches[i].Add(input);
                        typeCount[i]++;
                        matchedInputcount++;
                        match = true;
                        break;
                    }
                }
                if (!match) failedMatches.Add(input);
            }

            foreach (var match in matches)
            {
                Console.WriteLine("Type {0}:", match.Key);
                foreach (var input in match.Value)
                {
                    Console.WriteLine(input);
                }
                Console.WriteLine("\n\n\n");
            }

            Console.WriteLine("Failed matches: ");
            foreach (string input in failedMatches)
            {
                Console.WriteLine(input);
            }

            Console.WriteLine("\nMatched {0}/{1} strings", matchedInputcount, inputs.Length);
            Console.WriteLine("Failed to match {0}\n", failedMatches.Count);

            for (int i = 1; i <= regexMap.Count; i++)
            {
                int countForTypeI = typeCount.GetValueOrDefault(i); ;
                Console.WriteLine("Type{0} amount: {1}", i, countForTypeI);
            }

        }
    }
}
