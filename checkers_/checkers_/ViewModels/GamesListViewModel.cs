using checkers_.Models;
using checkers_.Services;
using checkers_.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace checkers_.ViewModels
{
    class GamesListViewModel : BaseNotification
    {
        public ObservableCollection<SourceHelper.GameInfo> Games { get; set; }
        private ICommand selectGame;
        private string gameLabel;
        public GamesListViewModel()
        {
            Games = SourceHelper.LoadAllGames();
        }

        public ICommand SelectGame
        {
            get
            {
                if (selectGame == null)
                {
                    selectGame = new RelayCommand<string>(SourceHelper.SelectGame);
                }
                return selectGame;
            }
        }
    }
}
