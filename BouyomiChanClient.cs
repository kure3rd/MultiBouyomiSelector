using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

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
