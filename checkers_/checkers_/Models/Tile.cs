using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace checkers_.Models
{
    class Tile : BaseNotification
    {
        public enum ETileType 
        {
            None,
            AlwaysEmpty,
            Empty,
            Red,
            Black,
            RedKing,
            BlackKing
        }

        public Tile() { Line = 0; Column = 0; Image = ""; }
        public Tile(int line, int column, string image)
        {
            Line = line;
            Column = column;
            Image = image;

        }

        public Tile(int line, int column, string image, ETileType type)
        {
            Line = line;
            Column = column;
            Image = image;
            TileType = type;
        }   

        private int line;
        public int Line
        {
            get { return line; }
            set
            {
                line = value;
                NotifyPropertyChanged("Line");
            }
        }

        private int column;
        public int Column
        {
            get { return column; }
            set
            {
                column = value;
                NotifyPropertyChanged("Column");
            }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                NotifyPropertyChanged("Image");
            }
        }

        private ETileType tileType;

        public ETileType TileType
        {
            get { return tileType; }
            set
            {
                tileType = value;
                NotifyPropertyChanged("TileType");
            }
        }

    }
}
