using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    class ValidationErrorCollectionImpl : IValidationErrorCollection
    {
        private ICollection<IValidationError> _errorCollection;

        public ValidationErrorCollectionImpl()
        {
            _errorCollection = new List<IValidationError>();
        }

        public bool HasBlockers => _errorCollection.Any(e => e.IsBlocker);

        public int Count => _errorCollection.Count;

        public bool IsReadOnly => _errorCollection.IsReadOnly;

        public IEnumerable<string> Messages => _errorCollection.Select(e => e.DisplayName);

        public void Add(IValidationError item)
        {
            _errorCollection.Add(item);
        }

        public void AddAll(IEnumerable<IValidationError> values)
        {
            foreach (IValidationError error in values)
            {
                Add(error);
            }
        }

        public void AddNewBlocker(string message)
        {
            _errorCollection.Add(new ValidationErrorImpl(true, message));
        }

        public void AddNewNonBlocker(string message)
        {
            _errorCollection.Add(new ValidationErrorImpl(false, message));
        }

        public void Clear()
        {
            _errorCollection.Clear();
        }

        public bool Contains(IValidationError item)
        {
            return _errorCollection.Contains(item);
        }

        public void CopyTo(IValidationError[] array, int arrayIndex)
        {
            _errorCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IValidationError> GetEnumerator()
        {
            foreach (IValidationError error in _errorCollection)
            {
                yield return error;
            }
        }

        public string MessagesJoined(string seperator)
        {
            return string.Join(seperator, Messages);
        }

        public bool Remove(IValidationError item)
        {
            return _errorCollection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
