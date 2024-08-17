using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

internal sealed class SSHDTemplate : CheckTemplate
{
        
    internal override Task<byte[]> GetCheckValue()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// A SSHD check template
    /// </summary>
    /// <param name="args">[0]:ConfigName,[1]:ExpectedValue</param>
    internal SSHDTemplate(params string[] args)
    {
            
    }
}

