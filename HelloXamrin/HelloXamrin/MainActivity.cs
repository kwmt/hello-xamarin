using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Com.Xamarin.Textcounter;
using Com.Igalata.Bubblepicker.Rendering;
using Com.Igalata.Bubblepicker.Adapter;
using System.Collections.Generic;
using Com.Igalata.Bubblepicker.Model;
using Com.Igalata.Bubblepicker;

namespace HelloXamrin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);


            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            BubblePicker picker = FindViewById<BubblePicker>(Resource.Id.picker);
            picker.BubbleSize = 20;
            picker.Adapter = new BubblePickerAdapter();
            picker.Listener = new BubblePickerListener(picker);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            int numVowels = TextCounter.NumVowels("mytext");
            int numConsonants = TextCounter.NumConsonants("mytext");


            Snackbar.Make(view, "numConsonants:" + numConsonants + "numVowels:" + numVowels, Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class BubblePickerAdapter : Java.Lang.Object, IBubblePickerAdapter
    {
        private List<string> _bubbles = new List<string>();
        public int TotalCount => _bubbles.Count;
        public BubblePickerAdapter()
        {
            for (int i = 0; i < 10; i++)
            {
                _bubbles.Add($"Item {i}");
            }
        }

        public PickerItem GetItem(int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= _bubbles.Count)
                return null;

            var result = _bubbles[itemIndex];
            var item = new PickerItem(result);
            return item;
        }
    }

    public class BubblePickerListener : Java.Lang.Object, IBubblePickerListener
    {
        public View Picker { get; }
        public BubblePickerListener(View picker)
        {
            Picker = picker;
        }

        public void OnBubbleDeselected(PickerItem item)
        {
            Snackbar.Make(Picker, $"Deselected: {item.Title}", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null)
                .Show();
        }

        public void OnBubbleSelected(PickerItem item)
        {
            Snackbar.Make(Picker, $"Selected: {item.Title}", Snackbar.LengthLong)
            .SetAction("Action", (Android.Views.View.IOnClickListener)null)
            .Show();
        }
    }
}
