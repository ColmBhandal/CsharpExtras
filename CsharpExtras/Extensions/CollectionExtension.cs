﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Extensions
{
    public static class CollectionExtension
    {
        public static bool TryGetSingleton<TValue>(this ICollection<TValue> collection, out TValue? singleton) where TValue : class
        {
            singleton = default;
            if (collection.Count == 0)
            {
                return false;
            }
            if (collection.Count == 1)
            {
                singleton = collection.First();
                return true;
            }
            return false;
        }

        public static TValue GetSingletonOrFail<TValue>(this ICollection<TValue> collection)
        {
            if (collection.Count == 0)
            {
                throw new ArgumentException("Colllection is empty. Cannot generate a singleton value from the collection.");
            }
            if (collection.Count == 1)
            {
                return collection.First();
            }
            throw new ArgumentException("Collection has more than 1 element. Failed to uniquely single out an element.");
        }
    }
}
