using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Com.Research.TwitterTrendingAutoExtraction.Utils;
using Com.Research.TwitterTrendingAutoExtraction.DataStructures;

namespace Com.Research.TwitterTrendingAutoExtraction.TrendExtraction
{
    public interface ITrendExtractor
    {
        void Extract( Configuration config, List<TweetsDocument> tweets);
    }
}
