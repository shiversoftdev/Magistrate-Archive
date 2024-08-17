using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Installer.Core.Templates
{
    /// <summary>
    /// A prewrapper for a check that is ready for translation
    /// </summary>
    internal class CheckPreWrapper
    {
        internal CSTemplate Template;
        internal CheckDefinition Definition;
        internal string ClassName; //Class name
        internal string Declarator; //Declaration text
        internal string Header; //Header info
        internal string InstanceName;
        internal string StateName;
    }
}
