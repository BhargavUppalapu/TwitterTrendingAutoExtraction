using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Com.Research.NLPCore;
using Com.Research.TwitterTrendingAutoExtraction;
using Com.Research.TwitterTrendingAutoExtraction.HashTagSpliter;

namespace Com.Research.TwitterTrendingAutoExtraction.Utils
{
    class Configuration
    {

        private String _configName;

        public String ConfigName
        {
            get { return _configName; }

        }
        public Configuration() { }
        /// <summary>
        /// Name for HashTagSplitter
        /// </summary>
        private string _hashTagSplitterName = null;
        private bool _isHasTagSplitterCreated = false;
        private string _hashTagSplitterModelFile;
        private readonly FactoryClass<IHashTagSplitter> _myHashTagSplitterFactory = new FactoryClass<IHashTagSplitter>();
        private IHashTagSplitter _myhashTagSplitter;

        public IHashTagSplitter HashTagSplitter
        {
            get { return _myhashTagSplitter; }
        }



        public void InitiateModules()
        {
            if(!string.IsNullOrEmpty(_hashTagSplitterName))
            _isHasTagSplitterCreated = CreateHashTagSplitter();
        }

        public bool CreateHashTagSplitter()
        {
            _myhashTagSplitter = _myHashTagSplitterFactory.Create(_hashTagSplitterName);
            return(_myhashTagSplitter.LoadModel(_hashTagSplitterModelFile));
        
        }
        
        
        public void InitializeFromIni(string configureFile)
        {
            IniParser parser = new IniParser(@configureFile);
            _hashTagSplitterModelFile = parser.GetSetting("appSettings", "COMPANY.Modules.HashTagSplitterModelFile");
            _hashTagSplitterName = parser.GetSetting("appSettings", "COMPANY.Modules.HashTagSplitte");
            InitiateModules();
        }

    }
}
