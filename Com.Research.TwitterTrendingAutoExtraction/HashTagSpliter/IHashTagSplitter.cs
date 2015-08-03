using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Research.TwitterTrendingAutoExtraction.HashTagSpliter
{ 
    interface IHashTagSplitter
    {

       
        /// <summary>
        /// Takes the list of Hashtags and writes the splitted tags into file.
        /// </summary>
        /// <param name="hashtags"> List of Hash Tags</param>
        /// <param name="outFilePath"> Out File to write the splitted Hashtags</param>
        void splitHashTag(string inFilePath, string outFilePath);

        /// <summary>
        /// Loads the ModelFile into Dictionary
        /// </summary>
        /// <param name="modelFile"></param>
        bool LoadModel(string modelFile);

    }
}
