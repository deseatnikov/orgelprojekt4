using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace orgel
{
    public static class Broadcaster
    {
        public const int PORT = 21928;

        public async static void SendUdpMessage(int port, byte[] message)
        {
            using UdpClient client = new UdpClient
            {
                EnableBroadcast = true
            };

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Broadcast, port);

            await client.SendAsync(message, message.Length, endpoint);

            client.Close();
        }
    }
}