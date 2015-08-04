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
            trends.Add("Clint Eastwood", null);
            trends.Add("Bradley Cooper", null);
            trends.Add("Chris Kyle", null);

           foreach ( TweetsDocument Tweet in tweets )
            {

                //1. Tokenize
                Tweet.tokens = config.Tokenizer.Tokenize(Tweet.tweet);

                //2. POS Tagging
                Tweet.POSTags = config.POSTagger.POSTag(Tweet.tokens);

                //3. NER 
                string taggedText = config.NER.ExtractEntities(Tweet.tokens);

                
                var regex = new Regex(@"<PERSON>.*</PERSON>");
                var matches = regex.Matches(taggedText);
                string[] NamesExtracted = new string[matches.Count];

                for(int i =0 ;i < matches.Count;i++)
                {
                    NamesExtracted[i] = matches[i].Value.Replace("<PERSON>", "").Replace("</PERSON>", "");

                    if(trends.ContainsKey(NamesExtracted[i]))
                    {
                        Dictionary<string,int> tweettrends = new Dictionary<string,int>();
                        for( int j=0 ; j< Tweet.POSTags.Length ;j++)// string POS in Tweet.POSTags)
                        {
                            if(Tweet.POSTags[j] == "JJ")
                            {
                                if(tweettrends.ContainsKey(Tweet.tokens[j]))
                                {
                                  tweettrends[Tweet.tokens[j]]++;  
                                }
                                else
                                {
                                    tweettrends.Add(Tweet.tokens[j],1);
                                }
                               
                            }
                        }

                        trends[NamesExtracted[i]] = tweettrends;
                    }
                }
                Tweet.Names = NamesExtracted;
                
            }

            //4. For the interested names find all the Verbs and Adjectives near by.

            foreach(string trend in trends.Keys)
            {
            
                var sortedDict = trends[trend].OrderByDescending(entry=>entry.Value)
                     .Take(5)
                     .ToDictionary(pair=>pair.Key,pair=>pair.Value);

                
            }






            //5. Find Synonims and make the same words into one list.
        }
    }
}
