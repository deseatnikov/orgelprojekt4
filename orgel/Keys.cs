using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace orgel
{
    public class Keys : Android.Support.V4.App.Fragment
    {
        readonly Keyboard keyboard = new Keyboard();
        readonly int[] keyboardDisplayRange = { 1, 11 };

        readonly Playback playback = new Playback();

        bool setting = false;
        TextView keyboardLabel;
        List<ImageButton> imageButtons;

        public static Keys NewInstance()
        {
            Keys fragment = new Keys();
            return fragment;
        }

        public Keys()
        {
        }

        private void Construct()
        {
            imageButtons = CreateImagebuttonsFromRange();
            ImageButton left = View.FindViewById<ImageButton>(Resource.Id.btnLeft);
            left.Click += OnKeyboardToLeft;
            left.Click += AnimateClick;
            ImageButton right = View.FindViewById<ImageButton>(Resource.Id.btnRight);
            right.Click += OnKeyboardToRight;
            right.Click += AnimateClick;

            ImageButton buttons = View.FindViewById<ImageButton>(Resource.Id.btnS);
            buttons.Click += AnimateClick;
            buttons.Click += FlipSetMode;
            ImageButton buttonb = View.FindViewById<ImageButton>(Resource.Id.btnB);
            buttonb.Click += AnimateClick;
            buttonb.Click += PlayBack;
            ImageButton buttono = View.FindViewById<ImageButton>(Resource.Id.btnO);
            buttono.Click += AnimateClick;
            buttono.Click += ClearPlayback;

            keyboardLabel = View.FindViewById<TextView>(Resource.Id.keysLabel);

            this.imageButtons = CreateImagebuttonsFromRange();
            SetKeyboardLabel();
            DrawKeys();
        }

        private void FlipSetMode(object sender, EventArgs e)
        {
            setting = !setting;
        }

        private void PlayBack(object sender, EventArgs e)
        {
            setting = false;
            playback.ReplayWithDelay(1000);
        }

        private void ClearPlayback(object sender, EventArgs e)
        {
            setting = false;
            playback.Clear();
        }

        private async void AnimateClick(object sender, EventArgs e)
        {
            await Task.Run(async () => {
                if (sender is ImageButton)
                {
                    (sender as ImageButton).ScaleX = 0.75f;
                    (sender as ImageButton).ScaleY = 0.75f;
                    await Task.Delay(100);
                    (sender as ImageButton).ScaleX = 1;
                    (sender as ImageButton).ScaleY = 1;
                } });
        }

        private void OnKeyboardToRight(object sender, EventArgs e)
        {
            if (keyboardDisplayRange[1] != 61)
            {
                keyboardDisplayRange[0] += 11;
                if (keyboardDisplayRange[0] == 56)
                {
                    keyboardDisplayRange[1] = 61;
                }
                else
                {
                    keyboardDisplayRange[1] = keyboardDisplayRange[0] + 10;
                }
                this.imageButtons = CreateImagebuttonsFromRange();
                SetKeyboardLabel();
                DrawKeys();
            }
        }

        private void OnKeyboardToLeft(object sender, EventArgs e)
        {
            if (keyboardDisplayRange[0] != 1)
            {
                keyboardDisplayRange[0] -= 11;
                keyboardDisplayRange[1] = keyboardDisplayRange[0] + 10;
                this.imageButtons = CreateImagebuttonsFromRange();
                SetKeyboardLabel();
                DrawKeys();
            }
        }

        private void SetKeyboardLabel()
        {
            keyboardLabel.Text = "" + keyboardDisplayRange[0] + " - " + keyboardDisplayRange[1];
        }

        private List<ImageButton> CreateImagebuttonsFromRange()
        {
            List<ImageButton> imageButtons = new List<ImageButton>();

            ImageButton img;
            for (int i = keyboardDisplayRange[0]; i <= keyboardDisplayRange[1]; i++)
            {
                img = new ImageButton(this.Context);
                img.SetImageResource(keyboard.GetKeyImageByNum(i));
                img.RotationY = keyboard.GetKeyFlippedByNum(i) ? 180 : 0;
                img.Touch += Img_Touch;
                //img.Click += AnimateClick;
                img.SetAdjustViewBounds(true);
                img.SetBackgroundColor(Android.Graphics.Color.Transparent);
                imageButtons.Add(img);
            }

            return imageButtons;
        }

        private void Img_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
            {
                if (sender is ImageButton)
                {
                    (sender as ImageButton).ScaleX = 0.75f;
                    (sender as ImageButton).ScaleY = 0.75f;
                    int num = imageButtons.IndexOf(imageButtons.Where(x => x == (sender as ImageButton)).First());
                    num += keyboardDisplayRange[0];
                    byte[] message = keyboard.GetNoteOnMessageByNum(num);
                    if (setting)
                    {
                        playback.Set(message);
                    }
                    else
                    {
                        Broadcaster.SendUdpMessage(Broadcaster.PORT, message);
                    }
                }
            }
            if (e.Event.Action == MotionEventActions.Up)
            {
                if (sender is ImageButton)
                {
                    (sender as ImageButton).ScaleX = 1;
                    (sender as ImageButton).ScaleY = 1;
                    int num = imageButtons.IndexOf(imageButtons.Where(x => x == (sender as ImageButton)).First());
                    num += keyboardDisplayRange[0];
                    byte[] message = keyboard.GetNoteOffMessageByNum(num);
                    if (setting)
                    {
                        playback.Set(message);
                    }
                    else
                    {
                        Broadcaster.SendUdpMessage(Broadcaster.PORT, message);
                    }
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //android tint on button
            View view = inflater.Inflate(Resource.Layout.keys, container, false);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            Construct();
        }

        private void DrawKeys()
        {
            RelativeLayout keyLayout = View.FindViewById<RelativeLayout>(Resource.Id.keyPad);
            keyLayout.RemoveAllViews();

            int defaultWidth = keyLayout.Width / CountLowers();

            RelativeLayout.LayoutParams layoutParams;

            int pos = 0;
            int id = 0;
            for (int i = keyboardDisplayRange[0]; i <= keyboardDisplayRange[1]; i++)
            {
                layoutParams = new RelativeLayout.LayoutParams(keyLayout.LayoutParameters);
                if (keyboard.GetKeyImageByNum(i).Equals(Resource.Drawable.keyup))
                {
                    layoutParams.Width = defaultWidth / 2;
                    layoutParams.Height = keyLayout.Height - keyLayout.Height / 4;
                    layoutParams.LeftMargin = pos * defaultWidth - defaultWidth / 4;
                }
                else
                {
                    layoutParams.Width = defaultWidth;
                    layoutParams.Height = keyLayout.Height;
                    layoutParams.LeftMargin = pos++ * defaultWidth;
                }
                keyLayout.AddView(imageButtons[id++], layoutParams);
            }
        }

        private int CountLowers()
        {
            int num = 0;
            for (int i = keyboardDisplayRange[0]; i <= keyboardDisplayRange[1]; i++)
            {
                if (!keyboard.GetKeyImageByNum(i).Equals(Resource.Drawable.keyup)) num++;
            }
            return num;
        }
    }
}