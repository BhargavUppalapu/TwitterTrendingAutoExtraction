using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Research.TwitterTrendingAutoExtraction.DataStructures
{

    public class wordDetails
    {
        public string word;
        public string POSTag;
        public int nameGroup;
        public List<string> trends =new List<string>();
        
    }

    //Cris--1-Stunning/Exciting 
    //Gayle--1--Stunning/Exciting



    public class TweetsDocument
    {
        public string tweet ;
        
        public List<wordDetails> words = new List<wordDetails>();
    }
}
