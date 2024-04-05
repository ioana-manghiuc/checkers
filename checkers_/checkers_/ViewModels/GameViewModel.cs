using checkers_.Services;
using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers_.ViewModels
{
    class GameViewModel : BaseNotification
    {
        private int redCapturedBlack;
        public int RedCapturedBlack
        {
            get { return redCapturedBlack; }
            set
            {
                if (value != redCapturedBlack)
                {
                    redCapturedBlack = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int blackCapturedRed;
        public int BlackCapturedRed
        {
            get { return blackCapturedRed; }
            set
            {
                if (value != blackCapturedRed)
                {
                    blackCapturedRed = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private CheckersBusinessLogic cbl;

        public GameViewModel()
        {
            ObservableCollection<ObservableCollection<Tile>> board = SourceHelper.InitializeGameBoard();
            cbl = new CheckersBusinessLogic(board);
            GameBoard = CellBoardToCellVMBoard(board);
        }

        private ObservableCollection<ObservableCollection<TileViewModel>> CellBoardToCellVMBoard(ObservableCollection<ObservableCollection<Tile>> board)
        {
            ObservableCollection<ObservableCollection<TileViewModel>> result = new ObservableCollection<ObservableCollection<TileViewModel>>();
            for (int i = 0; i < board.Count; i++)
            {
                ObservableCollection<TileViewModel> line = new ObservableCollection<TileViewModel>();
                for (int j = 0; j < board[i].Count; j++)
                {
                    Tile c = board[i][j];
                    TileViewModel tileVM = new TileViewModel(c.Line, c.Column, c.Image, c.TileType, cbl);
                    line.Add(tileVM);
                }
                result.Add(line);
            }
            return result;
        }

        public ObservableCollection<ObservableCollection<TileViewModel>> GameBoard { get; set; }
    }
}
