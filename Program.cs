using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
    List<FNF.Utility.BouyomiChanRemoting> RemotingObjectList;

    void Talk(object sender, System.EventArgs e)
    {
        Console.WriteLine("TalkText Called");
        foreach (var RemotingObject in RemotingObjectList)
        {
            RemotingObject.AddTalkTask2("Test A");
        }
    }

    public BouyomiChanServer(List<FNF.Utility.BouyomiChanRemoting> RemotingObjectList)
    {
        var ServerChannel = new IpcServerChannel("BouyomiChan");
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.TalkTextEvent += new EventHandler(Talk);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.RemotingObjectList = RemotingObjectList;
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

        XElement settings = XElement.Load("settings.xml");
        
        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new List<FNF.Utility.BouyomiChanRemoting>();
        foreach (XElement el in BouyomiChanLocations) {
            var BouyomiChan = new BouyomiChanClient("" + el.Name);
            BouyomiChanList.Add(BouyomiChan.RemotingObject);
            
            Console.WriteLine(el.Name);
        }

        var Server = new BouyomiChanServer(BouyomiChanList);

        Console.ReadLine();
    }
}
