using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Snowball
{
    public class Parser
    {
        private IndonesianStemmer _stemmer = new IndonesianStemmer();
        
        public Queue<string> Parse(string sentence)
        {
            string cleaned = Regex.Replace(sentence, @"[^\w\s]", "");
            string[] words = cleaned.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            
            Queue<string> wordQueue = new Queue<string>();
            foreach (string word in words)
            {
                wordQueue.Enqueue(word.ToLower());
            }

            return wordQueue;
        }

    }
}