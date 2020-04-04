using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dictionary
{
    public class RegexPatternDictionaryImpl<TValue> : IRegexPatternDictionary<TValue>
    {
        private readonly IDictionary<string, TValue> _baseDictionary;
        private readonly IDictionary<Regex, string> _regexDictionary;

        public RegexPatternDictionaryImpl()
        {
            _baseDictionary = new Dictionary<string, TValue>();
            _regexDictionary = new Dictionary<Regex, string>();
        }

        public void Add(string pattern, TValue value)
        {
            AddRegexToDictionary(pattern, pattern, value);
        }

        public ICollection<TValue> FindAllValuesThatMatch(string textKey)
        {
            ICollection<TValue> result = new List<TValue>();
            foreach (var item in _regexDictionary.Where(r => r.Key.IsMatch(textKey)))
            {
                result.Add(_baseDictionary[item.Value.ToString()]);
            }
            return result;
        }

        public bool HasMatch(string textKey)
        {
            return _regexDictionary.Any(r => r.Key.IsMatch(textKey));
        }

        public ICollection<(string pattern, TValue values)> FindAllPatternValuePairsThatMatch(string textKey)
        {
            ICollection<(string pattern, TValue values)> result = new List<(string pattern, TValue values)>();
            foreach (KeyValuePair<Regex, string> item in _regexDictionary.Where(r => r.Key.IsMatch(textKey)))
            {
                result.Add((item.Key.ToString(), _baseDictionary[item.Value.ToString()]));
            }
            return result;
        }

        public void AddEscapedFullMatchPattern(string pattern, TValue value)
        {
            string escapedPattern = Regex.Escape(pattern);
            string fullMatchPattern = MakePatternMatchFullString(escapedPattern);
            AddRegexToDictionary(fullMatchPattern, pattern, value);
        }

        public void AddFullMatchPattern(string pattern, TValue value)
        {
            string fullMatchPattern = MakePatternMatchFullString(pattern);
            AddRegexToDictionary(fullMatchPattern, pattern, value);
        }

        #region Base dictionary methods
        public TValue this[string key] { get => _baseDictionary[key]; set => _baseDictionary[key] = value; }

        public ICollection<string> Keys => _baseDictionary.Keys;

        public ICollection<TValue> Values => _baseDictionary.Values;

        public int Count => _baseDictionary.Count;

        public bool IsReadOnly => _baseDictionary.IsReadOnly;

        public void Add(KeyValuePair<string, TValue> item)
        {
            _baseDictionary.Add(item);
        }

        public void Clear()
        {
            _baseDictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return _baseDictionary.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _baseDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            _baseDictionary.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            return _baseDictionary.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _baseDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return _baseDictionary.Remove(item);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _baseDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _baseDictionary.GetEnumerator();
        }
        #endregion

        private void AddRegexToDictionary(string actualPattern, string keyPattern, TValue value)
        {
            Regex regex = new Regex(actualPattern, RegexOptions.Compiled);
            _baseDictionary.Add(keyPattern, value);
            _regexDictionary.Add(regex, keyPattern);
        }

        private string MakePatternMatchFullString(string pattern)
        {
            string updatedPattern = pattern;
            if (!pattern.StartsWith("^"))
            {
                updatedPattern = "^" + updatedPattern;
            }
            if (!pattern.EndsWith("$"))
            {
                updatedPattern += "$";
            }
            return updatedPattern;
        }
    }
}
