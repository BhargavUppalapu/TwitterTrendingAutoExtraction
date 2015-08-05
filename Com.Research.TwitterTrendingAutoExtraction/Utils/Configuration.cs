using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Com.Research.NLPCore;
using Com.Research.NLPCore.NamedEntityRecognition;
using Com.Research.NLPCore.POSTagging;
using Com.Research.TwitterTrendingAutoExtraction;
using Com.Research.TwitterTrendingAutoExtraction.HashTagSpliter;
using Com.Research.NLPCore.Tokenization;
using Com.Research.TwitterTrendingAutoExtraction.TrendExtraction;

namespace Com.Research.TwitterTrendingAutoExtraction.Utils
{
    public class Configuration
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


        /// <summary>
        /// Name for Tokenizer
        /// </summary>
        private string _tokenizerName = null;
        private bool _isTokenizerCreated = false;
        private readonly FactoryClass<ITokenizer> _myTokenizerFactory = new FactoryClass<ITokenizer>();
        private ITokenizer _myTokenizer;

        public ITokenizer Tokenizer
        {
            get { return _myTokenizer; }
        }




        /// <summary>
        /// Name for TrendExtractor
        /// </summary>
        private string _trendExtractorName = null;
        private bool _istrendExtractorCreated = false;
        private readonly FactoryClass<ITrendExtractor> _myTrendExtractorFactory = new FactoryClass<ITrendExtractor>();
        private ITrendExtractor _myTrendExtractor;

        public ITrendExtractor TrendExtractor
        {
            get { return _myTrendExtractor; }
        }




        /// <summary>
        /// Name for NamedEntity Recognizer
        /// </summary>
        private string _nerName = null;
        private bool _isNERCreated = false;
        private string _myNERModelFile;
        private readonly FactoryClass<INamedEntityRecognizer> _myNERFactory = new FactoryClass<INamedEntityRecognizer>();
        private INamedEntityRecognizer _myNER;

        public INamedEntityRecognizer NER
        {
            get { return _myNER; }
        }


        /// <summary>
        /// Name for POS Tagger
        /// </summary>
        private string _posTaggerNameName = null;
        private bool _isposTaggerCreated = false;
        private string _posTaggerModelFile;
        private readonly FactoryClass<IPOSTagger> _myPOSTaggerFactory = new FactoryClass<IPOSTagger>();
        private IPOSTagger _myposTagger;

        public IPOSTagger POSTagger
        {
            get { return _myposTagger; }
        }


        public string InputFileName;
        public string OutputFolder;
        public string NaesForTrends;


        public void InitiateModules()
        {
            if (!string.IsNullOrEmpty(_hashTagSplitterName))
                _isHasTagSplitterCreated = CreateHashTagSplitter();

            //Initializing Stanford CRFNER model.
            if (!string.IsNullOrEmpty(_tokenizerName))
                _isTokenizerCreated = CreateTokenizer();
            //OutputLogInfo(_isTokenizerCreated, "Tokenizer");

            if (!string.IsNullOrEmpty(_nerName))
                _isNERCreated = CreateNamedEntityRecognizer();

            if (!string.IsNullOrEmpty(_posTaggerNameName))
                _isposTaggerCreated = CreatePOSTagger();


            if (!string.IsNullOrEmpty(_trendExtractorName))
                _istrendExtractorCreated = CreateTrendExtractor();
        }

        public bool CreateNamedEntityRecognizer()
        {
            Console.WriteLine("Loading Named Entity Recognition Model. This may take a minute.");
            _myNER = _myNERFactory.Create(_nerName);
            return (_myNER.LoadModel(_myNERModelFile));
        }
        public bool CreateHashTagSplitter()
        {
            _myhashTagSplitter = _myHashTagSplitterFactory.Create(_hashTagSplitterName);
            return (_myhashTagSplitter.LoadModel(_hashTagSplitterModelFile));

        }

        public bool CreateTokenizer()
        {

            _myTokenizer = _myTokenizerFactory.Create(_tokenizerName);
            return (_myTokenizer != null) ? true : false;

        }


        public bool CreateTrendExtractor()
        {
            _myTrendExtractor = _myTrendExtractorFactory.Create(_trendExtractorName);
            return (_myTrendExtractor != null) ? true : false;
        }

        public bool CreatePOSTagger()
        {
            Console.WriteLine("Loading POS Tagger Model. This may take few seconds.");
            _myposTagger = _myPOSTaggerFactory.Create(_posTaggerNameName);
            return (_myposTagger.LoadModel(_posTaggerModelFile));

        }

        public void InitializeFromIni(string configureFile)
        {
            IniParser parser = new IniParser(@configureFile);
            _hashTagSplitterModelFile = parser.GetSetting("appSettings", "COMPANY.Modules.HashTagSplitterModelFile");
            _hashTagSplitterName = parser.GetSetting("appSettings", "COMPANY.Modules.HashTagSplitte");

            _tokenizerName = parser.GetSetting("appSettings", "COMPANY.Modules.Tokenizer");
            _trendExtractorName = parser.GetSetting("appSettings", "COMPANY.Modules.TrendExtractor");

            _myNERModelFile = parser.GetSetting("appSettings", "COMPANY.Modules.NamedEntityRecognizerModelFile");
            _nerName = parser.GetSetting("appSettings", "COMPANY.Modules.NamedEntityRecognizer");

            _posTaggerModelFile = parser.GetSetting("appSettings", "COMPANY.Modules.POSTaggerModelFile");
            _posTaggerNameName = parser.GetSetting("appSettings", "COMPANY.Modules.POSTagging");


            NaesForTrends = parser.GetSetting("appSettings", "COMPANY.Modules.NamesForTrends");
            InputFileName = parser.GetSetting("appSettings", "COMPANY.Modules.InputFile");
            OutputFolder = parser.GetSetting("appSettings", "COMPANY.Modules.OutPutFolder");

            InitiateModules();
        }

    }
}
