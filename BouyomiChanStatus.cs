public enum ProcessState
{
    Running,
    FileNotFound,
    DirectoryNotFound,
}
public enum ConnectStatus
{
    Connected,
    Disconnected,
}
public enum ServerStatus
{
    Wait,
    Busy,
    Other,
}

class BouyomiChanStatus
{
    public string ExeLocation;
    public string ChannelName;
    public string LastTalkText = "";
    public ProcessState ProcessState;
    public ConnectStatus isConnected;
    public ServerStatus isBusy;
}