using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using checkers_.Models;

namespace checkers_.Services
{
    class CheckersBusinessLogic
    {
        private ObservableCollection<ObservableCollection<Tile>> board;
        public CheckersBusinessLogic(ObservableCollection<ObservableCollection<Tile>> board)
        {
            this.board = board;
        }
    }
}
