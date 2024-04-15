using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using checkers_.Models;
using checkers_.ViewModels;
using static checkers_.Services.SourceHelper;

namespace checkers_.Services
{
    class CheckersBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Tile>> board;
        private GameViewModel gvm;
        private SavedGameViewModel sgvm;
        private StatisticsHelper sh = new StatisticsHelper();

        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board, SavedGameViewModel sgvm)
        {
            this.board = board;
            this.sgvm = sgvm;
            GameInfo gameInfo = SourceHelper.GetGameWithID(SourceHelper.GameID);

            if (gameInfo != null)
            {
                RedTurn = gameInfo.Turn;
                if (RedTurn)
                {
                    BlackTurn = "";
                    RedsTurn = "RED TURN";
                }
                else
                {
                    RedsTurn = "";
                    BlackTurn = "BLACK TURN";
                }
                RedCapturedBlack = gameInfo.CapturedBlackPieces;
                BlackCapturedRed = gameInfo.CapturedRedPieces;
                RedPieces = gameInfo.RedPieces;
                BlackPieces = gameInfo.BlackPieces;
                MultipleJumpsAllowed = gameInfo.MultipleJumpsAllowed;
                if (MultipleJumpsAllowed)
                    Modifier = 1;
                else if (!MultipleJumpsAllowed)
                    Modifier = 2;
            }
            else
            {
                Console.WriteLine("Error: Unable to parse game information.");
            }
        }

        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board, GameViewModel gameViewModel)
        {
            this.board = board;
            this.gvm = gameViewModel;
            RedsTurn = "RED TURN";           
            MultipleJumpsAllowed = SourceHelper.GetJumpOptionStatus();
            Console.WriteLine("\nMULTIPLE JUMPS constr: " + MultipleJumpsAllowed + "\n");
        }      

        private string TileSelected(Tile tile)
        {
            if (tile.TileType == Tile.ETileType.Red)
            {
                return "/checkers_;component/Resources/selected_man_red.png";
            }
            else if (tile.TileType == Tile.ETileType.Black)
            {
                return "/checkers_;component/Resources/selected_man_black.png";
            }
            else if (tile.TileType == Tile.ETileType.BlackKing)
            {
                return "/checkers_;component/Resources/selected_king_black.png";
            }
            else if (tile.TileType == Tile.ETileType.RedKing)
            {
                return "/checkers_;component/Resources/selected_king_red.png";
            }
            return "";
        }

        private string DeselectTile(Tile tile)
        {
            if (tile.TileType == Tile.ETileType.Red)
            {
                return "/checkers_;component/Resources/man_red.png";
            }
            else if (tile.TileType == Tile.ETileType.Black)
            {
                return "/checkers_;component/Resources/man_black.png";
            }
            else if (tile.TileType == Tile.ETileType.BlackKing)
            {
                return "/checkers_;component/Resources/king_black.png";
            }
            else if (tile.TileType == Tile.ETileType.RedKing)
            {
                return "/checkers_;component/Resources/king_red.png";
            }
            return "";
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
                if (gvm != null)
                {
                    gvm.RedTurn = "";
                    gvm.BlackTurn = "BLACK TURN";

                }
                else
                {
                    sgvm.RedTurn = "";
                    sgvm.BlackTurn = "BLACK TURN";
                }

            }
            else if (one.TileType == Tile.ETileType.Black || one.TileType == Tile.ETileType.BlackKing)
            {
                RedTurn = true;
                if (gvm != null)
                {
                    gvm.BlackTurn = "";
                    gvm.RedTurn = "RED TURN";
                }
                else
                {
                    sgvm.BlackTurn = "";
                    sgvm.RedTurn = "RED TURN";
                }
            }
        }
        private bool IsValidPosition(int line, int column)
        {
            return line >= 0 && line < board.Count && column >= 0 && column < board[0].Count;
        }
        private bool RedJumpAvailable(Tile first)
        {
            if (IsValidPosition(first.Line - 1, first.Column - 1) &&
                IsValidPosition(first.Line - 2, first.Column - 2) &&
                ((board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.Black) ||
                (board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.BlackKing)) &&
                (board[first.Line - 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line - 1, first.Column + 1) &&
                     IsValidPosition(first.Line - 2, first.Column + 2) &&
                     ((board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.Black) ||
                     (board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.BlackKing)) &&
                     (board[first.Line - 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }

            return false;
        }

        private bool BlackJumpAvailable(Tile first)
        {
            if (IsValidPosition(first.Line + 1, first.Column - 1) &&
                IsValidPosition(first.Line + 2, first.Column - 2) &&
                ((board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.Red) ||
                (board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.RedKing)) &&
                (board[first.Line + 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line + 1, first.Column + 1) &&
                     IsValidPosition(first.Line + 2, first.Column + 2) &&
                     ((board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.Red) ||
                     (board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.RedKing)) &&
                     (board[first.Line + 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }

            return false;
        }

        private bool RedKingJumpAvailable(Tile first)
        {
            if (IsValidPosition(first.Line - 1, first.Column - 1) &&
                IsValidPosition(first.Line - 2, first.Column - 2) &&
                ((board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.Black) ||
                (board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.BlackKing)) &&
                (board[first.Line - 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line - 1, first.Column + 1) &&
                     IsValidPosition(first.Line - 2, first.Column + 2) &&
                     ((board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.Black) ||
                     (board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.BlackKing)) &&
                     (board[first.Line - 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            if (IsValidPosition(first.Line + 1, first.Column - 1) &&
                IsValidPosition(first.Line + 2, first.Column - 2) &&
                ((board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.Black) ||
                (board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.BlackKing)) &&
                (board[first.Line + 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line + 1, first.Column + 1) &&
                     IsValidPosition(first.Line + 2, first.Column + 2) &&
                     ((board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.Black) ||
                     (board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.BlackKing)) &&
                     (board[first.Line + 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            return false;
        }

        private bool BlackKingJumpAvailable(Tile first)
        {
            if (IsValidPosition(first.Line - 1, first.Column - 1) &&
                IsValidPosition(first.Line - 2, first.Column - 2) &&
                ((board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.Black) ||
                (board[first.Line - 1][first.Column - 1].TileType == Tile.ETileType.BlackKing)) &&
                (board[first.Line - 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line - 1, first.Column + 1) &&
                     IsValidPosition(first.Line - 2, first.Column + 2) &&
                     ((board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.Black) ||
                     (board[first.Line - 1][first.Column + 1].TileType == Tile.ETileType.BlackKing)) &&
                     (board[first.Line - 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            if (IsValidPosition(first.Line + 1, first.Column - 1) &&
                IsValidPosition(first.Line + 2, first.Column - 2) &&
                ((board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.Black) ||
                (board[first.Line + 1][first.Column - 1].TileType == Tile.ETileType.BlackKing)) &&
                (board[first.Line + 2][first.Column - 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            else if (IsValidPosition(first.Line + 1, first.Column + 1) &&
                     IsValidPosition(first.Line + 2, first.Column + 2) &&
                     ((board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.Black) ||
                     (board[first.Line + 1][first.Column + 1].TileType == Tile.ETileType.BlackKing)) &&
                     (board[first.Line + 2][first.Column + 2].TileType == Tile.ETileType.Empty))
            {
                return true;
            }
            return false;
        }

        private bool JumpAvailable(Tile first)
        {
            if (first.TileType == Tile.ETileType.Red)
            {
                return RedJumpAvailable(first);
            }
            else if (first.TileType == Tile.ETileType.Black)
            {
                return BlackJumpAvailable(first);
            }
            else if (first.TileType == Tile.ETileType.RedKing)
            {
                return RedKingJumpAvailable(first);
            }
            else if (first.TileType == Tile.ETileType.BlackKing)
            {
                return BlackKingJumpAvailable(first);
            }
            return false;
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
                    if (gvm != null)
                    {
                        gvm.RedCapturedBlack++;
                        gvm.BlackPieces--;
                        if (gvm.RedCapturedBlack == 12)
                        {
                            gvm.RedTurn = "";
                            gvm.BlackTurn = "";
                            gvm.RedWin = "RED WINS";
                            sh.SaveStatistics(false, true, gvm.RedPieces);
                        }
                    }
                    else
                    {
                        sgvm.RedCapturedBlack++;
                        sgvm.BlackPieces--;
                        if (sgvm.RedCapturedBlack == 12)
                        {
                            sgvm.RedTurn = "";
                            sgvm.BlackTurn = "";
                            sgvm.RedWin = "RED WINS";
                            sh.SaveStatistics(false, true, sgvm.RedPieces);
                        }
                    }
                }
                else if (first.TileType == Tile.ETileType.Black || first.TileType == Tile.ETileType.BlackKing)
                {
                    if (gvm != null)
                    {
                        gvm.BlackCapturedRed++;
                        gvm.RedPieces--;
                        if (gvm.BlackCapturedRed == 12)
                        {
                            gvm.RedTurn = "";
                            gvm.BlackTurn = "";
                            gvm.BlackWin = "BLACK WINS";
                            sh.SaveStatistics(true, false, gvm.BlackPieces);
                        }
                    }
                    else
                    {
                        sgvm.BlackCapturedRed++;
                        sgvm.RedPieces--;
                        if (sgvm.BlackCapturedRed == 12)
                        {
                            sgvm.RedTurn = "";
                            sgvm.BlackTurn = "";
                            sgvm.BlackWin = "BLACK WINS";
                            sh.SaveStatistics(true, false, sgvm.BlackPieces);
                        }
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

                if (gvm != null)
                {
                    gvm.UpdateBoard(board[capturedRow][capturedCol]);
                }
                else
                {
                    sgvm.UpdateBoard(board[capturedRow][capturedCol]);
                }
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
                        GameStarted = true;
                        if(gvm != null)
                        {
                            Jumps = gvm.MultipleJumpsAllowed;
                            MultipleJumpsAllowed = gvm.MultipleJumpsAllowed;
                            SourceHelper.SetJumpOptionStatus(gvm.MultipleJumpsAllowed);
                        }                       
                        Console.WriteLine("MULTIPLE JUMPS after game started:" + MultipleJumpsAllowed);
                        tile.Image = TileSelected(tile);
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
                    if (MovementAllowed(firstTile, secondTile) && 
                            (( gvm != null && gvm.RedPieces != 0 && gvm.BlackPieces != 0  ) || (sgvm.RedPieces != 0 && sgvm.BlackPieces != 0 && sgvm != null)))
                    {
                        if (secondTile.TileType == Tile.ETileType.Empty)
                        {                           
                            // move conditions for men
                            if ((firstTile.TileType == Tile.ETileType.Red && firstTile.Line - secondTile.Line == 1) ||
                                (firstTile.TileType == Tile.ETileType.Black && secondTile.Line - firstTile.Line == 1))
                            {
                                SimpleMove(firstTile, secondTile);
                                SwitchTurn(secondTile);
                            }
                            if ((firstTile.TileType == Tile.ETileType.Black && secondTile.Line - firstTile.Line == 2) || 
                                (firstTile.TileType == Tile.ETileType.Red && firstTile.Line - secondTile.Line == 2))
                            {
                                SimpleJump(firstTile, secondTile);
                                while (JumpAvailable(secondTile) && MultipleJumpsAllowed)
                                {
                                    firstTile = secondTile;
                                    isFirstClick = false;
                                    tile.Image = DeselectTile(tile);
                                    return;
                                }
                                SwitchTurn(secondTile);
                            }

                            // move conditions for kings
                            if ((firstTile.TileType == Tile.ETileType.RedKing || firstTile.TileType == Tile.ETileType.BlackKing) &&
                                (firstTile.Line - secondTile.Line == 1 || secondTile.Line - firstTile.Line == 1))
                            {
                                SimpleMove(firstTile, secondTile);
                                SwitchTurn(secondTile);
                            }
                            if ((firstTile.TileType == Tile.ETileType.RedKing || firstTile.TileType == Tile.ETileType.BlackKing) &&
                                (firstTile.Line - secondTile.Line == 2 || secondTile.Line - firstTile.Line == 2))
                            {
                                SimpleJump(firstTile, secondTile);
                                SwitchTurn(secondTile);
                            }
                            //SwitchTurn(secondTile);
                        }
                        else
                        {
                            MessageBox.Show("you can't move there!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        isFirstClick = true;
                        tile.Image = DeselectTile(tile);
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

        public void SaveGame(object id)
        {
            int id2 = SourceHelper.GetNextGameID();
            if (gvm != null)
            {
                SourceHelper.SaveGameInfo(id2, RedTurn, gvm.BlackPieces, gvm.RedPieces, gvm.RedCapturedBlack, gvm.BlackCapturedRed, gvm.MultipleJumpsAllowed);
                SourceHelper.SaveBoard(id2, board);
            }
            else
            {
                SourceHelper.SaveGameInfo(id2, RedTurn, sgvm.BlackPieces, sgvm.RedPieces, sgvm.RedCapturedBlack, sgvm.BlackCapturedRed, sgvm.MultipleJumpsAllowed);
                SourceHelper.SaveBoard(id2, board);
            }          
        }

        public void MultipleJumpModifier(object obj)
        {
            if (!GameStarted)
            {
                if (gvm != null)
                {
                    gvm.MultipleJumpsAllowed = !gvm.MultipleJumpsAllowed;
                    MultipleJumpsAllowed = gvm.MultipleJumpsAllowed;
                    Jumps = gvm.MultipleJumpsAllowed;
                    Console.WriteLine("jumps in modifier" + Jumps);
                }
            }
            else
            {
                MessageBox.Show("You can't change this setting during the game!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Tile firstTile, secondTile;
        private bool isFirstClick = true;
        private int redCapturedBlack = 0;
        private int blackCapturedRed = 0;
        private int redPieces = 12;
        private int blackPieces = 12;
        private bool redTurn = true;
        private bool multipleJumpsAllowed = false;
        private static int modifier = 0;
        private static bool gameStarted = false;
        private static bool jumps = false;
        public int RedCapturedBlack { get { return redCapturedBlack; } set { redCapturedBlack = value; } }
        public int BlackCapturedRed { get { return blackCapturedRed; } set { blackCapturedRed = value; } }
        public int RedPieces { get { return redPieces; } set { redPieces = value; } }
        public int BlackPieces { get { return blackPieces; } set { blackPieces = value; } }
        public string RedWin { get; set; }
        public string BlackWin { get; set; }
        public bool RedTurn { get { return redTurn; } set { redTurn = value; } }
        public string RedsTurn { get; set; }
        public string BlackTurn { get; set; }
        public bool MultipleJumpsAllowed { get { return multipleJumpsAllowed; } set { multipleJumpsAllowed = value; } }
        public static int Modifier { get { return modifier; } set { modifier = value; } }
        public static bool GameStarted { get { return gameStarted; } set { gameStarted = value; } }
        public static bool Jumps{ get { return jumps; } set { jumps = value; } }
    }
}
