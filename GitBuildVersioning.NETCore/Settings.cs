using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBuildVersioning.NETCore
{
    public class Settings
    {
        public Settings()
        {
            Verbose = false;
        }

        public string LogDIR { get; set; }

        public string GitPath { get; set; }
        public bool Verbose { get; set; }
    }
}
