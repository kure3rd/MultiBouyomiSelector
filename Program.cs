using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace FNF.Utility {
    class BouyomiChanRemoting : MarshalByRefObject {
        public void AddTalkTask (string sTalkText) { }
        public void AddTalkTask (string sTalkText, int iSpeed, int iVolume, int vType) { AddTalkTask(sTalkText); }

        public void AddTalkTask (string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask(sTalkText); }
        public int  AddTalkTask2(string sTalkText) { return 0; }
        public int  AddTalkTask2(string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask2(sTalkText); return 0; }
        public void ClearTalkTasks() { Console.WriteLine("A"); }
        public void SkipTalkTask() { Console.WriteLine("A"); }

        public int  TalkTaskCount { get { return 0; }         }
        public int  NowTaskId     { get { return 0; }         }
        public bool NowPlaying    { get { return false; }         }
        public bool Pause         { get { return false; } set { } }
    }
}

class Program
{    
    static void Main()
    {
        var ServerChannel = new IpcServerChannel("BouyomiChan");
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        var ClientChannelA = new IpcClientChannel("hogeA", null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannelA, false);
        var RemotingObjectA = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://BouyomiChanA/Remoting");
        RemotingObjectA.AddTalkTask2("test A");

        var ClientChannelB = new IpcClientChannel("hogeB", null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannelB, false);
        var RemotingObjectB = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://BouyomiChanB/Remoting");
        RemotingObjectB.AddTalkTask2("test B");
    
 
        Console.WriteLine("Hello, World!");
        Console.WriteLine(ServerChannel.GetChannelUri());
        Console.ReadLine();
    }
}
