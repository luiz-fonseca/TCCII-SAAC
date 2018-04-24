using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace saac.Controls
{
    public class AdControlView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
            nameof(AdUnitId),
            typeof(string),
            typeof(AdControlView),
            string.Empty);
        
        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);

        }
    }
}
