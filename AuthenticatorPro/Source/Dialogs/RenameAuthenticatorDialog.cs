﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AuthenticatorPro.Data;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;

namespace AuthenticatorPro.Dialogs
{
    internal class RenameAuthenticatorDialog : DialogFragment
    {
        private readonly Action<object, EventArgs> _negativeButtonEvent;
        private readonly Action<object, EventArgs> _positiveButtonEvent;

        private EditText _issuerText;
        private EditText _usernameText;

        public RenameAuthenticatorDialog(Action<object, EventArgs> positive, Action<object, EventArgs> negative,
            Authenticator auth, int position)
        {
            RetainInstance = true;

            _positiveButtonEvent = positive;
            _negativeButtonEvent = negative;
            Authenticator = auth;
            Position = position;
        }

        public string Issuer => _issuerText.Text;

        public string Username => _usernameText.Text;

        public string IssuerError {
            set => _issuerText.Error = value;
        }

        public Authenticator Authenticator { get; }
        public int Position { get; }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var alert = new AlertDialog.Builder(Activity);
            alert.SetTitle(Resource.String.rename);

            alert.SetPositiveButton(Resource.String.rename, (EventHandler<DialogClickEventArgs>) null);
            alert.SetNegativeButton(Resource.String.cancel, (EventHandler<DialogClickEventArgs>) null);
            alert.SetCancelable(false);

            var view = Activity.LayoutInflater.Inflate(Resource.Layout.dialogRenameAuthenticator, null);
            _issuerText = view.FindViewById<EditText>(Resource.Id.dialogRenameAuthenticator_issuer);
            _usernameText = view.FindViewById<EditText>(Resource.Id.dialogRenameAuthenticator_username);
            alert.SetView(view);

            var dialog = alert.Create();
            dialog.Show();
            dialog.Window.SetSoftInputMode(SoftInput.StateAlwaysVisible);

            _issuerText.Text = Authenticator.Issuer;
            _usernameText.Text = Authenticator.Username;

            var renameButton = dialog.GetButton((int) DialogButtonType.Positive);
            var cancelButton = dialog.GetButton((int) DialogButtonType.Negative);

            renameButton.Click += _positiveButtonEvent.Invoke;
            cancelButton.Click += _negativeButtonEvent.Invoke;

            return dialog;
        }
    }
}