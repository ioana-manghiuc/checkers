using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Services;
using System.Xml.Linq;
using System.Windows.Input;
using checkers_.Commands;

namespace checkers_.ViewModels
{
    class StatisticsViewModel : BaseNotification
    {
        private StatisticsHelper sh;
        private ICommand reload;
        public StatisticsViewModel()
        {
            sh = new StatisticsHelper(this);
            BlackWins = sh.BlackWins;
            RedWins = sh.RedWins;
            MaxBlackPieces = sh.MaxBlackPieces;
            MaxRedPieces = sh.MaxRedPieces;
            sh.LoadStatistics();
        }

        public StatisticsViewModel(StatisticsHelper sh)
        {
            this.sh = sh;
            BlackWins = sh.BlackWins;
            RedWins = sh.RedWins;
            MaxBlackPieces = sh.MaxBlackPieces;
            MaxRedPieces = sh.MaxRedPieces;
        }

        public ICommand ReloadData
        {
            get
            {
                if (reload == null)
                {
                   reload = new RelayCommand<string>(sh.ReloadStatistics);
                }
                return reload;
            }

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
