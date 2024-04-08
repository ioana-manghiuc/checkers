using checkers_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Models;
using System.Net.NetworkInformation;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace checkers_.Services
{
    class SourceHelper
    {
        public static Tile CurrentTile { get; set; }
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

        public static ObservableCollection<ObservableCollection<Tile>> RestoreGameBoard(string xmlFilePath)
        {
            ObservableCollection<ObservableCollection<Tile>> board = new ObservableCollection<ObservableCollection<Tile>>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);

                XmlNodeList tileNodes = doc.SelectNodes("/SavedBoards/Tiles/Tile");

                foreach (XmlNode tileNode in tileNodes)
                {
                    int line = int.Parse(tileNode.SelectSingleNode("Line").InnerText);
                    int column = int.Parse(tileNode.SelectSingleNode("Column").InnerText);
                    string tileTypeStr = tileNode.SelectSingleNode("TileType").InnerText;

                    Tile.ETileType type = (Tile.ETileType)Enum.Parse(typeof(Tile.ETileType), tileTypeStr);

                    string image = GetImagePath(type);

                    Tile tile = new Tile(line, column, image, type);

                    if (board.Count <= line)
                    {
                        board.Add(new ObservableCollection<Tile>());
                    }
                    board[line].Add(tile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game board from XML: {ex.Message}");
            }

            return board;
        }

        private static string GetImagePath(Tile.ETileType type)
        {
            switch (type)
            {
                case Tile.ETileType.Red:
                    return "/checkers_;component/Resources/man_red.png";
                case Tile.ETileType.Black:
                    return "/checkers_;component/Resources/man_black.png";
                case Tile.ETileType.RedKing:
                    return "/checkers_;component/Resources/king_red.png";
                case Tile.ETileType.BlackKing:
                    return "/checkers_;component/Resources/king_black.png";
                case Tile.ETileType.Empty:
                    return "/checkers_;component/Resources/empty_cell.png";
                default:
                    return "/checkers_;component/Resources/always_empty_cell.png";
            }
        }

        internal class GameInfo
        {
            public int Id { get; set; }
            public string Label { get; set; }
            public bool Turn { get; set; }
            public int BlackPieces { get; set; }
            public int RedPieces { get; set; }
            public int CapturedBlackPieces { get; set; }
            public int CapturedRedPieces { get; set; }
        }

        public static GameInfo ParseGameInfo(string xmlFilePath)
        {
            try
            {
                XDocument doc = XDocument.Load(xmlFilePath);
                XElement searchedGame = doc.Root.Elements("Game")
                    .FirstOrDefault(g => (int)g.Element("Id") == 1);

                
                if (searchedGame != null)
                {
                    return new GameInfo
                    {
                        Turn = bool.Parse(searchedGame.Element("Turn").Value),
                        BlackPieces = int.Parse(searchedGame.Element("BlackPieces").Value),
                        RedPieces = int.Parse(searchedGame.Element("RedPieces").Value),
                        CapturedBlackPieces = int.Parse(searchedGame.Element("CapturedBlackPieces").Value),
                        CapturedRedPieces = int.Parse(searchedGame.Element("CapturedRedPieces").Value)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing game info from XML: {ex.Message}");
            }

            return null; 
        }
    }  
}
