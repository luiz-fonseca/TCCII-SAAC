using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using saac.Droid.Services;
using saac.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookLogout))]
namespace saac.Droid.Services
{
    public class FacebookLogout : IFacebookLogout
    {
        public void Logout()
        {
            CookieManager.Instance.RemoveAllCookie();
        }
    }
}