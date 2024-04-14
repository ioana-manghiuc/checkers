using checkers_.Services;
using checkers_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace checkers_.Views
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
        }

        private void HideButton(object sender, RoutedEventArgs e)
        {
            if(CheckersBusinessLogic.GameStarted)
            {
                MJ.Visibility = Visibility.Hidden;
                if(CheckersBusinessLogic.Jumps)
                {
                    MJText.Text = "(multiple jumps allowed)";
                }
                else if(!CheckersBusinessLogic.Jumps)
                {
                    MJText.Text = "(multiple jumps not allowed)";
                }
                MJText.Visibility = Visibility.Visible;
            }
        }
    }
}
