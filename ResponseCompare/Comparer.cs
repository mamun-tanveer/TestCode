using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCompare
{
    class Comparer
    {
        private IRegexCache regularExpressionsCache { get; set; }
        public Comparer(IRegexCache regexs)
        {
            regularExpressionsCache = regexs;
        }

        public bool CompareVsBaseline(IRequestResponse response, string baselineFilePath)
        {
            bool returnValue = false;
            try
            {
                string responseText = response.ResponseText;
                string baseLineText = getBaseLineText(response.InputFilePath, baselineFilePath);
                IEnumerable<string> regexList = regularExpressionsCache.GetRegexes(response.Url);
                applyRegexes(regexList, ref responseText, ref baseLineText);
                return (responseText == baseLineText);              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                returnValue = false;
            }

            return returnValue;
        }

        private string getBaseLineText(string responseFilePath, string baselineFilePath)
        {
            string baselineText = string.Empty;
            string responseFileName = Path.GetFileName(responseFilePath);
            
            using (var reader = new StreamReader(baselineFilePath))
            {
                baselineText = parseResponsefromFileText(reader.ReadToEnd());
            }

            return baselineText;
        }

        private string parseResponsefromFileText(string fileText)
        {
            throw new NotImplementedException();
        }

        private void applyRegexes(IEnumerable<String> regexList, ref string responseText, ref string baselineText)
        {
            foreach(string pattern in regexList)
            {
                baselineText = System.Text.RegularExpressions.Regex.Replace(baselineText, pattern, string.Empty);
                responseText = System.Text.RegularExpressions.Regex.Replace(responseText, pattern, string.Empty);
            }
        }
    }
}
