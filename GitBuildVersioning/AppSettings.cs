using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GitBuildVersioning
{
    public class AppSettings
    {
        public static Version Version(string assemblyName)
        {
            AssemblyName asm = new AssemblyName(assemblyName);
            return asm.Version;
        }

        public static string LogFile;

        public static string LogDIR
        {
            get
            {
                try
                {
                    string tmp = System.Configuration.ConfigurationManager.AppSettings["LogDIR"];
                    if (string.IsNullOrWhiteSpace(tmp)) { throw new Exception("LogDIR is NULL"); }
                    return tmp;
                }
                catch
                {
                    throw new Exception("LogDIR is NULL");
                }
            }
        }
        public static string TortoiseGitPath
        {
            get
            {
                try
                {
                    string tmp = System.Configuration.ConfigurationManager.AppSettings["TortoiseGitPath"];
                    if (string.IsNullOrWhiteSpace(tmp)) { throw new Exception("TortoiseGitPath is NULL"); }
                    return tmp;
                }
                catch
                {
                    throw new Exception("TortoiseGitPath is NULL");
                }
            }
        }
        public static string GitPath
        {
            get
            {
                try
                {
                    string tmp = System.Configuration.ConfigurationManager.AppSettings["GitPath"];
                    if (string.IsNullOrWhiteSpace(tmp)) { throw new Exception("GitPath is NULL"); }
                    return tmp;
                }
                catch
                {
                    throw new Exception("GitPath is NULL");
                }
            }
        }
    }
}
