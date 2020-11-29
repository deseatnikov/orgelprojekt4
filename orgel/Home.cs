using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace orgel
{
    public class Home : Fragment
    {
        public static Home NewInstance()
        {
            Home fragment = new Home();
            return fragment;
        }

        public Home() {}

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.home, container, false);
            return view;
        }
    }
}