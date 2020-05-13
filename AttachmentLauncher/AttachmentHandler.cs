using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentLauncher
{
    internal class AttachmentHandler
    {
        private string mFilePath;
        
        public AttachmentHandler(string attachmentFilePath)
        {
            mFilePath = attachmentFilePath;
        }

        public string GetAttachmentFile()
        {
            return parseFile();
        }

        private string parseFile()
        {
            using (var reader = new System.IO.StreamReader(mFilePath))
            {
                return reader.ReadLine();
            }
        }
    }
}
