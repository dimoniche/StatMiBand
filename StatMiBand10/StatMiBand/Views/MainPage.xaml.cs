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
    public sealed partial class MainPage : Page
    {
        private readonly string[] scopes = new string[] { "onedrive.readonly", "wl.offline_access", "wl.signin" };

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private async void InitializeClient(RoutedEventArgs e)
        {
            if (((App)Application.Current).OneDriveClient == null)
            {
                var client = OneDriveClientExtensions.GetUniversalClient(this.scopes) as OneDriveClient;

                try
                {
                    await client.AuthenticateAsync();
                    ((App)Application.Current).OneDriveClient = client;
                    Frame.Navigate(typeof(MainPage), e);
                }
                catch (OneDriveException exception)
                {
                    // Swallow the auth exception but write message for debugging.
                    Debug.WriteLine(exception.Error.Message);
                    client.Dispose();
                }
            }
            else
            {
                Frame.Navigate(typeof(MainPage), e);
            }
        }

    }
}
