using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitBuildVersioning.NETCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Git Build Versioning";

            


            if (args.Length == 4)
            {
                /*
                 * $(ProjectDir)
                 * $(ProjectName)
                 * $(ProjectExt)
                 * $(ConfigurationName)
                 */
                string projectDir = args[0];
                string projectName = args[1];
                string projectExt = args[2];
                string configurationName = args[3];

                string settingsPath = $"{projectDir}..\\Common\\Settings.json";

              

                var settings = AppSettings.Load(settingsPath);

                AppSettings.LogFile = $"{settings.LogDIR}\\GitBuildVersioning.NETCore-{projectName}-{DateTime.Now:yyyyMM}.log";
                bool isRelease = configurationName.ToLower() == "release" || configurationName.ToLower() == "prod" || configurationName.ToLower() == "uat" ? true : false;

                string projectFilePath = $"{projectDir}{projectName}{projectExt}";
                
                string lastCommittedRevision = string.Empty;


                Log.Add("--------------------------------------------------------------------------------");
                Log.Add($"ProjectDir: {projectDir}");
                Log.Add($"ProjectName: {projectName}");
                Log.Add($"ProjectExt: {projectExt}");
                Log.Add($"ConfigurationName: {configurationName}");
                Log.Add($"settingsPath: {settingsPath}");
                Log.Add($"isRelease: {isRelease}");
                Log.Add($"projectFilePath: {projectFilePath}");
                Log.Add($"GitPath: {settings.GitPath}");

                string projectFileData = System.IO.File.ReadAllText(projectFilePath);
                Match asvRaw = Regex.Match(projectFileData, "[<]AssemblyVersion[>]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[<][/]AssemblyVersion[>]");
                Match rxmVersion = Regex.Match(asvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+");

                Match fvRaw = Regex.Match(projectFileData, "[<]FileVersion[>][0-9]+[.][0-9]+[.][0-9]+[.]\\w+[<][/]FileVersion[>]");
                Match rxmFVersion = Regex.Match(fvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+[.]\\w+");

                Match pvRaw = Regex.Match(projectFileData, "[<]Version[>][0-9]+[.][0-9]+[.][0-9]+[<][/]Version[>]");
                Match rxmPVersion = Regex.Match(pvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+");


                Version oldAssemblyVersion = new Version(rxmVersion.Value);
                string oldFileVersion = rxmFVersion.Value;
                string oldPackageVersion = rxmPVersion.Value;

                Log.Add($"oldAssemblyVersion: {oldAssemblyVersion}");
                Log.Add($"oldFileVersion: {oldFileVersion}");
                Log.Add($"oldPackageVersion: {oldPackageVersion}");

                System.Diagnostics.ProcessStartInfo GitProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                GitProcessStartInfo.UseShellExecute = false;
                GitProcessStartInfo.CreateNoWindow = true;
                GitProcessStartInfo.RedirectStandardOutput = true;
                GitProcessStartInfo.FileName = string.Format("{0}\\git.exe", settings.GitPath);
                GitProcessStartInfo.Arguments = string.Format("-C {0} describe --always", projectDir);
                //git -C "D:\Users\Cameron\Source\Repos\Affinity Archive Agent" describe --always

                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(GitProcessStartInfo))
                {

                    using (System.IO.StreamReader sm = process.StandardOutput)
                    {
                        string resRaw = sm.ReadToEnd();
                        string[] res = resRaw.Split(new string[] { "\n" }, StringSplitOptions.None);
                        lastCommittedRevision = res[0];
                        Log.Add($"lastCommittedRevision: {lastCommittedRevision}");
                    }
                }

                int build = oldAssemblyVersion.Build;
                double revision = Tools.CalculateRevision(lastCommittedRevision);
                Log.Add($"calculatedRevision: {revision}");
                if (isRelease == false) { build = oldAssemblyVersion.Build + 1; }

                Version newAssemblyVersion = Version.Parse($"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}.{revision}");
                string newFileVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}.{lastCommittedRevision}";
                string newPackageVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}";

                Log.Add($"newAssemblyVersion: {newAssemblyVersion}");
                Log.Add($"newFileVersion: {newFileVersion}");
                Log.Add($"newPackageVersion: {newPackageVersion}");

                string updatedProjectFileData = projectFileData;

                if (newAssemblyVersion > oldAssemblyVersion)
                {
                    string nav = $"<AssemblyVersion>{newAssemblyVersion}</AssemblyVersion>";
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]AssemblyVersion[>]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[<][/]AssemblyVersion[>]", nav);
                }
                if (newFileVersion != oldFileVersion)
                {
                    string nafv = $"<FileVersion>{newFileVersion}</FileVersion>";
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]FileVersion[>][0-9]+[.][0-9]+[.][0-9]+[.]\\w+[<][/]FileVersion[>]", nafv);
                }
                if (newPackageVersion != oldPackageVersion)
                {
                    string napv = $"<Version>{newPackageVersion}</Version>";
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]Version[>][0-9]+[.][0-9]+[.][0-9]+[<][/]Version[>]", napv);
                }

                System.IO.File.WriteAllText(projectFilePath, updatedProjectFileData);
                Log.Add("--------------------------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("Usage: GitBuildVersioning.NETCore.exe [ProjectDir] [ProjectName] [ProjectExt] [ConfigurationName]");
                Console.WriteLine($"Num of args: {args.Length}");

                int count = 0;
                foreach (var arg in args)
                {
                    Console.WriteLine($"arg {count}: {arg}");

                    count++;
                }
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
