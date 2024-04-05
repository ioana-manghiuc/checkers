using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using checkers_.Models;

namespace checkers_.Services
{
    class CheckersBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Tile>> board;
        private Tile firstTile, secondTile;
        private bool isFirstClick = true;
        private int redCapturedBlack = 0;
        private int blackCapturedRed = 0;
        public int RedCapturedBlack { get { return redCapturedBlack; } set { redCapturedBlack = value; } }           
        public int BlackCapturedRed { get { return blackCapturedRed; } set { blackCapturedRed = value; } }
        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board)
        {
            this.board = board;
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

        private bool MovementAllowed(Tile first, Tile second)
        {
            if (first.TileType == Tile.ETileType.Red)
            {
                if (first.Line > second.Line && first.Column != second.Column && first.Line - second.Line <= 2)
                {
                    return true;
                }
            }
            else if (first.TileType == Tile.ETileType.Black)
            {
                if (first.Line < second.Line && first.Column != second.Column && second.Line - first.Line <= 2)
                {
                    return true;
                }
            }
            else if(first.TileType == Tile.ETileType.RedKing || first.TileType == Tile.ETileType.BlackKing)
            {
                if ((first.Line > second.Line || first.Line < second.Line ) &&
                    (first.Column != second.Column || first.Column != second.Column) &&
                    (first.Line - second.Line <= 2 || second.Line - first.Line <= 2))
                {
                    return true;
                }
            }
            return false;
        }

        private void SimpleJump(Tile first, Tile second)
        {
            Console.WriteLine("enterted simple jump code");
            int rowDiff = second.Line - first.Line;
            int colDiff = second.Column - first.Column;
            int capturedRow = first.Line + rowDiff / 2;
            int capturedCol = first.Column + colDiff / 2;

            Tile tileToCapture = board[capturedRow][capturedCol];
            tileToCapture.TileType = board[capturedRow][capturedCol].TileType;

            if (tileToCapture.TileType != first.TileType && tileToCapture.TileType != Tile.ETileType.Empty)
            {
                Console.WriteLine("enterted if");
                if (first.TileType == Tile.ETileType.Red || first.TileType == Tile.ETileType.RedKing)
                    RedCapturedBlack++;
                else if (first.TileType == Tile.ETileType.Black || first.TileType == Tile.ETileType.BlackKing)
                    BlackCapturedRed++;

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
                MessageBox.Show("you can't move there!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SwapTiles(Tile tile)
        {
            if (isFirstClick)
            {
                if (tile.TileType != Tile.ETileType.Empty && tile.TileType != Tile.ETileType.AlwaysEmpty)
                {
                    firstTile = tile;
                    isFirstClick = false;
                }
            }
            else
            {
                secondTile = tile;
                if (secondTile.TileType != Tile.ETileType.AlwaysEmpty)
                {
                    if (MovementAllowed(firstTile, secondTile))
                    {
                        if (secondTile.TileType == Tile.ETileType.Empty)
                        {  
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

            Console.WriteLine("Red captured black: " + RedCapturedBlack);
            Console.WriteLine("Black captured red: " + BlackCapturedRed);
        }
        public void ClickAction(Tile tile)
        {
            SwapTiles(tile);
        }
    }
}
