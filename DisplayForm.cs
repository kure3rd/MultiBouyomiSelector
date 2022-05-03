using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

class DisplayForm : Form
{
    Timer timer;
    BouyomiChanServer Server;
    public DisplayForm(XElement settings)
    {
        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new List<BouyomiChanClient>();
        foreach (XElement element in BouyomiChanLocations) {
            var client = new BouyomiChanClient("" + element.Name, element.Value);
            BouyomiChanList.Add(client);
        }

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Server = new BouyomiChanServer(IpcServerName, BouyomiChanList);

        timer = new Timer();
        timer.Tick += new EventHandler(Server.SendText);
        timer.Interval = 100;//ms
        timer.Start();
    }
}