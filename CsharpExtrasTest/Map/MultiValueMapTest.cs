using Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    [TestFixture]    
    class MultiValueMapTest
    {
        [Test]
        [Category("Quick")]
        [Category("Unit")]
        public void GivenEmptyMapWhenTwoItemsAddedToSameKeyThenBothItemsAreInSetForThatKey()
        {
            IMultiValueMap<string, int> multiValueMap = new MultiValueMapImpl<string, int>();
            string key = "bob";
            Assert.IsFalse(multiValueMap.ContainsKey(key), string.Format(
                "Given: expected test to start with empty map entry for key {0}, but map appears to contain value for that key."
                , key));
            const int value1 = 1;
            multiValueMap.Add(key, value1);
            const int value2 = 2;
            multiValueMap.Add(key, value2);
            Assert.AreEqual(multiValueMap[key], new int[] { value1, value2 });
        }
    }
}
