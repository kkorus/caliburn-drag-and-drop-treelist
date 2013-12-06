using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaliburnApp.UI.Extensions
{
    public static class LinqExtensionsMethods
    {
        public static IEnumerable<T> FlattenHierarchy<T>(this T node, Func<T, IEnumerable<T>> getChildEnumerator)
        {
            yield return node;
            if (getChildEnumerator(node) != null)
            {
                foreach (var child in getChildEnumerator(node))
                {
                    foreach (var childOrDescendant
                              in child.FlattenHierarchy(getChildEnumerator))
                    {
                        yield return childOrDescendant;
                    }
                }
            }
        }
    }
}
