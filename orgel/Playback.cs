using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace orgel
{
    class Playback
    {
        List<byte[]> SavedMessages = null;

        public Playback()
        {

        }

        public void Clear()
        {
            SavedMessages = null;
        }

        public void Set(byte[] message)
        {
            if (SavedMessages == null)
            {
                SavedMessages = new List<byte[]>
                {
                    message
                };
            }
            else
            {
                SavedMessages.Add(message);
            }
        }

        public async void ReplayWithDelay(int delayMilis)
        {
            if (SavedMessages != null)
            {
                await Task.Run(async () => {
                    foreach (byte[] item in SavedMessages)
                    {
                        Broadcaster.SendUdpMessage(Broadcaster.PORT, item);
                        await Task.Delay(delayMilis);
                    }
                });
            }
        }
    }
}