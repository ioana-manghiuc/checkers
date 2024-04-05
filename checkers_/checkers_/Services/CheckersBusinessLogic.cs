using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using checkers_.Models;

namespace checkers_.Services
{
    class CheckersBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Tile>> board;
        private Tile firstTile, secondTile, simpleJumpTile;
        private bool isFirstClick = true;
        private int redPieces = 12;
        private int blackPieces = 12;
        private int redCapturedBlack = 0;
        private int blackCapturedRed = 0;
        public int RedCapturedBlack { get { return redCapturedBlack; } set { redCapturedBlack = value; } }           
        public int BlackCapturedRed { get { return blackCapturedRed; } set { blackCapturedRed = value; } }
        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board)
        {
            this.board = board;
        }      

        private void SwapTilesContent(Tile first, Tile second)
        {
            string tempImage = first.Image;
            Tile.ETileType tempType = first.TileType;

            first.Image = second.Image;
            first.TileType = second.TileType;

            second.Image = tempImage;
            second.TileType = tempType;
        }

        private bool MovementAllowed(Tile first, Tile second)
        {
            if (first.TileType == Tile.ETileType.Red)
            {
                if ((first.Line - 1 == second.Line) && (first.Column - 1 == second.Column || first.Column + 1 == second.Column))
                {
                    return true;
                }
            }
            else if (first.TileType == Tile.ETileType.Black)
            {
                if ((first.Line + 1 == second.Line) && (first.Column - 1 == second.Column || first.Column + 1 == second.Column))
                {
                    return true;
                }
            }
            else if(first.TileType == Tile.ETileType.RedKing || first.TileType == Tile.ETileType.BlackKing)
            {
                if ((first.Line - 1 == second.Line || first.Line + 1 == second.Line) && 
                    (first.Column - 1 == second.Column || first.Column + 1 == second.Column))
                {
                    return true;
                }
            }
            return false;
        }

        // Multiple jump

        private void CapturePiece(Tile first, Tile second) // capture should be a simple jump. im so done.
        {
            if (first.TileType == Tile.ETileType.Red)
            {
                blackPieces--;
                RedCapturedBlack++;
            }
            if(first.TileType == Tile.ETileType.Black)
            {
                redPieces--;
                BlackCapturedRed++;
            }

            second.TileType = first.TileType;
            second.Image = first.Image;
            first.TileType = Tile.ETileType.Empty;
            first.Image = "/checkers_;component/Resources/empty_cell.png";
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
                            board[firstTile.Line][firstTile.Column].Line = secondTile.Line;
                            board[firstTile.Line][firstTile.Column].Column = secondTile.Column;
                            SwapTilesContent(firstTile, secondTile);
                        }
                        else if (secondTile.TileType != firstTile.TileType)
                        {
                            CapturePiece(firstTile, secondTile);
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
            Console.WriteLine("Red pieces: " + redPieces);
            Console.WriteLine("Black pieces: " + blackPieces);
            Console.WriteLine("Red captured black: " + redCapturedBlack);
            Console.WriteLine("Black captured red: " + blackCapturedRed);
        }
        public void ClickAction(Tile tile)
        {
            SwapTiles(tile);
        }
    }
}
