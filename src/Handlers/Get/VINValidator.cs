using System;

namespace Get
{
    public interface IValidateVIN
    {
        bool IsValid(string vin);
    }

    /// credit to this Gist https://gist.github.com/deeja/008c2c36764f11cbc02818e7d793738a
    public class VINValidator : IValidateVIN
    {
        public bool IsValid(string vin)
        {
            if (vin.Length != 17)
            {
                return false;
            }

            return GetCheckDigit(vin) == vin[8];
        }

        private static int Transliterate(char c)
        {
            return "0123456789.ABCDEFGH..JKLMN.P.R..STUVWXYZ".IndexOf(c) % 10;
        }

        private static char GetCheckDigit(String vin)
        {
            const string map = "0123456789X";
            const string weights = "8765432X098765432";

            int sum = 0;

            for (int i = 0; i < 17; ++i)
            {
                sum += Transliterate(vin[i]) * map.IndexOf(weights[i]);
            }
            return map[sum % 11];
        }
    }
}
