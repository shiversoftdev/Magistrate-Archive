using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using static Magistrate.Core.@const;

namespace Magistrate.Core
{
    /// <summary>
    /// Base engine class
    /// </summary>
    public abstract class Engine
    {
        private ForensicsData __forensics = new ForensicsData();
        private Gamestate __gamestate;
        private int __currentid = 0;
        private Random __random = new Random();
        internal static Random __irand = new Random();
        private bool FirstSerialize = true;

        public delegate void ACLWriteDelegate(string filepath);
        public ACLWriteDelegate FixFWriteACL;

        /// <summary>
        /// Engine frame used to invoke commands on the client. Lower frame commands come first
        /// </summary>
        private int __CurrentFrame = 1;

        private static int ScoreTickDuration =
#if DEBUG
            5024;
#else
            15024; // slightly different than client to prevent a deadlock
#endif

        private readonly Dictionary<Type, BaseModule> Modules = new Dictionary<Type, BaseModule>();
        private readonly List<ScoreItem> ScoringItems = new List<ScoreItem>();
        private List<string> ClientCommands = new List<string>();

        private System.Timers.Timer ScoringClock = new System.Timers.Timer();
        
        /// <summary>
        /// Register a check and require it's module type
        /// </summary>
        /// <param name="module"></param>
        protected void RegisterCheck<T>(CheckInfo Check, out T module) where T : BaseModule, new()
        {
            if(!Modules.ContainsKey(typeof(T)))
                Modules[typeof(T)] = new T();

            module = (T)Modules[typeof(T)];

            if(Check != null)
                module.Monitor(Check);
        }

        /// <summary>
        /// Register a scoring entry for the scoring client
        /// </summary>
        /// <param name="i"></param>
        protected void RegisterScoreEntry(ScoreItem item)
        {
            ScoringItems.Add(item);

            if(item.PointValue != 0)
                item.__id = __currentid++;
        }

        /// <summary>
        /// Register a forensics question (init only)
        /// </summary>
        protected void RegisterForensicsQuestion(string question, uint ID, params FQAnswer[] answers)
        {
            __forensics.AddQuestion(question, ID, answers);
        }

        /// <summary>
        /// Start the engine, and begin the scoring procedures
        /// </summary>
        public void BeginScoring()
        {
            // clear old client input buffers
            try
            {
                if (File.Exists(CSSE_PCI))
                    File.Delete(CSSE_PCI);
                if (File.Exists(CSSE_PCO))
                    File.Delete(CSSE_PCO);
            }
            catch { }

            StartEngine();

            //todo in future, maybe attempt a deserializer? PCO is a bit dangerous

            __gamestate = new Gamestate(CalcNumScoredChecks(), CalcMaxPositiveChecks(), __forensics);

            ScoringClock.Interval = ScoreTickDuration;
            ScoringClock.Elapsed += ScoringTick;
            ScoringClock.AutoReset = false;
            ScoringClock.Start();
        }

        private void ScoringTick(object sender, System.Timers.ElapsedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("TICK");
#endif
            try
            {
                // Evaluate each item's decryption state
                foreach (var item in ScoringItems)
                {
                    bool CheckStateInvoked = false;
                    foreach (var query in item.Query)
                    {
                        if (!query.Lock())
                            continue;

                        foreach (var state in query.States)
                        {
                            if (!CheckStateInvoked)
                                CheckStateInvoked = true;

                            if (item.TryDecrypt(CheckState.State2Key(state)))
                            {
                                query.Release();
                                goto NextItem;
                            }
                        }

                        query.Release();
                    }

                NextItem:;

                    if (!CheckStateInvoked)
                        item.InvokeClear();
                }
            }
            catch
            {
            }

            bool AnyDirty = false;
            // Iterate each state to determine if it is dirty
            foreach(var item in ScoringItems)
            {
                if(item.IsStateless)
                {
                    foreach(var constr in item.Constraints)
                    {
                        if ((constr.Key?.ItemState == true) && constr.Value == "or")
                        {
                            if (item.TryDecrypt(CheckState.FromAnswerString(constr.Key.AnswerString)))
                                break;
                        }
                    }
                }
                if(item.ItemDirty)
                {
                    AnyDirty = true;
                    ReportMessage(item);
                    item.CleanState();
                }
            }

            // invoked here instead of reportmessage to force a single serialize call per frame at max.
            if (AnyDirty || FirstSerialize)
            {
                SerializeGamestate(FirstSerialize);

                if(FirstSerialize)
                {
                    CMD_ForensicsParse();
                    FirstSerialize = false;
                    DispatchCommands(true);
                }
                else
                {
                    CMD_NotifyEvent();
                }
            }

            DispatchCommands(false);
            __CurrentFrame++;

            ScoringClock.Start();
        }

        /// <summary>
        /// Attempt to report a scoring message to the client buffer
        /// </summary>
        /// <param name="item"></param>
        /// <param name="Message"></param>
        private void ReportMessage(ScoreItem item)
        {
            if (item.PointValue == 0)
                return; // Do not report a state which has no value impact
#if DEBUG
            if (item.ItemState)
                Console.WriteLine($"[{item.PointValue} points] {item.ReportString}");
            else
                Console.WriteLine($"[{item.PointValue * -1} points] Check reverted");
#endif
        }

        /// <summary>
        /// Notify client of a gamestate change.
        /// </summary>
        private void CMD_NotifyEvent()
        {
            ReportCommand((__CurrentFrame, CSSE_CMD_NotifyEvent));
        }

        private void CMD_ForensicsParse()
        {
            ReportCommand((__CurrentFrame, CSSE_CMD_ForensicsParse));
        }

        private void ReportCommand(ClientCommand command)
        {
            if (ClientCommands.Count >= CSSE_COM_MAXQUEUE)
                ClientCommands.Clear();

            ClientCommands.Add(command.ToString());
        }

        /// <summary>
        /// Dispatch all bufferred commands for this frame.
        /// </summary>
        private void DispatchCommands(bool force)
        {
            if (ClientCommands.Count < 1)
                return;

            // note: there is a possible deadlock where the engine and the client check for a mutex in sync forever.
            // the solution is simply a different com tick rate between engine and client
            
            // there is another deadlock where the client mutex is never acquired. 
            // to fix this, we randomly force a lock acquisition about 10% of the time
            if (!force && !AcquireMutex(CSSE_PCI, __random.Next(100) < 10))
                return; // cant invoke a dispatch when the command mutex is not available

            try 
            {
                File.AppendAllLines(CSSE_PCI, ClientCommands);
                try { FixFWriteACL?.Invoke(CSSE_PCI); } catch { }
                ClientCommands.Clear();
            }
            finally
            {
                FreeMutex(CSSE_PCI);
            }
        }

        protected abstract void StartEngine();

        private uint CalcNumScoredChecks()
        {
            uint ct = 0;

            foreach (var c in ScoringItems)
                if (c.PointValue != 0)
                    ct++;

            return ct;
        }

        private uint CalcMaxPositiveChecks()
        {
            uint ct = 0;

            foreach (var c in ScoringItems)
                if (c.PointValue > 0)
                    ct++;

            return ct;
        }

        private void SerializeGamestate(bool Force = false)
        {
            try
            {
                foreach (var entry in ScoringItems)
                    if (entry.PointValue != 0)
                        entry.CopyTo(__gamestate.StateAt(entry.__id));

                var gsdata = __gamestate.Serialize();

                if (!(Force || AcquireMutex(CSSE_PGS, Force)))
                    return;

                try { File.WriteAllBytes(CSSE_PGS, gsdata); }
                catch { }
                finally { FreeMutex(CSSE_PGS); }
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.ToString());
#endif
            } //This method should *never* cause a fatal crash.
        }

        /// <summary>
        /// Free a pipe mutex
        /// </summary>
        /// <param name="pipename"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool FreeMutex(string pipename)
        {
            string MutexName = CSSE_MUTEX_STR(pipename, CSSE_COM_ENGINE);

            try
            {
                if (File.Exists(MutexName))
                    File.Delete(MutexName);
            }
            catch
            {
                return false;
            }
            

            return true;
        }

        /// <summary>
        /// Try to acquire a mutex for a specific pipe. If impossible, but forced, acquire it anyways.
        /// </summary>
        /// <param name="pipename"></param>
        /// <param name="force"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool AcquireMutex(string pipename, bool force = false)
        {
            if (HasMutex(pipename, CSSE_COM_ENGINE))
                return true;

            bool ClientHasMutex = HasMutex(pipename, CSSE_COM_CLIENT);

            if (ClientHasMutex && !force)
                return false;

            if (force) try { File.Delete(CSSE_MUTEX_STR(pipename, CSSE_COM_CLIENT)); } catch { }

            try
            {
                File.WriteAllText(CSSE_MUTEX_STR(pipename, CSSE_COM_ENGINE), "shiversoftdev");
            }
            catch
            {
                FreeMutex(CSSE_MUTEX_STR(pipename, CSSE_COM_ENGINE));
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return true if a mutex for the specified pipe exists
        /// </summary>
        /// <param name="pipename"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool HasMutex(string pipename, string inst)
        {
            string MutexName = CSSE_MUTEX_STR(pipename, inst);

            return File.Exists(MutexName);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ScoringData
    {
        public ulong Magic;
        public ulong TimeStartUnix;

        public uint NumChecksPassed;
        public uint NumChecksFailed;
        public uint MaxChecksPassed;
        public uint Pad1;

        public uint SI2Offset;
        public uint ScoreEntryTableOff;
        public uint ForensicsDataOff;
        public uint Pad2;

        public uint SI2PTR;
        public uint ScoreEntriesPTR;
        public uint ForensicsDataPTR;
        public uint Pad3;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ScoringEntryHead
    {
        public uint ID;
        public int ScoreValue;
        public uint ScoreMsgPtr;
        public uint CI2Ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ForensicsDataHead
    {
        public uint NumQuestions;
        public uint QuestionsPtr;
        public uint BufferSize;
        public uint Pad;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ForensicsQuestionHead
    {
        internal uint NumAnswers;
        internal uint AnswersPtr;
        internal uint QuestionID;
        internal uint Pad;
    }

    internal enum ForensicsAnswerType : uint
    {
        BoolValue = 0,
        IntValue = 1,
        FloatValue = 2,
        SingleLineFormatted = 3
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ForensicsAnswerHead
    {
        public ForensicsAnswerType AnswerType;
        public uint Pad;
        public ulong Pad2;
    }

    internal class ForensicsAnswer
    {
        private const int SIZEOF_ANSNAMETEXT = 128;
        private const int SIZEOF_ANSFORMATSTR = 128;

        internal ForensicsAnswerHead Data = new ForensicsAnswerHead();
        internal readonly byte[] NameText = new byte[SIZEOF_ANSNAMETEXT];
        internal readonly byte[] FormatString = new byte[SIZEOF_ANSFORMATSTR];

        internal ForensicsAnswer(ForensicsAnswerType t)
        {
            Data.AnswerType = t;
        }

        internal void SetLabel(string label)
        {
            for (int i = 0; i < NameText.Length; NameText[i++] = 0);

            if (label.Length >= SIZEOF_ANSNAMETEXT)
                return;

            Encoding.ASCII.GetBytes(label).CopyTo(NameText, 0);
        }

        internal void SetFormat(string format)
        {
            for (int i = 0; i < FormatString.Length; FormatString[i++] = 0) ;

            if (format.Length >= SIZEOF_ANSFORMATSTR)
                return;

            Encoding.ASCII.GetBytes(format).CopyTo(FormatString, 0);
        }

        public byte[] Serialize()
        {
            unsafe
            {
                byte[] NewBin = new byte[SizeOf()];
                uint lpCurrentIndex = 0;

                int __fahsize = sizeof(ForensicsAnswerHead);

                IntPtr __fahptr = Marshal.AllocHGlobal(__fahsize);
                Marshal.StructureToPtr(Data, __fahptr, true);
                Marshal.Copy(__fahptr, NewBin, 0, __fahsize);
                Marshal.FreeHGlobal(__fahptr);

                lpCurrentIndex += (uint)__fahsize;

                NameText.CopyTo(NewBin, lpCurrentIndex);
                lpCurrentIndex += SIZEOF_ANSNAMETEXT;

                FormatString.CopyTo(NewBin, lpCurrentIndex);
                lpCurrentIndex += SIZEOF_ANSFORMATSTR;

                return NewBin;
            }
        }
        public static int SizeOf()
        {
            unsafe
            {
                return sizeof(ForensicsAnswerHead) + SIZEOF_ANSNAMETEXT + SIZEOF_ANSFORMATSTR;
            }
        }
    }

    internal class ForensicsQuestion
    {
        private ForensicsQuestionHead Question = new ForensicsQuestionHead();
        private readonly byte[] QuestionRichText = new byte[SIZEOF_QRICHTEXT];
        private List<ForensicsAnswer> Answers = new List<ForensicsAnswer>();

        internal ForensicsQuestion(uint QuestionID)
        {
            Question.QuestionID = QuestionID;
        }

        internal void SetQuestionMessage(string message)
        {
            for (int i = 0; i < QuestionRichText.Length; QuestionRichText[i++] = 0);

            if (message.Length >= SIZEOF_QRICHTEXT)
                return;

            Encoding.ASCII.GetBytes(message).CopyTo(QuestionRichText, 0);
        }

        internal void AddAnswer(FQAnswer ans)
        {
            ForensicsAnswer answer = new ForensicsAnswer((ForensicsAnswerType)ans.Type);
            answer.SetLabel(ans.Label);
            answer.SetFormat(ans.Format);
            Answers.Add(answer);
        }

        public byte[] Serialize()
        {
            unsafe
            {
                byte[] NewBin = new byte[SizeOf()];
                uint lpCurrentIndex = 0;

                Question.NumAnswers = (uint)Answers.Count;

                int __fqhsize = sizeof(ForensicsQuestionHead);

                IntPtr __fqhptr = Marshal.AllocHGlobal(__fqhsize);
                Marshal.StructureToPtr(Question, __fqhptr, true);
                Marshal.Copy(__fqhptr, NewBin, 0, __fqhsize);
                Marshal.FreeHGlobal(__fqhptr);

                lpCurrentIndex += (uint)__fqhsize;

                QuestionRichText.CopyTo(NewBin, lpCurrentIndex);

                lpCurrentIndex += SIZEOF_QRICHTEXT;

                foreach (var entry in Answers)
                {
                    var dat = entry.Serialize();
                    dat.CopyTo(NewBin, lpCurrentIndex);
                    lpCurrentIndex += (uint)dat.Length;
                }

                return NewBin;
            }
        }

        public int SizeOf()
        {
            unsafe
            {
                return sizeof(ForensicsQuestionHead) + SIZEOF_QRICHTEXT + (ForensicsAnswer.SizeOf() * Answers.Count);
            }
        }
    }

    /// <summary>
    /// Forensics question answer structure used to generate static forensics information
    /// </summary>
    public struct FQAnswer
    {
        public uint Type;
        public string Format;
        public string Label;
        public uint Flags;
    }

    internal class ForensicsData
    {
        private ForensicsDataHead Data = new ForensicsDataHead();
        private List<ForensicsQuestion> Questions = new List<ForensicsQuestion>();

        public void AddQuestion(string question, uint ID, params FQAnswer[] answers)
        {
            ForensicsQuestion fq = new ForensicsQuestion(ID);
            fq.SetQuestionMessage(question);

            foreach(var ans in answers)
            {
                fq.AddAnswer(ans);
            }

            Questions.Add(fq);
        }

        public byte[] Serialize()
        {
            unsafe
            {
                byte[] NewBin = new byte[SizeOf()];
                uint lpCurrentIndex = 0;

                Data.NumQuestions = (uint)Questions.Count;
                Data.BufferSize = (uint)NewBin.Length;

                int __fdhsize = sizeof(ForensicsDataHead);

                IntPtr __fdhptr = Marshal.AllocHGlobal(__fdhsize);
                Marshal.StructureToPtr(Data, __fdhptr, true);
                Marshal.Copy(__fdhptr, NewBin, 0, __fdhsize);
                Marshal.FreeHGlobal(__fdhptr);

                lpCurrentIndex += (uint)__fdhsize;

                foreach(var entry in Questions)
                {
                    var dat = entry.Serialize();
                    dat.CopyTo(NewBin, lpCurrentIndex);
                    lpCurrentIndex += (uint)dat.Length;
                }

                return NewBin;
            }
        }

        public int SizeOfQuestions()
        {
            int ct = 0;

            foreach (var q in Questions)
                ct += q.SizeOf();

            return ct;
        }

        public int SizeOf()
        {
            unsafe
            {
                return sizeof(ForensicsDataHead) + SizeOfQuestions();
            }
        }
    }

    internal class ScoringEntry
    {
        public const int SIZEOF_SCOREMSG = 256;
        public const int SIZEOF_CI2 = 16;

        internal ScoringEntryHead EntryData = new ScoringEntryHead();
        internal readonly byte[] ScoreMessage = new byte[SIZEOF_SCOREMSG];
        internal readonly byte[] CI2Str = new byte[SIZEOF_CI2];

        public static int SizeOf()
        {
            unsafe
            {
                return sizeof(ScoringEntryHead) + SIZEOF_SCOREMSG + SIZEOF_CI2;
            }
        }

        public void SetScoreData(string InMSG = "", string CI2 = "")
        {
            ClearScoreData();

            if (InMSG.Length > SIZEOF_SCOREMSG - 1)
                InMSG = InMSG.Substring(0, SIZEOF_SCOREMSG - 1);

            if (CI2.Length > SIZEOF_CI2 - 1)
                CI2 = CI2.Substring(0, SIZEOF_CI2 - 1);

            Encoding.ASCII.GetBytes(InMSG).CopyTo(ScoreMessage, 0);
            Encoding.ASCII.GetBytes(CI2).CopyTo(CI2Str, 0);

            ScoreMessage[InMSG.Length] = 0x00;
            CI2Str[CI2.Length] = 0x00;
        }

        private void ClearScoreData()
        {
            for (int i = 0; i < ScoreMessage.Length; i++)
                ScoreMessage[i] = 0;

            for (int i = 0; i < CI2Str.Length; i++)
                CI2Str[i] = 0;
        }

        public byte[] Serialize()
        {
            unsafe
            {
                byte[] NewBin = new byte[SizeOf()];
                int __sentsize = sizeof(ScoringEntryHead);

                IntPtr __sentptr = Marshal.AllocHGlobal(__sentsize);
                Marshal.StructureToPtr(EntryData, __sentptr, true);
                Marshal.Copy(__sentptr, NewBin, 0, __sentsize);
                Marshal.FreeHGlobal(__sentptr);

                ScoreMessage.CopyTo(NewBin, __sentsize);
                CI2Str.CopyTo(NewBin, __sentsize + SIZEOF_SCOREMSG);

                return NewBin;
            }
        }
    }

    internal class Gamestate
    {
        private const string __MAGIC__ = "SESCORE\x00";
        private const string __CHARSET__ = "0123456789+qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM";
        private ScoringData GameData = new ScoringData();
        private readonly ScoringEntry[] ScoreEntryTable;
        private readonly ForensicsData ForensicsData;

        private string SI2
        {
            get
            {
                byte[] data = new byte[ScoringEntry.SIZEOF_CI2 - 1];
                foreach(var state in ScoreEntryTable)
                {
                    for (int i = 0; i < data.Length && state.CI2Str[i] != 0; i++)
                    {
                        data[i] = (byte)(((data[i] ^ state.CI2Str[i]) % 64) + 1);
                    }
                }

                int strlen = 0;

                int c;
                while (data.Length > strlen && (c = data[strlen] - 1) != -1)
                    data[strlen++] = (byte)(__CHARSET__[c]);

                unsafe
                {
                    fixed (byte* pAscii = data)
                    {
                        return new String((sbyte*)pAscii, 0, strlen);
                    }
                }
            }
        }

        public Gamestate(uint NumEntries, uint MaxPositive, ForensicsData ForensicsStatic)
        {
            ScoreEntryTable = new ScoringEntry[NumEntries];

            for (int i = 0; i < NumEntries; i++)
                ScoreEntryTable[i] = new ScoringEntry();
            
            GameData.MaxChecksPassed = MaxPositive;
            ForensicsData = ForensicsStatic;
        }

        /// <summary>
        /// Unchecked get method for score entry table index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ScoringEntry StateAt(int index)
        {
            return ScoreEntryTable[index];
        }

        public byte[] Serialize()
        {
            byte[] NewBin = new byte[GetSize()];
            int lpBinOffset = 0;

            // Update gamedata to accurately reflect the gamestate present

            GameData.Magic = BitConverter.ToUInt64(Encoding.ASCII.GetBytes(__MAGIC__), 0);

            GameData.NumChecksPassed = GetNumPassed();
            GameData.NumChecksFailed = GetNumPenalties();

            GameData.SI2Offset = (uint)GetSI2Offset();
            GameData.ScoreEntryTableOff = (uint)GetScoringTableOffset();
            GameData.ForensicsDataOff = (uint)GetForensicsTableOffset();

            unsafe
            {
                int __gdatsize = sizeof(ScoringData);

                IntPtr __gdatptr = Marshal.AllocHGlobal(__gdatsize);
                Marshal.StructureToPtr(GameData, __gdatptr, true);
                Marshal.Copy(__gdatptr, NewBin, lpBinOffset, __gdatsize);
                Marshal.FreeHGlobal(__gdatptr);
                lpBinOffset += __gdatsize;

                foreach(var entry in ScoreEntryTable)
                {
                    if (entry.EntryData.ScoreValue == 0 || entry.CI2Str[0] == 0)
                        continue;

                    var entryData = entry.Serialize();

                    entryData.CopyTo(NewBin, lpBinOffset);

                    lpBinOffset += entryData.Length;
                }

                var fdata = ForensicsData.Serialize();
                fdata.CopyTo(NewBin, lpBinOffset);
                lpBinOffset += fdata.Length;
            }

            Encoding.ASCII.GetBytes(SI2).CopyTo(NewBin, lpBinOffset);

            return NewBin;
        }

        private uint GetNumPassed()
        {
            uint ct = 0;

            foreach(var entry in ScoreEntryTable)
            {
                if (entry.EntryData.ScoreValue > 0 && entry.CI2Str[0] != 0)
                    ct++;
            }

            return ct;
        }

        private uint GetNumPenalties()
        {
            uint ct = 0;

            foreach (var entry in ScoreEntryTable)
            {
                if (entry.EntryData.ScoreValue < 0 && entry.CI2Str[0] != 0)
                    ct++;
            }

            return ct;
        }

        private int GetNumValidEntries()
        {
            int ct = 0;

            foreach (var entry in ScoreEntryTable)
            {
                if (entry.EntryData.ScoreValue != 0 && entry.CI2Str[0] != 0)
                    ct++;
            }

            return ct;
        }

        private int GetSize()
        {
            unsafe
            {
                return GetSI2Offset() + SI2.Length + 1;
            }
        }

        private int GetForensicsTableOffset()
        {
            unsafe
            {
                return GetScoringTableOffset() + (ScoringEntry.SizeOf() * GetNumValidEntries());
            }
        }

        private int GetSI2Offset()
        {
            unsafe
            {
                return GetForensicsTableOffset() + ForensicsData.SizeOf();
            }
        }

        private int GetScoringTableOffset()
        {
            unsafe
            {
                return sizeof(ScoringData);
            }
        }
    }

    internal class ClientCommand
    {
        public readonly string CommandString;
        public readonly int NumArgs;
        public readonly int FrameNumber;
        public readonly string[] Arguments;

        public ClientCommand(int frame, string cmd, params string[] args)
        {
            CommandString = cmd;
            NumArgs = args.Length;
            FrameNumber = frame;
            Arguments = args ?? new string[0];
        }

        public static implicit operator ClientCommand((int frame, string cmd, string[] args) CommandData)
        {
            return new ClientCommand(CommandData.frame, CommandData.cmd, CommandData.args);
        }

        public static implicit operator ClientCommand((int frame, string cmd) CommandData)
        {
            return new ClientCommand(CommandData.frame, CommandData.cmd);
        }

        public override string ToString()
        { 
            return $"{FrameNumber} {NumArgs} {CommandString} {string.Join(" ", Arguments)}";
        }
    }
}
