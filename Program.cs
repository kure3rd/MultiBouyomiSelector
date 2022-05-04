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
        XElement settings = XElement.Load("settings.xml");

        Application.EnableVisualStyles();
        Application.Run(new DisplayForm(settings));
    }
}

