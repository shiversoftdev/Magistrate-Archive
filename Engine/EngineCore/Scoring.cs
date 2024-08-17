using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Dynamic;
//TODO: Failure states through event binding: translator binds the failure event to a built in engine function, FAIL(ID), which for online reports to the networking layer, and offline, fails the eval.
//Online will report a detected failure, and allow server to qualify it through server side logic

//TODO: optional flag for checks to prevent state regression (optimization)

namespace Engine.Core
{
    /// <summary>
    /// The scoring singleton
    /// </summary>
    public static class Scoring
    {
        /// <summary>
        /// Number of ms to delay in between scoring
        /// </summary>
        private const int EngineTickDelay = 1000;

        internal static EngineFrame Engine;
        private static Thread scoring_thread;

        private const string ScoresName = "data.json";
        private const string ScoringDataTemplate =
        @"data =
        {
            ""scoring_info"": 
            {
                ""name"": ""{{name}}"",
                ""start_time"": ""{{start_time}}"",
                ""running_time"": ""{{running_time}}"",
                ""checks_total"": {{checks_total}},
                ""checks_complete"": {{checks_complete}}
            },
            ""checks"": 
            {
                {{checks}}
            }
        };
        ";

        private const string CheckDataTemplate =
        @"
            ""{{check_id}}"": 
            {
                ""points"": {{points}},
                ""description"": ""{{description}}""
            }
        ";

        internal static string PublicReadPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "csse-pub");// /usr/share for linux
            }
        }

        private static string ScoresPath
        {
            get
            {
                return Path.Combine(PublicReadPath, ScoresName);
            }
        }

        /// <summary>
        /// Start the engine. Can only be called once
        /// </summary>
        /// <param name="engine"></param>
        public static Thread StartEngine(EngineFrame engine)
        {
            if (Engine != null)
                return null; //throw new InvalidOperationException("An engine is already running. Cannot start a new engine in this instance.");
            Engine = engine;
            try
            {
                Directory.CreateDirectory(PublicReadPath); //if this gets deleted during a comp there is an issue with the anti-cheat driver. This is mainly for debugging
            }
            catch
            {
            }
            scoring_thread = new Thread(new ThreadStart(Score))
            {
                IsBackground = true
            };
            scoring_thread.Start();
            return scoring_thread;
        }

        /// <summary>
        /// Main Scoring thread
        /// </summary>
        private static async void Score()
        {
            while(Engine?.EngineRunning() ?? false)
            {
                await Engine.Tick();

                (uint, byte[])[] Batch = Engine.PollBatch(Networking.BatchSize);
#if OFFLINE
                await OfflineTick(Batch);
#else
                await Networking.SendStates(Batch);
#endif
                await Task.Delay(EngineTickDelay);
            }
        }

#region OFFLINE
#if OFFLINE
        /// <summary>
        /// The state of a scoring item
        /// </summary>
        public enum ScoringState
        {
            InProgress,
            Completed,
            Failed
        }

        //TODO: Move scoring code into the internal side of the engine, making the engine interface through its inherited properties so we dont have to use a public item...

        /// <summary>
        /// An offline scoring item
        /// </summary>
        public sealed class ScoringItem
        {

            private ScoringState _state_ = ScoringState.InProgress;
            /// <summary>
            /// The state of this scoring item
            /// </summary>
            public Scoring.ScoringState State //TODO: just call a point notify event that is inside the engine core so that the networking can use the same logic.
            {
                get
                {
                    return _state_;
                }

                internal set
                {
                    if (value == _state_)
                        return;

                    if (Scored) try
                    {
                        if (value == ScoringState.Completed)
                        {
                            if (NumPoints >= 0)
                                Engine?.ScoresUpdate("You gained " + NumPoints + " points!", "Points Earned!", EngineFrame.ScoringUpdateType.CheckPass, new object[] { ID, "gain.wav" });
                            else
                                Engine?.ScoresUpdate("You lost  " + Math.Abs(NumPoints) + " points!", "Points Lost!", EngineFrame.ScoringUpdateType.CheckFail, new object[] { ID, "loss.wav" });
                        }
                        else if (value == ScoringState.Failed)
                        {
                            if (NumPoints < 0)
                                Engine?.ScoresUpdate("You gained " + Math.Abs(NumPoints) + " points!", "Points Earned!", EngineFrame.ScoringUpdateType.CheckPass, new object[] { ID, "gain.wav" });
                            else
                                Engine?.ScoresUpdate("You lost  " + Math.Abs(NumPoints) + " points!", "Points Lost!", EngineFrame.ScoringUpdateType.CheckFail, new object[] { ID, "loss.wav" });
                        }
                        else if (value == ScoringState.InProgress)
                        {
                            if (NumPoints >= 0)
                            {
                                if (_state_ == ScoringState.Completed)
                                {
                                    Engine?.ScoresUpdate("You lost  " + Math.Abs(NumPoints) + " points!", "Points Lost!", EngineFrame.ScoringUpdateType.Warning, new object[] { ID, "loss.wav" });
                                }
                                else
                                {
                                    Engine?.ScoresUpdate("You regained  " + Math.Abs(NumPoints) + " points!", "Points Earned!", EngineFrame.ScoringUpdateType.Warning, new object[] { ID, "gain.wav" });
                                }
                            }
                            else
                            {
                                if (_state_ == ScoringState.Completed)
                                {
                                    Engine?.ScoresUpdate("You regained  " + Math.Abs(NumPoints) + " points!", "Points Earned!", EngineFrame.ScoringUpdateType.Warning, new object[] { ID, "gain.wav" });
                                }
                                else
                                {
                                    Engine?.ScoresUpdate("You lost  " + Math.Abs(NumPoints) + " points!", "Points Lost!", EngineFrame.ScoringUpdateType.Warning, new object[] { ID, "loss.wav" });
                                }
                            }
                        }
                    } catch { /* Couldnt report the score, dont crash the engine though... */ }
                    
                    try { Engine?.RequestSRP(); } catch {  }

                    _state_ = value;
                }
            }

            /// <summary>
            /// The expected value of this scoring item
            /// </summary>
            public uint ExpectedState = 0;

            /// <summary>
            /// The expected values of this scoring item
            /// </summary>
            public List<(EngineFrame.ACompareType CompareOp, byte[] data)> Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>();

            /// <summary>
            /// The number of points this scoring item is worth
            /// </summary>
            public short NumPoints = 0;
            /// <summary>
            /// This vuln's id
            /// </summary>
            public ushort ID = 0;

            public delegate string ScoringStatus();

            public ScoringStatus SuccessStatus = () => { return "Check passed."; }; //Quick explanation: We want status safestring'd, but safestring is not cross assembly for security reasons, so
                                                                                    //we make this a delegate so that the engine derivative with safestring can return the assigned safestring...
            public ScoringStatus FailureStatus = () => { return "Check failed."; };

            internal bool Scored { get; private set; } = true;

            public void NotScored()
            {
                Scored = false;
            }

            public override string ToString()
            {
                if (!Scored)
                    return "";

                switch (State)
                {
                    case ScoringState.Completed:
                        if(NumPoints >= 0)
                            return FormatStateObj(ID, NumPoints, SuccessStatus());
                        return FormatStateObj(ID, NumPoints, FailureStatus());
                    case ScoringState.Failed:
                        if (NumPoints >= 0)
                            return FormatStateObj(ID, NumPoints, FailureStatus());
                        return FormatStateObj(ID, NumPoints, SuccessStatus());
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Score an offline engine
        /// </summary>
        /// <param name="Batch">The batch to score</param>
        /// <returns></returns>
        private static async Task OfflineTick((uint, byte[])[] Batch)
        {
            await Task.Run(() =>
            {
                foreach(var checkresult in Batch)
                {
                    Engine.Evaluate((ushort)checkresult.Item1);
                }
                WriteScoringReport();
            });
        }

        private static void WriteScoringReport()
        {
            try
            {
                if(!Directory.Exists(PublicReadPath))
                {
                    Directory.CreateDirectory(PublicReadPath);
                }

                string result = ScoringDataTemplate;
                string[] scoringitems = Engine?.ScoringItems ?? new string[0];
                int TotalItems = Engine?.Count ?? 0;

                result = result.Replace("{{name}}", Engine.ImageName);
                //TODO Start Time
                result = result.Replace("{{start_time}}", "DISABLED");
                //TODO Running Time
                result = result.Replace("{{running_time}}", "DISABLED");

                result = result.Replace("{{checks_total}}", TotalItems.ToString());
                result = result.Replace("{{checks_complete}}", Engine?.NumSuccessfulChecks.ToString() ?? "0");

                string checktext = "";

                for(int i = 0; i < scoringitems.Length; i++)
                {
                    checktext += scoringitems[i] + "\r\n" + (i == scoringitems.Length - 1 ? "" : ",");
                }

                result = result.Replace("{{checks}}", checktext);

                File.WriteAllText(ScoresPath, result);
            }
            catch { } //File is probably busy
        }

        private static string FormatStateObj(ushort id, short pointValue, string description)
        {
            return CheckDataTemplate.Replace("{{check_id}}", id.ToString()).Replace("{{points}}", pointValue.ToString()).Replace("{{description}}", description.ToString());
        }
#endif
#endregion
    }
}
