using BankOCR.Objects;
using NUnit.Framework;
using System.Collections.Generic;

namespace BankOCR.Tests
{
    [TestFixture]
    public class AccountNumberTests
    {
        private readonly string[] _digits = new[]
        {
            " _ | ||_|",
            "     |  |",
            " _  _||_ ",
            " _  _| _|",
            "   |_|  |",
            " _ |_  _|",
            " _ |_ |_|",
            " _   |  |",
            " _ |_||_|",
            " _ |_| _|"
        };

        private readonly IDictionary<char, char[]> _engine = new Dictionary<char, char[]>
        {
            {' ', new[] { '_', '|' }},
            {'_', new[] { ' ', '|' }},
            {'|', new[] { ' ', '_' }}
        };

        [TestCase("000000000", " _  _  _  _  _  _  _  _  _ | || || || || || || || || ||_||_||_||_||_||_||_||_||_|")]
        [TestCase("123456789", "    _  _     _  _  _  _  _   | _| _||_||_ |_   ||_||_|  ||_  _|  | _||_|  ||_| _|")]
        [TestCase("000000051", " _  _  _  _  _  _  _  _    | || || || || || || ||_   ||_||_||_||_||_||_||_| _|  |")]
        [TestCase("711111111", " _                           |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |")]
        public void GivenAFileEntryWhenIParseItThenIOutputANumber(string expected, string entry)
        {
            var validator = new Validator(new Checksum(), new Parser(_digits));
            var suggestor = new Suggester(validator, _engine);
            var formatter = new Formatter();
            var accountNumber = new AccountNumber(validator, suggestor, formatter, entry);

            Assert.AreEqual(expected, accountNumber.ToString());
        }

        [TestCase("222222222 ILL", " _  _  _  _  _  _  _  _  _  _| _| _| _| _| _| _| _| _||_ |_ |_ |_ |_ |_ |_ |_ |_ ")]
        [TestCase("444444444 ILL", "                           |_||_||_||_||_||_||_||_||_|  |  |  |  |  |  |  |  |  |")]
        public void GivenAFileEntryWhichHoldsAValidAccountNumberWhenIParseItThenItFailsTheChecksum(string expected, string entry)
        {
            var validator = new Validator(new Checksum(), new Parser(_digits));
            var suggestor = new Suggester(validator, _engine);
            var formatter = new Formatter();
            var accountNumber = new AccountNumber(validator, suggestor, formatter, entry);

            Assert.AreEqual(expected, accountNumber.ToString());
        }

        [TestCase("49006771? ILL", "    _  _  _  _  _  _     _ |_||_|| || ||_   |  |  | _   | _||_||_||_|  |  |  | _|")]
        [TestCase("1234?678? ILL", "    _  _     _  _  _  _  _   | _| _||_| _ |_   ||_||_|  ||_  _|  | _||_|  ||_| _ ")]
        public void GivenAFileEntryWhichHoldsAUnParseableAccountNumberWhenIParseItThenTheInvalidCharactersAreMarked(string expected, string entry)
        {
            var validator = new Validator(new Checksum(), new Parser(_digits));
            var suggestor = new Suggester(validator, _engine);
            var formatter = new Formatter();
            var accountNumber = new AccountNumber(validator, suggestor, formatter, entry);

            Assert.AreEqual(expected, accountNumber.ToString());
        }

        [TestCase("711111111", "                             |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |")]
        [TestCase("777777177", " _  _  _  _  _  _  _  _  _   |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |")]
        [TestCase("200800000", " _  _  _  _  _  _  _  _  _  _|| || || || || || || || ||_ |_||_||_||_||_||_||_||_|")]
        [TestCase("333393333", " _  _  _  _  _  _  _  _  _  _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _|")]
        [TestCase("888888888 AMB ['888886888', '888888880', '888888988']", " _  _  _  _  _  _  _  _  _ |_||_||_||_||_||_||_||_||_||_||_||_||_||_||_||_||_||_|")]
        [TestCase("555555555 AMB ['555655555', '559555555']", " _  _  _  _  _  _  _  _  _ |_ |_ |_ |_ |_ |_ |_ |_ |_  _| _| _| _| _| _| _| _| _|")]
        [TestCase("666666666 AMB ['666566666', '686666666']", " _  _  _  _  _  _  _  _  _ |_ |_ |_ |_ |_ |_ |_ |_ |_ |_||_||_||_||_||_||_||_||_|")]
        [TestCase("999999999 AMB ['899999999', '993999999', '999959999']", " _  _  _  _  _  _  _  _  _ |_||_||_||_||_||_||_||_||_| _| _| _| _| _| _| _| _| _|")]
        [TestCase("333393333", " _  _  _  _  _  _  _  _  _  _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _| _|")]
        [TestCase("490067715 AMB ['490067115', '490067719', '490867715']", "    _  _  _  _  _  _     _ |_||_|| || ||_   |  |  ||_   | _||_||_||_|  |  |  | _|")]
        [TestCase("123456789", "    _  _     _  _  _  _  _  _| _| _||_||_ |_   ||_||_|  ||_  _|  | _||_|  ||_| _|")]
        [TestCase("000000051", " _     _  _  _  _  _  _    | || || || || || || ||_   ||_||_||_||_||_||_||_| _|  |")]
        [TestCase("490867715", "    _  _  _  _  _  _     _ |_||_|| ||_||_   |  |  | _   | _||_||_||_|  |  |  | _|")]
        public void GivenACorrectlyParsedAccountNumberWithAInValidChecksumWhenIProcessItThenIAttemptAGuessAtAValidAccountNumber(string expected, string entry)
        {
            var validator = new Validator(new Checksum(), new Parser(_digits));
            var suggestor = new Suggester(validator, _engine);
            var formatter = new Formatter();
            var accountNumber = new AccountNumber(validator, suggestor, formatter, entry);

            Assert.AreEqual(expected, accountNumber.ToString());
        }
    }
}