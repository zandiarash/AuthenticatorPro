﻿using System;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AuthenticatorPro.Data;
using AuthenticatorPro.Shared.Data;
using Google.Android.Material.BottomSheet;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Java.Lang;
using OtpNet;
using TextInputLayout = Google.Android.Material.TextField.TextInputLayout;

namespace AuthenticatorPro.Fragment
{
    internal class RenameAuthenticatorBottomSheet : BottomSheetDialogFragment
    {
        public event EventHandler<RenameEventArgs> Rename;

        private readonly int _itemPosition;
        private readonly string _issuer;
        private readonly string _username;

        private TextInputLayout _issuerLayout;

        private TextInputEditText _issuerText;
        private TextInputEditText _usernameText;


        public RenameAuthenticatorBottomSheet(int itemPosition, string issuer, string username)
        {
            RetainInstance = true;
            _itemPosition = itemPosition;
            _issuer = issuer;
            _username = username;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.sheetRenameAuthenticator, null);

            _issuerLayout = view.FindViewById<TextInputLayout>(Resource.Id.editIssuerLayout);
            _issuerText = view.FindViewById<TextInputEditText>(Resource.Id.editIssuer);
            _usernameText = view.FindViewById<TextInputEditText>(Resource.Id.editUsername);

            _issuerText.Text = _issuer;
            _usernameText.Text = _username;

            var cancelButton = view.FindViewById<MaterialButton>(Resource.Id.buttonCancel);
            cancelButton.Click += (s, e) =>
            {
                Dismiss();
            };

            var renameButton = view.FindViewById<MaterialButton>(Resource.Id.buttonRename);
            renameButton.Click += (s, e) =>
            {
                var issuer = _issuerText.Text.Trim();
                if(issuer == "")
                {
                    _issuerLayout.Error = GetString(Resource.String.noIssuer);
                    return;
                }

                var args = new RenameEventArgs(_itemPosition, issuer, _usernameText.Text);
                Rename?.Invoke(this, args);
                Dismiss();
            };

            _usernameText.EditorAction += (sender, args) =>
            {
                if(args.ActionId == ImeAction.Done)
                    renameButton.PerformClick();
            };

            return view;
        }

        public class RenameEventArgs : EventArgs
        {
            public readonly int ItemPosition;
            public readonly string Issuer;
            public readonly string Username;

            public RenameEventArgs(int itemPosition, string issuer, string username)
            {
                ItemPosition = itemPosition;
                Issuer = issuer;
                Username = username;
            }
        }
    }
}