using JamaaTech.Smpp.Net.Client;
using JamaaTech.Smpp.Net.Lib.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Protocol;

namespace DemoClient
{
    class Program
    {
        static SmppClient client = new SmppClient();

        static void Main(string[] args)
        {
            SmppConnectionProperties properties = client.Properties;

            properties.SystemID = "MOF";
            properties.Password = "Mof@123";
            properties.Port = 5050;
            properties.Host = "10.150.6.134";
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
            msg.DestinationAddress = "93766608880";

            msg.SourceAddress = "0728861899";
            //msg.Text = "سلام";
            msg.Text = "hi there";
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
