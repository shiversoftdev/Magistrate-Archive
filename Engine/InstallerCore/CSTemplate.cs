using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Installer.Core
{
    /// <summary>
    /// A c# template file format
    /// </summary>
    public sealed class CSTemplate
    {
        private CSTemplate()
        {
        }

        internal string[] Includes;

        internal string[] CodeLines;

        public static implicit operator CSTemplate(string[] FileLines)
        {
            List<string> includes = new List<string>();
            List<string> Code = new List<string>();
            foreach(string s in FileLines)
            {
                if(s.Length > 8 && s.Substring(0,8) == "//?using")
                {
                    includes.Add(s.Replace("//?using", "using"));
                }
                else
                {
                    Code.Add(s);
                }
            }
            return new CSTemplate() { Includes = includes.ToArray(), CodeLines = Code.ToArray() };
        }
    }
}