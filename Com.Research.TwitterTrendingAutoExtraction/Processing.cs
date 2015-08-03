using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Research.TwitterTrendingAutoExtraction
{
    class Processing
    {

        Utils.Configuration _config = new Utils.Configuration();


        public void ProcessWorkItem(string InputFilePath, string outFilePath)
        {
            _config.HashTagSplitter.splitHashTag(InputFilePath, outFilePath);

        }

        public Utils.Configuration InitfromConfig(string configurationFile)
        {
            _config.InitializeFromIni(configurationFile);
            return _config;
        }


    }
}
