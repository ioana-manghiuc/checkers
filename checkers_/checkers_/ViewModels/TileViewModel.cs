using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using checkers_.Models;
using checkers_.Services;
using checkers_.Commands;

namespace checkers_.ViewModels
{
    class TileViewModel
    {
        private Tile sTile;
        private ICommand clickCommand;
        private CheckersBusinessLogic cbl;

        public TileViewModel(int line, int column, string photo, CheckersBusinessLogic cbl)
        {
            STile = new Tile(line, column, photo);
            this.cbl = cbl;
        }

        public TileViewModel(int line, int column, string photo, Tile.ETileType type, CheckersBusinessLogic cbl)
        {
            STile = new Tile(line, column, photo, type);
            this.cbl = cbl;
        }

        public Tile STile
        {
            get { return sTile; }
            set { sTile = value; }
        }
        public ICommand ClickCommand
        {
            get
            {
                if (clickCommand == null)
                {
                    clickCommand = new RelayCommand<Tile>(cbl.ClickAction);
                }
                return clickCommand;
            }
        }

    }

}
