using System.IO;
using System.Xml.Linq;

public enum ProcessStatus
{
    Running,
    Runnable,
    FileNotFound,
    SettingNotFound,
    DirectoryNotFound,
}
public enum ConnectStatus
{
    Connected,
    Disconnected,
    DifferentName,
}
public enum ServerStatus
{
    Wait,
    Busy,
    Other,
}

class BouyomiChanStatus
{
    public string DirectoryLocation = null;
    public string IpcChannelName = null;
    public string LastTalkText = "";
    public ProcessStatus? ProcessState = null;
    public ConnectStatus? isConnected = null;
    public ServerStatus? isBusy = null;
    
    public string ExeLocation = null;
    public string SettingLocation = null;

    public BouyomiChanStatus(string IpcChannelName, string DirectoryLocation)
    {
        this.DirectoryLocation = DirectoryLocation;
        this.IpcChannelName = IpcChannelName;
        if (CheckFileExistance())
        {
            isConnected = CheckChannelName() ? ConnectStatus.Disconnected : ConnectStatus.DifferentName;
        }
        System.Console.WriteLine(IpcChannelName+":"+ProcessState+":"+isConnected);
    }

    private bool CheckFileExistance()
    {
        if (!Directory.Exists(DirectoryLocation))
        {
            ProcessState = ProcessStatus.DirectoryNotFound;
            return false;
        }

        var exelocation = Path.Combine(DirectoryLocation, "BouyomiChan.exe");
        if (!File.Exists(exelocation))
        {
            ProcessState = ProcessStatus.FileNotFound;
            return false;
        }
        ExeLocation = exelocation;

        var settinglocation = Path.Combine(DirectoryLocation, "BouyomiChan.setting");
        if (!File.Exists(settinglocation))
        {
            ProcessState = ProcessStatus.SettingNotFound;
            return false;
        }
        SettingLocation = settinglocation;

        ProcessState = ProcessStatus.Runnable;
        return true;
    }

    private bool CheckChannelName()
    {
        XElement BouyomiChanSetting = XElement.Load(SettingLocation);
        return IpcChannelName == BouyomiChanSetting.Element("IpcChannelName").Value;
    }
}