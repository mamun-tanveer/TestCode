using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace ExtendedFileProperties
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Test\90107a029p1-b01-abbr-land_635648179428504099.png";
            var file = ShellFile.FromFilePath(filePath);

            // Read and Write:

            string[] oldAuthors = file.Properties.System.Author.Value;
            string oldTitle = file.Properties.System.Title.Value;

            file.Properties.System.Author.Value = new string[] { "Author #1", "Author #2" };
            file.Properties.System.Title.Value = "Example Title";

        }
    }
}
