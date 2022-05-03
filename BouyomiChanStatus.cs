public enum ProcessState
{
    Running,
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
    public ProcessState? ProcessState = null;
    public ConnectStatus? isConnected = null;
    public ServerStatus? isBusy = null;
}