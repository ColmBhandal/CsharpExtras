using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CsharpExtras.Map.Dictionary
{
    /// <summary>
    /// A RegexPatternDictionary is defined as a dictionary that maps a regex pattern to a value. For that reason it is only generic
    /// on the type of value, the key type is always a string.
    /// This class makes it possible to associate a value object to any value that matches a given regex. This means that any given
    /// textKey may match multiple key-value-pairs in the dictionary because multiple regex patterns can match it. The simple example is
    /// to associate a value to the pattern '.*', this means that any queried textKey will always return (at least) that value object.
    /// 
    /// The default [] indexer methods are passed directly to the underlying dictionary, meaning it will work the same way as a
    /// string to value dictionary, looking up on pattern only.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IRegexPatternDictionary<TValue> : IDictionary<string, TValue>
    {
        /// <summary>
        /// Return a collection of the TValue objects whose keys (regex patterns) found a match in the given textKey.
        /// </summary>
        ICollection<TValue> FindAllValuesThatMatch(string textKey);

        /// <summary>
        /// Return a collection of the keys (regex patterns) and TValue objects whose keys found a match in the given textKey.
        /// </summary>
        ICollection<(string pattern, TValue values)> FindAllPatternValuePairsThatMatch(string textKey);

        /// <summary>
        /// Check if the given textKey is matched by any stored regex pattern.
        /// </summary>
        bool HasMatch(string textKey);

        /// <summary>
        /// Associate a given TValue object to an escaped regex pattern. This means that this TValue object will only ever
        /// be returned when the textKey matches the pattern defined here exactly. The key provided will be treated as a
        /// literal string, not a regex pattern.
        /// </summary>
        void AddEscapedFullMatchPattern(string pattern, TValue value);

        /// <summary>
        /// Associate the given TValue to the given pattern. This value will only be returned if the entire textKey string matches this
        /// pattern. This method will automatically surround the regex pattern with '^' and '$' to ensure the pattern matches the full
        /// string.
        /// </summary>
        void AddFullMatchPattern(string pattern, TValue value);
    }
}
