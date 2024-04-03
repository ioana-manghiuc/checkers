using System;
using System.IO;

namespace checkers_.ViewModels
{
    class MenuViewModel
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
