# GitBuildVersioning

This project required Git or TortoiseGit to be installed

Major and Minor are not changed.

Build not changed on Release (Default, Prod or UAT)

Revision is changed to a caculated number based on Last commit revision
Versioning example:

AssemblyVersion("1.0.1.227") -> AssemblyVersion("1.0.2.227")
AssemblyFileVersion("1.0.1.75d3b51") -> AssemblyFileVersion("1.0.2.75d3b51")

Usage: GitBuildVersioning.exe [Project Directory] [Assembly Name] [Build Type]

Example Visual Studion Pre-build event command line:

$(SolutionDir)Common\GitBuildVersioning.exe $(ProjectDir) $(TargetFileName) $(ConfigurationName)
