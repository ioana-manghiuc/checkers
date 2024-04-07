using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Services;

namespace checkers_.ViewModels
{
    class StatisticsViewModel : BaseNotification
    {
        private StatisticsHelper sh;

        public StatisticsViewModel()
        {
            sh = new StatisticsHelper(this);
            BlackWins = sh.BlackWins;
            RedWins = sh.RedWins;
            MaxBlackPieces = sh.MaxBlackPieces;
            MaxRedPieces = sh.MaxRedPieces;
        }

        private int blackWins = 0;
        public int BlackWins
        {
            get { return blackWins; }
            set
            {
                if (value != blackWins)
                {
                    blackWins = value;
                    NotifyPropertyChanged("BlackWins");
                }
            }
        }

        private int redWins = 0;
        public int RedWins
        {
            get { return redWins; }
            set
            {
                if (value != redWins)
                {
                    redWins = value;
                    NotifyPropertyChanged("RedWins");
                }
            }
        }

        private int maxBlackPieces = 0;
        public int MaxBlackPieces
        {
            get { return maxBlackPieces; }
            set
            {
                if (value != maxBlackPieces)
                {
                    maxBlackPieces = value;
                    NotifyPropertyChanged("MaxBlackPieces");
                }
            }
        }

        private int maxRedPieces = 0;
        public int MaxRedPieces
        {
            get { return maxRedPieces; }
            set
            {
                if (value != maxRedPieces)
                {
                    maxRedPieces = value;
                    NotifyPropertyChanged("MaxRedPieces");
                }
            }
        }
    }
}
