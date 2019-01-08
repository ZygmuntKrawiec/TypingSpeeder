using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingSpeeder.Providers
{
    class DataTextProvider
    {
        string path;
        int lineNumber = 0;
        IEnumerator<string> lineOfText = null;

        /// <summary>
        /// Constr. creates connection to Text.txt file with "Lorem ipsum...".
        /// </summary>
        public DataTextProvider()
        {
            path = @"../../Data/Text.txt";
        }

        /// <summary>
        /// Constr. creates connection to custom txt file.
        /// </summary>
        /// <param name="textFilePath"></param>
        public DataTextProvider(string textFilePath)
        {
            path = textFilePath;
        }

        /// <summary>
        /// Gets single word from a given file.
        /// </summary>
        /// <returns>A single word.</returns>
        public string GetWord()
        {
            // If line of text is equal null then read new line.
            if (lineOfText == null)
                lineOfText = readLine();
            // If line of text is still null then there is no line to read.
            if (lineOfText == null)
                return null;

            while (lineOfText.MoveNext())
            {
                return lineOfText.Current;
            }

            lineOfText = null;
            return GetWord();
        }

        private IEnumerator<string> readLine()
        {
            string strArr = null;

            try
            {
                // StreamReader is open/close every time when method readLine() is called
                // to released rescource. 
                using (var stream = new StreamReader(path))
                {
                    // Skip lines read before expected line.
                    for (int i = 0; i < lineNumber; i++)
                    {
                        stream.ReadLine();
                    }

                    // Repeat while strAtr is null or empty.
                    while (strArr == null || strArr == "")
                    {
                        // If stream is not at the end, read next line.
                        if (!stream.EndOfStream)
                        {
                            // Read needed string.
                            strArr = stream.ReadLine();
                        }
                        else
                        {
                            strArr = null;
                            lineNumber = 0;
                            break;
                        }
                    }

                    // If the line of text was read, then increment line number value
                    // to skip read lines by for loop.,
                    lineNumber++;
                }
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch
            {
                strArr = null;
            }

            // If string is not null then split it into array of string(words) 
            // If string is null it means that we reached to the end of file or exception was thrown.
            return strArr != null ? strArr.Split(' ').Cast<string>().GetEnumerator() : null;
        }

        /// <summary>
        /// Sets initial position of the text provider.
        /// </summary>
        public void Reset()
        {
            lineNumber = 0;
            lineOfText = null;
        }
    }
}
