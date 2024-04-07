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
            BlackWins = int.Parse(stats.Element("BlackWins").Value);
            RedWins = int.Parse(stats.Element("RedWins").Value);
            MaxBlackPieces = int.Parse(stats.Element("MaxBlackPieces").Value);
            MaxRedPieces = int.Parse(stats.Element("MaxRedPieces").Value);
        }

        public void SaveStatistics(bool blackState, bool redState, int winnersPieces)
        {
            if(blackState)
            {
                BlackWins++;
                if(winnersPieces > MaxBlackPieces)
                {
                    MaxBlackPieces = winnersPieces;
                }
            }
            else if(redState)
            {
                RedWins++;
                if(winnersPieces > MaxRedPieces)
                {
                    MaxRedPieces = winnersPieces;
                }
            }

            XDocument doc = XDocument.Load("Resources/statistics.xml");
            var stats = doc.Descendants("Statistics").First();
            stats.Element("BlackWins").Value = BlackWins.ToString();
            stats.Element("RedWins").Value = RedWins.ToString();
            stats.Element("MaxBlackPieces").Value = MaxBlackPieces.ToString();
            stats.Element("MaxRedPieces").Value = MaxRedPieces.ToString();
            doc.Save("Resources/statistics.xml");

            LoadStatistics();
        }
    }
}
