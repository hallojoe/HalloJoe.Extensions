using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HalloJoe.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Count chars - optionally ignoring white space
        /// </summary>
        /// <param name="text"></param>
        /// <param name="ignorewhitespace"></param>
        /// <returns>Number of chars in text</returns>
        public static int CountChars(this string text, bool ignorewhitespace = false) => ignorewhitespace ? text.Replace(" ", string.Empty).Length : text.Length;

        /// <summary>
        /// Trim chars in any string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="any"></param>
        /// <returns></returns>
        public static string Trim(this string s, string any) => s.Trim(any.ToCharArray());

        /// <summary>
        /// Trim numbers of string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrimNumbers(this string s) => s.Trim("0123456789");

        /// <summary>
        /// Replace first occourance of search string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceFirst(string input, string search, string replace) =>
            ReplaceAt(input, search, replace, input.IndexOf(search));

        /// <summary>
        /// Replace last occourance of seach string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceLast(string input, string search, string replace) =>
            ReplaceAt(input, search, replace, input.LastIndexOf(search));

        /// <summary>
        /// Replace seach string at give position
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        internal static string ReplaceAt(string input, string search, string replace, int pos)
        {
            if (pos < 0) return input;
            return input.Substring(0, pos) + replace + input.Substring(pos + search.Length);
        }

        /// <summary>
        /// Remove first occourance of search string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string RemoveFirst(string input, string search) =>
            input.Remove(input.IndexOf(search), search.Length);

        /// <summary>
        /// Remove last occourance of search string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="search"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string RemoveLast(string input, string search) =>
            input.Remove(input.LastIndexOf(search), search.Length);

        /// <summary>
        /// Uppercase first char in string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UpperFirst(this string s) => s[0].ToString().ToUpper() + s.Substring(1);


        /// <summary>
        /// Get all words in (plain)text. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>IEnumerable of words in text</returns>
        public static IEnumerable<string> GetWordsRaw(this string text)
        {
            foreach (Match word in Regex.Matches(text, @"[\S]+"))
                yield return word.Value;
        }

        public static string GetLixableWord(this string word, bool removePotentialSmileys = true)
        {
            bool endsWithPunctuation = char.IsPunctuation(word[word.Length - 1]);
            if (endsWithPunctuation)
                word = word.Substring(0, word.Length - 1);
            bool hasIsolatedPunctuation = false;
            string result = string.Empty;
            foreach (var c in word)
                if (!char.IsPunctuation(c))
                    result += c;
                else
                    if (!hasIsolatedPunctuation)
                    hasIsolatedPunctuation = true;

            if (removePotentialSmileys && hasIsolatedPunctuation && result.Length == 1)
                return string.Empty;

            if (hasIsolatedPunctuation && result.Length < 7)
                result = result.PadRight(7, 'X');

            if (!hasIsolatedPunctuation && endsWithPunctuation)
                result += ". ";

            return result;
        }

        /// <summary>
        /// Get all words in (plain)text. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns>IEnumerable of words in text</returns>
        public static IEnumerable<string> GetWords(this string text)
        {
            char[] delimiters = ":;.!\"?()<>•–".ToCharArray();
            var words = Regex.Matches(text, @"[\S]+");
            var tempWord = string.Empty;
            foreach (Match word in words)
            {
                tempWord = word.Value.Trim(delimiters).ToLower();
                if (!string.IsNullOrEmpty(tempWord)) yield return tempWord;
            }
        }

        /// <summary>
        /// WIP
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetWords2(this string text)
        {
            // forkortelse?
            // [a-zA-Z]\.[a-zA-Z]+\.?

            char[] delimiters = ":;.!\"?()<>•–".ToCharArray();
            var words = Regex.Matches(text, @"[\S]+");
            var tempWord = string.Empty;
            foreach (Match word in words)
            {
                tempWord = word.Value.Trim(delimiters).ToLower();
                if (!string.IsNullOrEmpty(tempWord)) yield return tempWord;
            }
        }




        /// <summary>
        /// Count all words in (plain)text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int CountWords(this string text) => text.GetWords().Count();

        /// <summary>
        /// Get distict words in (plain)text. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDistinctWords(this string text) => text.GetWords().Distinct();

        /// <summary>
        /// Count distict words in (plain)text. 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int CountDistinctWords(this string text) => text.GetDistinctWords().Count();

        /// <summary>
        /// Get occourances of word in list as list 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="word"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetOccurrences(this string text, string word, StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
            text.GetWords().Where(x => x.Equals(word, comparison));

        /// <summary>
        /// Count occourances of word in list 
        /// </summary>
        /// <param name="words"></param>
        /// <returns>Number of word in text</returns>
        public static int CountOccurrences(this string text, string word, StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
            text.GetOccurrences(word, comparison).Count();

        /// <summary>
        /// Get sentences...kinda - not reliable but seem to work ~ok as of feeling, no evidence
        /// </summary>
        /// <param name="text"></param>
        /// <returns>All sentences in text</returns>
        public static IEnumerable<string> GetSentences(this string text)
        {
            foreach (Match m in Regex.Matches(text, "(?sx-m)[^\\r\\n].*?(?:(?:\\.|\\?|!)\\s?)"))
                yield return m.Value;
        }

        /// <summary>
        /// Count sentences
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Number of sentences in text</returns>
        public static int CountSentences(this string text) => text.GetSentences().Count();

        /// <summary>
        /// Get paragraphs
        /// todo: account for headers
        /// </summary>
        /// <param name="text"></param>
        /// <returns>All paragraphs in text</returns>
        public static IEnumerable<string> GetParagraphs(this string text) => text
                .Trim(new[] { '\r', '\n', ' ' })
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        /// Count paragraphs
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Number of paragraphs in text</returns>
        public static int CountParagraphs(this string text) => text.GetParagraphs().Count();

        /// <summary>
        /// Count syllables in word 
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Number of sylables in word</returns>
        public static int CountSyllables(this string word, string specialVowels = "æøå")
        {
            word = word.ToLower().Trim();
            string pattern = "[aeiouy" + specialVowels + "]+";
            int count = Regex.Matches(word, pattern).Count;
            if (word.EndsWith("e")) //check for wording ending with vowel,consonant,e                
                if (Regex.IsMatch(word, "[aeiouy" + specialVowels + "][^aeiouy" + specialVowels + "]e$"))
                    count -= 1;
            if (count < 1)
                count = 1;
            return count;
        }

        /// <summary>
        /// Count avg. chars per word in words 
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Number of avg. chars per word in words</returns>
        public static double AverageCharactersPerWord(this string text)
        {
            var words = text
                .GetWords();
            var chars = text
                .CountChars(true);
            return (double)chars / (double)words.Count();
        }

        /// <summary>
        /// Count syllables in word - Alternative
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Number of sylables in word</returns>
        public static int CountSyllablesAlt(this string word, string specialVowels = "æøå")
        {
            word = word.ToLower().Trim();
            int count = System.Text.RegularExpressions.Regex.Matches(word, "[aeiouy" + specialVowels + "]+").Count;
            if ((word.EndsWith("e") || (word.EndsWith("es") || word.EndsWith("ed"))) && !word.EndsWith("le"))
                count--;
            return count;
        }

        /// <summary>
        /// Get density of word in given text
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static float ComputeDensity(this string text, string word)
        {
            return text
                .GetWords()
                .ToList()
                .ComputeDensity(word);
        }

        public static double ComputeLexicalDensity(this string text, IEnumerable<string> ignoreList = null)
        {
            var words = text.GetWords();
            return words.ComputeLexicalDensity(ignoreList);
        }


        /// <summary>
        /// Computes the prominence of a word in text
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Prominence value (0-100) of word in text</returns>
        /// <remarks>Higher is better</remarks>
        public static float ComputeProminence(this string text, string word)
        {
            var words = text.GetWords();
            //prominence = (totalwords - ((positionsum - 1) / positionsnum)) * (100 / totalwords)
            // Where
            //totalwords = total number of words in string
            //positionsum = the sum of each position of the word we are analyzing 
            //(example: if a word occurs on position 2 and 5 $positionsum is 7)
            //positionsnum = The number of positions
            //The prominence of one word in the first position, in a ten word sentence having unique words only is 
            //(10 - ((1 - 1) / 1)) * (100 / 10)) = 100%.
            //If that same word would be the last word in the sentence, it's prominence would be
            //(10 - ((10 - 1) / 1)) * (100 / 10)) = 10%.
            //If that same word would occur twice, on position 1 and 10, it's prominence would be
            //(10 - ((11 - 1) / 2)) * (100 / 10)) = 50%
            int positionSum = 0;
            int positionsNum = 0;
            int i = 0;
            foreach (string w in words)
            {
                if (w == word)
                {
                    positionSum += (i + 1);
                    positionsNum += 1;
                }
                i += 1;
            }
            float prominence = 0;
            if (positionSum > 0)
                prominence = (text.GetWords().Count() - ((positionSum - 1) / positionsNum)) * (100 / text.GetWords().Count());

            return prominence;
        }

        /// <summary>
        /// Computes the Gunning FOG index of text
        /// http://en.wikipedia.org/wiki/Gunning_fog_index
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Gunning FOG index</returns>
        public static double ComputeGunningFogIndex(this string text)
        {
            //Select a passage (such as one or more full paragraphs) of around 100 words. Do not omit any sentences;
            //Determine the average sentence length. (Divide the number of words by the number of sentences.);
            //Count the "complex" words: those with three or more syllables. Do not include proper nouns, familiar jargon, or compound words. Do not include common suffixes (such as -es, -ed, or -ing) as a syllable;
            //Add the average sentence length and the percentage of complex words; and
            //Multiply the result by 0.4.        
            List<string> words = text.GetWords().ToList();
            List<string> sentences = text.GetSentences().ToList();
            if (sentences.Count < 1)
                return 0.0;
            int wordsCount = words.Count;
            int sentencesCount = sentences.Count;
            double avgSentenceLength = (wordsCount / sentencesCount); // + 100
            List<string> complexWords = words.Where(x => x.CountSyllablesAlt() >= 3).ToList();
            int complexWordsCount = complexWords.Count;
            double percentageOfCompexWords = complexWordsCount / wordsCount;
            double indexval = 0.4 * (avgSentenceLength + 100 * ((double)complexWordsCount / wordsCount));
            return indexval;
            //double gradeLevel = 0.4 * (avgSentenceLength + percentageOfCompexWords);
            //return gradeLevel;
            //todo: clean up!
        }

        /// <summary>
        /// WIP - remomve punctuation that have neighbour characters that are not whitespace
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceIsolatedPunctuation(this string text)
        {
            var punctuation = ".,:;".ToCharArray();

            var parts = text.Split(new string[] { "\r\n", "\n", " " }, StringSplitOptions.RemoveEmptyEntries);
            var processedParts = new List<string>();
            foreach (var part in parts)
            {
                var endsWithPunctuation = part[part.Length - 1] == '.';
                var processedPart = new StringBuilder();
                var foundIsolatedPunctuation = false;
                var trimmedPart = part.Trim();
                for (var i = 0; i < trimmedPart.Length; i++)
                {
                    var current = trimmedPart[i];
                    var prev = i - 1 >= 0 ? trimmedPart[i - 1] : '#';
                    var next = i + 1 >= trimmedPart.Length ? '#' : trimmedPart[i + 1];
                    if (!punctuation.Contains(trimmedPart[i]))
                        processedPart.Append(trimmedPart[i]);
                    else
                        foundIsolatedPunctuation = true;

                    if (!foundIsolatedPunctuation && endsWithPunctuation && i == trimmedPart.Length - 1)
                        processedPart.Append('.');
                }
                processedParts.Add(processedPart.ToString());
            }
            return string.Join(" ", processedParts.ToArray());
        }

        /// <summary>
        /// Compute Flesch Kincaid index
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ComputeFlecshKincaidIndex(this string text)
        {
            List<string> words = text.GetWords().ToList();
            List<string> sentences = text.GetSentences().ToList();
            int wordsCount = words.Count;
            int sentencesCount = sentences.Count;
            int syllablesCount = 0;
            foreach (string word in words)
                syllablesCount += word.CountSyllables();
            double indexval = 0.39 * (((double)wordsCount / sentencesCount) + 11.8 * ((double)syllablesCount / wordsCount) - 15.59);
            return indexval;
        }

        /// <summary>
        /// Compute automated readability index
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ComputeAutomatedReadabilityIndex(this string text)
        {
            // 
            int charcount = text.CountChars(true);  //space characters need to be ignored in character count.
            int wordcount = text.GetWords().Count();
            int sentencecount = text.GetSentences().Count();
            double index = 4.71 * ((double)charcount / wordcount) + 0.5 * ((double)wordcount / sentencecount) - 21.43;
            return index;
        }

        /// <summary>
        /// Compute Flesch reading ease
        /// source1: http://en.wikipedia.org/wiki/Flesch%E2%80%93Kincaid_readability_tests#Flesch_Reading_Ease
        /// </summary>
        /// <param name="text">The text to compute against</param>
        /// <returns>Flesch reading ease value</returns>
        public static double ComputeFleschReadingEase(this string text)
        {
            List<string> words = text.GetWords().ToList();
            List<string> sentences = text.GetSentences().ToList();
            int wordcount = words.Count();
            int sentencecount = sentences.Count;
            int syllablesCount = 0;
            foreach (string word in words)
                syllablesCount += word.CountSyllablesAlt();
            double ease = 206.835 - (1.015 * wordcount) / sentencecount - (84.6 * syllablesCount) / wordcount;
            return ease;
        }

        /// <summary>
        /// Compute SMOG index
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ComputeSmogIndex(this string text)
        {
            List<string> words = text
                .GetWords()
                .ToList();
            List<string> sentences = text
                .GetSentences()
                .ToList();
            int wordsCount = words
                .Count;
            int sentencesCount = sentences
                .Count;
            List<string> complexWords = words
                .Where(x => x.CountSyllablesAlt() >= 3)
                .ToList();
            int complexWordsCount = complexWords
                .Count;
            double smog = Math.Sqrt(complexWordsCount * 30.0 / sentencesCount) + 3.0;
            return smog;
        }


        /// <summary>
        /// Compute Coleman liau index
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ComputeColemanLiauIndex(this string text)
        {
            var wordCount = text
                .GetWords()
                .Count();
            var sentenceCount = text
                .GetSentences()
                .Count();
            var letterNumberCount = text
                .CountChars(true);
            return
                (5.89 * letterNumberCount) / wordCount - (30.0 * sentenceCount) / wordCount - 15.8;
        }

        /// <summary>
        /// WIP - Compute LIX index
        /// LIX is a readability measure indicating the difficulty of reading a text 
        /// developed by Swedish scholar Carl-Hugo Björnsson. 
        /// It is computed as follows:
        ///A is the number of words,
        ///B is the number of periods (defined by period, colon or capital first letter), and
        ///C is the number of long words (more than 6 letters).
        /// source: Wikipedia
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static double ComputeLixIndex(this string text)
        {
            // number of words
            var words = text
                .GetWords();
            var wordCount = words
                .Count();
            // todo: this will not return what is needed?
            var sentenceCount = text
                .GetSentences()
                .Count();
            // get long words count
            var longWords = words
                .Where(x => x.Length > 6);
            double lix =
                (wordCount / sentenceCount) + (longWords.Count() * 100) / wordCount;
            return lix;
        }

        /// <summary>
        /// Find min of 3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static int Min(int a, int b, int c) => Math.Min(Math.Min(a, b), c);

        /// <summary>
        /// Compute the distance between two strings
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static int ComputeDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] distance = new int[n + 1, m + 1]; // matrix
            int cost = 0;
            if (n == 0) return m;
            if (m == 0) return n;
            //init1
            for (int i = 0; i <= n; distance[i, 0] = i++) ;
            for (int j = 0; j <= m; distance[0, j] = j++) ;
            //find min distance
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                {
                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                    distance[i, j] = Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1, distance[i - 1, j - 1] + cost);
                }
            return distance[n, m];
        }

        /// <summary>
        /// Compute smilarity between two strings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float GetSimilarity(this string a, string b)
        {
            float maxLen = a.Length;
            if (maxLen < b.Length) maxLen = b.Length;
            if (maxLen == 0.0F) return 1.0F;
            else return 1.0F - ComputeDistance(a, b) / maxLen;
        }

        /// <summary>
        /// Crappy generate slug
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GenerateSlug(this string s)
        {
            string str = s.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Trim().Substring(0, str.Length <= 45 ? str.Length : 45);
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        /// <summary>
        /// Remove accents
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RemoveAccent(this string s)
        {
            byte[] b = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(s);
            return System.Text.Encoding.UTF8.GetString(b, 0, b.Length -1);
        }
    }
}
