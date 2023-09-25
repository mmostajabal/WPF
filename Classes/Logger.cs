using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.Classes;

namespace Test
{
    public sealed class Logger
    {
        private static readonly object lockObj = new object();
        private static Logger instance = null;
        private Logger()
        { }

        public static Logger GetInstance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new Logger();
                    }
                }
                return instance;
            }
        }

        /*******************************************************
         * Log 
         * string message
         * *****************************************************/
        public void Log(string message)
        {
            FileOperation fileOperation = new FileOperation();
            fileOperation.CreateLog(message, fileOperation.GeneratFileNmae());
        }
    }
}
