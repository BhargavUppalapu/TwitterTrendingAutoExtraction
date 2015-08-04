using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Com.Research.TwitterTrendingAutoExtraction.Utils;
using Com.Research.TwitterTrendingAutoExtraction.DataStructures;
using Com.Research.NLPCore.WordSegmentaion;


namespace Com.Research.TwitterTrendingAutoExtraction.HashTagSpliter
{
    public class ViterbiUnigramHashTagSplitter : IHashTagSplitter
    {

        private ViterbiUnigramWordSegmenter _wordSegmentorModel = null;
        private string _viterbiModelFile;

        public String ViterbiModelFile
        {
            get { return _viterbiModelFile; }
            set{_viterbiModelFile = value;}
        
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ViterbiUnigramHashTagSplitter()
        {
            _wordSegmentorModel = new ViterbiUnigramWordSegmenter();  
        }



        public List<string> extractHashTagsFromText(string text)
        {
            List<string> hashtags = new List<string>();
            hashtags = ExtractHashTags.ExtractTags(text);
            return hashtags;
        }
        

        public void splitHashTag(string inFilePath, string outFilePath,List<TweetsDocument> tweets)
        {

            FileStream fs_OutFile = new FileStream(outFilePath + "//output_HashTagSplit.txt", FileMode.Append);
            StreamWriter sw_OutFile = new StreamWriter(fs_OutFile, System.Text.Encoding.UTF8);
            //StreamReader sr_as = new StreamReader(inFilePath, System.Text.Encoding.UTF8);
            string[] lines = System.IO.File.ReadAllLines(inFilePath);
            
            //string text = sr_as.ReadToEnd();
            List<string> hashtags = new List<string>();

            foreach (string text in lines)
            {
                string curText = text;
                hashtags = ExtractHashTags.ExtractTags(text);
                TweetsDocument tweet = new TweetsDocument();

                foreach (string hashtag in hashtags)
                {
                    List<string> words = _wordSegmentorModel.ExtractSegments(hashtag);
                    // "#" +hashtag + "\t-->";
                    string output = "";
                    foreach (string word in words)
                    {
                        output = output + " " + word;
                    }

                    sw_OutFile.WriteLine("#" + hashtag + "\t-->" + output);

                    curText = curText.Replace("#" + hashtag, output);
                    
                }

                tweet.tweet = curText;
                tweets.Add(tweet);
            }
            sw_OutFile.Close();
            fs_OutFile.Close();
            
            //sr_as.Close();
        }


        public bool LoadModel(string modelFile)
        {
            return _wordSegmentorModel.LoadModel(modelFile);
        }
    }
}
