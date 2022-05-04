using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

class DisplayLabel : Label
{
    public DisplayLabel(string name) : base()
    {
        Name = name;
        Text = name;
        AutoSize = true;
    }
}
class EnumDisplayLabel : Label
{
    public EnumDisplayLabel(Enum state) : base()
    {
        if (state is not null) Text = state.ToString();
    }
    public void Update(Enum state)
    {
        if (state is not null) Text = state.ToString();
    }
}

class ClientPanel : TableLayoutPanel
{
    DisplayLabel DirectionLocateLabel;
    DisplayLabel IpcChannelLabel;
    DisplayLabel LastTalkLabel;
    EnumDisplayLabel ProcessLabel;
    EnumDisplayLabel ConnectionLabel;
    EnumDisplayLabel BusyLabel;

    BouyomiChanStatus Status;
//    public delegate void PanelUpdateEventHandler(object sender, PanelUpdateEventArgs s);
 //   public event PanelUpdateEventHandler PanelUpdateEvent;
    public ClientPanel(BouyomiChanStatus status) : base()
    {
        Status = status;
        ColumnCount = 3;
        RowCount = 3;
        AutoSize = true;

        Controls.Add(new DisplayLabel("Path:"), 0, 0);
        Controls.Add(new DisplayLabel("Channel:"), 0, 1);
        Controls.Add(new DisplayLabel("LastTalk:"), 0, 2);
        DirectionLocateLabel = new DisplayLabel(System.IO.Path.GetFileName(Status.DirectoryLocation));
        Controls.Add(DirectionLocateLabel, 1, 0);
        IpcChannelLabel = new DisplayLabel(Status.IpcChannelName);
        Controls.Add(IpcChannelLabel, 1, 1);
        LastTalkLabel = new DisplayLabel(Status.LastTalkText);
        Controls.Add(LastTalkLabel, 1, 2);
        ProcessLabel = new EnumDisplayLabel(Status.ProcessState);
        Controls.Add(ProcessLabel, 2, 0);
        ConnectionLabel = new EnumDisplayLabel(Status.isConnected);
        Controls.Add(ConnectionLabel, 2, 1);
        BusyLabel = new EnumDisplayLabel(Status.isBusy);
        Controls.Add(BusyLabel, 2, 2);
    }

    public void Update(BouyomiChanStatus status)
    {
        LastTalkLabel.Text = status.LastTalkText;
        ProcessLabel.Update(status.ProcessState);
        ConnectionLabel.Update(status.isConnected);
        BusyLabel.Update(status.isBusy);
    }
}
class DisplayForm : Form
{
    List<ClientPanel> ClientPanelList;
    Timer timer;
    BouyomiChanServer Server;
    FlowLayoutPanel layoutPanel;

    public DisplayForm(XElement settings)
    {
        timer = new Timer();
        ClientPanelList = new List<ClientPanel>();
        layoutPanel = new FlowLayoutPanel();
        layoutPanel.FlowDirection = FlowDirection.TopDown;
        layoutPanel.Size = new System.Drawing.Size(1000,1000);

        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new List<BouyomiChanClient>();
        foreach (XElement element in BouyomiChanLocations) {
            var client = new BouyomiChanClient("" + element.Name, element.Value);

            var panel = new ClientPanel(client.Status);
//            panel.Location = new System.Drawing.Point(0, 0);

            BouyomiChanList.Add(client);
            layoutPanel.Controls.Add(panel);
            ClientPanelList.Add(panel);
        }
        Controls.Add(layoutPanel);
        timer.Tick += new EventHandler(UpdatePanels);

        string IpcServerName = settings.Element("IpcChannelName").Value;
        Server = new BouyomiChanServer(IpcServerName, BouyomiChanList);
        timer.Tick += new EventHandler(Server.SendText);

        timer.Interval = 100;//ms
        timer.Start();
    }

    private void UpdatePanels(object sender, EventArgs e)
    {
        Console.WriteLine("Update");
        var zip = ClientPanelList.Zip(Server.ClientList, (panel, client) => new { Panel = panel, Client = client });
        foreach (var item in zip)
        {
            item.Panel.Update(item.Client.Status);
        }
    }
}