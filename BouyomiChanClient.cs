using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

class BouyomiChanClient
{
    public FNF.Utility.BouyomiChanRemoting RemotingObject = null;
    public BouyomiChanStatus Status;
    private Process BouyomiChanProcess;
    public BouyomiChanClient(string IpcChannelName, string DirectoryLocation)
    {
        Status = new BouyomiChanStatus(IpcChannelName, DirectoryLocation);
        if (Status.ProcessState != ProcessStatus.Runnable) return;

        if (!CheckBouyomiChanRunning())
        {
            BouyomiChanProcess = Process.Start(Status.ExeLocation);
            if (BouyomiChanProcess is null) return;
            if (!BouyomiChanProcess.WaitForInputIdle()) return;
        }

        var ClientChannel = new IpcClientChannel(IpcChannelName, null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannel, false);
        RemotingObject = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://"+IpcChannelName+"/Remoting");

        Console.WriteLine(IpcChannelName+":Client Created");

        
    }
    private bool CheckBouyomiChanRunning()
    {
        Process[] BouyomiChanProcesses = Process.GetProcessesByName("BouyomiChan");
        foreach (var process in BouyomiChanProcesses)
        {
            string ProcessDirectory = Path.GetDirectoryName(process.MainModule.FileName);
            Console.WriteLine(ProcessDirectory+":"+Status.DirectoryLocation);
            if (ProcessDirectory == Status.DirectoryLocation)
            {
                BouyomiChanProcess = process;
                return true;
            }
        }
        return false;
    }
    public bool NowPlaying {
        get { return RemotingObject is null ? true : RemotingObject.NowPlaying; }
    }
}
