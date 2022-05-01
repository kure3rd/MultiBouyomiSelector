using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;

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
    List<BouyomiChanClient> BouyomiChanClientList;

    void Talk(object sender, System.EventArgs e)
    {
        Console.WriteLine("TalkText Called");
        foreach (var BouyomiChanClient in BouyomiChanClientList)
        {
            BouyomiChanClient.RemotingObject.AddTalkTask2("A");
        }
    }

    public BouyomiChanServer(string IpcServerName, List<BouyomiChanClient> BouyomiChanClientList)
    {
        var ServerChannel = new IpcServerChannel(IpcServerName);
        ChannelServices.RegisterChannel(ServerChannel, false);
        var RemotingObject = new FNF.Utility.BouyomiChanRemoting();
        RemotingObject.TalkTextEvent += new EventHandler(Talk);
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(FNF.Utility.BouyomiChanRemoting));

        this.BouyomiChanClientList = BouyomiChanClientList;
    }
}

class BouyomiChanSetting
{
    public string IpcChannelName;
    public string Directory;

    public BouyomiChanSetting(string IpcChannelName, string Directory)
    {
        this.IpcChannelName = IpcChannelName;
        this.Directory = Directory;
    }
    public BouyomiChanSetting(XElement SettingXml)
    {
        this.IpcChannelName = "" + SettingXml.Name;
        this.Directory = SettingXml.Value;
    }
    public bool isEqualChannelName(BouyomiChanSetting other)
    {
        return this.IpcChannelName == other.IpcChannelName;
    }
    public bool isEqualDirectory(BouyomiChanSetting other)
    {
        return this.Directory == other.Directory;
    }
    public bool isEqualSetting(BouyomiChanSetting other)
    {
        return this.isEqualChannelName(other) && this.isEqualDirectory(other);
    }
}

class BouyomiChanClient : IDisposable
{
    private BouyomiChanSetting SelectorSideSetting;
//    private BouyomiChanSetting ServerSideSetting;
    public FNF.Utility.BouyomiChanRemoting RemotingObject;
    private IpcClientChannel ClientChannel;
    private Process _bouyomiChanProcess = null;

    public BouyomiChanClient(BouyomiChanSetting SelectorSideSetting)
    {
        _bouyomiChanProcess = StartBouyomiChanProcess(SelectorSideSetting.Directory);
        ClientChannel = _makeIpcClientChannel(SelectorSideSetting.IpcChannelName);
        RemotingObject = _connectBouyomiChanRemoting(ClientChannel.ChannelName);
        this.SelectorSideSetting = SelectorSideSetting;
//        this.ServerSideSetting = ServerSideSetting;
    }
    private static IpcClientChannel _makeIpcClientChannel(string IpcChannelName)
    {
        var ClientChannel = new IpcClientChannel(IpcChannelName, null);
        ChannelServices.RegisterChannel(ClientChannel, false);
        return ClientChannel;
    }
    private static FNF.Utility.BouyomiChanRemoting _connectBouyomiChanRemoting(string ClientChannelName)
    {
        return (FNF.Utility.BouyomiChanRemoting)Activator.GetObject(typeof(FNF.Utility.BouyomiChanRemoting), "ipc://" + ClientChannelName + "/Remoting");
    }
    public Process CheckBouyomiChanRunning(string BouyomiChanSettingDirectory)
    {
        Process[] BouyomiChanProcesses = Process.GetProcessesByName("BouyomiChan");
        foreach (var BouyomiChanProcess in BouyomiChanProcesses)
        {
            string BouyomiChanProcessDirectory = Path.GetDirectoryName(BouyomiChanProcess.MainModule.FileName);
            if (BouyomiChanSettingDirectory == BouyomiChanProcessDirectory)
            {
                Console.WriteLine("Process Found:"+BouyomiChanProcessDirectory);
                return BouyomiChanProcess;
            }
        }
        Console.WriteLine("Process Not Found:"+BouyomiChanSettingDirectory);
        return null;
    }
    public Process StartBouyomiChanProcess(string BouyomiChanSettingDirectory)
    {
        var process = CheckBouyomiChanRunning(BouyomiChanSettingDirectory);
        if (process != null) return process;
        if (!System.IO.Directory.Exists(BouyomiChanSettingDirectory))
        {
            throw new DirectoryNotFoundException("Directory Not Found set in "+ClientChannel.ChannelName);
        }
        string BouyomiChanExePath = BouyomiChanSettingDirectory + "\\BouyomiChan.exe";
        if (!System.IO.File.Exists(BouyomiChanExePath))
        {
            throw new FileNotFoundException("BouyomiChan.exe Not Found");
        }
        process = Process.Start(BouyomiChanExePath);
        process.EnableRaisingEvents = true;
        process.Exited += _endBouyomiChanProcess;
        return process;
    }
    private void _endBouyomiChanProcess(object sender, EventArgs e)
    {
        //now printing ...
    }

    #region IDisposable Supports

    private bool _disposedValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing) { }

            if (ClientChannel != null)
            {
                ChannelServices.UnregisterChannel(ClientChannel);
                ClientChannel = null;
            }

            if (_bouyomiChanProcess != null)
            {
                _bouyomiChanProcess.Close();
                _bouyomiChanProcess = null;
            }
            _disposedValue = true;
        }
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
    #endregion
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        XElement settings = XElement.Load("settings.xml");   

//        if (System.Convert.ToBoolean(settings.Element("LaunchAllBouyomiChan").Value))
//        {
        Process[] BouyomiChanProcesses = Process.GetProcessesByName("BouyomiChan");
        foreach (var BouyomiChanProcess in BouyomiChanProcesses) {
            string BouyomiChanPath = Path.GetDirectoryName(BouyomiChanProcess.MainModule.FileName);
            Console.WriteLine("Process:"+BouyomiChanPath);
            XElement BouyomiChanSettings = XElement.Load(BouyomiChanPath+"\\BouyomiChan.setting");
            Console.WriteLine("BouyomiChan.setting:"+BouyomiChanSettings.Element("IpcChannelName").Value);
        }
//        }


        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanClientList = new List<BouyomiChanClient>();
        foreach (XElement el in BouyomiChanLocations) {
            var SelectorSideSetting = new BouyomiChanSetting(el);
            var client = new BouyomiChanClient(SelectorSideSetting);
            BouyomiChanClientList.Add(client);

            Console.WriteLine(el.Name);
        }

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Console.WriteLine(IpcServerName);
        var Server = new BouyomiChanServer(IpcServerName, BouyomiChanClientList);

        Console.ReadLine();

        foreach (var client in BouyomiChanClientList)
        {
            client.Dispose();
        }
    }
}
