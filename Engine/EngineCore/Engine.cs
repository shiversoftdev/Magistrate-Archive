using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core;
//?installer.includes

namespace Engine.Core
{
    /// <summary>
    /// Auto generated class by the installer [DO NOT EDIT]
    /// </summary>
#if DEBUG
    public class Engine : EngineFrame
#else
    internal class Engine : EngineFrame
#endif
    {

        //?installer.fields

#if DEBUG
        public Engine()
#else
        internal Engine()
#endif
        {
            //?installer.init
        }

        protected override async Task Tick()
        {
            //?installer.tick
        }

        //
    }

    //?installer.classes
}
