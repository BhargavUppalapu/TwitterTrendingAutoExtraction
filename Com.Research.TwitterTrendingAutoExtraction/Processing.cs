using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Com.Research.TwitterTrendingAutoExtraction.DataStructures;

namespace Com.Research.TwitterTrendingAutoExtraction
{
    class Processing
    {

        Utils.Configuration _config = new Utils.Configuration();
        List<TweetsDocument> tweets = new List<TweetsDocument>();

        public void ProcessWorkItem(string InputFilePath, string outFilePath)
        {
            Console.WriteLine("Extracting Hastags and Segmenting the words.....\n");
            _config.HashTagSplitter.splitHashTag(InputFilePath, outFilePath,tweets);
            Console.WriteLine("Completed extraction and segmentation. Please check the output_HashTagSplit.txt file in the output folder for the results..\n");


            Console.WriteLine("Starting the Trend Extraction.....\n");
            _config.TrendExtractor.Extract(_config,tweets);

            Console.WriteLine("I am done. Check the output_Trends.txt file for the results and please press any key to close...");
            Console.Read();
        }

        public Utils.Configuration InitfromConfig(string configurationFile)
        {
            _config.InitializeFromIni(configurationFile);
            return _config;
        }


    }
}
