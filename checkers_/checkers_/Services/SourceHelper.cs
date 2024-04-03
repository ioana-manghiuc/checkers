using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Models;

namespace checkers_.Services
{
    class SourceHelper
    {
        public static ObservableCollection<ObservableCollection<Tile>> InitializeGameBoard()
        {
            ObservableCollection<ObservableCollection<Tile>> board = new ObservableCollection<ObservableCollection<Tile>>();

            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();

                for (int j = 0; j < 8; j++)
                {
                    string image = "";
                    if (i < 3)
                    {
                        if ((i + j) % 2 == 1)
                            image = "/checkers_;component/Resources/man_black.png";
                        else
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                    }
                    else if (i >= 5)
                    {
                        if ((i + j) % 2 == 1)
                            image = "/checkers_;component/Resources/man_red.png";
                        else
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                    }
                    else
                    {
                        if ((i + j) % 2 == 1)
                            image = "/checkers_;component/Resources/empty_cell.png";
                        else
                            image = "/checkers_;component/Resources/always_empty_cell.png";
                    }

                    Tile tile = new Tile(i, j, image);
                    row.Add(tile);
                }

                board.Add(row);
            }

            return board;
        }
    }
}
