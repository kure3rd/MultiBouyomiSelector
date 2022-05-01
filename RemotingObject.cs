using System;

namespace FNF.Utility {
    class TalkEventArgs : EventArgs
    {
        public string Message;
    }
    class BouyomiChanRemoting : MarshalByRefObject {
        public delegate void TalkEventHandler(object sender, TalkEventArgs e);
        public event TalkEventHandler TalkEvent;
        public void AddTalkTask (string sTalkText) { var e = new TalkEventArgs(); e.Message = sTalkText; TalkEvent(this, e); }
        public void AddTalkTask (string sTalkText, int iSpeed, int iVolume, int vType) { AddTalkTask(sTalkText); }

        public void AddTalkTask (string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask(sTalkText); }
        public int  AddTalkTask2(string sTalkText) { var e = new TalkEventArgs(); e.Message = sTalkText; TalkEvent(this, e); return 0; }
        public int  AddTalkTask2(string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask2(sTalkText); return 0; }
        public void ClearTalkTasks() { Console.WriteLine("A"); }
        public void SkipTalkTask() { Console.WriteLine("A"); }

        public int  TalkTaskCount { get { return 0; }         }
        public int  NowTaskId     { get { return 0; }         }
        public bool NowPlaying    { get { return false; }         }
        public bool Pause         { get { return false; } set { } }
    }
}
