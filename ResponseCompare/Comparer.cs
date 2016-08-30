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
        public bool CompareVsBaseline(IRequestResponse test, IRequestResponse baseline)
        {
            bool returnValue = false;
            try
            {
                string testResponse = test.ResponseText;
                string baselineResponse = baseline.ResponseText;
                IEnumerable<string> regexList = test.RegExs;
                applyRegexes(regexList, ref testResponse, ref baselineResponse);
                return (testResponse == baselineResponse);              
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
