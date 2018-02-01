using saac.Interfaces;
using saac.UWP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookLogout))]
namespace saac.UWP.Services
{
    public class FacebookLogout : IFacebookLogout
    {
        public void Logout()
        {
            HttpBaseProtocolFilter filter = new HttpBaseProtocolFilter();
            HttpCookieManager cookieManager = filter.CookieManager;
            foreach (HttpCookie cookie in cookieManager.GetCookies(new Uri("https://www.facebook.com")))
            {
                cookieManager.DeleteCookie(cookie);
            }
        }
    }
}
