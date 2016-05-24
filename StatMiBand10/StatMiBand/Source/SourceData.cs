using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatMiBand.Source
{
    public class SourceData
    {
        public IOneDriveClient OneDriveClient { get; set; }
        private readonly string[] scopes = new string[] { "onedrive.readonly", "wl.offline_access", "wl.signin" };

        String xmlData;

        public SourceData()
        {
            InitializeClient();
        }

        private async void InitializeClient()
        {
            if (this.OneDriveClient == null)
            {
                var client = OneDriveClientExtensions.GetUniversalClient(this.scopes) as OneDriveClient;

                try
                {
                    await client.AuthenticateAsync();
                    this.OneDriveClient = client;
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

            }
        }

        public async Task ReadXmlData()
        {
            try
            {
                System.IO.Stream stream = await this.OneDriveClient.Drive.Root.ItemWithPath("/debug/Activity.db").Content.Request().GetAsync();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);

                xmlData = System.Text.Encoding.UTF8.GetString(buffer);
            }
            catch (Exception e)
            {

            }
        }

    }
}
