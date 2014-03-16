using System;
using System.Collections.Generic;
using System.Text;

namespace BankOCR.Objects
{
    public class Suggester
    {
        private readonly Validator _validator;
        private readonly IDictionary<char, char[]> _engine;

        public Suggester(Validator validator, IDictionary<char, char[]> engine)
        {
            _engine = engine;
            _validator = validator;
        }

        public string[] Suggestions(string entry)
        {
            string output;
            var suggestions = new List<string>();

            Suggest(entry, suggestion =>
            {
                if (_validator.IsValid(suggestion, out output))
                {
                    suggestions.Add(output);
                }
            });

            return suggestions.ToArray();
        }

        private void Suggest(string entry, Action<string> suggestion)
        {
            for (int index = 0; index < entry.Length; index++)
            {
                var alternate = new StringBuilder(entry);

                foreach (var character in _engine[entry[index]])
                {
                    alternate[index] = character;
                    suggestion(alternate.ToString());
                }
            }
        }
    }
}
