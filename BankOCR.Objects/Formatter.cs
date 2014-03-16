using System;
using System.Linq;

namespace BankOCR.Objects
{
    public class Formatter
    {
        public string Output(string output, string[] suggestions)
        {
            if (suggestions.Length == 0)
            {
                return string.Format("{0} ILL", output);
            }

            if (suggestions.Length == 1)
            {
                return suggestions.First();
            }

            Array.Sort(suggestions);

            return string.Format("{0} AMB ['{1}']", output, string.Join("', '", suggestions));
        }
    }
}
