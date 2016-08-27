using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ResponseCompare
{
    
    public class FolderCompare
    {
        private IEnumerable<string> files1;
        private IEnumerable<string> files2;
        private IFileCompare comparer; 

        public FolderCompare(string folder1, string folder2)
        {
            files1 = Directory.GetFiles(folder1);
            files2 = Directory.GetFiles(folder2);   
        }

        public bool Compare()
        {
            bool returnValue = true;
            var intersect = new HashSet<string>(files1.Intersect(files2));

            returnValue = (intersect.Count == files1.Count());

            return returnValue;
        }
    }
}
