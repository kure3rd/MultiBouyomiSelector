using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

public static class Program
{
    [STAThread]
    public static void Main()
    {
//        Console.WriteLine("Hello, World!");
        using var form = new Form
        {
            FormBorderStyle = FormBorderStyle.Sizable,
            Text = "Hello WinForms",
            ClientSize = new System.Drawing.Size(800, 600)
        };

        form.Show();

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

        while (true)
        {
            Application.DoEvents();
        }
    }
}
