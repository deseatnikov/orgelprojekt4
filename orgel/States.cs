using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace orgel
{
    public class States : Fragment
    {
        public static States NewInstance()
        {
            States fragment = new States();
            /*Bundle args = new Bundle();
            fragment.Arguments = args;*/
            return fragment;
        }

        public States() { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.states, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            Construct();
        }

        // Pls dont judge, was in a hurry
        private void Construct()
        {
            ImageButton buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState21);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState22);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState23);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState24);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState25);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState26);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState11);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState12);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState13);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnState14);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnStatep1);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnStatep2);
            buttonb.Click += AnimateClick;

            buttonb = View.FindViewById<ImageButton>(Resource.Id.btnStatep3);
            buttonb.Click += AnimateClick;
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
                }
            });
        }
    }
}