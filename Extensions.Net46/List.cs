using System;
using System.Collections.Generic;
using System.Linq;

namespace HalloJoe.Extensions
{
    /// <summary>
    /// IList extension methods
    /// </summary>
    public static class ListExtensions
    {

        /// <summary>
        /// Shuffle IList
        /// </summary>
        internal static Random _rnd = new Random();
        /// <summary>
        /// Shuffle list using Fisher-yates algorithm.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IList<T>> GetPartitions<T>(this IList<T> list, int size) => 
            list.AsEnumerable().GetPartitions(size);
    }
}