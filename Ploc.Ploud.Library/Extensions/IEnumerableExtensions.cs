using System.Collections.Generic;

namespace Ploc.Ploud.Library
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}
