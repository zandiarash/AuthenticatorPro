﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AuthenticatorPro.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace AuthenticatorPro.Activity
{
    [Activity]
    internal class SettingsActivity : DayNightActivity
    {
        private ProgressBar _progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activitySettings);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetTitle(Resource.String.settings);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowHomeEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_action_arrow_back);

            _progressBar = FindViewById<ProgressBar>(Resource.Id.appBarProgressBar);

            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.layoutFragment, new SettingsFragment())
                .Commit();
        }

        public async void SetDatabaseEncryption(bool isEncrypted)
        {
            _progressBar.Visibility = ViewStates.Visible;
            Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);

            await Database.UpdateEncryption(this, isEncrypted);

            Window.ClearFlags(WindowManagerFlags.NotTouchable);
            _progressBar.Visibility = ViewStates.Gone;
        }

        public override bool OnSupportNavigateUp()
        {
            Finish();
            return base.OnSupportNavigateUp();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if(item.ItemId == Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            Finish();
            base.OnBackPressed();
        }
    }
}