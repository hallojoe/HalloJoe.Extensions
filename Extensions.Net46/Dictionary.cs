using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloJoe.Extensions
{
    public static class DictionaryExtenstions
    {
        /// <summary>
        /// Sort dictionary by value desc
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        internal static Dictionary<string, int> ToValueSortedDictionary(this Dictionary<string, int> dict)
        {
            var items =
                from k in dict.Keys
                orderby dict[k]
                descending
                select k;
            Dictionary<string, int> tmp =
                new Dictionary<string, int>();
            foreach (string item in items)
                tmp.Add(item, dict[item]);
            return tmp;
        }
    }
}
