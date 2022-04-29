using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;


public class BouyomiChanRemoting : MarshalByRefObject {
    public void AddTalkTask (string sTalkText) { Console.WriteLine('A'); }
    public void AddTalkTask (string sTalkText, int iSpeed, int iVolume, int vType) { Console.WriteLine('B'); }

    public void AddTalkTask (string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { Console.WriteLine('C'); }
    public int  AddTalkTask2(string sTalkText) { Console.WriteLine('D'); return 0; }
    public int  AddTalkTask2(string sTalkText, int iSpeed, int iTone, int iVolume, int vType) { Console.WriteLine('E'); return 0; }
    public void ClearTalkTasks() { }
    public void SkipTalkTask() { }

    public int  TalkTaskCount { get { return 0; }         }
    public int  NowTaskId     { get { return 0; }         }
    public bool NowPlaying    { get { return true; }         }
    public bool Pause         { get { return false; } set { } }
}


class Program
{    static void Main()
    {
        var ServerChannel = new IpcServerChannel("BouyomiChan");
        ChannelServices.RegisterChannel(ServerChannel, false);

        var RemotingObject = new BouyomiChanRemoting();
        RemotingServices.Marshal(RemotingObject, "Remoting", typeof(BouyomiChanRemoting));
        
        Console.WriteLine("Hello, World!");
        Console.WriteLine(ServerChannel.GetChannelUri());
        Console.ReadLine();
    }
}
