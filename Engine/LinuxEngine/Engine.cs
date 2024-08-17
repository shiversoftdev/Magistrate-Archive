using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Engine.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;
using static Engine.Core.EngineFrame;
using Engine.Core;


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

        public override string __PUBLIC => "UFmfC1rL98a2MEGI";
protected override uint __F4__ => 0xBD2F1585;

private __704fbb660a07a0504b81bc6d91b40b4c c_0; private byte[] c_0_s;


private __bb853c5712cc0b2a5e987f95b0ac3d59 c_1; private byte[] c_1_s;


private __1f5a087ae65f6de7093b8154f54fd619 c_2; private byte[] c_2_s;


private __0efff66ab46289d47dabe75601e5f20e c_3; private byte[] c_3_s;


private __1fc68463d5c68d0d036d959b4f7bb8e6 c_4; private byte[] c_4_s;


private __b991735d024e236ef6a8da81489d1c29 c_5; private byte[] c_5_s;


private __1f73da3a75f20a37c1cdb9110e7562ff c_6; private byte[] c_6_s;


private __354760caa4c2e743f26f1ece173beb90 c_7; private byte[] c_7_s;


private __2712858277ea4c3bf7f99de729f293ae c_8; private byte[] c_8_s;


private __1a4a9652eca01f14805def9f6dbfab62 c_9; private byte[] c_9_s;


private __c01e82919b7404725c6cee52e60af82d c_10; private byte[] c_10_s;


private __5481f419d5c2e139329e0a6e67f13ea3 c_11; private byte[] c_11_s;


private __de8cb45aba89a98ff61294059480438c c_12; private byte[] c_12_s;


private __4d3ff50a9fb8252ba1fc728ad7fada78 c_13; private byte[] c_13_s;


private __7ef99b6825760da113bbb5383cc2006d c_14; private byte[] c_14_s;


private __fecd4d61606bd795c1892e7cf2c91abf c_15; private byte[] c_15_s;


private __f92601c209f69906ae15b7f548a62aad c_16; private byte[] c_16_s;


private __cfaa82823ecec87726847938a4562606 c_17; private byte[] c_17_s;


private __88005bccb46180c6ed87491c24867fb8 c_18; private byte[] c_18_s;


private __3c5e7c861ca9d68c9523e27b1451d15c c_19; private byte[] c_19_s;


private __019992010296c0b41cb626e5533ad3ee c_20; private byte[] c_20_s;


private __1f5fb1f779ef65d8b042bf7a324660bd c_21; private byte[] c_21_s;


private __e13e87f1634c4cb04f221a7d437db86a c_22; private byte[] c_22_s;


private __642c58f0f0858e621a8f144f3a4abd9e c_23; private byte[] c_23_s;


private __7df5e608f75ce44fc58e8af8c9b935bc c_24; private byte[] c_24_s;


private __903a3faf53cf32e1a29c8379e7e074c1 c_25; private byte[] c_25_s;


private __38c885f73584c9bf99f79d6be441c16b c_26; private byte[] c_26_s;


private __fc4200a2f8e67c12459b90f25a2ff6ad c_27; private byte[] c_27_s;


private __2443080c260b402453289ed22812dba9 c_28; private byte[] c_28_s;


private __ff24e59d219e3bccea90a5acb6368c52 c_29; private byte[] c_29_s;


private __9b0026460cc58de2a01c727c857e8211 c_30; private byte[] c_30_s;


private __bf19db9ed12812bc1ca2149322f73379 c_31; private byte[] c_31_s;


private __a25f51b73ae79ab23d7c369df7a32401 c_32; private byte[] c_32_s;


private __8502165c9d443e894a55d2df67368597 c_33; private byte[] c_33_s;



#if DEBUG
        public Engine()
#else
        internal Engine()
#endif
        {
            try { c_0 = new __704fbb660a07a0504b81bc6d91b40b4c((SafeString)(new byte[]{ 123,133,151,94,113,194,124,95,178,114,140,84,110,5,124,206,206,62,230,195,53,206,198,249,158,231,86,190,37,246,116,132,}) , (SafeString)(new byte[]{ 190,67,234,138,150,14,116,148,255,239,138,119,166,176,24,97,177,5,173,180,73,140,247,56,143,154,19,22,43,152,26,200,}) ){ Flags = (byte)0, __ALT__ = (uint)2033046078, RequestSingletonData = RequestSingletonData }; c_0.CheckFailed = () => { Fail((ushort)0); }; RegisterSingleton((uint)2080239702, c_0); Scoring.ScoringItem si_0 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x1,0x7D,0xCF,0x4E, }),  }, NumPoints = (short)2, ID = (ushort)0}; si_0.SuccessStatus = () => { return c_0.CompletedMessage; }; si_0.FailureStatus = () => { return c_0.FailedMessage; };Expect((ushort)0,si_0); InitState(0, new byte[0]);si_0.SuccessStatus = () => { return (SafeString)(new byte[]{ 155,36,148,130,48,102,171,11,13,199,119,252,80,185,40,132,139,17,69,135,255,110,35,128,153,178,122,96,5,238,99,82,196,186,48,134,98,93,126,121,94,147,234,149,132,249,143,48, }); };} catch { }
try { c_1 = new __bb853c5712cc0b2a5e987f95b0ac3d59((SafeString)(new byte[]{ 186,166,69,168,92,4,185,55,109,139,176,233,242,251,237,202,24,0,212,85,169,15,108,88,227,71,221,241,105,225,160,198,}) , (SafeString)(new byte[]{ 152,73,231,143,253,51,186,220,157,39,86,213,228,79,23,80,59,152,125,224,130,232,154,236,254,187,7,205,64,239,248,218,}) ){ Flags = (byte)0, __ALT__ = (uint)2038257734, RequestSingletonData = RequestSingletonData }; c_1.CheckFailed = () => { Fail((ushort)1); }; RegisterSingleton((uint)2070772340, c_1); Scoring.ScoringItem si_1 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x99,0xAF,0x4C,0xCF, }),  }, NumPoints = (short)4, ID = (ushort)1}; si_1.SuccessStatus = () => { return c_1.CompletedMessage; }; si_1.FailureStatus = () => { return c_1.FailedMessage; };Expect((ushort)1,si_1); InitState(1, new byte[0]);si_1.SuccessStatus = () => { return (SafeString)(new byte[]{ 0,7,11,131,230,224,197,171,143,15,96,44,100,26,13,186,44,130,4,141,67,34,34,106,87,30,73,35,108,92,240,176,133,238,14,62,151,145,59,246,133,86,18,157,91,244,241,145, }); };} catch { }
try { c_2 = new __1f5a087ae65f6de7093b8154f54fd619((SafeString)(new byte[]{ 122,56,33,221,21,143,240,38,157,208,59,95,132,155,55,191,83,61,65,12,135,211,202,221,179,204,147,165,107,208,26,251,}) , (SafeString)(new byte[]{ 190,131,216,2,198,4,226,158,121,164,195,3,235,117,239,225,139,166,163,8,87,202,18,102,173,187,127,238,15,246,225,140,}) ){ Flags = (byte)0, __ALT__ = (uint)979959149, RequestSingletonData = RequestSingletonData }; c_2.CheckFailed = () => { Fail((ushort)2); }; RegisterSingleton((uint)2015917413, c_2); Scoring.ScoringItem si_2 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x83,0x55,0x66,0xDF, }),  }, NumPoints = (short)69, ID = (ushort)2}; si_2.SuccessStatus = () => { return c_2.CompletedMessage; }; si_2.FailureStatus = () => { return c_2.FailedMessage; };Expect((ushort)2,si_2); InitState(2, new byte[0]);} catch { }
try { c_3 = new __0efff66ab46289d47dabe75601e5f20e((SafeString)(new byte[]{ 24,30,131,131,209,63,238,62,144,152,214,110,192,70,38,221,101,186,134,25,108,40,93,168,26,77,183,237,133,99,56,89,65,10,26,108,248,20,226,20,214,55,96,148,30,29,136,101,}) , (SafeString)(new byte[]{ 71,63,63,100,95,40,11,172,39,249,108,53,118,151,186,157,118,214,2,244,35,8,174,218,69,130,209,179,245,166,223,22,}) , (SafeString)(new byte[]{ 203,166,102,43,237,130,236,52,53,8,255,205,43,104,18,101,219,248,150,202,254,177,114,134,178,211,229,3,5,216,210,85,43,90,238,16,240,40,80,188,234,134,145,72,207,131,55,58,49,180,21,236,127,218,137,233,183,82,139,3,199,166,192,37,}) ){ Flags = (byte)0, __ALT__ = (uint)1837954233, RequestSingletonData = RequestSingletonData }; c_3.CheckFailed = () => { Fail((ushort)3); }; RegisterSingleton((uint)798758531, c_3); Scoring.ScoringItem si_3 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xA8,0x1D,0x32,0x3F,0x43,0x64,0x39,0x6D,0x44,0x4A,0xD2,0xFB,0xBA,0x6F,0x45,0xC, }),  }, NumPoints = (short)2, ID = (ushort)3}; si_3.SuccessStatus = () => { return c_3.CompletedMessage; }; si_3.FailureStatus = () => { return c_3.FailedMessage; };Expect((ushort)3,si_3); InitState(3, new byte[0]);si_3.SuccessStatus = () => { return (SafeString)(new byte[]{ 231,19,95,70,236,9,220,99,171,119,92,31,124,19,153,135,94,144,117,7,177,223,76,191,94,240,30,104,232,8,172,113,35,128,6,194,61,66,164,215,43,194,35,235,13,203,84,90, }); };} catch { }
try { c_4 = new __1fc68463d5c68d0d036d959b4f7bb8e6((SafeString)(new byte[]{ 86,19,235,210,100,240,223,169,3,112,95,27,207,94,212,29,95,98,124,121,236,29,111,89,105,230,126,34,232,229,135,134,248,100,168,140,113,23,99,112,34,247,12,214,101,73,230,167,}) , (SafeString)(new byte[]{ 44,145,60,118,162,105,168,203,165,102,57,105,76,250,49,244,106,226,176,205,87,162,107,149,15,48,195,111,134,64,161,40,}) , (SafeString)(new byte[]{ 69,215,115,11,174,44,1,65,28,55,104,250,199,85,126,51,220,113,73,22,178,167,241,60,65,201,136,105,34,219,109,140,174,132,84,69,185,200,8,250,22,196,165,230,113,88,170,114,192,185,131,168,17,134,187,185,104,78,93,112,32,173,39,178,}) ){ Flags = (byte)0, __ALT__ = (uint)761009843, RequestSingletonData = RequestSingletonData }; c_4.CheckFailed = () => { Fail((ushort)4); }; RegisterSingleton((uint)756866689, c_4); Scoring.ScoringItem si_4 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x9B,0x3E,0x53,0x11,0xB9,0x98,0x61,0xC7,0x22,0xFD,0x40,0xB0,0x5E,0x2,0x1D,0xD5, }),  }, NumPoints = (short)2, ID = (ushort)4}; si_4.SuccessStatus = () => { return c_4.CompletedMessage; }; si_4.FailureStatus = () => { return c_4.FailedMessage; };Expect((ushort)4,si_4); InitState(4, new byte[0]);} catch { }
try { c_5 = new __b991735d024e236ef6a8da81489d1c29((SafeString)(new byte[]{ 176,181,99,46,55,42,236,215,161,172,158,224,20,247,224,197,24,136,94,215,170,83,254,120,189,53,51,155,39,137,199,123,240,174,164,165,196,6,35,83,133,24,220,4,165,190,111,51,}) , (SafeString)(new byte[]{ 99,32,176,224,253,136,15,8,195,121,187,63,238,93,90,140,254,235,131,220,5,47,57,58,152,160,231,10,6,236,68,18,}) , (SafeString)(new byte[]{ 217,169,78,230,64,39,142,62,33,195,58,16,214,29,183,89,36,135,197,82,52,103,97,135,6,115,11,95,153,83,132,101,130,142,3,215,215,102,106,229,130,173,200,106,188,195,71,61,38,65,140,32,229,241,62,174,51,207,126,179,215,28,158,159,}) ){ Flags = (byte)0, __ALT__ = (uint)759977691, RequestSingletonData = RequestSingletonData }; c_5.CheckFailed = () => { Fail((ushort)5); }; RegisterSingleton((uint)1837932209, c_5); Scoring.ScoringItem si_5 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xC5,0x94,0x1D,0x6E,0xF0,0xDC,0xB0,0x10,0xEA,0x8F,0x36,0xE6,0x84,0x52,0x96,0x56, }),  }, NumPoints = (short)3, ID = (ushort)5}; si_5.SuccessStatus = () => { return c_5.CompletedMessage; }; si_5.FailureStatus = () => { return c_5.FailedMessage; };Expect((ushort)5,si_5); InitState(5, new byte[0]);si_5.SuccessStatus = () => { return (SafeString)(new byte[]{ 56,102,132,157,219,105,32,239,7,179,162,71,51,149,159,209,19,22,28,34,223,47,14,134,180,145,122,100,186,209,156,84,251,41,152,205,255,168,52,161,242,251,216,118,130,53,88,163, }); };} catch { }
try { c_6 = new __1f73da3a75f20a37c1cdb9110e7562ff((SafeString)(new byte[]{ 79,92,200,88,82,88,140,43,88,24,72,197,66,14,6,228,226,79,56,177,57,86,40,100,188,18,241,11,85,47,158,121,217,171,202,232,245,47,153,102,84,189,138,227,101,71,169,9,}) , (SafeString)(new byte[]{ 11,23,69,54,122,208,19,69,196,100,7,144,18,130,36,245,98,222,192,24,130,10,22,90,202,218,255,72,134,145,247,79,}) ){ Flags = (byte)0, __ALT__ = (uint)2050555693, RequestSingletonData = RequestSingletonData }; c_6.CheckFailed = () => { Fail((ushort)6); }; RegisterSingleton((uint)2053685055, c_6); Scoring.ScoringItem si_6 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x83,0x55,0x66,0xDF, }),  }, NumPoints = (short)3, ID = (ushort)6}; si_6.SuccessStatus = () => { return c_6.CompletedMessage; }; si_6.FailureStatus = () => { return c_6.FailedMessage; };Expect((ushort)6,si_6); InitState(6, new byte[0]);} catch { }
try { c_7 = new __354760caa4c2e743f26f1ece173beb90((SafeString)(new byte[]{ 148,137,73,168,235,172,204,218,244,61,196,111,38,249,4,40,57,249,84,152,170,138,127,19,47,44,119,156,231,253,240,176,163,128,225,15,241,215,87,16,49,229,166,125,59,13,251,131,}) , (SafeString)(new byte[]{ 61,185,247,174,244,27,134,5,185,214,92,182,108,226,216,216,105,199,86,196,50,16,203,160,114,121,105,191,77,147,155,57,}) , (SafeString)(new byte[]{ 173,123,205,161,218,237,129,170,234,20,5,87,71,112,13,236,195,129,148,102,63,35,154,174,55,23,61,143,74,251,129,33,30,51,32,190,7,167,9,137,111,239,126,240,140,40,164,170,160,224,184,11,137,43,96,184,210,182,177,246,94,188,101,69,}) ){ Flags = (byte)0, __ALT__ = (uint)756817545, RequestSingletonData = RequestSingletonData }; c_7.CheckFailed = () => { Fail((ushort)7); }; RegisterSingleton((uint)765255409, c_7); Scoring.ScoringItem si_7 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xC0,0x64,0x0,0xBE,0x3E,0x78,0xAC,0x1C,0xDC,0xEC,0xC3,0x85,0x53,0xB,0x82,0xDE, }),  }, NumPoints = (short)2, ID = (ushort)7}; si_7.SuccessStatus = () => { return c_7.CompletedMessage; }; si_7.FailureStatus = () => { return c_7.FailedMessage; };Expect((ushort)7,si_7); InitState(7, new byte[0]);si_7.SuccessStatus = () => { return (SafeString)(new byte[]{ 2,246,158,240,26,87,92,42,84,198,148,65,45,197,29,235,94,186,214,55,67,40,40,154,6,133,11,225,166,40,24,123,160,214,191,224,120,120,60,175,71,31,143,157,179,225,151,43, }); };} catch { }
try { c_8 = new __2712858277ea4c3bf7f99de729f293ae((SafeString)(new byte[]{ 166,25,74,143,9,38,152,45,121,86,56,233,166,31,132,192,55,145,143,108,164,206,33,89,173,76,48,120,7,224,5,173,132,24,175,56,46,14,171,120,195,155,125,98,189,239,227,66,100,184,112,179,141,108,247,133,72,67,44,188,150,1,191,106,127,158,126,59,223,188,127,129,166,152,47,129,211,65,215,14,119,246,131,93,227,31,79,214,97,183,250,39,117,67,202,63,21,37,91,7,37,87,189,101,58,150,147,42,102,88,69,174,85,92,53,120,166,170,133,129,52,183,154,57,233,65,241,29,130,186,87,41,150,200,192,233,229,2,130,156,244,79,72,233,}) , (SafeString)(new byte[]{ 171,116,234,113,252,0,97,104,216,129,104,107,54,200,180,88,80,237,224,116,183,192,141,81,56,157,108,208,222,186,136,117,}) ){ Flags = (byte)0, __ALT__ = (uint)2041729060, RequestSingletonData = RequestSingletonData }; c_8.CheckFailed = () => { Fail((ushort)8); }; RegisterSingleton((uint)2033334790, c_8); Scoring.ScoringItem si_8 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xF7,0x41,0xFA,0x82, }),  }, NumPoints = (short)7, ID = (ushort)8}; si_8.SuccessStatus = () => { return c_8.CompletedMessage; }; si_8.FailureStatus = () => { return c_8.FailedMessage; };Expect((ushort)8,si_8); InitState(8, new byte[0]);si_8.SuccessStatus = () => { return (SafeString)(new byte[]{ 43,2,163,202,81,197,221,239,138,32,90,159,55,27,253,8,157,17,199,192,195,251,19,178,53,214,204,252,98,54,25,36,61,100,197,162,50,56,182,27,217,46,42,231,121,72,15,232, }); };} catch { }
try { c_9 = new __1a4a9652eca01f14805def9f6dbfab62((SafeString)(new byte[]{ 249,217,193,80,42,235,41,113,90,187,129,90,23,109,236,57,165,79,79,249,48,54,100,221,142,201,205,11,151,236,53,76,105,217,52,81,62,129,133,133,119,124,156,239,229,133,120,151,}) , (SafeString)(new byte[]{ 71,158,194,191,13,144,229,220,211,118,18,121,110,248,213,247,223,51,45,127,40,165,148,248,25,30,126,254,4,22,16,151,}) ){ Flags = (byte)0, __ALT__ = (uint)2024332565, RequestSingletonData = RequestSingletonData }; c_9.CheckFailed = () => { Fail((ushort)9); }; RegisterSingleton((uint)2049447181, c_9); Scoring.ScoringItem si_9 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)9}; si_9.SuccessStatus = () => { return c_9.CompletedMessage; }; si_9.FailureStatus = () => { return c_9.FailedMessage; };Expect((ushort)9,si_9); InitState(9, new byte[0]);si_9.SuccessStatus = () => { return (SafeString)(new byte[]{ 131,95,70,92,122,44,160,32,183,13,52,217,222,37,209,30,156,107,93,31,252,45,213,219,45,26,107,173,203,36,166,111,156,212,172,210,96,104,238,36,236,62,236,116,72,31,91,0,72,252,29,161,34,25,227,166,203,189,64,222,189,112,240,79, }); };} catch { }
try { c_10 = new __c01e82919b7404725c6cee52e60af82d((SafeString)(new byte[]{ 192,246,130,129,84,216,95,163,98,103,243,128,150,71,75,244,130,182,128,229,99,235,59,31,55,236,60,141,192,77,102,143,207,238,102,47,206,148,133,25,56,136,167,28,196,87,75,239,155,114,196,85,80,255,143,117,81,182,186,224,216,234,190,243,}) , (SafeString)(new byte[]{ 157,113,85,238,77,13,204,75,107,214,124,222,99,83,189,189,247,72,207,162,200,161,31,183,128,141,68,246,224,181,206,157,}) ){ Flags = (byte)0, __ALT__ = (uint)2029551391, RequestSingletonData = RequestSingletonData }; c_10.CheckFailed = () => { Fail((ushort)10); }; RegisterSingleton((uint)2063103837, c_10); Scoring.ScoringItem si_10 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)10}; si_10.SuccessStatus = () => { return c_10.CompletedMessage; }; si_10.FailureStatus = () => { return c_10.FailedMessage; };Expect((ushort)10,si_10); InitState(10, new byte[0]);} catch { }
try { c_11 = new __5481f419d5c2e139329e0a6e67f13ea3((SafeString)(new byte[]{ 117,97,43,193,114,226,198,233,117,168,238,239,232,156,67,95,9,129,248,154,55,148,48,229,131,6,121,76,163,133,143,100,198,231,138,38,162,233,161,43,225,105,119,110,161,16,46,166,201,199,132,22,229,166,50,204,242,209,43,135,28,208,177,30,}) , (SafeString)(new byte[]{ 24,124,156,118,243,30,35,169,83,191,47,219,213,96,107,159,39,61,76,79,103,222,115,224,147,28,164,151,3,237,34,36,}) ){ Flags = (byte)0, __ALT__ = (uint)2021143885, RequestSingletonData = RequestSingletonData }; c_11.CheckFailed = () => { Fail((ushort)11); }; RegisterSingleton((uint)985177933, c_11); Scoring.ScoringItem si_11 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)11}; si_11.SuccessStatus = () => { return c_11.CompletedMessage; }; si_11.FailureStatus = () => { return c_11.FailedMessage; };Expect((ushort)11,si_11); InitState(11, new byte[0]);} catch { }
try { c_12 = new __de8cb45aba89a98ff61294059480438c((SafeString)(new byte[]{ 238,107,179,137,236,141,74,123,98,86,136,254,240,142,213,180,232,124,107,155,82,219,234,128,54,249,101,135,15,181,219,147,67,165,121,94,182,15,207,236,216,169,38,104,152,26,21,55,40,222,35,22,125,117,14,128,77,131,205,167,235,102,13,181,}) , (SafeString)(new byte[]{ 53,102,239,234,227,238,157,255,223,144,87,52,243,194,132,5,228,36,254,226,116,104,197,67,89,148,60,106,29,141,73,211,}) ){ Flags = (byte)0, __ALT__ = (uint)989372231, RequestSingletonData = RequestSingletonData }; c_12.CheckFailed = () => { Fail((ushort)12); }; RegisterSingleton((uint)946363669, c_12); Scoring.ScoringItem si_12 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)12}; si_12.SuccessStatus = () => { return c_12.CompletedMessage; }; si_12.FailureStatus = () => { return c_12.FailedMessage; };Expect((ushort)12,si_12); InitState(12, new byte[0]);} catch { }
try { c_13 = new __4d3ff50a9fb8252ba1fc728ad7fada78((SafeString)(new byte[]{ 13,17,16,168,214,210,185,114,159,236,87,41,238,107,84,182,108,177,212,185,0,233,235,221,13,237,217,150,9,12,167,18,186,71,168,217,164,245,179,252,22,63,185,45,116,180,55,237,}) , (SafeString)(new byte[]{ 113,195,97,19,49,190,59,182,156,101,34,174,5,48,103,166,130,133,24,101,135,95,161,156,126,113,88,129,121,118,56,4,}) ){ Flags = (byte)0, __ALT__ = (uint)2058927981, RequestSingletonData = RequestSingletonData }; c_13.CheckFailed = () => { Fail((ushort)13); }; RegisterSingleton((uint)2021193575, c_13); Scoring.ScoringItem si_13 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)13}; si_13.SuccessStatus = () => { return c_13.CompletedMessage; }; si_13.FailureStatus = () => { return c_13.FailedMessage; };Expect((ushort)13,si_13); InitState(13, new byte[0]);si_13.SuccessStatus = () => { return (SafeString)(new byte[]{ 142,144,102,232,191,183,65,219,70,197,194,230,239,192,24,15,211,73,236,227,171,5,135,42,62,238,153,224,38,166,223,128,201,69,98,238,144,133,193,248,97,228,190,255,144,239,226,24,51,79,87,253,6,212,194,252,106,154,96,53,137,231,146,204, }); };} catch { }
try { c_14 = new __7ef99b6825760da113bbb5383cc2006d((SafeString)(new byte[]{ 53,111,103,204,233,141,168,232,46,207,58,163,146,1,83,96,47,128,134,157,30,247,146,32,64,98,38,56,170,176,120,99,44,109,17,109,178,151,211,120,111,121,89,88,29,53,170,210,}) , (SafeString)(new byte[]{ 42,231,89,125,140,243,175,233,144,202,248,163,174,192,57,46,27,107,133,188,66,138,1,210,157,134,64,177,213,203,161,28,}) ){ Flags = (byte)0, __ALT__ = (uint)975748405, RequestSingletonData = RequestSingletonData }; c_14.CheckFailed = () => { Fail((ushort)14); }; RegisterSingleton((uint)2029573381, c_14); Scoring.ScoringItem si_14 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)14}; si_14.SuccessStatus = () => { return c_14.CompletedMessage; }; si_14.FailureStatus = () => { return c_14.FailedMessage; };Expect((ushort)14,si_14); InitState(14, new byte[0]);} catch { }
try { c_15 = new __fecd4d61606bd795c1892e7cf2c91abf((SafeString)(new byte[]{ 233,218,114,153,122,161,5,32,164,71,172,167,31,151,39,148,117,190,153,215,90,152,102,25,165,218,179,168,42,203,193,69,162,252,208,246,184,79,12,120,251,29,76,210,98,114,126,132,}) , (SafeString)(new byte[]{ 129,186,94,238,169,154,5,147,75,152,231,135,53,180,91,134,53,190,13,114,235,79,58,39,124,158,74,164,183,225,7,51,}) ){ Flags = (byte)0, __ALT__ = (uint)2020095359, RequestSingletonData = RequestSingletonData }; c_15.CheckFailed = () => { Fail((ushort)15); }; RegisterSingleton((uint)2021185349, c_15); Scoring.ScoringItem si_15 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)15}; si_15.SuccessStatus = () => { return c_15.CompletedMessage; }; si_15.FailureStatus = () => { return c_15.FailedMessage; };Expect((ushort)15,si_15); InitState(15, new byte[0]);} catch { }
try { c_16 = new __f92601c209f69906ae15b7f548a62aad((SafeString)(new byte[]{ 5,251,84,232,215,211,89,26,45,7,201,109,64,2,141,126,58,94,167,128,224,134,22,225,172,110,28,54,94,99,16,192,60,23,6,96,26,70,23,60,161,253,179,73,235,109,179,88,}) , (SafeString)(new byte[]{ 39,59,27,112,170,74,0,235,56,85,75,84,86,36,83,123,52,173,125,61,192,148,225,35,138,178,187,119,50,122,179,126,}) ){ Flags = (byte)0, __ALT__ = (uint)976780669, RequestSingletonData = RequestSingletonData }; c_16.CheckFailed = () => { Fail((ushort)16); }; RegisterSingleton((uint)979943239, c_16); Scoring.ScoringItem si_16 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0xD1,0xED,0x7D, }),  }, NumPoints = (short)4, ID = (ushort)16}; si_16.SuccessStatus = () => { return c_16.CompletedMessage; }; si_16.FailureStatus = () => { return c_16.FailedMessage; };Expect((ushort)16,si_16); InitState(16, new byte[0]);} catch { }
try { c_17 = new __cfaa82823ecec87726847938a4562606((SafeString)(new byte[]{ 171,232,117,251,181,130,8,134,50,9,45,26,8,254,168,182,76,65,244,130,111,180,57,35,194,243,107,20,177,141,124,49,}) ){ Flags = (byte)0, __ALT__ = (uint)1580416048, RequestSingletonData = RequestSingletonData }; c_17.CheckFailed = () => { Fail((ushort)17); }; RegisterSingleton((uint)1579383810, c_17); Scoring.ScoringItem si_17 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xE9,0x4F,0x8F,0x3F, }),  }, NumPoints = (short)2, ID = (ushort)17}; si_17.SuccessStatus = () => { return c_17.CompletedMessage; }; si_17.FailureStatus = () => { return c_17.FailedMessage; };Expect((ushort)17,si_17); InitState(17, new byte[0]);si_17.SuccessStatus = () => { return (SafeString)(new byte[]{ 180,43,109,147,77,235,121,79,77,192,141,204,152,241,75,183,104,192,254,94,57,174,114,143,133,10,49,41,227,202,10,251,203,26,172,112,135,74,44,138,193,159,227,235,33,142,24,180,103,0,45,252,125,7,115,145,49,87,45,76,186,3,20,54, }); };} catch { }
try { c_18 = new __88005bccb46180c6ed87491c24867fb8((SafeString)(new byte[]{ 185,183,225,218,150,252,203,115,69,58,29,142,52,174,112,151,80,42,95,96,113,223,47,187,214,169,103,186,254,16,81,59,38,190,103,67,231,130,52,166,234,12,174,74,125,222,210,101,}) , (SafeString)(new byte[]{ 158,171,236,179,102,197,110,85,63,40,30,223,131,8,81,67,69,12,69,31,182,89,137,125,202,87,166,54,186,158,122,141,}) , (SafeString)(new byte[]{ 180,142,194,128,133,139,85,9,105,161,142,66,105,196,128,224,179,120,30,125,187,171,95,153,49,238,133,147,206,167,130,126,233,142,34,223,79,45,60,231,121,96,169,131,150,178,126,49,129,181,164,155,24,241,246,214,97,55,203,248,64,172,20,129,}) ){ Flags = (byte)0, __ALT__ = (uint)1838964465, RequestSingletonData = RequestSingletonData }; c_18.CheckFailed = () => { Fail((ushort)18); }; RegisterSingleton((uint)1868363449, c_18); Scoring.ScoringItem si_18 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x7F,0xE5,0xC6,0x91,0x9D,0x2,0xB1,0xF9,0x61,0x52,0x9F,0xB7,0xAC,0xAD,0xDF,0xAD, }),  }, NumPoints = (short)3, ID = (ushort)18}; si_18.SuccessStatus = () => { return c_18.CompletedMessage; }; si_18.FailureStatus = () => { return c_18.FailedMessage; };Expect((ushort)18,si_18); InitState(18, new byte[0]);si_18.SuccessStatus = () => { return (SafeString)(new byte[]{ 169,37,54,57,135,114,19,81,243,7,231,140,36,113,110,51,12,239,229,210,76,37,118,180,15,205,228,223,219,12,110,62,85,160,7,27,218,235,100,108,97,222,53,92,186,222,205,25, }); };} catch { }
try { c_19 = new __3c5e7c861ca9d68c9523e27b1451d15c((SafeString)(new byte[]{ 42,177,216,188,118,145,249,49,167,40,123,218,232,79,193,161,8,206,9,43,213,170,186,193,82,96,225,134,142,25,248,211,24,12,229,184,149,95,102,102,70,80,7,6,68,121,59,92,47,46,83,243,141,73,162,27,235,152,42,177,46,198,216,233,63,221,168,80,206,120,252,204,23,88,145,34,202,156,211,68,150,175,190,149,67,244,54,92,220,12,91,18,113,244,3,180,51,0,249,9,74,255,244,77,73,72,148,211,242,190,92,215,}) , (SafeString)(new byte[]{ 191,250,211,190,221,147,88,46,48,37,229,188,138,115,218,165,22,163,130,5,232,241,81,225,120,6,21,137,52,3,71,72,}) ){ Flags = (byte)0, __ALT__ = (uint)967981180, RequestSingletonData = RequestSingletonData }; c_19.CheckFailed = () => { Fail((ushort)19); }; RegisterSingleton((uint)971166332, c_19); Scoring.ScoringItem si_19 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.NEQ, new byte[] { 0xE9,0x3C,0x34,0x58, }),  }, NumPoints = (short)2, ID = (ushort)19}; si_19.SuccessStatus = () => { return c_19.CompletedMessage; }; si_19.FailureStatus = () => { return c_19.FailedMessage; };Expect((ushort)19,si_19); InitState(19, new byte[0]);si_19.SuccessStatus = () => { return (SafeString)(new byte[]{ 20,48,234,173,194,182,148,94,52,36,104,110,57,54,3,95,181,190,49,104,36,69,45,86,41,187,83,156,38,50,131,161,154,184,243,97,12,119,28,101,31,171,4,182,29,119,116,138, }); };} catch { }
try { c_20 = new __019992010296c0b41cb626e5533ad3ee((SafeString)(new byte[]{ 54,211,124,25,121,157,131,161,244,92,238,106,225,251,213,221,46,112,101,194,75,177,107,108,151,255,132,72,191,231,138,209,22,202,80,157,184,196,163,204,159,36,144,167,96,138,12,161,200,230,201,14,75,118,71,36,132,76,97,156,80,10,226,61,183,21,174,193,141,81,237,221,193,31,175,65,181,14,91,170,120,217,64,103,158,240,215,239,27,107,115,21,142,128,49,78,}) , (SafeString)(new byte[]{ 75,245,184,0,104,16,85,94,63,155,64,125,118,88,199,38,54,192,43,185,220,88,205,34,18,254,51,183,35,255,196,135,}) ){ Flags = (byte)0, __ALT__ = (uint)1005754396, RequestSingletonData = RequestSingletonData }; c_20.CheckFailed = () => { Fail((ushort)20); }; RegisterSingleton((uint)2066930270, c_20); Scoring.ScoringItem si_20 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x15,0x93,0xEB,0xAA, }),  }, NumPoints = (short)2, ID = (ushort)20}; si_20.SuccessStatus = () => { return c_20.CompletedMessage; }; si_20.FailureStatus = () => { return c_20.FailedMessage; };Expect((ushort)20,si_20); InitState(20, new byte[0]);si_20.SuccessStatus = () => { return (SafeString)(new byte[]{ 194,214,116,192,134,101,180,54,101,99,194,5,226,21,218,83,215,225,186,4,138,20,239,149,109,71,172,55,5,16,223,52,202,123,180,190,238,99,223,31,169,146,24,167,103,201,206,92, }); };} catch { }
try { c_21 = new __1f5fb1f779ef65d8b042bf7a324660bd((SafeString)(new byte[]{ 74,68,28,133,143,42,225,76,111,150,0,118,38,7,237,174,119,107,60,206,100,77,193,70,251,109,63,155,208,11,184,82,38,149,13,119,255,98,42,12,42,14,41,211,237,189,132,47,190,230,35,47,15,11,236,170,63,103,3,107,15,48,210,225,16,201,181,168,61,200,52,125,173,3,153,90,96,79,141,155,}) , (SafeString)(new byte[]{ 111,157,6,10,65,51,172,33,124,145,183,144,51,147,105,110,50,110,212,127,197,137,231,238,110,243,221,89,68,180,193,143,}) ){ Flags = (byte)0, __ALT__ = (uint)967995484, RequestSingletonData = RequestSingletonData }; c_21.CheckFailed = () => { Fail((ushort)21); }; RegisterSingleton((uint)958558302, c_21); Scoring.ScoringItem si_21 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.NEQ, new byte[] { 0x4B,0x48,0xF4,0x3C, }),  }, NumPoints = (short)3, ID = (ushort)21}; si_21.SuccessStatus = () => { return c_21.CompletedMessage; }; si_21.FailureStatus = () => { return c_21.FailedMessage; };Expect((ushort)21,si_21); InitState(21, new byte[0]);si_21.SuccessStatus = () => { return (SafeString)(new byte[]{ 71,107,14,155,244,33,237,12,32,40,149,255,157,165,74,244,250,140,113,118,201,66,132,113,117,87,121,248,20,117,193,28,87,154,23,114,143,251,95,17,42,102,102,79,161,183,162,219, }); };} catch { }
try { c_22 = new __e13e87f1634c4cb04f221a7d437db86a((SafeString)(new byte[]{ 160,238,83,40,73,33,23,191,86,80,201,66,159,109,238,73,71,163,153,136,130,246,190,138,44,140,208,200,244,163,90,83,207,60,253,75,132,22,218,180,85,118,66,248,105,43,166,27,5,181,103,19,60,245,224,176,246,29,17,149,190,184,168,17,140,227,43,25,43,4,139,245,180,158,199,45,66,183,201,148,}) , (SafeString)(new byte[]{ 40,230,14,22,164,214,112,0,177,201,191,169,54,220,68,149,174,229,125,251,116,150,143,105,52,120,137,32,6,235,204,76,}) ){ Flags = (byte)0, __ALT__ = (uint)2075292182, RequestSingletonData = RequestSingletonData }; c_22.CheckFailed = () => { Fail((ushort)22); }; RegisterSingleton((uint)2040721478, c_22); Scoring.ScoringItem si_22 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x63,0xF2,0xBC,0x93, }),  }, NumPoints = (short)6, ID = (ushort)22}; si_22.SuccessStatus = () => { return c_22.CompletedMessage; }; si_22.FailureStatus = () => { return c_22.FailedMessage; };Expect((ushort)22,si_22); InitState(22, new byte[0]);si_22.SuccessStatus = () => { return (SafeString)(new byte[]{ 5,215,123,214,69,140,77,176,166,166,3,223,0,45,113,23,112,74,112,105,50,50,215,189,65,54,191,5,176,118,24,97,120,186,119,219,149,34,50,60,212,38,22,167,46,159,46,84, }); };} catch { }
try { c_23 = new __642c58f0f0858e621a8f144f3a4abd9e((SafeString)(new byte[]{ 246,61,81,129,226,77,90,54,188,158,99,155,25,232,209,34,246,47,5,47,226,40,206,26,145,118,235,188,64,69,250,67,182,34,42,177,35,9,41,120,231,25,67,106,19,191,172,107,223,54,117,70,230,172,84,69,117,187,39,15,25,247,144,116,158,68,6,24,243,166,10,221,144,227,123,134,99,152,201,103,201,233,194,211,252,52,154,202,204,253,93,164,191,92,162,194,130,49,120,83,16,185,215,113,67,77,125,120,20,100,108,18,224,177,86,32,240,136,125,169,88,217,176,181,109,29,161,198,61,3,227,88,64,245,128,96,184,155,128,178,111,195,55,232,}) , (SafeString)(new byte[]{ 181,61,50,71,144,104,221,249,56,203,117,87,40,165,147,67,187,27,76,213,24,182,32,103,215,76,254,214,255,150,168,136,}) ){ Flags = (byte)0, __ALT__ = (uint)958568550, RequestSingletonData = RequestSingletonData }; c_23.CheckFailed = () => { Fail((ushort)23); }; RegisterSingleton((uint)2074235004, c_23); Scoring.ScoringItem si_23 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x82,0x8D,0x17,0xE, }),  }, NumPoints = (short)6, ID = (ushort)23}; si_23.SuccessStatus = () => { return c_23.CompletedMessage; }; si_23.FailureStatus = () => { return c_23.FailedMessage; };Expect((ushort)23,si_23); InitState(23, new byte[0]);si_23.SuccessStatus = () => { return (SafeString)(new byte[]{ 102,186,9,83,10,47,111,20,89,12,40,255,106,230,35,191,219,91,197,207,122,227,169,164,220,59,205,11,196,251,156,137,77,193,153,123,43,144,157,133,254,142,167,54,70,201,198,185, }); };} catch { }
try { c_24 = new __7df5e608f75ce44fc58e8af8c9b935bc((SafeString)(new byte[]{ 208,167,182,253,3,176,80,71,185,94,233,131,211,177,86,230,35,154,29,213,84,162,54,216,221,38,22,209,228,123,187,230,252,227,214,197,214,215,94,60,159,63,197,121,183,101,169,14,115,77,157,69,80,206,246,154,67,180,21,176,68,244,116,42,181,173,32,221,250,37,167,5,8,53,43,148,244,113,183,195,214,232,89,186,239,157,105,119,234,104,9,171,249,85,147,20,115,121,89,168,106,62,42,136,62,204,168,146,200,238,115,232,}) , (SafeString)(new byte[]{ 20,240,183,225,129,158,228,210,44,249,161,209,22,97,21,174,211,138,71,54,233,150,11,126,139,73,14,221,249,234,223,124,}) ){ Flags = (byte)0, __ALT__ = (uint)959623190, RequestSingletonData = RequestSingletonData }; c_24.CheckFailed = () => { Fail((ushort)24); }; RegisterSingleton((uint)993186324, c_24); Scoring.ScoringItem si_24 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x12,0x52,0x34,0x19, }),  }, NumPoints = (short)2, ID = (ushort)24}; si_24.SuccessStatus = () => { return c_24.CompletedMessage; }; si_24.FailureStatus = () => { return c_24.FailedMessage; };Expect((ushort)24,si_24); InitState(24, new byte[0]);si_24.SuccessStatus = () => { return (SafeString)(new byte[]{ 120,232,130,215,46,160,71,27,74,209,130,199,160,10,16,116,138,205,91,44,62,94,199,36,228,145,112,182,77,77,172,160,47,19,129,29,241,195,124,151,141,112,224,20,147,221,104,1, }); };} catch { }
try { c_25 = new __903a3faf53cf32e1a29c8379e7e074c1((SafeString)(new byte[]{ 246,71,32,72,189,237,48,247,246,183,170,229,193,130,237,82,25,85,106,172,168,248,177,220,5,247,9,243,11,77,165,218,236,0,86,86,187,255,125,157,149,8,44,162,75,169,66,109,203,56,188,87,0,182,120,252,245,184,198,13,11,170,3,150,185,68,85,99,35,54,229,114,152,103,117,195,233,168,192,226,}) , (SafeString)(new byte[]{ 178,33,209,52,163,103,4,105,87,91,198,159,49,217,138,108,202,225,159,238,162,33,147,22,69,25,194,139,216,96,196,154,}) ){ Flags = (byte)0, __ALT__ = (uint)2070032910, RequestSingletonData = RequestSingletonData }; c_25.CheckFailed = () => { Fail((ushort)25); }; RegisterSingleton((uint)2032277598, c_25); Scoring.ScoringItem si_25 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.NEQ, new byte[] { 0x40,0xBE,0x25,0xBE, }),  }, NumPoints = (short)3, ID = (ushort)25}; si_25.SuccessStatus = () => { return c_25.CompletedMessage; }; si_25.FailureStatus = () => { return c_25.FailedMessage; };Expect((ushort)25,si_25); InitState(25, new byte[0]);si_25.SuccessStatus = () => { return (SafeString)(new byte[]{ 120,12,148,223,216,98,150,163,100,132,195,218,209,4,67,130,103,112,201,85,85,227,16,122,75,253,150,62,245,179,19,249,155,145,89,179,46,181,46,145,107,147,192,176,123,95,241,157, }); };} catch { }
try { c_26 = new __38c885f73584c9bf99f79d6be441c16b((SafeString)(new byte[]{ 231,93,5,177,241,23,101,118,111,214,210,143,143,178,30,52,179,7,9,11,183,78,108,101,89,66,214,131,20,14,229,50,125,92,88,57,27,116,251,237,233,37,220,59,157,178,111,0,237,125,191,27,128,2,40,114,230,206,150,53,114,82,90,182,86,176,41,171,137,93,148,24,160,30,80,88,251,176,118,247,}) , (SafeString)(new byte[]{ 18,110,34,47,9,97,62,62,199,182,209,26,207,37,219,23,202,196,99,250,78,198,17,23,176,162,13,86,136,129,218,95,}) ){ Flags = (byte)0, __ALT__ = (uint)967973452, RequestSingletonData = RequestSingletonData }; c_26.CheckFailed = () => { Fail((ushort)26); }; RegisterSingleton((uint)2078431846, c_26); Scoring.ScoringItem si_26 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.NEQ, new byte[] { 0x3,0x4F,0x92,0x5, }),  }, NumPoints = (short)3, ID = (ushort)26}; si_26.SuccessStatus = () => { return c_26.CompletedMessage; }; si_26.FailureStatus = () => { return c_26.FailedMessage; };Expect((ushort)26,si_26); InitState(26, new byte[0]);si_26.SuccessStatus = () => { return (SafeString)(new byte[]{ 46,145,187,36,41,34,3,228,105,240,22,164,59,246,237,134,14,161,249,96,110,237,170,102,233,158,37,188,152,210,147,131,123,91,162,56,141,39,123,53,234,116,192,97,247,210,12,181, }); };} catch { }
try { c_27 = new __fc4200a2f8e67c12459b90f25a2ff6ad((SafeString)(new byte[]{ 8,58,71,229,118,30,83,253,168,188,181,88,112,34,209,54,2,34,90,0,159,222,118,237,0,170,68,52,71,114,178,79,154,97,21,39,41,110,236,154,215,216,155,115,233,254,227,53,}) , (SafeString)(new byte[]{ 195,84,192,27,229,160,125,190,0,3,33,62,117,106,96,211,199,171,131,159,54,144,127,120,172,15,60,0,137,139,125,134,}) ){ Flags = (byte)0, __ALT__ = (uint)979933015, RequestSingletonData = RequestSingletonData }; c_27.CheckFailed = () => { Fail((ushort)27); }; RegisterSingleton((uint)2063129863, c_27); Scoring.ScoringItem si_27 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x83,0x55,0x66,0xDF, }),  }, NumPoints = (short)4, ID = (ushort)27}; si_27.SuccessStatus = () => { return c_27.CompletedMessage; }; si_27.FailureStatus = () => { return c_27.FailedMessage; };Expect((ushort)27,si_27); InitState(27, new byte[0]);si_27.SuccessStatus = () => { return (SafeString)(new byte[]{ 43,76,174,127,141,40,178,45,20,2,108,223,148,225,17,162,215,209,112,66,196,136,146,48,200,211,124,18,59,140,44,192,121,140,37,14,162,51,170,140,229,158,233,101,84,190,207,145, }); };} catch { }
try { c_28 = new __2443080c260b402453289ed22812dba9((SafeString)(new byte[]{ 233,49,9,89,163,91,231,225,35,103,181,236,230,25,34,133,233,180,153,241,114,113,83,54,220,71,205,194,214,188,170,62,234,146,156,254,12,135,31,225,221,3,11,37,82,42,15,185,}) , (SafeString)(new byte[]{ 112,153,142,141,78,75,227,191,231,35,137,78,208,162,220,47,31,46,24,164,24,228,13,228,143,246,69,116,32,142,80,14,}) , (SafeString)(new byte[]{ 118,129,159,153,91,216,28,128,254,57,140,33,120,232,180,166,214,45,0,231,243,83,168,129,109,146,247,15,125,18,67,33,39,143,37,87,178,249,251,17,85,175,154,216,153,222,255,195,16,243,240,148,57,77,231,211,223,92,214,15,115,119,62,104,}) ){ Flags = (byte)0, __ALT__ = (uint)1868356801, RequestSingletonData = RequestSingletonData }; c_28.CheckFailed = () => { Fail((ushort)28); }; RegisterSingleton((uint)798815915, c_28); Scoring.ScoringItem si_28 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x1E,0xAD,0xF5,0x81,0x5D,0x2A,0xEC,0x3C,0xD9,0xBA,0xA6,0xC4,0x70,0x68,0xBC,0x37, }),  }, NumPoints = (short)5, ID = (ushort)28}; si_28.SuccessStatus = () => { return c_28.CompletedMessage; }; si_28.FailureStatus = () => { return c_28.FailedMessage; };Expect((ushort)28,si_28); InitState(28, new byte[0]);si_28.SuccessStatus = () => { return (SafeString)(new byte[]{ 179,1,235,112,135,78,245,235,245,227,202,65,235,56,206,67,189,32,0,95,225,168,84,87,228,208,249,168,183,203,177,21,158,31,61,68,183,136,223,14,174,94,223,179,200,165,231,199, }); };} catch { }
try { c_29 = new __ff24e59d219e3bccea90a5acb6368c52((SafeString)(new byte[]{ 3,38,103,153,0,16,100,201,185,58,208,59,99,242,231,126,178,21,222,38,162,130,157,237,172,89,103,169,62,102,199,84,205,203,157,225,156,231,56,50,123,199,107,142,149,73,87,209,70,16,51,66,90,75,106,133,92,160,233,175,7,55,186,48,}) , (SafeString)(new byte[]{ 103,148,47,146,179,175,151,119,155,209,224,86,222,191,74,18,248,143,28,4,205,152,34,252,3,139,20,17,244,31,121,211,}) ){ Flags = (byte)0, __ALT__ = (uint)1737539041, RequestSingletonData = RequestSingletonData }; c_29.CheckFailed = () => { Fail((ushort)29); }; RegisterSingleton((uint)1733350889, c_29); Scoring.ScoringItem si_29 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x94,0x4E,0x5B,0x8A, }),  }, NumPoints = (short)4, ID = (ushort)29}; si_29.SuccessStatus = () => { return c_29.CompletedMessage; }; si_29.FailureStatus = () => { return c_29.FailedMessage; };Expect((ushort)29,si_29); InitState(29, new byte[0]);si_29.SuccessStatus = () => { return (SafeString)(new byte[]{ 71,178,166,32,44,170,179,147,175,42,78,108,245,245,194,180,142,104,92,60,252,220,141,165,155,33,117,122,121,176,105,36,146,32,248,106,16,201,79,196,148,132,100,81,52,16,245,194, }); };} catch { }
try { c_30 = new __9b0026460cc58de2a01c727c857e8211((SafeString)(new byte[]{ 204,21,233,24,253,202,242,65,99,33,7,20,121,194,98,231,171,5,239,178,108,89,3,245,129,81,163,58,134,251,71,132,188,201,211,125,113,165,193,85,52,20,229,244,160,224,38,252,120,214,219,142,35,20,63,238,243,23,154,126,89,217,101,40,}) , (SafeString)(new byte[]{ 161,9,8,42,228,90,19,33,96,254,71,123,241,70,198,81,31,119,141,129,185,93,149,151,46,207,110,140,227,63,184,8,}) ){ Flags = (byte)0, __ALT__ = (uint)1740682729, RequestSingletonData = RequestSingletonData }; c_30.CheckFailed = () => { Fail((ushort)30); }; RegisterSingleton((uint)1737547163, c_30); Scoring.ScoringItem si_30 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xFE,0x1E,0xFB,0x4F, }),  }, NumPoints = (short)4, ID = (ushort)30}; si_30.SuccessStatus = () => { return c_30.CompletedMessage; }; si_30.FailureStatus = () => { return c_30.FailedMessage; };Expect((ushort)30,si_30); InitState(30, new byte[0]);si_30.SuccessStatus = () => { return (SafeString)(new byte[]{ 50,112,242,190,200,92,196,96,195,82,214,229,158,41,161,121,181,138,130,6,22,62,24,201,189,198,255,39,155,147,53,114,67,60,189,216,244,88,176,136,159,132,22,167,11,142,11,124, }); };} catch { }
try { c_31 = new __bf19db9ed12812bc1ca2149322f73379((SafeString)(new byte[]{ 193,246,222,33,15,1,80,157,3,92,148,149,115,133,137,64,155,102,61,237,46,155,73,150,190,93,180,31,42,108,53,147,129,42,148,143,243,4,99,122,221,58,160,159,48,165,162,174,26,187,46,154,50,245,52,114,26,43,15,151,221,89,254,252,}) , (SafeString)(new byte[]{ 211,153,34,73,4,107,232,12,246,229,2,211,209,232,126,9,168,173,94,71,135,243,135,97,213,180,138,41,233,139,74,1,}) ){ Flags = (byte)0, __ALT__ = (uint)1702911475, RequestSingletonData = RequestSingletonData }; c_31.CheckFailed = () => { Fail((ushort)31); }; RegisterSingleton((uint)1732255665, c_31); Scoring.ScoringItem si_31 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x16,0x69,0xF,0x4E, }),  }, NumPoints = (short)8, ID = (ushort)31}; si_31.SuccessStatus = () => { return c_31.CompletedMessage; }; si_31.FailureStatus = () => { return c_31.FailedMessage; };Expect((ushort)31,si_31); InitState(31, new byte[0]);si_31.SuccessStatus = () => { return (SafeString)(new byte[]{ 177,103,25,174,174,135,75,252,44,148,156,68,219,176,192,57,48,167,36,37,68,124,10,219,97,61,96,226,104,205,17,10,217,106,227,181,50,9,112,84,177,101,140,2,192,87,250,81, }); };} catch { }
try { c_32 = new __a25f51b73ae79ab23d7c369df7a32401((SafeString)(new byte[]{ 162,133,253,24,107,156,153,173,130,184,87,222,222,178,145,77,208,238,154,131,194,183,116,145,237,233,251,206,120,66,172,63,128,216,131,72,175,162,145,242,109,213,100,157,81,239,166,21,80,232,137,171,78,7,10,1,72,254,155,149,147,10,14,113,}) , (SafeString)(new byte[]{ 115,10,127,219,225,247,184,16,185,163,212,203,71,160,100,37,101,253,84,83,3,225,24,10,185,110,11,172,151,66,14,104,}) ){ Flags = (byte)0, __ALT__ = (uint)659595193, RequestSingletonData = RequestSingletonData }; c_32.CheckFailed = () => { Fail((ushort)32); }; RegisterSingleton((uint)630242777, c_32); Scoring.ScoringItem si_32 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0xB2,0x41,0xA9,0x1, }),  }, NumPoints = (short)12, ID = (ushort)32}; si_32.SuccessStatus = () => { return c_32.CompletedMessage; }; si_32.FailureStatus = () => { return c_32.FailedMessage; };Expect((ushort)32,si_32); InitState(32, new byte[0]);si_32.SuccessStatus = () => { return (SafeString)(new byte[]{ 213,207,178,125,160,121,208,135,93,239,235,11,0,7,74,101,198,172,189,192,180,232,136,162,248,196,8,173,179,5,242,36,239,181,2,62,101,88,89,190,158,197,179,120,69,49,198,131, }); };} catch { }
try { c_33 = new __8502165c9d443e894a55d2df67368597((SafeString)(new byte[]{ 242,86,120,63,33,14,183,190,246,31,16,141,51,236,153,121,108,219,182,91,137,217,138,235,43,225,97,195,72,31,136,81,227,190,137,164,109,1,247,103,214,97,20,230,185,182,123,81,10,79,194,250,227,105,245,218,123,50,103,92,95,215,188,93,}) , (SafeString)(new byte[]{ 68,27,192,236,37,221,101,134,227,226,235,2,253,202,113,142,66,237,126,29,65,173,103,25,118,227,14,99,156,217,95,243,}) ){ Flags = (byte)0, __ALT__ = (uint)1694553505, RequestSingletonData = RequestSingletonData }; c_33.CheckFailed = () => { Fail((ushort)33); }; RegisterSingleton((uint)666934697, c_33); Scoring.ScoringItem si_33 = new Scoring.ScoringItem() {Expected = new List<(EngineFrame.ACompareType CompareOp, byte[] data)>(){ (EngineFrame.ACompareType.EQ, new byte[] { 0x76,0x52,0xEC,0x3E, }),  }, NumPoints = (short)4, ID = (ushort)33}; si_33.SuccessStatus = () => { return c_33.CompletedMessage; }; si_33.FailureStatus = () => { return c_33.FailedMessage; };Expect((ushort)33,si_33); InitState(33, new byte[0]);si_33.SuccessStatus = () => { return (SafeString)(new byte[]{ 204,242,171,125,86,238,93,136,17,155,89,153,135,238,99,225,19,1,69,27,144,56,207,128,153,236,136,252,155,24,255,163,98,86,159,152,131,65,188,81,81,147,135,121,49,150,92,192, }); };} catch { }
SetImageName(@"Space Force Server");
AND(1, 2);
AND(3, 4);
AND(5, 6);
AND(9, 10);
AND(9, 11);
AND(9, 12);
AND(13, 14);
AND(13, 15);
AND(13, 16);

        }

        protected override async Task Tick()
        {
            if(c_0?.Enabled ?? false){ try{c_0_s = await c_0.CheckState(); RegisterCheck((ushort)0|((uint)c_0.Flags << 16),c_0_s);} catch{ } }
if(c_1?.Enabled ?? false){ try{c_1_s = await c_1.CheckState(); RegisterCheck((ushort)1|((uint)c_1.Flags << 16),c_1_s);} catch{ } }
if(c_2?.Enabled ?? false){ try{c_2_s = await c_2.CheckState(); RegisterCheck((ushort)2|((uint)c_2.Flags << 16),c_2_s);} catch{ } }
if(c_3?.Enabled ?? false){ try{c_3_s = await c_3.CheckState(); RegisterCheck((ushort)3|((uint)c_3.Flags << 16),c_3_s);} catch{ } }
if(c_4?.Enabled ?? false){ try{c_4_s = await c_4.CheckState(); RegisterCheck((ushort)4|((uint)c_4.Flags << 16),c_4_s);} catch{ } }
if(c_5?.Enabled ?? false){ try{c_5_s = await c_5.CheckState(); RegisterCheck((ushort)5|((uint)c_5.Flags << 16),c_5_s);} catch{ } }
if(c_6?.Enabled ?? false){ try{c_6_s = await c_6.CheckState(); RegisterCheck((ushort)6|((uint)c_6.Flags << 16),c_6_s);} catch{ } }
if(c_7?.Enabled ?? false){ try{c_7_s = await c_7.CheckState(); RegisterCheck((ushort)7|((uint)c_7.Flags << 16),c_7_s);} catch{ } }
if(c_8?.Enabled ?? false){ try{c_8_s = await c_8.CheckState(); RegisterCheck((ushort)8|((uint)c_8.Flags << 16),c_8_s);} catch{ } }
if(c_9?.Enabled ?? false){ try{c_9_s = await c_9.CheckState(); RegisterCheck((ushort)9|((uint)c_9.Flags << 16),c_9_s);} catch{ } }
if(c_10?.Enabled ?? false){ try{c_10_s = await c_10.CheckState(); RegisterCheck((ushort)10|((uint)c_10.Flags << 16),c_10_s);} catch{ } }
if(c_11?.Enabled ?? false){ try{c_11_s = await c_11.CheckState(); RegisterCheck((ushort)11|((uint)c_11.Flags << 16),c_11_s);} catch{ } }
if(c_12?.Enabled ?? false){ try{c_12_s = await c_12.CheckState(); RegisterCheck((ushort)12|((uint)c_12.Flags << 16),c_12_s);} catch{ } }
if(c_13?.Enabled ?? false){ try{c_13_s = await c_13.CheckState(); RegisterCheck((ushort)13|((uint)c_13.Flags << 16),c_13_s);} catch{ } }
if(c_14?.Enabled ?? false){ try{c_14_s = await c_14.CheckState(); RegisterCheck((ushort)14|((uint)c_14.Flags << 16),c_14_s);} catch{ } }
if(c_15?.Enabled ?? false){ try{c_15_s = await c_15.CheckState(); RegisterCheck((ushort)15|((uint)c_15.Flags << 16),c_15_s);} catch{ } }
if(c_16?.Enabled ?? false){ try{c_16_s = await c_16.CheckState(); RegisterCheck((ushort)16|((uint)c_16.Flags << 16),c_16_s);} catch{ } }
if(c_17?.Enabled ?? false){ try{c_17_s = await c_17.CheckState(); RegisterCheck((ushort)17|((uint)c_17.Flags << 16),c_17_s);} catch{ } }
if(c_18?.Enabled ?? false){ try{c_18_s = await c_18.CheckState(); RegisterCheck((ushort)18|((uint)c_18.Flags << 16),c_18_s);} catch{ } }
if(c_19?.Enabled ?? false){ try{c_19_s = await c_19.CheckState(); RegisterCheck((ushort)19|((uint)c_19.Flags << 16),c_19_s);} catch{ } }
if(c_20?.Enabled ?? false){ try{c_20_s = await c_20.CheckState(); RegisterCheck((ushort)20|((uint)c_20.Flags << 16),c_20_s);} catch{ } }
if(c_21?.Enabled ?? false){ try{c_21_s = await c_21.CheckState(); RegisterCheck((ushort)21|((uint)c_21.Flags << 16),c_21_s);} catch{ } }
if(c_22?.Enabled ?? false){ try{c_22_s = await c_22.CheckState(); RegisterCheck((ushort)22|((uint)c_22.Flags << 16),c_22_s);} catch{ } }
if(c_23?.Enabled ?? false){ try{c_23_s = await c_23.CheckState(); RegisterCheck((ushort)23|((uint)c_23.Flags << 16),c_23_s);} catch{ } }
if(c_24?.Enabled ?? false){ try{c_24_s = await c_24.CheckState(); RegisterCheck((ushort)24|((uint)c_24.Flags << 16),c_24_s);} catch{ } }
if(c_25?.Enabled ?? false){ try{c_25_s = await c_25.CheckState(); RegisterCheck((ushort)25|((uint)c_25.Flags << 16),c_25_s);} catch{ } }
if(c_26?.Enabled ?? false){ try{c_26_s = await c_26.CheckState(); RegisterCheck((ushort)26|((uint)c_26.Flags << 16),c_26_s);} catch{ } }
if(c_27?.Enabled ?? false){ try{c_27_s = await c_27.CheckState(); RegisterCheck((ushort)27|((uint)c_27.Flags << 16),c_27_s);} catch{ } }
if(c_28?.Enabled ?? false){ try{c_28_s = await c_28.CheckState(); RegisterCheck((ushort)28|((uint)c_28.Flags << 16),c_28_s);} catch{ } }
if(c_29?.Enabled ?? false){ try{c_29_s = await c_29.CheckState(); RegisterCheck((ushort)29|((uint)c_29.Flags << 16),c_29_s);} catch{ } }
if(c_30?.Enabled ?? false){ try{c_30_s = await c_30.CheckState(); RegisterCheck((ushort)30|((uint)c_30.Flags << 16),c_30_s);} catch{ } }
if(c_31?.Enabled ?? false){ try{c_31_s = await c_31.CheckState(); RegisterCheck((ushort)31|((uint)c_31.Flags << 16),c_31_s);} catch{ } }
if(c_32?.Enabled ?? false){ try{c_32_s = await c_32.CheckState(); RegisterCheck((ushort)32|((uint)c_32.Flags << 16),c_32_s);} catch{ } }
if(c_33?.Enabled ?? false){ try{c_33_s = await c_33.CheckState(); RegisterCheck((ushort)33|((uint)c_33.Flags << 16),c_33_s);} catch{ } }

        }

        //
    }

    //cst
internal sealed class __704fbb660a07a0504b81bc6d91b40b4c : CheckTemplate
{
private readonly SafeString SVCName;
private readonly ServiceCheckType CheckType;
internal override SafeString CompletedMessage
{
get
{
return $"Service '{SVCName.ToString()}' check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Service '{SVCName.ToString()}' check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
switch(CheckType)
{
case ServiceCheckType.Status:
string qs = @"systemctl status " + SVCName + @" | grep -o ""[Aa]ctive:[^\)]*)""";
value = PrepareState32((await qs.Bash()).Trim());
break;
default:
break;
}
return value;
}
/// <summary>
/// A service check template
/// </summary>
/// <param name="args">[0]:ServiceName, [1]:ServiceCheckType, [2+]:args..</param>
internal __704fbb660a07a0504b81bc6d91b40b4c(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
SVCName = args[0];
try
{
Enum.TryParse(args[1], true, out CheckType);
}
catch
{
Enabled = false;
}
}
}

//cst
internal sealed class __bb853c5712cc0b2a5e987f95b0ac3d59 : CheckTemplate
{
private readonly SafeString SVCName;
private readonly ServiceCheckType CheckType;
internal override SafeString CompletedMessage
{
get
{
return $"Service '{SVCName.ToString()}' check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Service '{SVCName.ToString()}' check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
switch(CheckType)
{
case ServiceCheckType.Status:
string qs = @"systemctl status " + SVCName + @" | grep -o ""[Aa]ctive:[^\)]*)""";
value = PrepareState32((await qs.Bash()).Trim());
break;
default:
break;
}
return value;
}
/// <summary>
/// A service check template
/// </summary>
/// <param name="args">[0]:ServiceName, [1]:ServiceCheckType, [2+]:args..</param>
internal __bb853c5712cc0b2a5e987f95b0ac3d59(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
SVCName = args[0];
try
{
Enum.TryParse(args[1], true, out CheckType);
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __1f5a087ae65f6de7093b8154f54fd619 : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __1f5a087ae65f6de7093b8154f54fd619(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __0efff66ab46289d47dabe75601e5f20e : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __0efff66ab46289d47dabe75601e5f20e(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __1fc68463d5c68d0d036d959b4f7bb8e6 : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __1fc68463d5c68d0d036d959b4f7bb8e6(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __b991735d024e236ef6a8da81489d1c29 : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __b991735d024e236ef6a8da81489d1c29(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __1f73da3a75f20a37c1cdb9110e7562ff : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __1f73da3a75f20a37c1cdb9110e7562ff(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __354760caa4c2e743f26f1ece173beb90 : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __354760caa4c2e743f26f1ece173beb90(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
internal sealed class __2712858277ea4c3bf7f99de729f293ae : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __2712858277ea4c3bf7f99de729f293ae(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __1a4a9652eca01f14805def9f6dbfab62 : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __1a4a9652eca01f14805def9f6dbfab62(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __c01e82919b7404725c6cee52e60af82d : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __c01e82919b7404725c6cee52e60af82d(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __5481f419d5c2e139329e0a6e67f13ea3 : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __5481f419d5c2e139329e0a6e67f13ea3(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __de8cb45aba89a98ff61294059480438c : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __de8cb45aba89a98ff61294059480438c(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __4d3ff50a9fb8252ba1fc728ad7fada78 : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __4d3ff50a9fb8252ba1fc728ad7fada78(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __7ef99b6825760da113bbb5383cc2006d : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __7ef99b6825760da113bbb5383cc2006d(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __fecd4d61606bd795c1892e7cf2c91abf : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __fecd4d61606bd795c1892e7cf2c91abf(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __f92601c209f69906ae15b7f548a62aad : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __f92601c209f69906ae15b7f548a62aad(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
internal sealed class __cfaa82823ecec87726847938a4562606 : CheckTemplate
{
private readonly SafeString GroupName;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await $"awk -F':' '/{GroupName.ToString()}/{{print $4}}' /etc/group".Bash();
result = result.Trim().ToLower();
string[] split = result.Split(',');
int MaxSize = 0;
foreach (var s in split)
MaxSize = Math.Max(s.Length, MaxSize);
char[] ResultantString = new char[MaxSize];
for (int i = 0;
i < MaxSize;
i++)
ResultantString[i] = '?';
foreach(string s in split)
{
for(int i = 0;
i < s.Length;
i++)
{
ResultantString[i] = (char)(ResultantString[i] ^ s[i]);
}
}
string data = new string(ResultantString);
return PrepareState32(data);
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A group members template
/// </summary>
/// <param name="args">[0]:groupname</param>
internal __cfaa82823ecec87726847938a4562606(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
GroupName = args[0];
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __88005bccb46180c6ed87491c24867fb8 : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __88005bccb46180c6ed87491c24867fb8(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
internal sealed class __3c5e7c861ca9d68c9523e27b1451d15c : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __3c5e7c861ca9d68c9523e27b1451d15c(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __019992010296c0b41cb626e5533ad3ee : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __019992010296c0b41cb626e5533ad3ee(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __1f5fb1f779ef65d8b042bf7a324660bd : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __1f5fb1f779ef65d8b042bf7a324660bd(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __e13e87f1634c4cb04f221a7d437db86a : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __e13e87f1634c4cb04f221a7d437db86a(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __642c58f0f0858e621a8f144f3a4abd9e : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __642c58f0f0858e621a8f144f3a4abd9e(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __7df5e608f75ce44fc58e8af8c9b935bc : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __7df5e608f75ce44fc58e8af8c9b935bc(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __903a3faf53cf32e1a29c8379e7e074c1 : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __903a3faf53cf32e1a29c8379e7e074c1(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
internal sealed class __38c885f73584c9bf99f79d6be441c16b : CheckTemplate
{
private readonly SafeString Command;
private int? CMDTimeout = null;
internal override SafeString CompletedMessage
{
get
{
return $"Shell command check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
return $"Shell command check failed.";
}
}
internal enum ServiceCheckType
{
/// <summary>
/// Determine the status of the service
/// </summary>
Status
}
internal override async Task<byte[]> GetCheckValue()
{
try
{
string result = await Command.ToString().Bash();
return PrepareState32(result.Trim());
}
catch
{
return new byte[0];
}
}
/// <summary>
/// A shell output template
/// </summary>
/// <param name="args">[0]:command, [1]:? uint DeltaTime, [2]:? command timeout</param>
internal __38c885f73584c9bf99f79d6be441c16b(params string[] args)
{
TickDelay = 10000;
if(args.Length < 1)
{
Enabled = false;
return;
}
Command = args[0];
try
{
TickDelay = Convert.ToUInt32(args[1]);
CMDTimeout = Convert.ToInt32(args[2]);
}
catch
{
}
}
}

//cst
/* via paul
[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
public static extern int GetFileAttributes(string fileName);
public static bool FileExists(string location)
{
if (GetFileAttributes(location) != -1)
return true;
return false;
}
*/
class __fc4200a2f8e67c12459b90f25a2ff6ad : CheckTemplate
{
private readonly SafeString Location;
private enum CheckType
{
File,
Directory,
}
private readonly CheckType Check;
internal override SafeString CompletedMessage
{
get
{
if(Check == CheckType.File)
try { return Path.GetFileName(Location) + " check passed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check passed.";
} catch { }
if (Check == CheckType.File)
return "File check passed.";
else
return "Folder check passed.";
}
}
internal override SafeString FailedMessage
{
get
{
if (Check == CheckType.File)
try { return Path.GetFileName(Location) + " check failed.";
} catch { }
else
try { return Path.GetFileName(Path.GetDirectoryName(Location)) + " check failed.";
} catch { }
if (Check == CheckType.File)
return "File check failed.";
else
return "Folder check failed.";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
switch(Check)
{
case CheckType.File:
value = await Task.FromResult(PrepareState32(File.Exists(Location)));
return value;
case CheckType.Directory:
value = await Task.FromResult(PrepareState32(Directory.Exists(Location)));
return value;
}
return value;
}
catch
{
Enabled = false;
//security exception... how can a service run into service perm issues???
return value;
}
}
/// <summary>
///
/// </summary>
/// <param name="args">args[0] Location, args[1] status of file</param>
internal __fc4200a2f8e67c12459b90f25a2ff6ad(params string[] args)
{
if (args.Length < 2)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
Enum.TryParse(args[1], true, out CheckType checkType);
Check = checkType;
}
catch
{
Enabled = false;
}
}
}

//cst
/*
NOTE: This class will only search a maximum of 8 files into the directory by default. This is to prevent nuking.
*/
class __2443080c260b402453289ed22812dba9 : CheckTemplate
{
private readonly SafeString Location;
private readonly int MaxFileEntries = 12;
private readonly ulong FileSize;
private readonly byte[] Hash;
private string FoundFileName;
private enum CheckType
{
File,
Directory
}
internal override SafeString CompletedMessage
{
get
{
return "Quarantined " + Path.GetFileName(FoundFileName) + ".";
}
}
internal override SafeString FailedMessage
{
get
{
return "Failed to quarantine a file";
}
}
internal async override Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
try
{
DirectoryInfo d = new DirectoryInfo(Location);
if (!d.Exists)
return value;
FileInfo[] fi = d.GetFiles();
for (int i = 0;
i < fi.Length && i < MaxFileEntries;
i++)
{
if (fi[i].Length == (long)FileSize && fi[i].Exists)
{
byte[] data = await File.ReadAllBytesAsync(fi[i].FullName);
byte[] hash_b = MD5(data);
//We compare locally so that we can identify a potential candidate to send off to the server.
//This forces both server authority and client integrity at the expense of a hash being stored locally.
if (bcomp(hash_b, Hash))
{
FoundFileName = fi[i].FullName;
return hash_b;
}
}
}
return value;
}
catch
{
return value;
}
}
private bool bcomp(byte[] a, byte[] b)
{
if (a.Length != b.Length)
return false;
for (int i = 0;
i < a.Length;
i++)
if (a[i] != b[i])
return false;
return true;
}
//TODO: one of these checks is not firing (may be disabling its-self)
/// <summary>
///
/// </summary>
/// <param name="args">[0]:string FolderPath, [1]:uint64 SizeOfFile, [2]: byte[16] MD5 Hash, [3]:? MaxFilesOverride</param>
internal __2443080c260b402453289ed22812dba9(params string[] args)
{
if (args.Length < 3)
{
Enabled = false;
return;
}
Location = Environment.ExpandEnvironmentVariables(args[0]);
try
{
FileSize = Convert.ToUInt64(args[1]);
Hash = StringToHex(args[2]);
if (Hash.Length != 16)
throw new ArgumentException("Quarantine parameter was incorrect");
}
catch
{
Enabled = false;
}
TickDelay = 30000;
try
{
if (args.Length > 3)
{
MaxFileEntries = Convert.ToInt32(args[3]);
}
}
catch
{
}
}
private static byte[] StringToHex(string s)
{
if (s.Length % 2 == 1)
s = "0" + s;
byte[] bytes = new byte[s.Length / 2];
for (int i = 0;
i < s.Length;
i += 2)
bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
return bytes;
}
}

//cst
internal sealed class __ff24e59d219e3bccea90a5acb6368c52 : CheckTemplate
{
internal string ForensicsPath = "";
internal readonly bool CaseSensitive = false;
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
if(!File.Exists(ForensicsPath))
return value;
try
{
string[] lines = File.ReadAllLines(ForensicsPath);
Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
value = PrepareState32(ForensicsMask(lines));
}
catch
{
Console.WriteLine("EXCEPT!");
}
return value;
}
private string ForensicsMask(string[] lines)
{
List<char> byteronis = new List<char>();
foreach (string line in lines)
{
string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
{
string rline = line.Trim();
if (!CaseSensitive)
rline = rline.ToLower();
rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
for (int i = 0;
i < rline.Length;
i++)
{
if (byteronis.Count <= i)
byteronis.Add(rline[i]);
else
byteronis[i] = (char)(byteronis[i] ^ rline[i]);
}
}
}
return new string(byteronis.ToArray());
}
/// <summary>
/// A Forensics check. Defaults to case insensitive
/// </summary>
/// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
internal __ff24e59d219e3bccea90a5acb6368c52(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
ForensicsPath = args[0];
TickDelay = 20000;
try
{
CaseSensitive = Convert.ToBoolean(args[1]);
}
catch
{
CaseSensitive = false;
}
}
}

//cst
internal sealed class __9b0026460cc58de2a01c727c857e8211 : CheckTemplate
{
internal string ForensicsPath = "";
internal readonly bool CaseSensitive = false;
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
if(!File.Exists(ForensicsPath))
return value;
try
{
string[] lines = File.ReadAllLines(ForensicsPath);
Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
value = PrepareState32(ForensicsMask(lines));
}
catch
{
Console.WriteLine("EXCEPT!");
}
return value;
}
private string ForensicsMask(string[] lines)
{
List<char> byteronis = new List<char>();
foreach (string line in lines)
{
string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
{
string rline = line.Trim();
if (!CaseSensitive)
rline = rline.ToLower();
rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
for (int i = 0;
i < rline.Length;
i++)
{
if (byteronis.Count <= i)
byteronis.Add(rline[i]);
else
byteronis[i] = (char)(byteronis[i] ^ rline[i]);
}
}
}
return new string(byteronis.ToArray());
}
/// <summary>
/// A Forensics check. Defaults to case insensitive
/// </summary>
/// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
internal __9b0026460cc58de2a01c727c857e8211(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
ForensicsPath = args[0];
TickDelay = 20000;
try
{
CaseSensitive = Convert.ToBoolean(args[1]);
}
catch
{
CaseSensitive = false;
}
}
}

//cst
internal sealed class __bf19db9ed12812bc1ca2149322f73379 : CheckTemplate
{
internal string ForensicsPath = "";
internal readonly bool CaseSensitive = false;
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
if(!File.Exists(ForensicsPath))
return value;
try
{
string[] lines = File.ReadAllLines(ForensicsPath);
Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
value = PrepareState32(ForensicsMask(lines));
}
catch
{
Console.WriteLine("EXCEPT!");
}
return value;
}
private string ForensicsMask(string[] lines)
{
List<char> byteronis = new List<char>();
foreach (string line in lines)
{
string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
{
string rline = line.Trim();
if (!CaseSensitive)
rline = rline.ToLower();
rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
for (int i = 0;
i < rline.Length;
i++)
{
if (byteronis.Count <= i)
byteronis.Add(rline[i]);
else
byteronis[i] = (char)(byteronis[i] ^ rline[i]);
}
}
}
return new string(byteronis.ToArray());
}
/// <summary>
/// A Forensics check. Defaults to case insensitive
/// </summary>
/// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
internal __bf19db9ed12812bc1ca2149322f73379(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
ForensicsPath = args[0];
TickDelay = 20000;
try
{
CaseSensitive = Convert.ToBoolean(args[1]);
}
catch
{
CaseSensitive = false;
}
}
}

//cst
internal sealed class __a25f51b73ae79ab23d7c369df7a32401 : CheckTemplate
{
internal string ForensicsPath = "";
internal readonly bool CaseSensitive = false;
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
if(!File.Exists(ForensicsPath))
return value;
try
{
string[] lines = File.ReadAllLines(ForensicsPath);
Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
value = PrepareState32(ForensicsMask(lines));
}
catch
{
Console.WriteLine("EXCEPT!");
}
return value;
}
private string ForensicsMask(string[] lines)
{
List<char> byteronis = new List<char>();
foreach (string line in lines)
{
string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
{
string rline = line.Trim();
if (!CaseSensitive)
rline = rline.ToLower();
rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
for (int i = 0;
i < rline.Length;
i++)
{
if (byteronis.Count <= i)
byteronis.Add(rline[i]);
else
byteronis[i] = (char)(byteronis[i] ^ rline[i]);
}
}
}
return new string(byteronis.ToArray());
}
/// <summary>
/// A Forensics check. Defaults to case insensitive
/// </summary>
/// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
internal __a25f51b73ae79ab23d7c369df7a32401(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
ForensicsPath = args[0];
TickDelay = 20000;
try
{
CaseSensitive = Convert.ToBoolean(args[1]);
}
catch
{
CaseSensitive = false;
}
}
}

//cst
internal sealed class __8502165c9d443e894a55d2df67368597 : CheckTemplate
{
internal string ForensicsPath = "";
internal readonly bool CaseSensitive = false;
internal override async Task<byte[]> GetCheckValue()
{
byte[] value = new byte[0];
if(!File.Exists(ForensicsPath))
return value;
try
{
string[] lines = File.ReadAllLines(ForensicsPath);
Console.WriteLine(String.Join(",",PrepareState32(ForensicsMask(lines))));
value = PrepareState32(ForensicsMask(lines));
}
catch
{
Console.WriteLine("EXCEPT!");
}
return value;
}
private string ForensicsMask(string[] lines)
{
List<char> byteronis = new List<char>();
foreach (string line in lines)
{
string tline = Regex.Replace(line.Trim().ToLower(), "\\s+", "");
if (tline.StartsWith("answer:", StringComparison.InvariantCultureIgnoreCase))
{
string rline = line.Trim();
if (!CaseSensitive)
rline = rline.ToLower();
rline = Regex.Replace(rline, "^[Aa][Nn][Ss][Ww][Ee][Rr][\\s]*:[\\s]*", "");
for (int i = 0;
i < rline.Length;
i++)
{
if (byteronis.Count <= i)
byteronis.Add(rline[i]);
else
byteronis[i] = (char)(byteronis[i] ^ rline[i]);
}
}
}
return new string(byteronis.ToArray());
}
/// <summary>
/// A Forensics check. Defaults to case insensitive
/// </summary>
/// <param name="args">[0]:FilePath, [1]:CaseSensitive</param>
internal __8502165c9d443e894a55d2df67368597(params string[] args)
{
if(args.Length < 2)
{
Enabled = false;
return;
}
ForensicsPath = args[0];
TickDelay = 20000;
try
{
CaseSensitive = Convert.ToBoolean(args[1]);
}
catch
{
CaseSensitive = false;
}
}
}

//cst
//NOTE: This file is sourced from ScoringEngine/InstallerCore/CheckTemplate.cs
//      Changes made outside of the source file will not be applied!
//https://docs.microsoft.com/en-us/windows/desktop/api/wuapi/nf-wuapi-iupdateservicemanager-get_services
//https://p0w3rsh3ll.wordpress.com/2013/01/09/get-windows-update-client-configuration/
/// <summary>
/// A check template to be used to define a new system check
/// </summary>
internal abstract class CheckTemplate : global::Engine.Core.EngineFrame.SingletonHost
{
#region REQUIRED
/// <summary>
/// Get the state of the check as an unsigned integer (4 byte solution, little endian)
/// </summary>
/// <returns>The state of the check</returns>
internal abstract Task<byte[]> GetCheckValue();
internal uint __ALT__ = 0x0;
/// <summary>
/// This is the method to use to request singleton data
/// </summary>
internal SingletonRequestDelegate RequestSingletonData;
/// <summary>
/// Singleton will call this method to request data from a singletonhost
/// </summary>
/// <param name="Client">The client calling this method</param>
/// <param name="args">The arguments for this interaction</param>
/// <returns></returns>
async Task<object[]> SingletonHost.RequestSingleAction(SingletonHost Client, object[] args)
{
return await RequestSingletonAction(Client, args);
}
/// <summary>
/// Create the check
/// </summary>
/// <param name="args">Arguments to pass to the check</param>
internal protected CheckTemplate(params string[] args)
{
}
#endregion
#region OPTIONAL
private uint _tickdelay_ = 1000;
/// <summary>
/// Speed at which this value updates
/// </summary>
internal virtual uint TickDelay
{
get
{
return _tickdelay_;
}
set
{
_tickdelay_ = value;
}
}
/// <summary>
/// The message to display for offline scoring when the check is completed
/// </summary>
internal virtual SafeString CompletedMessage => "Check passed";
/// <summary>
/// The message to display for offline scoring when the check is failed
/// </summary>
internal virtual SafeString FailedMessage => "Check failed";
/// <summary>
/// Request the singleton for this class to perform an action
/// </summary>
/// <param name="Client">The client calling this method</param>
/// <param name="args">The arguments for this interaction</param>
/// <returns></returns>
internal virtual async Task<object[]> RequestSingletonAction(SingletonHost Client, object[] args)
{
return null;
//default returns null, meaning a singleton is just not implemented for this class
}
#endregion
#region PRIVATE
/// <summary>
/// Timer for internal ticking
/// </summary>
private System.Diagnostics.Stopwatch Timer = new System.Diagnostics.Stopwatch();
/// <summary>
/// State of current check
/// </summary>
private byte[] CachedState = new byte[4];
/// <summary>
/// Used to lock the state check to only allow one async state to run
/// </summary>
private bool STATE_LOCK;
internal ushort Flags;
#endregion
//Specifications (since i literally cant think rn)
//GetState() -> Offline? -> Score += (Failed? -score : State = expected ? score : 0)
//              Online?  -> NetComm(CID,STATEVALUE)
//ONLINE SPEC: CANNOT TICK MORE THAN 1 TIME PER SECOND, MUST GET BATCHED (bandwith limiting)
//ONLINE SPEC: Consolidate data. IE: Hash strings using a PJW Hash, everything else is a number at or below 4 bytes. All states are reported as unsigned integers.
//ONLINE SPEC: Batch sizes should be less than [1024] byte returns
//ONLINE SPEC: Online batch ticking should be equal to or slower than internal ticking. Ticks are only called when the value is being batched for sending
//OFFLINE SPEC: Min tickrate = 1ms, default = 1000ms, max = 300000ms
//OFFLINE SPEC: Still hash strings with PJW Hash
//OFFLINE SPEC: Must return a score
//Implementation Specifications
//Returns the value of the check to communicate between networks
//internal abstract uint GetOnlineState()
//Returns the score of the state
//internal abstract int GetOfflineState()
/// <summary>
/// Can this check be ticked at this current point in time
/// </summary>
/// <returns></returns>
internal bool CanTick()
{
if (STATE_LOCK || Failed)
return false;
//Already ticking
if(!Timer.IsRunning)
{
Timer.Start();
return true;
}
return Timer.ElapsedMilliseconds >= TickDelay;
}
/// <summary>
/// Return the state of this check
/// </summary>
/// <returns></returns>
internal async Task<byte[]> CheckState()
{
if (CanTick())
{
STATE_LOCK = true;
try { CachedState = await GetCheckValue();
} catch { } //respect tick timer even after an exception
STATE_LOCK = false;
Timer.Restart();
}
return CachedState;
}
/// <summary>
/// Call to force the check to tick nice time checkstate is requested
/// </summary>
internal void ForceTick()
{
Timer.Stop();
Timer.Reset();
}
/// <summary>
/// Convert a string into a state
/// </summary>
/// <param name="content">The string to convert</param>
/// <returns>A state version of a string</returns>
private byte[] PrepareString32(string content) //PJW hash
{
return MD5F32(content);
uint hash = 0, high;
foreach(char s in content)
{
hash = (hash << 4) + (uint)s;
if ((high = hash & 0xF0000000) > 0)
hash ^= high >> 24;
hash &= ~high;
}
return BitConverter.GetBytes(hash);
}
internal byte[] MD5F16(string content)
{
byte[] data = __MD5__.ComputeHash(System.Text.Encoding.ASCII.GetBytes(content));
byte[] final = new byte[8];
for (int i = 0;
i < 2;
i++) //Fold md5 using addition. Could also xor.
{
for (int j = 0;
j < 8;
j++)
{
final[i] += data[i * 8 + j];
}
}
return final;
}
internal byte[] MD5(string content)
{
return __MD5__.ComputeHash(System.Text.Encoding.ASCII.GetBytes(content));
}
internal byte[] MD5(byte[] content)
{
return __MD5__.ComputeHash(content);
}
private MD5 __MD5__ = System.Security.Cryptography.MD5.Create();
private byte[] MD5F32(string content)
{
byte[] data = __MD5__.ComputeHash(System.Text.Encoding.ASCII.GetBytes(content));
byte[] final = new byte[4];
for(int i = 0;
i < 4;
i++) //Fold md5 using addition. Could also xor.
{
for(int j = 0;
j < 4;
j++)
{
final[i] += data[i * 4 + j];
}
}
return final;
}
/// <summary>
/// Prepare a state for replication
/// </summary>
/// <param name="o_state">The state to be prepared</param>
/// <returns>A UINT version of the state</returns>
internal byte[] PrepareState32(object o_state)
{
if (o_state == null)
return new byte[4];
if(o_state is bool)
return PrepareString32(o_state.ToString().ToLower());
return PrepareString32(o_state.ToString());
}
private bool __enabled__ = true;
/// <summary>
/// Is this check enabled for evaluation?
/// </summary>
internal bool Enabled
{
get
{
return Failed ? false : __enabled__;
}
set
{
__enabled__ = Failed ? false : value;
}
}
/// <summary>
/// Has this check been irreversably failed
/// </summary>
private bool __failed__ = false;
/// <summary>
/// Has this check been irreversably failed
/// </summary>
internal bool Failed
{
get
{
return __failed__;
}
set
{
if (__failed__ || !value)
return;
__failed__ = true;
CheckFailed?.Invoke();
}
}
/// <summary>
/// A scoring event
/// </summary>
internal delegate void ScoringEvent();
/// <summary>
/// Called when the check fails
/// </summary>
internal ScoringEvent CheckFailed;
/// <summary>
/// Flags for a check definition
/// </summary>
[Flags]
internal enum CheckDefFlags : byte
{
/// <summary>
/// Dont consider this check for points or offline scoring emit. This is strictly a hierarchy check.
/// </summary>
NoPoints = 1
}
private bool HasFlag(CheckDefFlags flag)
{
return (Flags & (byte)flag) > 0;
}
}
internal sealed class SafeString
{
private static readonly byte[] __key__ = new byte[] {183,42,97,45,217,15,124,53,112,159,80,118,119,47,66,52};//{ 0x00, 0xc5, 0x6c, 0xdd, 0x38, 0x8d, 0xa7, 0x02, 0x43, 0x92, 0x96, 0xae, 0x31, 0x99, 0x8f, 0x79 };
private byte[] data;
/// <summary>
/// Create a string from a safe string
/// </summary>
/// <param name="s"></param>
public static implicit operator string(SafeString s)
{
return D(s.data);
}
/// <summary>
/// Create a safe string from a normal string
/// </summary>
/// <param name="s"></param>
public static implicit operator SafeString(string s)
{
SafeString st = new SafeString
{
data = E(s)
};
return st;
}
/// <summary>
/// Create a byte array into a safestring
/// </summary>
/// <param name="dat"></param>
public static explicit operator SafeString(byte[] dat)
{
SafeString st = new SafeString
{
data = dat
};
return st;
}
public override string ToString()
{
return D(data);
}
private static byte[] E(string str)
{
if (str == null)
str = "";
byte[] encrypted;
byte[] IV;
using (Aes aesAlg = Aes.Create())
{
aesAlg.Key = __key__;
aesAlg.GenerateIV();
IV = aesAlg.IV;
aesAlg.Mode = CipherMode.CBC;
var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
// Create the streams used for encryption.
using (var msEncrypt = new MemoryStream())
{
using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
{
using (var swEncrypt = new StreamWriter(csEncrypt))
{
//Write all data to the stream.
swEncrypt.Write(str);
}
encrypted = msEncrypt.ToArray();
}
}
}
var combinedIvCt = new byte[IV.Length + encrypted.Length];
Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
// Return the encrypted bytes from the memory stream.
return combinedIvCt;
}
private static string D(byte[] str)
{
// Declare the string used to hold
// the decrypted text.
string plaintext = null;
// Create an Aes object
// with the specified key and IV.
using (Aes aesAlg = Aes.Create())
{
aesAlg.Key = __key__;
byte[] IV = new byte[aesAlg.BlockSize / 8];
byte[] cipherText = new byte[str.Length - IV.Length];
Array.Copy(str, IV, IV.Length);
Array.Copy(str, IV.Length, cipherText, 0, cipherText.Length);
aesAlg.IV = IV;
aesAlg.Mode = CipherMode.CBC;
// Create a decrytor to perform the stream transform.
ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
// Create the streams used for decryption.
using (var msDecrypt = new MemoryStream(cipherText))
{
using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
{
using (var srDecrypt = new StreamReader(csDecrypt))
{
// Read the decrypted bytes from the decrypting stream
// and place them in a string.
plaintext = srDecrypt.ReadToEnd();
}
}
}
}
return plaintext;
}
}
internal static class Extensions
{
#region Process Async from https://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why/39872058#39872058
public static async Task<int> StartProcess(
string filename,
string arguments,
string workingDirectory = null,
int? timeout = null,
TextWriter outputTextWriter = null,
TextWriter errorTextWriter = null)
{
using (var process = new Process()
{
StartInfo = new ProcessStartInfo()
{
CreateNoWindow = true,
Arguments = arguments,
FileName = filename,
RedirectStandardOutput = outputTextWriter != null,
RedirectStandardError = errorTextWriter != null,
UseShellExecute = false,
WorkingDirectory = workingDirectory
}
})
{
process.Start();
var cancellationTokenSource = timeout.HasValue ?
new CancellationTokenSource(timeout.Value) :
new CancellationTokenSource();
var tasks = new List<Task>(3) { process.WaitForExitAsync(cancellationTokenSource.Token) };
if (outputTextWriter != null)
{
tasks.Add(ReadAsync(
x =>
{
process.OutputDataReceived += x;
process.BeginOutputReadLine();
},
x => process.OutputDataReceived -= x,
outputTextWriter,
cancellationTokenSource.Token));
}
if (errorTextWriter != null)
{
tasks.Add(ReadAsync(
x =>
{
process.ErrorDataReceived += x;
process.BeginErrorReadLine();
},
x => process.ErrorDataReceived -= x,
errorTextWriter,
cancellationTokenSource.Token));
}
await Task.WhenAll(tasks);
return process.ExitCode;
}
}
/// <summary>
/// Waits asynchronously for the process to exit.
/// </summary>
/// <param name="process">The process to wait for cancellation.</param>
/// <param name="cancellationToken">A cancellation token. If invoked, the task will return
/// immediately as cancelled.</param>
/// <returns>A Task representing waiting for the process to end.</returns>
public static Task WaitForExitAsync(
this Process process,
CancellationToken cancellationToken = default(CancellationToken))
{
process.EnableRaisingEvents = true;
var taskCompletionSource = new TaskCompletionSource<object>();
EventHandler handler = null;
handler = (sender, args) =>
{
process.Exited -= handler;
taskCompletionSource.TrySetResult(null);
};
process.Exited += handler;
if (cancellationToken != default(CancellationToken))
{
cancellationToken.Register(
() =>
{
process.Exited -= handler;
taskCompletionSource.TrySetCanceled();
});
}
return taskCompletionSource.Task;
}
/// <summary>
/// Reads the data from the specified data recieved event and writes it to the
/// <paramref name="textWriter"/>.
/// </summary>
/// <param name="addHandler">Adds the event handler.</param>
/// <param name="removeHandler">Removes the event handler.</param>
/// <param name="textWriter">The text writer.</param>
/// <param name="cancellationToken">The cancellation token.</param>
/// <returns>A task representing the asynchronous operation.</returns>
public static Task ReadAsync(
this Action<DataReceivedEventHandler> addHandler,
Action<DataReceivedEventHandler> removeHandler,
TextWriter textWriter,
CancellationToken cancellationToken = default(CancellationToken))
{
var taskCompletionSource = new TaskCompletionSource<object>();
DataReceivedEventHandler handler = null;
handler = new DataReceivedEventHandler(
(sender, e) =>
{
if (e.Data == null)
{
removeHandler(handler);
taskCompletionSource.TrySetResult(null);
}
else
{
textWriter.WriteLine(e.Data);
}
});
addHandler(handler);
if (cancellationToken != default(CancellationToken))
{
cancellationToken.Register(
() =>
{
removeHandler(handler);
taskCompletionSource.TrySetCanceled();
});
}
return taskCompletionSource.Task;
}
#endregion
/// <summary>
/// Get the bash output of a command
/// </summary>
/// <param name="cmd"></param>
/// <returns></returns>
public static async Task<string> Bash(this string cmd, int? timeout = null)
{
var escapedArgs = cmd.Replace("\"", "\\\"");
StringWriter stdout = new StringWriter();
StringWriter stderr = new StringWriter();
string result = "";
try
{
var _out = await StartProcess("/bin/bash", $"-c \"{escapedArgs}\"", Environment.CurrentDirectory, timeout, stdout, stderr);
if (_out == 0)
result = stdout.ToString();
else
result = stderr.ToString();
}
catch
{
result = stderr.ToString();
}
finally
{
stdout.Dispose();
stderr.Dispose();
}
return result;
}
/// <summary>
/// Safely set bytes in a list
/// </summary>
/// <param name="RawData">The list to set bytes in</param>
/// <param name="index">The index of the bytes</param>
/// <param name="bytes">The byte array to write</param>
public static void SetBytes(this List<byte> RawData, int index, byte[] bytes)
{
while (index + bytes.Length > RawData.Count)
RawData.Add(0x0);
for (int i = 0;
i < bytes.Length;
i++)
{
RawData[i + index] = bytes[i];
}
return;
}
/// <summary>
/// Safely get bytes from a list of bytes
/// </summary>
/// <param name="RawData">The list to read from</param>
/// <param name="index">The index to read at</param>
/// <param name="count">The amount of bytes to read</param>
/// <returns></returns>
public static byte[] GetBytes(this List<byte> RawData, int index, int count)
{
while (index + count > RawData.Count)
RawData.Add(0x0);
byte[] bytes = new byte[count];
for (int i = 0;
i < count;
i++)
{
bytes[i] = RawData[i + index];
}
return bytes;
}
/// <summary>
/// Read a null terminated string from a byte list
/// </summary>
/// <param name="RawData">The raw data to parse from</param>
/// <param name="index">The index of the string. Gets modified to be past the null character</param>
/// <returns></returns>
public static string ReadString(this List<byte> RawData, ref int index)
{
string result = "";
while (index < RawData.Count && RawData[index] != 0x0)
{
result += (char)RawData[index];
index++;
}
index++;
return result;
}
}


}
