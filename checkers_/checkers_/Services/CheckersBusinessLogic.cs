using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using checkers_.Models;
using checkers_.ViewModels;

namespace checkers_.Services
{
    class CheckersBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Tile>> board;
        private GameViewModel gvm;
        private Tile firstTile, secondTile;
        private bool isFirstClick = true;
        private int redCapturedBlack = 0;
        private int blackCapturedRed = 0;
        private int redPieces = 12;
        private int blackPieces = 12;
        private bool redTurn = true;
        public int RedCapturedBlack { get { return redCapturedBlack; } set { redCapturedBlack = value; } }           
        public int BlackCapturedRed { get { return blackCapturedRed; } set { blackCapturedRed = value; } }
        public int RedPieces { get { return redPieces; } set { redPieces = value; } }
        public int BlackPieces { get { return blackPieces; } set { blackPieces = value; } }
        public string RedWin { get; set; }
        public string BlackWin { get; set; }
        public bool RedTurn { get { return redTurn; } set { redTurn = value; } }
        public string RedsTurn { get; set; }
        public string BlackTurn { get; set; }
            
        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board, GameViewModel gameViewModel)
        {
            this.board = board;
            this.gvm = gameViewModel;
            RedsTurn = "RED TURN";
        }      

        private bool MovementAllowed(Tile first, Tile second)
        {

            if (first.TileType == Tile.ETileType.Red && RedTurn)
            {
                if (first.Line > second.Line && first.Column != second.Column && first.Line - second.Line <= 2)
                {
                    return true;
                }
            }
            else if (first.TileType == Tile.ETileType.Black && !RedTurn)
            {
                if (first.Line < second.Line && first.Column != second.Column && second.Line - first.Line <= 2)
                {
                    return true;
                }
            }
            else if ((first.TileType == Tile.ETileType.RedKing && RedTurn) || (first.TileType == Tile.ETileType.BlackKing && !RedTurn))
            {
                if ((first.Line > second.Line || first.Line < second.Line) &&
                    (first.Column != second.Column || first.Column != second.Column) &&
                    (first.Line - second.Line <= 2 || second.Line - first.Line <= 2))
                {
                    return true;
                }
            }
            return false;
        }

        private void SwitchTurn(Tile one)
        {
            if (one.TileType == Tile.ETileType.Red || one.TileType == Tile.ETileType.RedKing)
            {
                RedTurn = false;
                gvm.RedTurn = "";
                gvm.BlackTurn = "BLACK TURN";
            }
            else if (one.TileType == Tile.ETileType.Black || one.TileType == Tile.ETileType.BlackKing)
            {
                RedTurn = true;
                gvm.BlackTurn = "";
                gvm.RedTurn = "RED TURN";
            }
        }

        private void SimpleMove(Tile first, Tile second)
        {
            string tempImage = first.Image;
            Tile.ETileType tempType = first.TileType;

            first.Image = second.Image;
            first.TileType = second.TileType;

            second.Image = tempImage;
            second.TileType = tempType;

            board[first.Line][first.Column] = first;
            board[second.Line][second.Column] = second;
        }

        private void SimpleJump(Tile first, Tile second)
        {
            int rowDiff = second.Line - first.Line;
            int colDiff = second.Column - first.Column;
            int capturedRow = first.Line + rowDiff / 2;
            int capturedCol = first.Column + colDiff / 2;

            Tile tileToCapture = board[capturedRow][capturedCol];
            tileToCapture.TileType = board[capturedRow][capturedCol].TileType;

            if (tileToCapture.TileType != first.TileType && tileToCapture.TileType != Tile.ETileType.Empty)
            {
                if (first.TileType == Tile.ETileType.Red || first.TileType == Tile.ETileType.RedKing)
                {
                    gvm.RedCapturedBlack++; 
                    gvm.BlackPieces--;
                    if(gvm.RedCapturedBlack == 12)
                    {
                        gvm.RedTurn = "";
                        gvm.BlackTurn = ""; 
                        gvm.RedWin = "RED WINS";
                    }
                }
                else if (first.TileType == Tile.ETileType.Black || first.TileType == Tile.ETileType.BlackKing)
                {
                    gvm.BlackCapturedRed++;
                    gvm.RedPieces--;
                    if (gvm.BlackCapturedRed == 12)
                    {
                        gvm.RedTurn = "";
                        gvm.BlackTurn = "";
                        gvm.BlackWin = "BLACK WINS";                        
                    }
                }

                second.TileType = first.TileType;
                second.Image = first.Image;
                first.TileType = Tile.ETileType.Empty;
                first.Image = "/checkers_;component/Resources/empty_cell.png";

                board[first.Line][first.Column] = first;
                board[second.Line][second.Column] = second;
                board[capturedRow][capturedCol].TileType = Tile.ETileType.Empty;
                board[capturedRow][capturedCol].Image = "/checkers_;component/Resources/empty_cell.png";
            }
            else
            {
                MessageBox.Show("you can't move there! - jump", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SwapTiles(Tile tile)
        {
            if (isFirstClick)
            {
                if (tile.TileType != Tile.ETileType.Empty && tile.TileType != Tile.ETileType.AlwaysEmpty)
                {
                    if(RedTurn && (tile.TileType == Tile.ETileType.Black || tile.TileType == Tile.ETileType.BlackKing))
                    {
                        MessageBox.Show("it's red's turn!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (!RedTurn && (tile.TileType == Tile.ETileType.Red || tile.TileType == Tile.ETileType.RedKing))
                    {
                        MessageBox.Show("it's black's turn!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        firstTile = tile;
                        isFirstClick = false;
                    }
                }
            }
            else
            {
                secondTile = tile;
                if (secondTile.TileType != Tile.ETileType.AlwaysEmpty)
                {
                    if (MovementAllowed(firstTile, secondTile) && gvm.RedPieces != 0 && gvm.BlackPieces != 0)
                    {
                        if (secondTile.TileType == Tile.ETileType.Empty)
                        {
                            SwitchTurn(firstTile);
                            // move conditions for men
                            if ((firstTile.TileType == Tile.ETileType.Red && firstTile.Line - secondTile.Line == 1) ||
                                (firstTile.TileType == Tile.ETileType.Black && secondTile.Line - firstTile.Line == 1))
                            {
                                SimpleMove(firstTile, secondTile);
                            }
                            if ((firstTile.TileType == Tile.ETileType.Black && secondTile.Line - firstTile.Line == 2) || 
                                (firstTile.TileType == Tile.ETileType.Red && firstTile.Line - secondTile.Line == 2))
                            {
                                SimpleJump(firstTile, secondTile);
                            }

                            // move conditions for kings
                            if ((firstTile.TileType == Tile.ETileType.RedKing || firstTile.TileType == Tile.ETileType.BlackKing) &&
                                (firstTile.Line - secondTile.Line == 1 || secondTile.Line - firstTile.Line == 1))
                            {
                                SimpleMove(firstTile, secondTile);
                            }
                            if ((firstTile.TileType == Tile.ETileType.RedKing || firstTile.TileType == Tile.ETileType.BlackKing) &&
                                (firstTile.Line - secondTile.Line == 2 || secondTile.Line - firstTile.Line == 2))
                            {
                                SimpleJump(firstTile, secondTile);
                            }                           
                        }
                        else
                        {
                            MessageBox.Show("you can't move there!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        isFirstClick = true;                        
                    }
                }
            }

            if (tile.TileType == Tile.ETileType.Red && tile.Line == 0)
            {
                tile.TileType = Tile.ETileType.RedKing;
                tile.Image = "/checkers_;component/Resources/king_red.png";
            }

            if (tile.TileType == Tile.ETileType.Black && tile.Line == 7)
            {
                tile.TileType = Tile.ETileType.BlackKing;
                tile.Image = "/checkers_;component/Resources/king_black.png";
            }
        }
        public void ClickAction(Tile tile)
        {
            SwapTiles(tile);
        }
    }
}
