using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Xml.Linq;

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

class ConcurrentRingBuffer<T>
{
    private ConcurrentQueue<T> _queue;
    public ConcurrentRingBuffer(int capacity)
    {
        _queue = new ConcurrentQueue<T>();
    }
    public void Enqueue(T data) => _queue.Enqueue(data);
    public bool TryDequeue(out T result, int tail)
    {
        while (_queue.Count >= tail)
        {
            _queue.TryDequeue(out result);
        }
        return _queue.TryDequeue(out result);
    }
}

class BouyomiChanServer
{
    List<FNF.Utility.BouyomiChanRemoting> RemotingObjectList;
    ConcurrentRingBuffer<string> MessageQueue;

    void Talk(object sender, FNF.Utility.TalkEventArgs e)
    {
        Console.WriteLine("TalkText Called");
        MessageQueue.Enqueue(e.Message);
        string message;

        foreach (var RemotingObject in RemotingObjectList)
        {
            if (!RemotingObject.NowPlaying && MessageQueue.TryDequeue(out message, 3))
            {
                RemotingObject.AddTalkTask2(message);
            }
        }
    }

    public BouyomiChanServer(string IpcServerName, List<FNF.Utility.BouyomiChanRemoting> RemotingObjectList)
    {
        var ServerChannel = new IpcServerChannel(IpcServerName);
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.TalkEvent += new FNF.Utility.BouyomiChanRemoting.TalkEventHandler(Talk);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.RemotingObjectList = RemotingObjectList;
        this.MessageQueue = new ConcurrentRingBuffer<string>(10);
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

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Console.WriteLine(IpcServerName);
        var Server = new BouyomiChanServer(IpcServerName, BouyomiChanList);

        Console.ReadLine();
    }
}
