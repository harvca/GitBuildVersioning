using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GitBuildVersioning.NETCore
{
    public class AppSettings
    {
        public static Version Version(string assemblyName)
        {
            AssemblyName asm = new AssemblyName(assemblyName);
            return asm.Version;
        }

        public static Settings Load(string filePath)
        {
            var retVal = new Settings();

            if (System.IO.File.Exists(filePath))
            {
                var rawJSON = System.IO.File.ReadAllText(filePath);

                retVal = JsonSerializer.Deserialize<Settings>(rawJSON);
            }

            return retVal;
        }

        public static string LogFile;


    }
}
