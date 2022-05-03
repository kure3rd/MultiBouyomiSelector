using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.Remoting;

class BouyomiChanClient// : IDisposable
{
    public FNF.Utility.BouyomiChanRemoting RemotingObject = null;
    public BouyomiChanStatus Status;
    private Process BouyomiChanProcess;
    private IpcClientChannel ClientChannel = null;
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

        ClientChannel = new IpcClientChannel(IpcChannelName, null); //チャンネル名は何でもいい
        ChannelServices.RegisterChannel(ClientChannel, false);
        RemotingObject = (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://"+IpcChannelName+"/Remoting");

        Console.WriteLine(IpcChannelName+":Client Created");

        
    }
    ~BouyomiChanClient()
    {
        Dispose(false);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (ClientChannel != null)
        {
            ChannelServices.UnregisterChannel(ClientChannel);
            ClientChannel = null;
        }
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
        get 
        { 
            try 
            {
                return RemotingObject is null ? true : RemotingObject.NowPlaying;
            }
            catch (RemotingException)
            {
                return true;
            }

        }
    }
}
