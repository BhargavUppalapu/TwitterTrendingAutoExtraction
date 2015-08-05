using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Com.Research.TwitterTrendingAutoExtraction.Utils;
using Com.Research.TwitterTrendingAutoExtraction.DataStructures;

namespace Com.Research.TwitterTrendingAutoExtraction.TrendExtraction
{
    class TrendExtractor : ITrendExtractor
    {


        public void Extract(Configuration config, List<TweetsDocument> tweets)
        {

            Dictionary<string, Dictionary<string, int>> trends = new Dictionary<string, Dictionary<string, int>>();

            FileStream fs_OutFile = new FileStream(config.OutputFolder + "//output_Trends.txt", FileMode.Append);
            StreamWriter sw_OutFile = new StreamWriter(fs_OutFile, System.Text.Encoding.UTF8);

            string[] names = File.ReadAllLines(config.NaesForTrends);
            string[] tokens;
            string[] POS;

            foreach (string name in names)
            {
                string[] nameTokens = config.Tokenizer.Tokenize(name);
                string ConcatinatedTokens = nameTokens[0].ToLower();
                for( int i=1;i<nameTokens.Length;i++)//string nameToken in nameTokens)
                {
                    ConcatinatedTokens = ConcatinatedTokens + "|" + nameTokens[i].ToLower();
                }
                trends.Add(ConcatinatedTokens, new Dictionary<string, int>());
            }

            int count = 0;
            foreach (TweetsDocument Tweet in tweets)
            {
                if (count % 100 == 0)
                {
                    Console.WriteLine("Completed " + Convert.ToString(count) + " out of " + Convert.ToString(tweets.Count));
                    Console.WriteLine("processing the tweet: " + Tweet.tweet);
                }

                count++;

                //1. Tokenize
                tokens = config.Tokenizer.Tokenize(Tweet.tweet);

                //2. POS Tagging
                POS = config.POSTagger.POSTag(tokens);

                //3. NER 
                string taggedText = config.NER.ExtractEntities(tokens);

                string[] taggedTextTokens = config.Tokenizer.Tokenize(taggedText);
                string[] taggedTextPOSTag = config.POSTagger.POSTag(taggedTextTokens);
                bool namestart = false;
                int nameGroup = 0;

                for (int i = 0; i < taggedTextTokens.Length; i++)// string taggedToken in taggedTextTokens)
                {

                    if (taggedTextTokens[i] != "<PERSON>" && taggedTextTokens[i] != "</PERSON>")
                    {
                        wordDetails wordDetail = new wordDetails();
                        wordDetail.POSTag = taggedTextPOSTag[i];
                        wordDetail.word = taggedTextTokens[i];
                        if (namestart)
                        {
                            while (taggedTextTokens[i+1] != "</PERSON>")
                            { 
                               wordDetail.word = wordDetail.word + "|" +taggedTextTokens[i+1];
                               i++;
                            }
                            wordDetail.nameGroup = nameGroup;
                        }
                        else
                        {
                            wordDetail.nameGroup = -1;
                        }
                        Tweet.words.Add(wordDetail);
                    }
                    else
                    {
                        if (taggedTextTokens[i] == "<PERSON>")
                        {
                            namestart = true;
                        }
                        else
                        {
                            nameGroup++;
                            namestart = false;
                        }
                    }

                }
                //4. For the interested names find all the Verbs and Adjectives near by.

                for (int j = 0; j < Tweet.words.Count; j++)// wordDetails word in Tweet.words)
                {
                    if (Tweet.words[j].nameGroup >= 0)
                    {
                        string currName = "";
                        int curNameGroup = Tweet.words[j].nameGroup;
                        foreach (wordDetails word in Tweet.words)
                        {
                            if (word.nameGroup == curNameGroup)
                            {
                                currName = currName + word.word.ToLower();
                            }
                        }

                        int back = j - 1;
                        bool foudJJ = false;
                        String CurrTweet = "";

                        while (back - 1 > 0 && foudJJ == false)
                        {
                            if ((Tweet.words[back].POSTag == "JJ" && Tweet.words[back - 1].POSTag == "VBJ"))
                            {
                                foudJJ = true;
                                CurrTweet = Tweet.words[back - 1].word.ToLower() + "|" + Tweet.words[back].word.ToLower();
                                Tweet.words[j].trends.Add(CurrTweet);
                                if (trends.ContainsKey(currName))
                                {
                                    if (trends[currName].ContainsKey(CurrTweet))
                                    {
                                        trends[currName][CurrTweet]++;
                                    }
                                    else
                                    {
                                        trends[currName].Add(CurrTweet, 1);
                                    }
                                }

                            }
                            else if (Tweet.words[back].POSTag == "JJ" && Tweet.words[back + 1].POSTag == "NN")
                            {
                                foudJJ = true;
                                CurrTweet = Tweet.words[back].word.ToLower() + "|" + Tweet.words[back + 1].word.ToLower();
                                Tweet.words[j].trends.Add(CurrTweet);
                                if (trends.ContainsKey(currName))
                                {
                                    if (trends[currName].ContainsKey(CurrTweet))
                                    {
                                        trends[currName][CurrTweet]++;
                                    }
                                    else
                                    {
                                        trends[currName].Add(CurrTweet, 1);
                                    }
                                }
                            }
                            back--;
                        }

                        int front = j + 1;
                        foudJJ = false;
                        CurrTweet = "";
                        while (front + 1 < Tweet.words.Count && foudJJ == false)
                        {
                            if (Tweet.words[front].POSTag == "JJ" && Tweet.words[front - 1].POSTag == "VBJ")
                            {
                                foudJJ = true;
                                CurrTweet = Tweet.words[front - 1].word.ToLower() + "|" + Tweet.words[front].word.ToLower();
                                Tweet.words[j].trends.Add(CurrTweet);
                                if (trends.ContainsKey(currName))
                                {
                                    if (trends[currName].ContainsKey(CurrTweet))
                                    {
                                        trends[currName][CurrTweet]++;
                                    }
                                    else
                                    {
                                        trends[currName].Add(CurrTweet, 1);
                                    }
                                }
                            }
                            else if (Tweet.words[front].POSTag == "JJ" && Tweet.words[front + 1].POSTag == "NN")
                            {
                                CurrTweet = Tweet.words[front].word.ToLower() + "|" + Tweet.words[front + 1].word.ToLower();
                                foudJJ = true;
                                Tweet.words[j].trends.Add(CurrTweet);
                                if (trends.ContainsKey(currName))
                                {
                                    if (trends[currName].ContainsKey(CurrTweet))
                                    {
                                        trends[currName][CurrTweet]++;
                                    }
                                    else
                                    {
                                        trends[currName].Add(CurrTweet, 1);
                                    }
                                }

                            }
                            front++;
                        }
                    }
                }

            }


            foreach (string trend in trends.Keys)
            {
                var sortedDict = trends[trend].OrderByDescending(entry => entry.Value)
                     .Take(5)
                     .ToDictionary(pair => pair.Key, pair => pair.Value);
                
                Dictionary<string,int> result = sortedDict;
                sw_OutFile.WriteLine("\nThe Top 5 trends for " + trend.Replace("|", " ") + " are:");
                int num = 1;
                foreach (string key in result.Keys)
                { 
                    //Console.WriteLine(trend + "-->" + key + "-->" + Convert.ToString(result[key]));
                    sw_OutFile.WriteLine( Convert.ToString(num) +". "+ key.Replace("|"," "));
                    num++;
                }

            }

            //5. Find Synonims and make the same words into one list.

            sw_OutFile.Close();
            fs_OutFile.Close();
            
        }
    }
}
