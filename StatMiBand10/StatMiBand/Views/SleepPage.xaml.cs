using System;
using StatMiBand.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.OneDrive.Sdk;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using StatMiBand.Source;

namespace StatMiBand.Views
{
    public sealed partial class SleepPage : Page
    {

        public SleepPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SourceData data = ((App)Application.Current).XmlData;
        }


    }
}
