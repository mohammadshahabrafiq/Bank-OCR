using System;
using System.Text;

namespace BankOCR.Objects
{
    public class Parser
    {
        private const string InvalidCharacter = "?";

        private readonly string[] _digits;

        public Parser(string[] digits)
        {
            _digits = digits;
        }

        public string Parse(string entry)
        {
            const int notFound = -1;
            const int digitLength = 3;
            const int fileEntryLength = 27;
            const int accountNumberLength = 9;

            var outputBuilder = new StringBuilder();

            for (int accountNumberIndex = 0; accountNumberIndex < accountNumberLength; accountNumberIndex++)
            {
                var digitBuilder = new StringBuilder();

                for (int digitIndex = 0; digitIndex < digitLength; digitIndex++)
                {
                    digitBuilder.Append(entry.Substring((accountNumberIndex * digitLength) + (fileEntryLength * digitIndex), digitLength));
                }

                var digit = Array.IndexOf(_digits, digitBuilder.ToString());

                outputBuilder.Append(digit == notFound ? InvalidCharacter : digit.ToString());
            }

            return outputBuilder.ToString();
        }
    }
}
