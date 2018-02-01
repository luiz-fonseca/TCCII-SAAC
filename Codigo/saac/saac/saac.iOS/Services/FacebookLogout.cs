using Foundation;
using saac.Interfaces;
using saac.iOS.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookLogout))]
namespace saac.iOS.Services
{
    public class FacebookLogout : IFacebookLogout
    {
        public void Logout()
        {
            foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
            {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }
        }
    }
}
