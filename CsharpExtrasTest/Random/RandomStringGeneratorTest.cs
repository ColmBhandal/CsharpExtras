using RandomDataGen.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Random
{
    [TestFixture]
    [Category("Unit")]
    class RandomStringGeneratorTest
    {
        [Test]
        public void GivenRandomStringGeneratorWhen1000StringsGeneratedThenNoDuplicatesFound()
        {
            int rndStringLength = 6;
            int maxIterations = 1000;
            IRandomStringGenerator generator = NewGenerator();
            ISet<string> resultSet = new HashSet<string>();

            for (int index = 0; index < maxIterations; index++)
            {
                string rnd = generator.RandomAlphaNumericUpperCaseString(rndStringLength);
                Assert.False(resultSet.Contains(rnd), string.Format("Duplicate random string found '{0}' at index {1}", rnd, index));
                resultSet.Add(rnd);
            }
        }

        [Test]
        public void GivenRandomStringGeneratorWhenRandomStringsGeneratedThenAllCharactersAreUsed()
        {
            ISet<char> alphaChars = new HashSet<char> { '0', '1', '2', '3', '4', '5' };
            ISet<char> foundChars = new HashSet<char>();

            string alpha = string.Join("", alphaChars);
            int maxIterations = 100;
            int rndStringLength = 4;
            IRandomStringGenerator generator = NewGenerator();

            for (int index = 0; index < maxIterations; index++)
            {
                string rnd = generator.RandomString(rndStringLength, alpha);
                char[] rndChars = rnd.ToCharArray();
                foreach (char c in rndChars)
                {
                    foundChars.Add(c);
                }
                if (alphaChars.Count == foundChars.Count)
                {
                    Assert.Pass("All chars found at index: " + index);
                }
            }
            Assert.Fail(string.Format("Not all characters found in random strings. Found: '{0}'", string.Join(", ", alphaChars)));
        }

        [Test]
        public void GivenRandomStringGeneratorWhenRandomStringsGeneratedThenAllCharactersHaveEvenDistributionWithin10Percent()
        {
            ISet<char> alphaChars = new HashSet<char> { '0', '1', '2', '3', '4', '5' };
            IDictionary<char, int> charCount = new Dictionary<char, int>();

            string alpha = string.Join("", alphaChars);
            int maxIterations = 10000;
            int rndStringLength = 1;
            IRandomStringGenerator generator = NewGenerator();

            for (int index = 0; index < maxIterations; index++)
            {
                string rnd = generator.RandomString(rndStringLength, alpha);
                char[] rndChars = rnd.ToCharArray();
                foreach (char c in rndChars)
                {
                    if (charCount.ContainsKey(c))
                    {
                        charCount[c]++;
                    } 
                    else
                    {
                        charCount.Add(c, 1);
                    }
                }
            }

            double expectedCharCount = (maxIterations * rndStringLength) / alphaChars.Count;
            double delta = expectedCharCount * 0.1;
            foreach (var pair in charCount)
            {
                Assert.AreEqual(expectedCharCount, pair.Value, delta, "Character distribution for char '{0}' not even", pair.Key);
            }
        }

        private IRandomStringGenerator NewGenerator()
        {
            return new RandomStringGeneratorImpl();
        }
    }
}
