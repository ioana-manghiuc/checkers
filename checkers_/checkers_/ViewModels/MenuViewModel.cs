using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using checkers_.Commands;
using checkers_.Views;


namespace checkers_.ViewModels
{
    internal class MenuViewModel
    {
        public string AboutInfo { get; }

        public MenuViewModel()
        {
            AboutInfo = LoadAboutInfo();
        }

        private string LoadAboutInfo()
        {
            try
            {
                string filePath = "Resources/about_data.txt";
                if (File.Exists(filePath)) { return File.ReadAllText(filePath); }
                else { return "File not found: " + filePath; }
            }
            catch (Exception ex)
            {
                return "Error reading file: " + ex.Message;
            }
        }
    }
}
