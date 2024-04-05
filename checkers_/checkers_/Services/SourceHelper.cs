using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Models;
using System.Net.NetworkInformation;

namespace checkers_.Services
{
    class SourceHelper
    {

        public static string ManBlack = "/checkers_;component/Resources/man_black.png";
        public static string ManRed = "/checkers_;component/Resources/man_red.png";
        public static string EmptyCell = "/checkers_;component/Resources/empty_cell.png";
        public static string AlwaysEmptyCell = "/checkers_;component/Resources/always_empty_cell.png";
        public static string KingBlack = "/checkers_;component/Resources/king_black.png";
        public static string KingRed = "/checkers_;component/Resources/king_red.png";

        public static Tile CurrentTile { get; set;}
        public static Tile NextTile { get; set; }
        public static ObservableCollection<ObservableCollection<Tile>> InitializeGameBoard()
        {
            ObservableCollection<ObservableCollection<Tile>> board = new ObservableCollection<ObservableCollection<Tile>>();

            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();

                for (int j = 0; j < 8; j++)
                {
                    string image = "";
                    Tile.ETileType type = Tile.ETileType.None;
                    if (i < 3)
                    {
                        if ((i + j) % 2 == 1)
                        {
                            image = "/checkers_;component/Resources/man_black.png";
                            type = Tile.ETileType.Black;
                        }
                        else
                        {
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                            type = Tile.ETileType.AlwaysEmpty;

                        }

                    }
                    else if (i >= 5)
                    {
                        if ((i + j) % 2 == 1)
                        {
                            image = "/checkers_;component/Resources/man_red.png";
                            type = Tile.ETileType.Red;
                        }
                           
                        else
                        {
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                            type = Tile.ETileType.AlwaysEmpty;
                        }        
                    }
                    else
                    {
                        if ((i + j) % 2 == 1)
                        {
                            image = "/checkers_;component/Resources/empty_cell.png";
                            type = Tile.ETileType.Empty;
                        }
                            
                        else
                        {
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                            type = Tile.ETileType.AlwaysEmpty;
                        }                        
                    }

                    Tile tile = new Tile(i, j, image, type);
                    row.Add(tile);
                }

                board.Add(row);
            }

            return board;
        }
    }
}
