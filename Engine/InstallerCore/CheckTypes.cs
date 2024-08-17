using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Installer.Core
{

    //TODO:
    //UserAccountTemplate (needs to support special accounts too!!!)
    //ProgramTemplate - Checks if a program is installed, uninstalled, or updated. Checks standard installation locations. If you need an unregistered program check, use FileCheckTemplate instead.
    //WindowsOptionalFeaturesTemplate
    //AntivirusTemplate
    //SecuritySettingsTemplate

    //TODO Auto generate this file using a compile time tool. Should auto-detect templates in the project and generate the meta-data required for the engine to function.

    /*
        WARNING: Strict naming rules apply to all templates! Failing to do so will result in templates being ignored by the engine!

            1. All templates much contain Template as the last word of their name (ex: CheckTemplate, ForensicsTemplate, etc)
            2. All template names are CASE SENSITIVE. They must be named exactly the same as the class they are tied to. (ex: class CheckTemplate -> CheckTypes.CheckTemplate)
            3. All template names much match their filenames (ex: class CheckTemplate -> CheckTypes.CheckTemplate -> CheckTemplate.cs)
    */

    /// <summary>
    /// All types of checks enumerated. Must match online database.
    /// </summary>
    public enum CheckTypes : ushort
    {
        CheckTemplate = 0,
        FileVersionTemplate = 1,
        RegTemplate = 2,
        ServiceTemplate = 3,
        FileCheckTemplate = 4,
        FirewallTemplate = 5,
        ShareDetectionTemplate = 6,
        OptionalFeaturesTemplate = 7,
        UpdateTemplate = 8,
        ShareInfoTemplate = 9,
        UserAccountsTemplate = 10,
        ProgramInfoTemplate = 11,
        SSHDTemplate = 12,
        SeceditTemplate = 13,
        ForensicsContentTemplate = 14,
        VibeCheckFileTemplate = 15,
        QuarantineCheckTemplate = 16,
        ShellOutTemplate = 17,
        GroupMembersTemplate = 18,
    }
}
