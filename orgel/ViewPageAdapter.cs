using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;

namespace orgel
{
    public class ViewPageAdapter : FragmentPagerAdapter
    {
        readonly Android.Support.V4.App.Fragment[] fragments;

        public ViewPageAdapter(Android.Support.V4.App.FragmentManager manager, Android.Support.V4.App.Fragment[] fragments) : base(manager)
        {
            this.fragments = fragments;
        }

        public override int Count => fragments.Length;

        public override Android.Support.V4.App.Fragment GetItem(int position) => fragments[position];
    }
}