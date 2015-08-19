using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Diagnostics;

namespace TileUpdater
{
    public sealed class Updater : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background task starting");

            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text03);

            var tileText = tileXml.GetElementsByTagName("text");
            (tileText[0] as XmlElement).InnerText = "Row 0";
            (tileText[1] as XmlElement).InnerText = "Row 1";
            (tileText[2] as XmlElement).InnerText = "Row 2";
            (tileText[3] as XmlElement).InnerText = "Row 3";
            var tileNotification = new TileNotification(tileXml);
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            tileUpdater.Update(tileNotification);
        }
    }
}
