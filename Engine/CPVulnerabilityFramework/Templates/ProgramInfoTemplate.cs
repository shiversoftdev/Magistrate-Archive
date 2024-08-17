using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPVulnerabilityFramework.Templates
{
    class ProgramInfoTemplate : CheckTemplate
    {
        internal override SafeString CompletedMessage => base.CompletedMessage;

        internal override SafeString FailedMessage => base.FailedMessage;

        internal override Task<byte[]> GetCheckValue()
        {
            throw new NotImplementedException();
        }

        internal ProgramInfoTemplate(params string[] args)
        {
            //Program, TypeOfCheck
            //Typeofcheck = Installed, RunAtStart,
        }
    }
}
