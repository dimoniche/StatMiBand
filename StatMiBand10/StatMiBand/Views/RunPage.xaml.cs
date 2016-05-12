using System;
using StatMiBand.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.OneDrive.Sdk;
using System.Diagnostics;

namespace StatMiBand.Views
{
    public sealed partial class RunPage : Page
    {
        public RunPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

    }
}
