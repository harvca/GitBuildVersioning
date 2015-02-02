using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBuildVersioning
{
  public class Log
    {
        public static void Add(string message)
        {
            try
            {
                if (System.IO.File.Exists(AppSettings.LogFile) == false)
                {
                    System.IO.File.Create(AppSettings.LogFile).Close();
                }
                Console.WriteLine(message);
                System.IO.File.AppendAllText(AppSettings.LogFile, message + Environment.NewLine);
            }
            catch { }
        }
    }

}