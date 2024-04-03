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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace checkers_.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void NewGameBtnClick(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
        }   
        private void FileBtnClick(object sender, RoutedEventArgs e)
        {
            MainMenuView.Visibility = Visibility.Collapsed;           
            HelpMenu.Visibility = Visibility.Collapsed;
            StatisticsMenu.Visibility = Visibility.Collapsed;
            FileMenu.Visibility = Visibility.Visible;
        }

        private void HelpBtnClick(object sender, RoutedEventArgs e)
        {
            MainMenuView.Visibility = Visibility.Collapsed;           
            StatisticsMenu.Visibility = Visibility.Collapsed;
            FileMenu.Visibility = Visibility.Collapsed;
            HelpMenu.Visibility = Visibility.Visible;
        }

        private void StatsBtnClick(object sender, RoutedEventArgs e)
        {
            MainMenuView.Visibility = Visibility.Collapsed;
            HelpMenu.Visibility = Visibility.Collapsed;            
            FileMenu.Visibility = Visibility.Collapsed;
            StatisticsMenu.Visibility = Visibility.Visible;
        }

        private void BackToMainClick(object sender, RoutedEventArgs e)
        {           
            HelpMenu.Visibility = Visibility.Collapsed;
            FileMenu.Visibility = Visibility.Collapsed;
            StatisticsMenu.Visibility = Visibility.Collapsed;
            MainMenuView.Visibility = Visibility.Visible;
        }

        private void BackToFileClick(object sender, RoutedEventArgs e)
        {
            MainMenuView.Visibility = Visibility.Collapsed;
            HelpMenu.Visibility = Visibility.Collapsed;            
            StatisticsMenu.Visibility = Visibility.Collapsed;
            FileMenu.Visibility = Visibility.Visible;
        }
    }
}
