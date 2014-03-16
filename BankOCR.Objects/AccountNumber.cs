
namespace BankOCR.Objects
{
    public class AccountNumber
    {
        private readonly string _entry;
        private readonly Suggester _suggestor;
        private readonly Validator _validator;
        private readonly Formatter _formatter;

        public AccountNumber(Validator validator, Suggester suggester, Formatter formatter, string entry)
        {
            _entry = entry;
            _suggestor = suggester;
            _validator = validator;
            _formatter = formatter;
        }

        public override string ToString()
        {
            string output;

            if (_validator.IsValid(_entry, out output))
            {
                return output;
            }

            return _formatter.Output(output, _suggestor.Suggestions(_entry));
        }
    }
}
