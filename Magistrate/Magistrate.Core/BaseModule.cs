using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Timers;

namespace Magistrate.Core
{
    /// <summary>
    /// Base check module
    /// </summary>
    public abstract class BaseModule
    {
        private Timer InternalTimer = new Timer() { Interval = 1000 };
        private HashSet<CheckInfo> CheckArray = new HashSet<CheckInfo>();
        private bool TimerTickedOnce = false;
        /// <summary>
        /// Identity of this module, used for scoring lookups, randomly generated at compile time.
        /// </summary>
        public string ModuleID;

        public BaseModule()
        {
            InternalTimer.Elapsed += Tick;
            InternalTimer.AutoReset = false;
        }

        public void StartModule()
        {
            Tick(null, null);
        }

        protected void SetTickRate(double IntervalMS)
        {
            InternalTimer.Interval = IntervalMS + Engine.__irand.Next(0, 999);

            if (TimerTickedOnce)
                InternalTimer.Start();
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            TimerTickedOnce = true;
            foreach (var check in CheckArray)
            {
                try 
                {
                    var results = QueryState(check);

                    if (!check.Lock())
                        continue;

                    check.SetStates(results);
                } 
                catch { }

                check.Release();
            }

            InternalTimer.Start();
        }

        /// <summary>
        /// Request this module to monitor a new check
        /// </summary>
        /// <param name="info"></param>
        public void Monitor(CheckInfo info)
        {
            CheckArray.Add(info);
        }

        protected abstract List<CheckState> QueryState(CheckInfo info);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<CheckState> SingleState(byte[] data)
        {
            return new List<CheckState>() { new CheckState(data) };
        }
    }

    /// <summary>
    /// Information that allows a module to process a specific check
    /// </summary>
    public sealed class CheckInfo
    {
        /// <summary>
        /// Concurrency lock
        /// </summary>
        private bool Locked = false;

        internal const int StateSize = 0x40;
        internal object[] Data;
        private SHA512 Hasher = new SHA512Managed();
        internal List<byte[]> States = new List<byte[]>();

        private readonly List<CheckState> __maintain__ = new List<CheckState>() { };

        internal static readonly SHA512 StaticHasher = new SHA512Managed();
        public static readonly byte[] DEFAULT = new byte[StateSize];

        /// <summary>
        /// Salt injected into the hashes for this state (bruteforce resistance, different per check)
        /// </summary>
        public string ISALT = "";

        /// <summary>
        /// Operator for this check entry
        /// </summary>
        public string Operation;

        public CheckInfo(params object[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Calculate a hash for the input data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] ComputeHash(string data)
        {
            if (data == null)
                return new byte[StateSize];

            return Hasher.ComputeHash(Encoding.ASCII.GetBytes(ISALT + data));
        }

        public bool Lock()
        {
            if (Locked)
                return false;
            return Locked = true;
        }

        public bool Release()
        {
            if (Locked)
                return !(Locked = false);
            return Locked;
        }

        internal void SetStates(List<CheckState> states)
        {
            if (states == __maintain__)
                return;

            States.Clear();
            
            if (states == null)
                return;

            foreach (var state in states)
                States.Add(state.bytes);
        }

        /// <summary>
        /// Maintain previous state resolution for a frame; Used for error resistance.
        /// </summary>
        /// <returns></returns>
        public List<CheckState> Maintain()
        {
            return __maintain__;
        }

        public object GetArgument(int index)
        {
            if (index >= Data.Length)
                return null;

            return Data[index];
        }
    }

    public sealed class CheckState
    {
        public const int StateSize = 0x40;
        internal byte[] bytes = new byte[StateSize];

        public CheckState(byte[] data)
        {
            int i;

            if (data == null)
                data = new byte[0x40];

            for (i = 0; i < StateSize && i < data.Length; i++)
            {
                bytes[i] = data[i];
            }

            while (i < StateSize)
                bytes[i++] = 0;
        }

        public static ulong FastStringHash(string input)
        {
            return @const.FNV1a(input);
        }

        internal static byte[] FromAnswerString(string reportstring = "#")
        {
            return State2Key(CheckInfo.StaticHasher.ComputeHash(Encoding.ASCII.GetBytes(reportstring)));
        }

        public static byte[] State2Key(byte[] state)
        {
            byte[] final = new byte[0x20];

            if (state == null || state.Length < 0x40)
                return final;

            for (int i = 0; i < 0x20; i++)
                final[i] = (byte)(state[i] ^ state[Math.Min(state.Length - 1, i + 0x20)]);

            return final;
        }
    }

}
