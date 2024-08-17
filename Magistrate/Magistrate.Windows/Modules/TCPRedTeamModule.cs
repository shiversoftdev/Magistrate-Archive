using Magistrate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Magistrate.Windows.Modules
{
    internal sealed class TCPRedTeamModule : BaseModule
    {
        private bool DebugVerbose = false;
        public TCPRedTeamModule()
        {
#if DEBUG
            SetTickRate(20000);
#else
            SetTickRate(60000 * 5);//every 5 minutes
#endif
        }
        
        //note: in the future all this stuff needs to go 100% async.
        //modules need to report their states on their own.
        //all these modules basically run synchronously which results in some annoying artifacts such as this
        //we have a denial of service opportunity here, but it will affect the entire engine.
        //if some smart ass decides to DoS us with a massive data stream on the affected port/address
        //the entire scoring subsystem will synchronously await the data
        //crafed optimally, this will result in MaxDataChunks * 3000 ms overall op time, which can be bad
        //in the original check this was written for, this would result in a 300 second DoS!
        //due to the tickrate of this check, this would result in an *infinite denial of service*
        //is there any advantage to attacking the engine like this? no. but it is still a concern when
        //a client can manipulate the execution state of the engine. this could be combined with
        //other oddities to result in a targeted timing attack, or any number of other interesting attacks
        protected override List<CheckState> QueryState(CheckInfo info)
        {
            string cmdline = info.GetArgument(0)?.ToString();
            string host = info.GetArgument(1)?.ToString();
            string portstring = info.GetArgument(2)?.ToString();

            if (cmdline == null || host == null || portstring == null)
                return SingleState(CheckInfo.DEFAULT);

            if(!int.TryParse(portstring, out int port))
                return SingleState(CheckInfo.DEFAULT);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                
                s.ReceiveTimeout = 3000;
                s.SendTimeout = 3000;
                
                s.Connect(host, port);

                ReceiveAllDiscard(s, 100, 1024); //dont receive more than 102400 (100kb) data, because if we are getting that much data we are probably getting screwed with

                ChunkifySendString(s, cmdline, 1024);

                ReceiveAllDiscard(s, 100, 1024); //dont kill conn early! nc will eat shit and die

#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine("Sent successfully (or so we think)");
#endif
            }
            catch(Exception e)
            {
#if DEBUG
                if (DebugVerbose)
                    Console.WriteLine(e.ToString());
#endif
            }
            finally
            {
                System.Threading.Thread.Sleep(1000);
                s.Close();
            }

            return SingleState(CheckInfo.DEFAULT);
        }

        private void ChunkifySendString(Socket s, string message, int DataBufferSize = 1024)
        {
            byte[] dbuffer = null;
            int ChunkTakeSize;
            List<byte> data = new List<byte>();
            data.AddRange(Encoding.ASCII.GetBytes(message));
            data.Add(0);

            while(data.Count > 0)
            {
                // Zero out buffer
                dbuffer = new byte[DataBufferSize];

                ChunkTakeSize = Math.Min(data.Count, DataBufferSize);

                // copy a chunk
                data.GetRange(0, ChunkTakeSize);
                data.CopyTo(dbuffer);

                // remove data
                data.RemoveRange(0, ChunkTakeSize);

                s.Send(dbuffer, SocketFlags.None);
            }
        }

        /// <summary>
        /// unsafe, throws exceptions, discards data, and is basically a really mean, dirty function.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="MaxDataBufferCount"></param>
        /// <param name="DataBufferSize"></param>
        private void ReceiveAllDiscard(Socket s, int MaxDataBufferCount = 100, int DataBufferSize = 1024)
        {
            byte[] dbuffer = new byte[DataBufferSize];
            try { while (s.Receive(dbuffer, DataBufferSize, SocketFlags.None) > 0 && --MaxDataBufferCount > 0) ; } catch { }
        }
    }
}
