using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

class DisplayForm : Form
{
    BouyomiChanServer Server;
    public DisplayForm(XElement settings)
    {
        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new List<FNF.Utility.BouyomiChanRemoting>();
        foreach (XElement element in BouyomiChanLocations) {
            var BouyomiChan = new BouyomiChanClient("" + element.Name);
            BouyomiChanList.Add(BouyomiChan.RemotingObject);
        }

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Server = new BouyomiChanServer(IpcServerName, BouyomiChanList);
    }
}