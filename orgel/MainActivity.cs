using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Timers;

namespace orgel
{
    [Activity(Label = "@string/app_name", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.FullUser)]
    public class MainActivity : AppCompatActivity
    {
        ViewPager viewpager;
        readonly UdpListener listener = new UdpListener(Broadcaster.PORT);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            viewpager = FindViewById<ViewPager>(Resource.Id.viewpager);
            viewpager.Adapter = new ViewPageAdapter(SupportFragmentManager,
                new Android.Support.V4.App.Fragment[] {
                    Home.NewInstance(),
                    Keys.NewInstance(),
                    States.NewInstance()
                });

            listener.StartListening();
            Timer timer = new Timer { AutoReset = true, Interval=TimeSpan.FromSeconds(1).TotalMilliseconds };
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            listener.HandleMessages();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

