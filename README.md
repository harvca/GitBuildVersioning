# GitBuildVersioning
GitBuildVersioning helps versioning .NET Framework and .NET Core / .NET projects in Visual Studio

  * Major and Minor versions are not changed.
  * Build not changed on Release (Default, Prod or UAT).
  * AssemblyVersion Revision is a calculated number based on Last commit revision.
  * FileVersion Revision is the Last commit revision.
  
  Versioning example:

    .NET Framework:

        AssemblyVersion("1.0.98.422")
        AssemblyFileVersion("1.0.98.93e8238")

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

