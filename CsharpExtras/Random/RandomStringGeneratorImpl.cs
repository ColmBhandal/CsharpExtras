using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.RandomDataGen
{
    class RandomStringGeneratorImpl : IRandomStringGenerator
    {
        // Global Random instance that is shared across all threads - note that Random is NOT thread-safe
        // Design inspired by this discussion: https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
        private static readonly Random _globalRandom = new Random();

        // Thread-specific instance of Random
        // This Lazy property gets initialized the first time a given thread accesses this class
        // The _globalRandom is used to generate a unique seed for each thread-specific instance
        [ThreadStatic] private static Random? _threadRandom;
        private static Random ThreadRandom => _threadRandom ??= BuildRandomInstanceForThread();

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
                stringChars[i] = alphabet[ThreadRandom.Next(alphabet.Length)];
            }
            return new string(stringChars);
        }

        private static Random BuildRandomInstanceForThread()
        {
            lock (_globalRandom)
            {
                int seed = _globalRandom.Next();
                return new Random(seed);
            }
        }

    }
}
