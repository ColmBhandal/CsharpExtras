using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.RandomDataGen.Impl
{
    class RandomStringGeneratorImpl : IRandomStringGenerator
    {
        private readonly Random _random = new Random();
        private const string NumberSet = "0123456789";
        private const string AlphaUpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static readonly string AlphaLowerCase = AlphaUpperCase.ToLower();
        private static readonly string AlphaNumericUpperCase = NumberSet + AlphaUpperCase;
        private static readonly string AlphaNumericLowerCase = NumberSet + AlphaLowerCase;
        private static readonly string AlphaNumericMixedCase = NumberSet + AlphaUpperCase + AlphaLowerCase;

        public string RandomAlphaNumericUpperCaseString(int length)
        {
            return RandomString(length, AlphaNumericUpperCase);
        }

        public string RandomAlphaNumericLowerCaseString(int length)
        {
            return RandomString(length, AlphaNumericLowerCase);
        }

        public string RandomAlphaNumericMixedCaseString(int length)
        {
            return RandomString(length, AlphaNumericMixedCase);
        }

        public string RandomString(int length, string alphabet)
        {
            if (string.IsNullOrEmpty(alphabet))
            {
                throw new ArgumentException("Cannot generate random string for empty alphabet.");
            }

            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = alphabet[_random.Next(alphabet.Length)];
            }
            return new string(stringChars);
        }
    }
}
