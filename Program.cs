using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Threading.Tasks;

namespace FNF.Utility {
    class BouyomiChanRemoting : MarshalByRefObject {
        public event EventHandler TalkTextEvent;
        public void AddTalkTask (string sTalkText) { TalkTextEvent(this, EventArgs.Empty); }
        public void AddTalkTask (string sTalkText, int iSpeed, int iVolume, int vType) { AddTalkTask(sTalkText); }

        public void AddTalkTask (string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask(sTalkText); }
        public int  AddTalkTask2(string sTalkText) { TalkTextEvent(this, EventArgs.Empty); return 0; }
        public int  AddTalkTask2(string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { AddTalkTask2(sTalkText); return 0; }
        public void ClearTalkTasks() { Console.WriteLine("A"); }
        public void SkipTalkTask() { Console.WriteLine("A"); }

        public int  TalkTaskCount { get { return 0; }         }
        public int  NowTaskId     { get { return 0; }         }
        public bool NowPlaying    { get { return false; }         }
        public bool Pause         { get { return false; } set { } }
    }
}

class BouyomiChanServer
{
    FNF.Utility.BouyomiChanRemoting RemotingObjectA;
    FNF.Utility.BouyomiChanRemoting RemotingObjectB;

    void Talk(object sender, System.EventArgs e)
    {
        Console.WriteLine("TalkText Called");
        RemotingObjectA.AddTalkTask2("test A");
        RemotingObjectB.AddTalkTask2("test B");
    }

    public BouyomiChanServer(FNF.Utility.BouyomiChanRemoting RemotingObjectA, FNF.Utility.BouyomiChanRemoting RemotingObjectB)
    {
        var ServerChannel = new IpcServerChannel("BouyomiChan");
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.TalkTextEvent += new EventHandler(Talk);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.RemotingObjectA = RemotingObjectA;
        this.RemotingObjectB = RemotingObjectB;
    }
}

class BouyomiChanClient
{
    public FNF.Utility.BouyomiChanRemoting RemotingObject;
    public BouyomiChanClient(string IpcChannelName)
    {
        var ClientChannel = new IpcClientChannel(IpcChannelName, null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannel, false);
        RemotingObject = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://"+IpcChannelName+"/Remoting");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        var ClientA = new BouyomiChanClient("BouyomiChanA");
        var ClientB = new BouyomiChanClient("BouyomiChanB");

        var Server = new BouyomiChanServer(ClientA.RemotingObject, ClientB.RemotingObject);

//        Console.WriteLine(ServerChannel.GetChannelUri());
        Console.ReadLine();
    }
}
