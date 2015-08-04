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
            
            _config.HashTagSplitter.splitHashTag(InputFilePath, outFilePath,tweets);
            _config.TrendExtractor.Extract(_config,tweets);



        }

        public Utils.Configuration InitfromConfig(string configurationFile)
        {
            _config.InitializeFromIni(configurationFile);
            return _config;
        }


    }
}
