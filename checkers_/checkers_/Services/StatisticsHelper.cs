using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using checkers_.ViewModels;

namespace checkers_.Services
{
    class StatisticsHelper
    {
        private StatisticsViewModel svm;
        private int blackWins = 0;
        private int redWins = 0;
        private int maxBlackPieces = 0;
        private int maxRedPieces = 0;

        public int BlackWins { get { return blackWins; } set { blackWins = value; } }
        public int RedWins { get { return redWins; } set { redWins = value; } }
        public int MaxBlackPieces { get { return maxBlackPieces; } set { maxBlackPieces = value; } }
        public int MaxRedPieces { get { return maxRedPieces; } set { maxRedPieces = value; } }
      

        public StatisticsHelper()
        {
            svm = new StatisticsViewModel(this);
            LoadStatistics();
        }
        public StatisticsHelper(StatisticsViewModel svm)
        {
            this.svm = svm;
            LoadStatistics();
        }

        public void LoadStatistics()
        {
            XDocument doc = XDocument.Load("Resources/statistics.xml");
            var stats = doc.Descendants("Statistics").First();
            svm.BlackWins = int.Parse(stats.Element("BlackWins").Value);
            svm.RedWins = int.Parse(stats.Element("RedWins").Value);
            svm.MaxBlackPieces = int.Parse(stats.Element("MaxBlackPieces").Value);
            svm.MaxRedPieces = int.Parse(stats.Element("MaxRedPieces").Value);
        }
        public void ReloadStatistics(string filePath)
        {
            filePath = "Resources/statistics.xml";
            XDocument doc = XDocument.Load(filePath);
            var stats = doc.Descendants("Statistics").First();
            svm.BlackWins = int.Parse(stats.Element("BlackWins").Value);
            svm.RedWins = int.Parse(stats.Element("RedWins").Value);
            svm.MaxBlackPieces = int.Parse(stats.Element("MaxBlackPieces").Value);
            svm.MaxRedPieces = int.Parse(stats.Element("MaxRedPieces").Value);
        }

        public void SaveStatistics(bool blackState, bool redState, int winnersPieces)
        {
            if(blackState)
            {
                svm.BlackWins++;
                if(winnersPieces > MaxBlackPieces)
                {
                    svm.MaxBlackPieces = winnersPieces;
                }
            }
            else if(redState)
            {
                svm.RedWins++;
                if(winnersPieces > MaxRedPieces)
                {
                    svm.MaxRedPieces = winnersPieces;
                }
            }

            XDocument doc = XDocument.Load("Resources/statistics.xml");
            var stats = doc.Descendants("Statistics").First();
            stats.Element("BlackWins").Value = svm.BlackWins.ToString();
            stats.Element("RedWins").Value = svm.RedWins.ToString();
            stats.Element("MaxBlackPieces").Value = svm.MaxBlackPieces.ToString();
            stats.Element("MaxRedPieces").Value = svm.MaxRedPieces.ToString();
            doc.Save("Resources/statistics.xml");

            ReloadStatistics("Resources/statistics.xml");
        }
    }
}
