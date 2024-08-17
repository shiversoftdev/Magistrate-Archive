using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magistrate.BuildTools
{
    internal sealed class ScoringManifest
    {
        /// <summary>
        /// Manifest watermarking.
        /// </summary>
        public string Magistrate { get; set; }

        /// <summary>
        /// Manifest module definitions.
        /// </summary>
        public MModuleDefinition[] ModuleDef { get; set; }

        /// <summary>
        /// Manifest check definitions.
        /// </summary>
        public MCheckDefinition[] CheckDef { get; set; }

        /// <summary>
        /// Resolve a check definition from this manifest.
        /// </summary>
        /// <param name="CheckName"></param>
        /// <returns></returns>
        public MCheckDefinition ResolveCheckDef(string CheckName, bool AssertFatal = false)
        {
            foreach (var cdef in CheckDef)
                if (cdef.Name == CheckName)
                    return cdef;

            if(AssertFatal)
                Root.Error($"Failed to find a manifest entry for check '{CheckName}'. Remember that check names are case sensitive.");
            
            return null;
        }

        public MModuleDefinition ResolveModuleDef(string ModuleName, bool AssertFatal = false)
        {
            foreach (var module in ModuleDef)
                if (module.Name == ModuleName)
                    return module;

            if (AssertFatal)
                Root.Error($"Failed to find a manifest entry for module '{ModuleName}'. Remember that module names are case sensitive.");

            return null;
        }
    }

    /// <summary>
    /// Module definition for scoring manifest.
    /// </summary>
    public class MModuleDefinition
    {
        /// <summary>
        /// Name of this module. Case sensitive.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of argument names for this module.
        /// </summary>
        public string[] Args { get; set; }
    }

    /// <summary>
    /// Check definition for scoring manifest.
    /// </summary>
    public class MCheckDefinition
    { 
        /// <summary>
        /// Name of the check.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required module import for this check.
        /// </summary>
        public string Module { get; set; }

        /// <summary>
        /// Operators that are considered valid for this check.
        /// </summary>
        public string[] Operators { get; set; }

        /// <summary>
        /// All random variants of this check that are supported for random module state injection.
        /// </summary>
        public Dictionary<string, string>[] Random { get; set; }
    }
}
