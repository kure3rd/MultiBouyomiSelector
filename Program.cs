using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        XElement settings = XElement.Load("settings.xml");   

        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new List<FNF.Utility.BouyomiChanRemoting>();
        foreach (XElement el in BouyomiChanLocations) {
            var BouyomiChan = new BouyomiChanClient("" + el.Name);
            BouyomiChanList.Add(BouyomiChan.RemotingObject);

            Console.WriteLine(el.Name);
        }

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Console.WriteLine(IpcServerName);
        var Server = new BouyomiChanServer(IpcServerName, BouyomiChanList);

        Console.ReadLine();
    }
}
