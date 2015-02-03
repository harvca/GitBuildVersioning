using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBuildVersioning
{
    public class Tools
    {
        public static int CalculateRevision(string rev)
        {
            int output = 0;
            char[] chars = rev.ToCharArray();
            foreach (char c in chars)
            {
                switch (c.ToString().ToLower())
                {
                    case "a":
                        output = output + 101;
                        break;
                    case "b":
                        output = output + 102;
                        break;
                    case "c":
                        output = output + 103;
                        break;
                    case "d":
                        output = output + 104;
                        break;
                    case "e":
                        output = output + 105;
                        break;
                    case "f":
                        output = output + 106;
                        break;
                    case "g":
                        output = output + 107;
                        break;
                    case "h":
                        output = output + 108;
                        break;
                    case "i":
                        output = output + 109;
                        break;
                    case "j":
                        output = output + 110;
                        break;
                    case "k":
                        output = output + 111;
                        break;
                    case "l":
                        output = output + 112;
                        break;
                    case "m":
                        output = output + 113;
                        break;
                    case "n":
                        output = output + 114;
                        break;
                    case "o":
                        output = output + 115;
                        break;
                    case "p":
                        output = output + 116;
                        break;
                    case "q":
                        output = output + 117;
                        break;
                    case "r":
                        output = output + 118;
                        break;
                    case "s":
                        output = output + 119;
                        break;
                    case "t":
                        output = output + 120;
                        break;
                    case "u":
                        output = output + 121;
                        break;
                    case "v":
                        output = output + 122;
                        break;
                    case "w":
                        output = output + 123;
                        break;
                    case "x":
                        output = output + 124;
                        break;
                    case "y":
                        output = output + 125;
                        break;
                    case "z":
                        output = output + 126;
                        break;
                    case "1":
                        output = output + 1;
                        break;
                    case "2":
                        output = output + 2;
                        break;
                    case "3":
                        output = output + 3;
                        break;
                    case "4":
                        output = output + 4;
                        break;
                    case "5":
                        output = output + 5;
                        break;
                    case "6":
                        output = output + 6;
                        break;
                    case "7":
                        output = output + 7;
                        break;
                    case "8":
                        output = output + 8;
                        break;
                    case "9":
                        output = output + 9;
                        break;
                    default:
                        output = output + 10;
                        break;
                }
            }

            return output;
        }
    }
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