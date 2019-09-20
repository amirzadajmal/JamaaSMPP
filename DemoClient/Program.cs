using JamaaTech.Smpp.Net.Client;
using System;
using System.Threading;
using JamaaTech.Smpp.Net.Lib;

namespace DemoClient
{
    class Program
    {
        static SmppClient client = new SmppClient();

        static void Main(string[] args)
        {
            SmppConnectionProperties properties = client.Properties;

            properties.SystemID = "XXX";
            properties.Password = "XXXXXXX";
            properties.Port = 5050;
            properties.Host = "XX.XXX.X.XXX";
            properties.SourceAddress = "0728861899";
            properties.DefaultEncoding = DataCoding.UCS2;

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = 15000;

            //Start smpp client
            client.Start();

            client.ConnectionStateChanged += Client_ConnectionStateChanged;
            client.MessageSent += Client_MessageSent;

            while (client.ConnectionState != SmppConnectionState.Connected)
                Thread.Sleep(100);

            var msg = new TextMessage();

            Console.Write("Please enter your phone number: ");
            msg.DestinationAddress = "XXXXXXXXXXXX";

            msg.SourceAddress = "XXXXXXXXXX";
            msg.Text = "سلام";
            msg.RegisterDeliveryNotification = true;

            client.SendMessage(msg);
            Console.ReadLine();
        }

        private static void Client_MessageSent(object sender, MessageEventArgs e)
        {
            Console.WriteLine("Message sent");
            Console.ReadLine();
        }

        private static void Client_ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            Console.WriteLine(e.CurrentState.ToString());
        }
    }
}
