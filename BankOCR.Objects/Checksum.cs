
namespace BankOCR.Objects
{
    public class Checksum
    {
        public bool Validate(string output)
        {
            var digit = 0;
            var checksum = 0;

            for (int index = 1; index <= output.Length; index++)
            {
                if (!int.TryParse(output[output.Length - index].ToString(), out digit))
                {
                    return false;
                }

                checksum += digit * index;
            }

            return checksum % 11 == 0;
        }
    }
}
