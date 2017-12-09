using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using JamaaTech.Smpp.Net.Client;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Protocol;

namespace SmsApi.Controllers
{
    public class HomeController : ApiController
    {
        private static SmppClient client;

        public HomeController()
        {
            client = new SmppClient();
        }

        public string Get(string number, string message)
        {
            SmppConnectionProperties properties = client.Properties;
            properties.SystemID = "MOF";
            properties.Password = "Mof@123";
            properties.Port = 5050;
            properties.Host = "10.150.6.134";
            properties.DefaultServiceType = ServiceType.DEFAULT;
            properties.DefaultEncoding = DataCoding.UCS2;
            properties.SourceAddress = "0728861899";

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            client.Start();

            client.ConnectionStateChanged += client_ConnectionStateChanged;

            while (client.ConnectionState != SmppConnectionState.Connected)
                Thread.Sleep(150);

            try
            {
                var msg = new TextMessage();

                msg.DestinationAddress = number;
                msg.SourceAddress = "0728861899";
                msg.Text = message;
                msg.RegisterDeliveryNotification = true;

                client.SendMessage(msg);

                return "Message sent";
            }
            catch (SmppClientException ex)
            {
                return "Message failed";
            }
            catch (Exception ex)
            {
                return "Message failed";
            }
        }

        void client_ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            Console.WriteLine(e.CurrentState.ToString());
        }
    }
}
