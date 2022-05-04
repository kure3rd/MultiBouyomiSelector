using System;
using System.Collections.Generic;   
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

public class Program
{
    [STAThread]
    public static void Main()
    {
        XElement settings;
        try
        {
            settings = XElement.Load("settings.xml");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Failed to load settings.xml");
            return;
        }

        IEnumerable<XElement> BouyomiChanLocations = from el in settings.Elements("BouyomiChanLocations").Elements() select el;
        var CheckedLocations = new List<XElement>();
        foreach (var e1 in BouyomiChanLocations)
        {
            foreach (var e2 in CheckedLocations)
            {
                if (e1.Name == e2.Name || e1.Value == e2.Value)
                {
                    MessageBox.Show(e2.ToString() + "\n" + e1.ToString(), "場所かチャンネル名が同じ設定になっています");
                    return;
                }
            }
            CheckedLocations.Add(e1);
        }

        Application.EnableVisualStyles();
        Application.Run(new DisplayForm(settings));
    }
}

