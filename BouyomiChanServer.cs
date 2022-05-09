using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

class BouyomiChanServer
{
    public List<BouyomiChanClient> ClientList;
    private Queue<BouyomiChanClient> ClientQueue;
    ConcurrentRingBuffer<string> MessageQueue;

    void ReceiveText(object sender, FNF.Utility.ReceiveTextEventArgs e)
    {
        MessageQueue.Enqueue(e.Message);
    }
    
    public void SendText(object sender, EventArgs e)
    {
        int c = 0;
        string message;
        foreach (var client in ClientQueue)
        {
            c++;
            client.UpdateStatus();
            if (!client.NowPlaying && MessageQueue.TryDequeue(out message, 3))
            {
                client.Talk(message);
                break;
            }
        }
        for (int i = c; i < ClientQueue.Count; i++) ClientQueue.Enqueue(ClientQueue.Dequeue());
        if (c < ClientQueue.Count) Console.WriteLine(ClientQueue.Peek().Status.IpcChannelName);
    }

    public BouyomiChanServer(string IpcServerName, List<BouyomiChanClient> ClientList, ConcurrentRingBuffer<string> MessageQueue)
    {
        var ServerChannel = new IpcServerChannel(IpcServerName);
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.ReceiveTextEvent += new FNF.Utility.BouyomiChanRemoting.ReceiveTextEventHandler(ReceiveText);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.ClientList = ClientList;
        this.ClientQueue = new Queue<BouyomiChanClient>(ClientList);
        this.MessageQueue = MessageQueue;
    }
}
