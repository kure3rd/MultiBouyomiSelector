using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

class BouyomiChanServer
{
    List<FNF.Utility.BouyomiChanRemoting> RemotingObjectList;
    ConcurrentRingBuffer<string> MessageQueue;

    void ReceiveText(object sender, FNF.Utility.ReceiveTextEventArgs e)
    {
        Console.WriteLine("TalkText Called");
        MessageQueue.Enqueue(e.Message);
    }
    
    public void SendText(object sender, EventArgs e)
    {
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
        RemotingObject.ReceiveTextEvent += new FNF.Utility.BouyomiChanRemoting.ReceiveTextEventHandler(ReceiveText);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.RemotingObjectList = RemotingObjectList;
        this.MessageQueue = new ConcurrentRingBuffer<string>(10);
    }
}
