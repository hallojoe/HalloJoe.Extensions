using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloJoe.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Get distict enumerable of T by property selector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> list, Func<T, object> propertySelector) =>
            list.GroupBy(propertySelector).Select(x => x.First());

        /// <summary>
        /// Count sum of syllables words - WIP see: count syllables methods...
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Number of sylables in words</returns>
        public static int CountSyllables(this IEnumerable<string> words, string specialVowels = "æøå")
        {
            int syllables = 0;
            foreach (var word in words) syllables += word.CountSyllablesAlt(specialVowels);
            return syllables;
        }

        /// <summary>
        /// Count average syllables per word in words 
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Number of sylables per word in words</returns>
        public static double AverageSyllablesPerWord(this IEnumerable<string> words) =>
            words.CountSyllables() / (double)words.Count();

        /// <summary>
        /// Creates a keyword cloud weighted by keyword occourances
        /// </summary>
        /// <param name="keywordSize"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// Weighted
        public static Dictionary<string, int> GetWordCloud(this IEnumerable<string> list, int keywordSize = 1)
        {
            List<string> dataList = list.ToList();

            // set keywordSize
            keywordSize = keywordSize <= 0 ? 1 : keywordSize;
            keywordSize = keywordSize - 1;

            // result
            Dictionary<string, int> dict = new Dictionary<string, int>();

            // 
            string tempWord = null;
            int tempWordCount = 0;
            int listCount = dataList.Count();

            for (int i = 0; i <= listCount - 1; i++)
            {
                tempWord = string.Empty;
                tempWordCount = 0;

                // generate word combination of given keywordSize
                for (int j = 0; j <= keywordSize; j++)
                    if (i + j <= listCount - 1)
                    {
                        tempWord = string.Concat(tempWord, " ", dataList[i + j]);
                        tempWordCount += 1;
                    }

                tempWord = tempWord.Trim();
                if (tempWordCount == keywordSize + 1)
                    if (dict.ContainsKey(tempWord))
                        dict[tempWord] = dict[tempWord] + 1;
                    else
                        dict.Add(tempWord, 1);
            }
            return dict.ToValueSortedDictionary();
        }

        /// <summary>
        /// Get density of word in given words
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static float ComputeDensity(this IEnumerable<string> words, string word)
        {
            // list enumerable
            List<string> list = words.ToList();

            // get size/length of existing 
            int size = word.Split(' ').Length;

            // get keyword cloud
            Dictionary<string, int> cloud = list.GetWordCloud(size);

            // i = number of times word exist in cloud
            float i = 0;

            if (cloud.ContainsKey(word.ToLower()))
                i = cloud[word];
            if (i > 0)
                return (i / (list.Count / size)) * 100;

            return i;
        }

        /// <summary>
        /// Get lexical density of  words
        /// Lexical density of _text (http://en.wikipedia.org/wiki/Lexical_density)
        /// Ld = (NLex / N) * 100
        /// Where:
        /// Ld = lexical density of analysed texts
        /// NLex = the number of lexical word tokens (nouns, adjectives, verbs, adverbs) in the analysed text
        /// N = the number of all tokens (total number of words) in the analysed text
        /// </summary>
        /// <param name="ignoreList"></param>
        /// <returns></returns>
        public static double ComputeLexicalDensity(this IEnumerable<string> words, IEnumerable<string> ignoreList = null)
        {
            // todo: explain better - above and below
            double wordsCount = (double)words.Count();
            List<string> tokens = words.Except(ignoreList).ToList();
            double tokensCount = (double)tokens.Count;
            double i = tokensCount / wordsCount;
            return i * 100;
        }

        /// <summary>
        /// Get IEnumerable in partitions of desired size
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IList<T>> GetPartitions<T>(this IEnumerable<T> list, int size)
        {
            List<T> partial = new List<T>(size);
            foreach (T item in list)
            {
                if (partial.Count == size)
                {
                    yield return partial;
                    partial = new List<T>(size);
                }
                partial.Add(item);
            }
            if (partial.Count > 0)
                yield return partial;
        }

    }
}
