using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Models;
using checkers_.Services;

namespace checkers_.ViewModels
{
    internal class TileViewModel
    {
        CheckersBusinessLogic cbl;

        public TileViewModel(int line, int column, string photo, CheckersBusinessLogic cbl)
        {
            STile = new Tile(line, column, photo);
            this.cbl = cbl;
        }

        public Tile STile { get; set; }
    }
}
