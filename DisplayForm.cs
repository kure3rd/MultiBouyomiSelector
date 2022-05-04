using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Drawing;

class DisplayLabel : Label
{
    public DisplayLabel() : base()
    {
        //Anchor = AnchorStyles.Bottom;
        Margin = new Padding(2);
        TextAlign = ContentAlignment.MiddleLeft;
    }
}
class PropertyDisplaylabel : DisplayLabel
{
    public PropertyDisplaylabel(string name) : base()
    {
        Name = name;
        Text = name;
        Anchor = AnchorStyles.Right;
        TextAlign = ContentAlignment.MiddleRight;
    }
}
class StaticDisplayLabel : DisplayLabel
{
    public StaticDisplayLabel(string name) : base()
    {
        Name = name;
        Text = name;
        Anchor = AnchorStyles.Left;
        AutoEllipsis = true;
        AutoSize = true;
    }
}
class EnumDisplayLabel : DisplayLabel
{
    public EnumDisplayLabel(Enum state) : base()
    {
        if (state is not null)
        {
            Name = state.GetType().Name;
            Text = state.ToString();
        }
    }
    public void Update(Enum state)
    {
        if (state is not null)
        {
            Text = state.ToString();
        }
        else
        {
            Text = "";
        }
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

    public ClientPanel(BouyomiChanStatus status) : base()
    {
        Name = status.IpcChannelName;
        ColumnCount = 3;
        RowCount = 3;
        AutoSize = true;
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
        ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));

        Controls.Add(new PropertyDisplaylabel("Path:"), 0, 0);
        Controls.Add(new PropertyDisplaylabel("Channel:"), 0, 1);
        Controls.Add(new PropertyDisplaylabel("LastTalk:"), 0, 2);
        DirectionLocateLabel = new StaticDisplayLabel(System.IO.Path.GetFileName(status.DirectoryLocation));
        Controls.Add(DirectionLocateLabel, 1, 0);
        IpcChannelLabel = new StaticDisplayLabel(status.IpcChannelName);
        Controls.Add(IpcChannelLabel, 1, 1);
        LastTalkLabel = new StaticDisplayLabel(status.LastTalkText);
        Controls.Add(LastTalkLabel, 1, 2);
        ProcessLabel = new EnumDisplayLabel(status.ProcessState);
        Controls.Add(ProcessLabel, 2, 0);
        ConnectionLabel = new EnumDisplayLabel(status.isConnected);
        Controls.Add(ConnectionLabel, 2, 1);
        BusyLabel = new EnumDisplayLabel(status.isBusy);
        Controls.Add(BusyLabel, 2, 2);
    }

    public void Update(BouyomiChanStatus status)
    {
        LastTalkLabel.Text = status.LastTalkText;
        ProcessLabel.Update(status.ProcessState);
        ConnectionLabel.Update(status.isConnected);
        BusyLabel.Update(status.isBusy);
    }
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Red, ButtonBorderStyle.Solid);
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
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;

        timer = new Timer();
        ClientPanelList = new List<ClientPanel>();
        layoutPanel = new FlowLayoutPanel();
        layoutPanel.FlowDirection = FlowDirection.TopDown;
        layoutPanel.AutoSize = true;
        layoutPanel.Margin = new Padding(10);

        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var BouyomiChanList = new Queue<BouyomiChanClient>();
        foreach (XElement element in BouyomiChanLocations) {
            var client = new BouyomiChanClient("" + element.Name, element.Value);

            var panel = new ClientPanel(client.Status);
//            panel.Location = new System.Drawing.Point(0, 0);

            BouyomiChanList.Enqueue(client);
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
        var zip = ClientPanelList.Zip(Server.ClientQueue, (panel, client) => new { Panel = panel, Client = client });
        foreach (var item in zip)
        {
            item.Panel.Update(item.Client.Status);
        }
    }
}