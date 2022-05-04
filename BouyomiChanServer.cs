using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

class BouyomiChanServer
{
    public List<BouyomiChanClient> ClientList;
    ConcurrentRingBuffer<string> MessageQueue;

    void ReceiveText(object sender, FNF.Utility.ReceiveTextEventArgs e)
    {
        Console.WriteLine("TalkText Called");
        MessageQueue.Enqueue(e.Message);
    }
    
    public void SendText(object sender, EventArgs e)
    {
        string message;
        foreach (var client in ClientList)
        {
            if (!client.NowPlaying && MessageQueue.TryDequeue(out message, 3))
            {
                client.Talk(message);
            }
        }
    }

    public BouyomiChanServer(string IpcServerName, List<BouyomiChanClient> ClientList)
    {
        var ServerChannel = new IpcServerChannel(IpcServerName);
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.ReceiveTextEvent += new FNF.Utility.BouyomiChanRemoting.ReceiveTextEventHandler(ReceiveText);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.ClientList = ClientList;
        this.MessageQueue = new ConcurrentRingBuffer<string>(10);
    }
}
