using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Test.Classes
{
    public class FileOperation
    {
        /**********************************************************************
         * CrerateCSVFile
         * ItemCollection dataList
         * string path
         * string baseFileName = ""  Optional
         * string extension ="csv"  Optional
         * ********************************************************************/
        public Boolean CrerateCSVFile<T>(ItemCollection dataList, string path, string baseFileName = "", string extension = "csv")
        {
            path += "\\" + GeneratFileNmae(baseFileName, extension);

            using (TextWriter outputfile = new StreamWriter(path))
            {
                foreach (T data in dataList)
                {
                    outputfile.WriteLine(string.Join(",", data.ToString()));
                }
            }

            return true;
        }
        /**********************************************************************
         * CreateLog
         * string message
         * string path
         * ********************************************************************/
        public Boolean CreateLog(string message, string path)
        {
            string fileName = "";

            fileName = DateTime.Now.ToString().Replace(":", "_").Replace("/", "_").Replace(" ", "_") + ".log";

            using (TextWriter outputfile = new StreamWriter(fileName))
            {
                outputfile.WriteLine(message.ToString());
            }

            return true;
        }
        /**********************************************************************
         * GeneratFileNmae
         * string baseFileName = ""  Optional
         * string extension = "txt"  Optional
         * ********************************************************************/
        public string GeneratFileNmae(string baseFileName = "", string extension = "txt")
        {
            return baseFileName + DateTime.Now.ToString().Replace(":", "_").Replace("/", "_").Replace(" ", "_") + "." + extension;
        }
    }
}
