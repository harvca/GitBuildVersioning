using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitBuildVersioning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Git Build Versioning";
            if (args.Count() == 3)
            {
                string projectPath = args[0];
                string assembly = args[1];
                string buildType = args[2];
                AppSettings.LogFile = string.Format("{0}\\GitBuildVersioning-{1}-{2}.log", AppSettings.LogDIR,assembly, DateTime.Now.ToString("yyyyMM"));
                bool isRelease = buildType.ToLower() == "release" || buildType.ToLower() == "prod" || buildType.ToLower() == "uat" ? true : false;
                string assemblyInfoPath = string.Format("{0}Properties\\AssemblyInfo.cs", projectPath);
                string lastCommittedRevision = string.Empty;
                
               
                Log.Add("--------------------------------------------------------------------------------");
                Log.Add("projectPath: " + projectPath);
                Log.Add("assembly: " + assembly);
                Log.Add("buildType: " + buildType);
                Log.Add("isRelease: " + isRelease);
                Log.Add("assemblyInfoPath: " + assemblyInfoPath);
                Log.Add("GitPath: " + AppSettings.GitPath);

                string assemblyInfoData = System.IO.File.ReadAllText(assemblyInfoPath);
                Match asvRaw = Regex.Match(assemblyInfoData, "AssemblyVersion[(][\"]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[\"][)]");
                Match rxmVersion = Regex.Match(asvRaw.Value, "([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)");

                Match asfvRaw = Regex.Match(assemblyInfoData, "AssemblyFileVersion[(][\"]([0-9]+[.][0-9]+[.][0-9]+[.]\\w+)[\"][)]");
                Match rxmFVersion = Regex.Match(asfvRaw.Value, "([0-9]+[.][0-9]+[.][0-9]+[.]\\w+)");

                Version oldAssemblyVersion = new Version(rxmVersion.Value);
                string oldAssemblyFileVersion = rxmFVersion.Value; 

                Log.Add("oldAssemblyVersion: " + oldAssemblyVersion);
                Log.Add("oldAssemblyFileVersion: " + oldAssemblyFileVersion);

                System.Diagnostics.ProcessStartInfo GitProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                GitProcessStartInfo.UseShellExecute = false;
                GitProcessStartInfo.CreateNoWindow = true;
                GitProcessStartInfo.RedirectStandardOutput = true;
                GitProcessStartInfo.FileName = string.Format("{0}\\git.exe", AppSettings.GitPath);
                GitProcessStartInfo.Arguments = string.Format("-C {0} describe --always", projectPath);
                //git -C "D:\Users\Cameron\Source\Repos\Affinity Archive Agent" describe --always

                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(GitProcessStartInfo))
                {
                    
                    using (System.IO.StreamReader sm = process.StandardOutput)
                    {
                        string resRaw = sm.ReadToEnd();
                        string[] res = resRaw.Split(new string[] { "\n" }, StringSplitOptions.None);
                        lastCommittedRevision = res[0];
                        Log.Add("lastCommittedRevision: " + lastCommittedRevision);
                    }
                }

                int build = oldAssemblyVersion.Build;
                int revision = Tools.CalculateRevision(lastCommittedRevision);
                Log.Add("calculatedRevision: " + revision);
                if (isRelease == false) { build = oldAssemblyVersion.Build + 1; }

                Version newAssemblyVersion = Version.Parse(string.Format("{0}.{1}.{2}.{3}", oldAssemblyVersion.Major, oldAssemblyVersion.Minor, build, revision));
                string newAssemblyFileVersion = string.Format(string.Format("{0}.{1}.{2}.{3}", oldAssemblyVersion.Major, oldAssemblyVersion.Minor, build, lastCommittedRevision));
                Log.Add("newAssemblyVersion: " + newAssemblyVersion);
                Log.Add("newAssemblyFileVersion: " + newAssemblyFileVersion);

                string updatedAssemblyInfoData = assemblyInfoData;

                if (newAssemblyVersion > oldAssemblyVersion)
                {
                    updatedAssemblyInfoData =  Regex.Replace(updatedAssemblyInfoData, "([0-9]+)([.][0-9]+[.][0-9]+[.][0-9]+)", newAssemblyVersion.ToString());
                }
                if (newAssemblyFileVersion != oldAssemblyFileVersion)
                {
                    string nafv = string.Format("AssemblyFileVersion(\"{0}\")", newAssemblyFileVersion);
                    updatedAssemblyInfoData =  Regex.Replace(updatedAssemblyInfoData, "AssemblyFileVersion[(][\"]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[\"][)]", nafv);
                }

                System.IO.File.WriteAllText(assemblyInfoPath, updatedAssemblyInfoData);
                Log.Add("--------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("Usage: GitBuildVersioning.exe [Project Directory] [Assembly Name] [Build Type]");
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
