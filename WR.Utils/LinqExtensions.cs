using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WR.Utils
{
    public static class LinqExtensions
    {
        /// <summary>
        /// 扩展Distinct功能
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {

                if (seenKeys.Add(keySelector(element)))
                {

                    yield return element;

                }
            }
        }
    }
}
