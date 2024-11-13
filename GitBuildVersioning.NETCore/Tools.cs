using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBuildVersioning.NETCore
{
    public class Tools
    {
        public static int CalculateRevision(string rev)
        {
            int output = 0;
            char[] chars = rev.ToCharArray();
            foreach (char c in chars)
            {
                int i = (int)c;
                output = output + i;
            }

            return output;
        }
    }
    public class Log
    {
        public static void Add(string message,bool verbose)
        {
            try
            {
                if (System.IO.File.Exists(AppSettings.LogFile) == false)
                {
                    System.IO.File.Create(AppSettings.LogFile).Close();
                }

                if (verbose == true)
                {
                    Console.WriteLine(message);
                }

                System.IO.File.AppendAllText(AppSettings.LogFile, message + Environment.NewLine);
            }
            catch { }
        }
    }

}