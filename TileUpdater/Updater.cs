using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Diagnostics;
using System.Threading;

namespace TileUpdater
{
    public sealed class Updater : IBackgroundTask
    {
        private XmlDocument tileXml;
        private XmlElement tileText;
        private Bus[] buses;

        public struct Bus
        {
            public string name;
            public int time;

            public Bus(string name, int time) {
                this.name = name;
                this.time = time;
            }
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background task starting");

            tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text04);

            tileText = tileXml.GetElementsByTagName("text")[0] as XmlElement;

            buses = new Bus[]
            {
                new Bus("99", 3),
                new Bus("88", 7),
                new Bus("6", 16),
                new Bus("47", 21),
            };

            new Timer(onTimer, new object(), 0, 1000);
        }

        private void onTimer(object state)
        {
            tileText.InnerText = "";

            foreach (Bus bus in buses)
            {
                tileText.InnerText += String.Format("%s: %02d", bus.name, bus.time);
                //if (bus.time > 0) bus.time--;
            }
            var tileNotification = new TileNotification(tileXml);
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Update(tileNotification);
        }
    }
}
