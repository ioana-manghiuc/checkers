﻿using checkers_.Models;
using checkers_.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace checkers_.ViewModels
{
    class SavedGameViewModel : BaseNotification
    {
        private CheckersBusinessLogic cbl;

        public SavedGameViewModel()
        {
            ObservableCollection<ObservableCollection<Tile>> board = SourceHelper.RestoreGameBoard("Resources/saved_boards.xml");
            cbl = new CheckersBusinessLogic(board, this);
            SavedGameBoard = CellBoardToCellVMBoard(board);
            RedCapturedBlack = cbl.RedCapturedBlack;
            BlackCapturedRed = cbl.BlackCapturedRed;
            RedPieces = cbl.RedPieces;
            BlackPieces = cbl.BlackPieces;
            RedWin = cbl.RedWin;
            BlackWin = cbl.BlackWin;
            RedTurn = cbl.RedsTurn;
            BlackTurn = cbl.BlackTurn;
        }

        private int redCapturedBlack = 0;
        public int RedCapturedBlack
        {
            get { return redCapturedBlack; }
            set
            {
                if (value != redCapturedBlack)
                {
                    redCapturedBlack = value;
                    NotifyPropertyChanged("redCapturedBlack");
                }
            }
        }

        private int blackCapturedRed = 0;
        public int BlackCapturedRed
        {
            get { return blackCapturedRed; }
            set
            {
                if (value != blackCapturedRed)
                {
                    blackCapturedRed = value;
                    NotifyPropertyChanged("blackCapturedRed");
                }
            }
        }

        private int redPieces = 12;
        public int RedPieces
        {
            get { return redPieces; }
            set
            {
                if (value != redPieces)
                {
                    redPieces = value;
                    NotifyPropertyChanged("RedPieces");
                }
            }
        }

        private int blackPieces = 12;
        public int BlackPieces
        {
            get { return blackPieces; }
            set
            {
                if (value != blackPieces)
                {
                    blackPieces = value;
                    NotifyPropertyChanged("BlackPieces");
                }
            }
        }

        private string redWin = "";
        public string RedWin
        {
            get { return redWin; }
            set
            {
                if (value != redWin)
                {
                    redWin = value;
                    NotifyPropertyChanged("RedWin");
                }
            }
        }

        private string blackWin = "";
        public string BlackWin
        {
            get { return blackWin; }
            set
            {
                if (value != blackWin)
                {
                    blackWin = value;
                    NotifyPropertyChanged("BlackWin");
                }
            }
        }

        private string redTurn = "";
        public string RedTurn
        {
            get { return redTurn; }
            set
            {
                if (value != redTurn)
                {
                    redTurn = value;
                    NotifyPropertyChanged("RedTurn");
                }
            }
        }
        public string blackTurn = "";
        public string BlackTurn
        {
            get { return blackTurn; }
            set
            {
                if (value != blackTurn)
                {
                    blackTurn = value;
                    NotifyPropertyChanged("BlackTurn");
                }
            }
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

        public ObservableCollection<ObservableCollection<TileViewModel>> SavedGameBoard { get; set; }

    }
}