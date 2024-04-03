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
        public Tile() { Line = 0; Column = 0; Image = ""; }
        public Tile(int line, int column, string image)
        {
            Line = line;
            Column = column;
            Image = image;
            //if (CanHavePiece && !HasPiece)
            //{
            //    Image = image;
            //}
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
        bool HasPiece
        {
            get
            {
                if(Image == "/checkers_;component/Resources/empty_cell.png")
                {
                    return false; // does not have piece
                }
                else
                {
                    return true;
                }
            }       
        }


        bool CanHavePiece
        {
            get
            {
                if(Image == "/checkers_;component/Resources/always_empty_cell.png")
                {
                    return false;
                }
                return true;
            }
        }
    }
}
