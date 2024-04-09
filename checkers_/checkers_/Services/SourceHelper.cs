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
using System.Windows;
using System.Security.Cryptography;
using checkers_.Views;
using checkers_.ViewModels;

namespace checkers_.Services
{
    class SourceHelper
    {
        public static Tile CurrentTile { get; set; }
        public static Tile NextTile { get; set; }

        public static int GameID { get; set; }
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

        public static ObservableCollection<ObservableCollection<Tile>> RestoreGameBoardO()
        {
            ObservableCollection<ObservableCollection<Tile>> board = new ObservableCollection<ObservableCollection<Tile>>();

            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();

                for (int j = 0; j < 8; j++)
                {
                    string image = "";
                    Tile.ETileType type = Tile.ETileType.None;

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

                    Tile tile = new Tile(i, j, image, type);
                    row.Add(tile);
                }
                board.Add(row);
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Resources/saved_boards.xml");

                XmlNodeList tileNodes = doc.SelectNodes("/SavedBoards/Tiles/Tile");

                foreach (XmlNode tileNode in tileNodes)
                {
                    int line = int.Parse(tileNode.SelectSingleNode("Line").InnerText);
                    int column = int.Parse(tileNode.SelectSingleNode("Column").InnerText);
                    string tileTypeStr = tileNode.SelectSingleNode("TileType").InnerText;

                    Tile.ETileType type = (Tile.ETileType)Enum.Parse(typeof(Tile.ETileType), tileTypeStr);
                    string image = GetImagePath(type);
                    Tile tile = new Tile(line, column, image, type);
                    board[line][column] = tile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game board from XML: {ex.Message}");
            }

            return board;
        }
       
        public class GameInfo
        {
            public int Id { get; set; }
            public string Label { get; set; }
            public bool Turn { get; set; }
            public int BlackPieces { get; set; }
            public int RedPieces { get; set; }
            public int CapturedBlackPieces { get; set; }
            public int CapturedRedPieces { get; set; }
        }

        public static ObservableCollection<GameInfo> LoadAllGames()
        {
            List<GameInfo> listInfo = new List<GameInfo>();
            try
            {
                XDocument doc = XDocument.Load("Resources/saved_games.xml");

                listInfo = doc.Root.Elements("Game")
                    .Select(g => new GameInfo
                    {
                        Id = int.Parse(g.Element("Id").Value),
                        Label = g.Element("Label").Value,
                        Turn = bool.Parse(g.Element("Turn").Value),
                        BlackPieces = int.Parse(g.Element("BlackPieces").Value),
                        RedPieces = int.Parse(g.Element("RedPieces").Value),
                        CapturedBlackPieces = int.Parse(g.Element("CapturedBlackPieces").Value),
                        CapturedRedPieces = int.Parse(g.Element("CapturedRedPieces").Value)
                    }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading games from XML: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new ObservableCollection<GameInfo>();
            }

            ObservableCollection<GameInfo> games = new ObservableCollection<GameInfo>(listInfo);
            return games;
        }

        public static GameInfo GetGameWithID(int id)
        {
            try
            {
                XDocument doc = XDocument.Load("Resources/saved_games.xml");
                XElement searchedGame = doc.Root.Elements("Game")
                    .FirstOrDefault(g => (int)g.Element("Id") == id);


                if (searchedGame != null)
                {
                    return new GameInfo
                    {
                        Id = int.Parse(searchedGame.Element("Id").Value),
                        Label = searchedGame.Element("Label").Value,
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

        public static void SelectGame(string label)
        {
            ObservableCollection<GameInfo> games = LoadAllGames();
            GameInfo selectedGame = games.FirstOrDefault(g => g.Label == label);

            if (selectedGame != null)
            {
                GameID = selectedGame.Id;
                Console.WriteLine("game id: " + GameID);

                GameInfo gameInfo = GetGameWithID(GameID);
                ObservableCollection<ObservableCollection<Tile>> board = RestoreGameBoard(GameID);

                SavedGameViewModel savedGameViewModel = new SavedGameViewModel();

                SavedGameWindow savedGameWindow = new SavedGameWindow();
                savedGameWindow.Show();
            }
            else
            {
                MessageBox.Show("Selected game not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static ObservableCollection<ObservableCollection<Tile>> RestoreGameBoard(int gameID)
        {
            ObservableCollection<ObservableCollection<Tile>> board = new ObservableCollection<ObservableCollection<Tile>>();

            for (int i = 0; i < 8; i++)
            {
                ObservableCollection<Tile> row = new ObservableCollection<Tile>();

                for (int j = 0; j < 8; j++)
                {
                    string image = "";
                    Tile.ETileType type = Tile.ETileType.None;

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

                    Tile tile = new Tile(i, j, image, type);
                    row.Add(tile);
                }
                board.Add(row);
            }

            try
            {
                XDocument doc = XDocument.Load("Resources/saved_boards.xml");
                var gameNodes = doc.Root.Elements("Tiles").Where(node => (int)node.Element("GameID") == gameID);

                foreach (var gameNode in gameNodes)
                {
                    foreach (var tileNode in gameNode.Elements("Tile"))
                    {
                        int line = int.Parse(tileNode.Element("Line").Value);
                        int column = int.Parse(tileNode.Element("Column").Value);
                        string tileTypeStr = tileNode.Element("TileType").Value;

                        Tile.ETileType type = (Tile.ETileType)Enum.Parse(typeof(Tile.ETileType), tileTypeStr);
                        string image = GetImagePath(type);
                        Tile tile = new Tile(line, column, image, type);
                        board[line][column] = tile;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game board from XML: {ex.Message}");
            }

            return board;
        }


    }
}
