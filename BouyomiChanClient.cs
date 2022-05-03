using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Xml.Linq;
using System.IO;

class BouyomiChanClient
{
    public FNF.Utility.BouyomiChanRemoting RemotingObject;
    public BouyomiChanStatus Status;
    public BouyomiChanClient(string IpcChannelName, string DirectoryLocation)
    {
        Status = new BouyomiChanStatus();
        Status.DirectoryLocation = DirectoryLocation;
        Status.IpcChannelName = IpcChannelName;
        if (!CheckChannelName()) return;

        var ClientChannel = new IpcClientChannel(IpcChannelName, null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannel, false);
        RemotingObject = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://"+IpcChannelName+"/Remoting");


        
    }
    public bool CheckChannelName()
    {
        if(!Directory.Exists(Status.DirectoryLocation))
        {
            Status.ProcessState = ProcessState.DirectoryNotFound;
            return false;
        }
        string BouyomiChanSettingLocation = Path.Combine(Status.DirectoryLocation, "BouyomiChan.setting");
        if(!File.Exists(BouyomiChanSettingLocation))
        {
            Status.ProcessState = ProcessState.SettingNotFound;
            return false;
        }
        XElement BouyomiChanSetting = XElement.Load(BouyomiChanSettingLocation);
        if (Status.IpcChannelName != BouyomiChanSetting.Element("IpcChannelName").Value)
        {
            Status.isConnected = ConnectStatus.DifferentName;
            return false;
        }
        return true;
    }
}
