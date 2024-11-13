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

               
                    Log.Add("--------------------------------------------------------------------------------",settings.Verbose);
                    Log.Add($"ProjectDir: {projectDir}", settings.Verbose);
                    Log.Add($"ProjectName: {projectName}", settings.Verbose);
                    Log.Add($"ProjectExt: {projectExt}", settings.Verbose);
                    Log.Add($"ConfigurationName: {configurationName}", settings.Verbose);
                    Log.Add($"settingsPath: {settingsPath}", settings.Verbose);
                    Log.Add($"isRelease: {isRelease}", settings.Verbose);
                    Log.Add($"projectFilePath: {projectFilePath}", settings.Verbose);
                    Log.Add($"GitPath: {settings.GitPath}", settings.Verbose);

                string projectFileData = System.IO.File.ReadAllText(projectFilePath);
                Match asvRaw = Regex.Match(projectFileData, "[<]AssemblyVersion[>]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[<][/]AssemblyVersion[>]");
                Match rxmVersion = Regex.Match(asvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+");

                Match fvRaw = Regex.Match(projectFileData, "[<]InformationalVersion[>][0-9]+[.][0-9]+[.][0-9]+[.]\\w+[<][/]InformationalVersion[>]");
                Match rxmFVersion = Regex.Match(fvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+[.]\\w+");

                Match pvRaw = Regex.Match(projectFileData, "[<]Version[>][0-9]+[.][0-9]+[.][0-9]+[<][/]Version[>]");
                Match rxmPVersion = Regex.Match(pvRaw.Value, "[0-9]+[.][0-9]+[.][0-9]+");


                Version oldAssemblyVersion = new Version(rxmVersion.Value);
                string oldFileVersion = rxmFVersion.Value;
                string oldPackageVersion = rxmPVersion.Value;

                Log.Add($"oldAssemblyVersion: {oldAssemblyVersion}", settings.Verbose);
                Log.Add($"oldFileVersion: {oldFileVersion}", settings.Verbose);
                Log.Add($"oldPackageVersion: {oldPackageVersion}", settings.Verbose);

                System.Diagnostics.ProcessStartInfo GitProcessStartInfo = new System.Diagnostics.ProcessStartInfo();
                GitProcessStartInfo.UseShellExecute = false;
                GitProcessStartInfo.CreateNoWindow = true;
                GitProcessStartInfo.RedirectStandardOutput = true;
                GitProcessStartInfo.FileName = string.Format("{0}\\git.exe", settings.GitPath);
                GitProcessStartInfo.Arguments = string.Format("-C {0} describe --always", projectDir);

                using (System.Diagnostics.Process process = System.Diagnostics.Process.Start(GitProcessStartInfo))
                {

                    using (System.IO.StreamReader sm = process.StandardOutput)
                    {
                        string resRaw = sm.ReadToEnd();
                        string[] res = resRaw.Split(new string[] { "\n" }, StringSplitOptions.None);
                        lastCommittedRevision = res[0];
                        Log.Add($"lastCommittedRevision: {lastCommittedRevision}", settings.Verbose);
                    }
                }

                int build = oldAssemblyVersion.Build;
                double revision = Tools.CalculateRevision(lastCommittedRevision);
                Log.Add($"calculatedRevision: {revision}", settings.Verbose);
                if (isRelease == false) { build = oldAssemblyVersion.Build + 1; }

               
                Version newAssemblyVersion = Version.Parse($"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}.{revision}");

                string newVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}.{revision}";
                string newFileVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}.{lastCommittedRevision}";
                string newPackageVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build}";

                if(projectName == "InSchool.WebApp")
                {
                    newVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build:000}.{revision}";
                    newFileVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build:000}.{lastCommittedRevision}";
                    newPackageVersion = $"{oldAssemblyVersion.Major}.{oldAssemblyVersion.Minor}.{build:000}";
                }

                Log.Add($"newAssemblyVersion: {newVersion}", settings.Verbose);
                Log.Add($"newFileVersion: {newFileVersion}", settings.Verbose);
                Log.Add($"newPackageVersion: {newPackageVersion}", settings.Verbose);

                string updatedProjectFileData = projectFileData;

                if (newAssemblyVersion > oldAssemblyVersion)
                {
                    string nav = $"<AssemblyVersion>{newVersion}</AssemblyVersion>";
                  
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]AssemblyVersion[>]([0-9]+[.][0-9]+[.][0-9]+[.][0-9]+)[<][/]AssemblyVersion[>]", nav);
                }
                if (newFileVersion != oldFileVersion)
                {
                    string nafv = $"<InformationalVersion>{newFileVersion}</InformationalVersion>";
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]InformationalVersion[>][0-9]+[.][0-9]+[.][0-9]+[.]\\w+[<][/]InformationalVersion[>]", nafv);
                }
                if (newPackageVersion != oldPackageVersion)
                {
                    string napv = $"<Version>{newPackageVersion}</Version>";
                    updatedProjectFileData = Regex.Replace(updatedProjectFileData, "[<]Version[>][0-9]+[.][0-9]+[.][0-9]+[<][/]Version[>]", napv);
                }

                System.IO.File.WriteAllText(projectFilePath, updatedProjectFileData);
                Log.Add("--------------------------------------------------------------------------------", settings.Verbose);
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
