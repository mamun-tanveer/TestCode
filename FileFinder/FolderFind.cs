using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileFinder
{
    class FolderFind
    {
        public IDictionary<string, List<string>> FileLocations { get; private set; }
        public int MaxRecursionLevel { get; set; } = 10; 

        public FolderFind(string rootPath, bool includeSubFolders = true)
        {
            FileLocations = new Dictionary<string, List<string>>();
            if(includeSubFolders)
            {
                recursiveAdd(rootPath,  0);
            }
            else
            {
                addFileLocations(rootPath);
            } 
        }

        private void recursiveAdd(string rootPath, int recurseLevel)
        {

            addFileLocations(rootPath);         
            if(recurseLevel < MaxRecursionLevel)
            {
                foreach(string dirPath in Directory.GetDirectories(rootPath))
                {
                    recursiveAdd(dirPath, recurseLevel + 1);
                }
            }
        }

        private void addFileLocations(string folderPath)
        {
            Console.WriteLine("Processing folder " + folderPath);
            foreach(string filePath in Directory.GetFiles(folderPath))
            {
                string filename = Path.GetFileName(filePath);
                if (FileLocations.ContainsKey(filename) == false)
                {
                    //never encountered before
                    FileLocations.Add(filename, new List<string>());
                }
                FileLocations[filename].Add(folderPath);
            }
        }
    }
}
