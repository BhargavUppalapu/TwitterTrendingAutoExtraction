using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Com.Research.TwitterTrendingAutoExtraction.Utils;
using Com.Research.NLPCore.WordSegmentaion;

namespace Com.Research.TwitterTrendingAutoExtraction
{
    class Program
    {
        static void Main(string[] args)
        {

            string configFile = "..\\..\\..\\resources\\configuration\\config.ini";
            string inputFile = "..\\..\\..\\resources\\corpus\\as.txt";//testCase.txt";
            string outputFile = "..\\..\\..\\resources\\corpus";
            Program prog = new Program();
            prog.run(configFile, inputFile, outputFile);
        }


        void run(string configFile, string inputFile, string outputFile)
        {
            Processing P = new Processing();
            Utils.Configuration config = P.InitfromConfig(configFile);
            P.ProcessWorkItem(inputFile, outputFile);
        }

    }
}
