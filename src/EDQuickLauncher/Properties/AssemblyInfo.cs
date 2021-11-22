using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customize this process see: https://aka.ms/assembly-info-properties

// Setting ComVisible to false makes the types in this assembly not visible to COM
// components.  If you need to access a type in this assembly from COM, set the ComVisible
// attribute to true on that type.

[assembly: ComVisible(false)]
[assembly: AssemblyCompany("Neko Gaming")]
#if RELEASE
[assembly: AssemblyConfiguration("Release")]
#else
[assembly: AssemblyConfiguration("Debug")]
#endif
[assembly: AssemblyCopyright("Copyright (c) 2021 Neko Boi Nick")]
[assembly: AssemblyDescription("EDQuickLauncher is a custom launcher for Elite: Dangerous, with the ability to launch other programs alongside Elite, such as EDDiscovery!")]
[assembly: AssemblyFileVersion("1.0.4")]
[assembly: AssemblyInformationalVersion("1.0.4")]
[assembly: AssemblyProduct("EDQuickLauncher")]
[assembly: AssemblyTitle("EDQuickLauncher")]
[assembly: AssemblyVersion("1.0.4")]
[assembly: AssemblyDelaySign(false)]
// We do not need this to be signed right now.
//[assembly: AssemblyKeyFile("keypair.snk")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/thakyZ/EDQuickLauncher")]
[assembly: NeutralResourcesLanguage("en-US")]

// The following GUID is for the ID of the typelib if this project is exposed to COM.

[assembly: Guid("923ef428-a20e-4db6-812f-e445ece4d8d0")]
[assembly: AssemblyMetadata("SquirrelAwareVersion", "1")]