using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StatMiBand
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly string[] scopes = new string[] { "onedrive.readonly", "wl.offline_access", "wl.signin" };
 

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += AccountSelection_Loaded;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.InitializeClient(e);
        }
 
        private async void AccountSelection_Loaded(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).OneDriveClient != null)
            {
                await ((App)Application.Current).OneDriveClient.SignOutAsync();

                var client = ((App)Application.Current).OneDriveClient as OneDriveClient;
                if (client != null)
                {
                    client.Dispose();
                }

                ((App)Application.Current).OneDriveClient = null;
            }
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

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
