
namespace BankOCR.Objects
{
    public class Validator
    {
        private readonly Parser _parser;
        private readonly Checksum _checksumValidator;

        public Validator(Checksum checksumValidator, Parser parser)
        {
            _parser = parser;
            _checksumValidator = checksumValidator;
        }

        public bool IsValid(string entry, out string output)
        {
            return _checksumValidator.Validate(output = _parser.Parse(entry));
        }
    }
}
