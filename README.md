# GitBuildVersioning

This project requires Git for Windows to be installed

Major and Minor versions are not changed.

Build not changed on Release (Default, Prod or UAT)

Revision is changed to a calculated number based on Last commit revision Versioning example:

  .NET Framework:
  
      AssemblyVersion("1.0.1.227") -> AssemblyVersion("1.0.2.227")
      AssemblyFileVersion("1.0.1.75d3b51") -> AssemblyFileVersion("1.0.2.75d3b51")
  
  .NET Core / .NET:
  
      <Version>1.0.98</Version>
      <AssemblyVersion>1.0.98.422</AssemblyVersion>
      <FileVersion>1.0.98.93e8238</FileVersion>
    
Usage:

  .NET Framework:
  
      GitBuildVersioning.exe [Project Directory] [Assembly Name] [Build Type]
  
  .NET Core / .NET:     
  
       GitBuildVersioning.NETCore.exe [ProjectDir] [ProjectName] [ProjectExt] [ConfigurationName]

Example Visual Studion Pre-build event command line:

  .NET Framework:
  
      $(SolutionDir)Common\GitBuildVersioning.exe $(ProjectDir) $(TargetFileName) $(ConfigurationName)
      
  .NET Core / .NET:
  
      $(SolutionDir)Common\GitBuildVersioning.NETCore.exe $(ProjectDir) $(ProjectName) $(ProjectExt) $(ConfigurationName)

