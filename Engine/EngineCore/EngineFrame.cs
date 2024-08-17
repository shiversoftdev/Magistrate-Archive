using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    /// <summary>
    /// Extend to create an engine framework
    /// </summary>
    public abstract class EngineFrame
    {

#if OFFLINE
        public enum ACompareType : byte
        {
            EQ,
            NEQ,
            GT,
            GTE,
            LT,
            LTE,
            IN,
            HAS
        }
#endif

        /// <summary>
        /// Deprecated 
        /// </summary>
        public virtual string __PUBLIC => "HBzQg7WNTmCBZU9L";

        /// <summary>
        /// bitmask overwrite
        /// </summary>
        protected virtual uint __F4__ => 0xAD267499;
        
        /// <summary>
        /// The directory of the engine
        /// </summary>
        private string EngineRoot => Environment.CurrentDirectory;

        private readonly Dictionary<ushort, CheckState> STATE = new Dictionary<ushort, CheckState>();

        private readonly List<ushort> QUEUE = new List<ushort>();

        private bool Exit;

        internal string ImageName = "Default Security Scenario";

        private Dictionary<uint, SingletonHost> SingletonRegistry = new Dictionary<uint, SingletonHost>();


        /// <summary>
        /// Used for CRITICAL exits. In the future, this will invoke a bluescreen
        /// </summary>
        protected void AbortEngine()
        {
            Exit = true;
        }

        /// <summary>
        /// Is the engine running?
        /// </summary>
        /// <returns></returns>
        public bool EngineRunning()
        {
            return !Exit;
        }

        /// <summary>
        /// Set the name of the image
        /// </summary>
        /// <param name="name"></param>
        protected void SetImageName(string name)
        {
            ImageName = name;
        }

        /// <summary>
        /// Register a check's state to pipe into the appropriate scoring channel
        /// </summary>
        /// <param name="id">The id of the state</param>
        /// <param name="state">The state of the check</param>
        protected void RegisterCheck(uint id, byte[] state)
        {
            InitState(id, state);
            if (!QUEUE.Contains((ushort)id))
                QUEUE.Add((ushort)id);
        }

        /// <summary>
        /// Register a check's state to pipe into the appropriate scoring channel
        /// </summary>
        /// <param name="id">The id of the state</param>
        /// <param name="state">The state of the check</param>
        protected void RegisterCheck(uint id, uint state)
        {
            RegisterCheck(id, BitConverter.GetBytes(state));
        }

        /// <summary>
        /// Register the state of a check
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        protected void InitState(uint id, byte[] state)
        {
            if (!STATE.ContainsKey((ushort)id))
                STATE[(ushort)id] = (id, state);
            else
                STATE[(ushort)id] += state;
        }

        /// <summary>
        /// Gets a batch to push to the server, while mutating the queue.
        /// </summary>
        /// <param name="BatchSize">The maximum amount of items to retrieve</param>
        /// <returns></returns>
        internal (uint, byte[])[] PollBatch(byte BatchSize)
        {
            BatchSize = Math.Min((byte)Math.Min(Networking.BatchSize, QUEUE.Count), BatchSize);
            (uint, byte[])[] Batch = new (uint, byte[])[BatchSize];
            for(byte i = 0; i < BatchSize; i++)
            {
                try
                {
                    Batch[i] = CheckState.StatePair(QUEUE[0], STATE[QUEUE[0]]);
                }
                catch
                {
                    Batch[i] = (0xFFFFFFFF, new byte[4]); //to catch key not found exception which should be impossible unless someone manually modifies the memory
                }
                QUEUE.RemoveAt(0);
            }
            return Batch;
        }

        /// <summary>
        /// Is the engine an online engine
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
#if OFFLINE
            return false;
#else
            return true;
#endif
        }

        /// <summary>
        /// Collect the states of all of the checks on the system
        /// </summary>
        internal protected abstract System.Threading.Tasks.Task Tick();

        #region OFFLINE
        #if OFFLINE
        /// <summary>
        /// A table of expected values for states
        /// </summary>
        private readonly Dictionary<ushort, Scoring.ScoringItem> EXPECTED = new Dictionary<ushort, Scoring.ScoringItem>();

        /// <summary>
        /// Register an offline check to be scored
        /// </summary>
        protected void Expect(ushort id, Scoring.ScoringItem ExpectedState)
        {
            EXPECTED[id] = ExpectedState;
        }

        /// <summary>
        /// Check the state of an expected check
        /// </summary>
        /// <param name="id">The ID of the check to evaluate</param>
        /// <returns></returns>
        internal void Evaluate(ushort id, bool AllowChildren = false)
        {
            if (!EXPECTED.ContainsKey(id) || !STATE.ContainsKey(id))
                return; //Cant evaluate a non-existant ID
            if (EXPECTED[id].State == Scoring.ScoringState.Failed)
                return; //Cant evaluate a failed scoring state

            if (STATE[id].Parents.Count > 0 && !AllowChildren)
                return; //Dont allow children to be evaluated unless their parents are evaluated

            if (STATE[id].Failed)
                EXPECTED[id].State = Scoring.ScoringState.Failed;
            else if (!CheckPassesCombinatoryOps(id))
                EXPECTED[id].State = Scoring.ScoringState.InProgress;
            else
                EXPECTED[id].State = Scoring.ScoringState.Completed;
        }

        /// <summary>
        /// Does the check being evaluated pass the AND check
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal bool CheckPassesCombinatoryOps(ushort id)
        {
            if (!EXPECTED.ContainsKey(id) || !STATE.ContainsKey(id))
                return false; //No state exists, cannot evaluate

            bool MyState = Validate(STATE[id].State, EXPECTED[id].Expected);

            if(!MyState)
            {
                foreach (CheckState s in STATE[id].Or)
                {
                    if (CheckPassesCombinatoryOps(s.ID))
                    {
                        MyState = true;
                        break;
                    }
                }
            }

            if (!MyState)
                return false; // Given the only checks remaining are AND checks, we dont even need to continue if we arent validated

            foreach (CheckState s in STATE[id].And)
            {
                if (!CheckPassesCombinatoryOps(s.ID))
                {
                    return false; //We failed a required AND check. Short circuit.
                }
            }

            return true;
        }

        private bool Validate(byte[] state, List<(ACompareType compareop, byte[] data)> states) //validate is an ORops on all possible query states
        {
            foreach((ACompareType compareop, byte[] data) PossibleState in states)
            {
                switch (PossibleState.compareop)
                {
                    case ACompareType.EQ:
                        if (StateEQ(state, PossibleState.data))
                            return true;
                        break;
                    case ACompareType.NEQ:
                        if (!StateEQ(state, PossibleState.data))
                            return true;
                        break;
                    //TODO Other stateops
                }
            }
            return false;
        }

        private bool StateEQ(byte[] state0, byte[] state1) //compare state equality. states MUST be equal length to be considered equal
        {
            if (state0.Length != state1.Length)
                return false;
            for (int i = 0; i < state0.Length; i++)
                if (state0[i] != state1[i])
                    return false;
            return true;
        }


        private List<string> items = new List<string>();
        internal int NumSuccessfulChecks
        {
            get
            {
                int count = 0;

                foreach (ushort id in EXPECTED.Keys)
                {
                    if (EXPECTED[id].Scored && EXPECTED[id].State == Engine.Core.Scoring.ScoringState.Completed && EXPECTED[id].NumPoints > 0)
                        count++;
                    if (EXPECTED[id].Scored && EXPECTED[id].State == Engine.Core.Scoring.ScoringState.Failed && EXPECTED[id].NumPoints < 0)
                        count++;
                }

                return count;
            }
        }

        internal string[] ScoringItems
        {
            get
            {
                items.Clear();
                foreach(ushort id in EXPECTED.Keys)
                {
                    EXPECTED[id].ID = id;
                    string result = EXPECTED[id].ToString();
                    if (result.Length > 0)
                        items.Add(result);
                }
                return items.ToArray();
            }
        }

        /// <summary>
        /// Gets the number of good checks in the engine
        /// </summary>
        internal int Count
        {
            get
            {
                int count = 0;
                foreach(var k in EXPECTED.Keys)
                {
                    if (EXPECTED[k].Scored && EXPECTED[k].NumPoints > 0)
                        count++;
                }
                return count;
            }
        }

        #endif
        #endregion

        /// <summary>
        /// Fail the check permanently
        /// </summary>
        /// <param name="id">The check to fail</param>
        public void Fail(ushort id)
        {
            STATE[id].EngineFlags |= (byte)CheckRuntimeFlags.Failure;
        }

        /// <summary>
        /// Flags that are set and managed by the engine at runtime
        /// </summary>
        [Flags]
        public enum CheckRuntimeFlags : byte
        {
            /// <summary>
            /// The check failed and will no longer be scored
            /// </summary>
            Failure = 1,

            /// <summary>
            /// The check is disabled
            /// </summary>
            Disabled = 2,

            /// <summary>
            /// The check state is corrupted, just discard
            /// </summary>
            Invalid = 128
        }

        /// <summary>
        /// The state of a check
        /// </summary>
        private sealed class CheckState
        {
            internal byte DefFlags;
            internal byte EngineFlags;
            internal byte[] State;
            internal ushort ID;

            /// <summary>
            /// List of checks that can only be completed after this check is complete
            /// </summary>
            internal List<CheckState> Before = new List<CheckState>();

            /// <summary>
            /// List of checks that may be completed instead of this check that will assume this check's identity
            /// </summary>
            internal List<CheckState> Or = new List<CheckState>();

            /// <summary>
            /// List of checks that must be completed for this check to be considered complete. Checks under this will assume this check's identity
            /// </summary>
            internal List<CheckState> And = new List<CheckState>();

            /// <summary>
            /// If we have any parents, a list of parent states that own us. One of our parent must be enabled if we have any parents.
            /// </summary>
            internal List<CheckState> Parents = new List<CheckState>();

            /// <summary>
            /// Will resolve true if ANY of our parents, or our parents parents, etc. contain the target. Prevents circular trees
            /// </summary>
            /// <param name="TargetParent"></param>
            /// <returns></returns>
            internal bool HasParentalRelation(CheckState Target)
            {
                //Edge case optimization
                foreach (CheckState s in Parents)
                    if (s == Target)
                        return true;

                foreach (CheckState s in Parents)
                {
                    if (s.HasParentalRelation(Target))
                        return true;
                }

                return false;
            }

            internal bool Failed
            {
                get { return (EngineFlags & (byte)CheckRuntimeFlags.Failure) > 0; }
            }

            internal bool Enabled //TODO implement this properly. the checktemplatebase needs to actually set this using a frame invokation
            {
                get { return (EngineFlags & (byte)CheckRuntimeFlags.Disabled) == 0; }
            }

            public static implicit operator CheckState((uint, byte[]) pair)
            {
                CheckState state = new CheckState();
                //first two bytes are ID
                //3rd byte is static flags
                //4th byte is runtime flags
                state.ID = (ushort)(pair.Item1 % 65536);
                state.DefFlags = (byte)(pair.Item1 >> 16);
                state.EngineFlags = (byte)(pair.Item1 >> 24);
                state.State = pair.Item2;
                return state;
            }

            public static CheckState operator +(CheckState checkstate, byte[] newstate)
            {
                checkstate.State = newstate;
                return checkstate;
            }

            /// <summary>
            /// Create a state pair (id + flags) => state from an ID and a state
            /// </summary>
            /// <param name="id">The id of the check</param>
            /// <param name="state">The check state object</param>
            /// <returns></returns>
            public static (uint, byte[]) StatePair(ushort id, CheckState state)
            {
                return (id | ((uint)state.DefFlags << 16) | ((uint)state.EngineFlags << 24), state.State);
            }
        }

        /// <summary>
        /// All the types of scoring updates
        /// </summary>
        public enum ScoringUpdateType
        {
            /// <summary>
            /// General info. Not positive or negative, just something to be aware of.
            /// </summary>
            Info, 
            /// <summary>
            /// Low level warning. Times getting short or something on that level. Can be positive or negative.
            /// </summary>
            Warning,
            /// <summary>
            /// You failed a check permanently.
            /// </summary>
            CheckFailNoRegress,
            /// <summary>
            /// You failed a check, but can remedy it
            /// </summary>
            CheckFail,
            /// <summary>
            /// You passed a check! Congrats!
            /// </summary>
            CheckPass,
            /// <summary>
            /// You really screwed up. This is a warning from anti-cheat or admins, or a builtin warning about a very severe action.
            /// </summary>
            CriticalWarning,
            /// <summary>
            /// Minor point penalty, usually for something like time overage or messing with a part of the engine process that you shouldnt after being warned.
            /// </summary>
            PenaltyMinor,
            /// <summary>
            /// Major point penalty. May cripple your score permanently or deduct experience from your profile. Only given out for major rule infractions or by anti-cheat
            /// </summary>
            PenaltyMajor,
            /// <summary>
            /// Disqualified from this image. If this notification goes off, your image will be destroyed and your access to the image will be revoked.
            /// </summary>
            Disqual
        }

        /// <summary>
        /// Bind in the engine to subscribe to scoring updates.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="title">Title of the message to send</param>
        /// <param name="UpdateType">The type of update that is being used</param>
        /// <param name="ScoringUpdateParams">Parameters for the update type</param>
        public delegate void ScoringUpdate(string message, string title, ScoringUpdateType UpdateType, params object[] ScoringUpdateParams);

        /// <summary>
        /// Bind the engine to subscribe to scoring updates
        /// </summary>
        public ScoringUpdate ScoresUpdate;

        /// <summary>
        /// An event to bind when requesting a scoring link. General purpose event. exceptions are automatically caught and handled.
        /// </summary>
        public event EventHandler WriteScoringLink;
        
        public virtual string ScoringReportLinkLocation
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Scoring Report.lnk");
            }
        }

        public string ScoringReportIndex
        {
            get
            {
                return Path.Combine(Engine.Core.Scoring.PublicReadPath, "index.html"); //TODO: change to localhost
            }
        }

        /// <summary>
        /// Request a re-write of the scoring report files
        /// </summary>
        public void RequestSRP()
        {
            string ZipPath = Path.Combine(EngineRoot, "score-report.zip");
            //Moving scoring report responsibilities to the engine core.

            try
            {   
                //Note: Linux really doesn't like us attempting to extract when files already exist soooo.... we have to clear the directory EXCEPT for data.json
                if(!File.Exists(ZipPath))
                {
                    //Get pub read zip and extract
                    using (var resource = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("EngineCore.score-report.zip"))
                    {
                        using (var file = new FileStream(ZipPath, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }

                DirectoryInfo d = new DirectoryInfo(Engine.Core.Scoring.PublicReadPath);
                if (!d.Exists)
                    d.Create();
                else
                {
                    DeleteRecursiveExcept(new string[] { "data.json" }, d, true);
                }

                ZipFile.ExtractToDirectory(ZipPath, Engine.Core.Scoring.PublicReadPath); //Note: there will be a bug if you try to add data.json to the zip. This also will die if SRP is open afaik
            }
            catch(Exception e)
            {
#if DEBUG
                Console.Write("Exception occured when extracting scoring report: " + e.ToString());
#endif
            }
            try
            {
                if (!File.Exists(Path.Combine(Engine.Core.Scoring.PublicReadPath, "srpview.dll")))
                    File.Copy("srpview.dll", Path.Combine(Engine.Core.Scoring.PublicReadPath, "srpview.dll"));
            }
            catch
            {

            }
        }

        private void DeleteRecursiveExcept(string[] FileNames, DirectoryInfo d, bool PreserveRoot=false)
        {
            foreach(FileInfo f in d.GetFiles())
            {
                if(f.Exists && !FileNames.ToList().Contains(f.Name.ToLower().Trim()))
                    try { f.Delete(); } catch { }
            }

            foreach (DirectoryInfo di in d.GetDirectories())
                DeleteRecursiveExcept(FileNames, di);

            if (!PreserveRoot && d.GetDirectories().Length < 1 && d.GetFiles().Length < 1)
                d.Delete();
        }

        /// <summary>
        /// Register a singleton class to the singleton registry
        /// </summary>
        protected void RegisterSingleton(uint __alt__, SingletonHost Singleton)
        {
            uint rid = GetRID(__alt__);
            if (!SingletonRegistry.ContainsKey(rid))
                SingletonRegistry[rid] = Singleton;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private uint GetRID(uint __alt__)
        {
            return ~(__F4__ & __alt__);
        }

        /// <summary>
        /// Delegate type as a template for checks to bind
        /// </summary>
        /// <param name="SingletonID"></param>
        /// <param name="client"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public delegate Task<object[]> SingletonRequestDelegate(uint SingletonID, SingletonHost client, params object[] args);

        protected async Task<object[]> RequestSingletonData(uint __alt__, SingletonHost client, params object[] args)
        {
            uint rid = GetRID(__alt__);

            if (!SingletonRegistry.ContainsKey(rid) || SingletonRegistry[rid] == null)
                return null;
            Console.WriteLine("Singleton Invokation success!");
            return await SingletonRegistry[rid].RequestSingleAction(client, args);
        }

        public interface SingletonHost
        {
            Task<object[]> RequestSingleAction(SingletonHost Client, object[] args);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void BEFORE(ushort a, ushort b)
        {
            if (STATE.ContainsKey(a) && STATE.ContainsKey(b))
                if (!STATE[a].Before.Contains(STATE[b]) && !STATE[a].HasParentalRelation(STATE[b]))
                {
                    STATE[b].Parents.Add(STATE[a]);
                    STATE[a].Before.Add(STATE[b]);
#if OFFLINE
                    if(EXPECTED.ContainsKey(b))
                        EXPECTED[b].NotScored();
#endif
                }
                    
            RebuildEvalTree();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void OR(ushort a, ushort b)
        {
            if (STATE.ContainsKey(a) && STATE.ContainsKey(b))
                if (!STATE[a].Or.Contains(STATE[b]) && !STATE[a].HasParentalRelation(STATE[b]))
                {
                    STATE[b].Parents.Add(STATE[a]);
                    STATE[a].Or.Add(STATE[b]);
#if OFFLINE
                    if (EXPECTED.ContainsKey(b))
                        EXPECTED[b].NotScored();
#endif
                }

            RebuildEvalTree();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected void AND(ushort a, ushort b)
        {
            if (STATE.ContainsKey(a) && STATE.ContainsKey(b))
                if (!STATE[a].And.Contains(STATE[b]) && !STATE[a].HasParentalRelation(STATE[b]))
                {
                    STATE[b].Parents.Add(STATE[a]);
                    STATE[a].And.Add(STATE[b]);
#if OFFLINE
                    if (EXPECTED.ContainsKey(b))
                        EXPECTED[b].NotScored();
#endif
                }

            RebuildEvalTree();
        }

        private void RebuildEvalTree()
        {
            //doesnt actually do anything yet. TODO: future eval tree for optimized evaluation
        }

    }
}
