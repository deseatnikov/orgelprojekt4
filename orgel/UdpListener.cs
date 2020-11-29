using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UdpListener
    {
        private readonly UdpClient udpClient;
        private readonly List<string> messages;

        public UdpListener(int port)
        {
            udpClient = new UdpClient(port);
            messages = new List<string>();
        }

        public async void StartListening()
        {
            while (true)
            {
                var result = await udpClient.ReceiveAsync();
                messages.Add(Encoding.ASCII.GetString(result.Buffer));
            }
        }

        public void HandleMessages()
        {
            foreach (string message in messages)
            {
                //TODO
            }
            messages.Clear();
        }
    }
}