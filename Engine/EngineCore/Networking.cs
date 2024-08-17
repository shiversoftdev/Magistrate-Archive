using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    /// <summary>
    /// Networking control center
    /// </summary>
    internal static class Networking
    {
        /// <summary>
        /// The size of check batches to send to the server
        /// </summary>
        internal static byte BatchSize = 128;

        private static async Task NetComm(NetMessage message)
        {
            await Task.Delay(1000);//todo
        }

        /// <summary>
        /// A network message to be sent by over the network
        /// </summary>
        internal struct NetMessage
        {
            internal NetMessageSubject Subject;
            internal byte[] Body;
        }

        internal enum NetMessageSubject : uint
        {
            /// <summary>
            /// Update the server on the states of vulns
            /// </summary>
            StateUpdate = 1,

        }

        /// <summary>
        /// Prepare and send states to the server
        /// </summary>
        /// <param name="States">The states to send to the server</param>
        /// <returns></returns>
        internal static async Task SendStates((uint,uint)[] States)
        {
            if (States == null)
                return;
            List<byte> data = new List<byte>();
            data.AddRange(BitConverter.GetBytes(States.Length));
            foreach(var state in States)
            {
                data.AddRange(BitConverter.GetBytes(state.Item1));
                data.AddRange(BitConverter.GetBytes(state.Item2));
            }
            NetMessage message = new NetMessage() { Subject = NetMessageSubject.StateUpdate, Body = data.ToArray() };
            await NetComm(message);
        }
    }
}
