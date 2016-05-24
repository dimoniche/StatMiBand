using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatMiBand.Source
{
    public class SourceData
    {
        public IOneDriveClient OneDriveClient { get; set; }
        private readonly string[] scopes = new string[] { "onedrive.readonly", "wl.offline_access", "wl.signin" };

        String stringData;

        JsonData data;

        public SourceData()
        {
            InitializeClient();
            data = new JsonData();
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

                stringData = System.Text.Encoding.UTF8.GetString(buffer);

                ParseJsonData();
            }
            catch (Exception e)
            {
                if(e == JsonData.InvalidVersion)
                {

                }
            }
        }

        public void ParseJsonData()
        {
            data = JsonData.FromJson(JObject.Parse(stringData));
        }

        public List<int> GetSteps()
        {
            List<int> steps = new List<int>();

            foreach(Days day in data.Days)
            {
                steps.Add(day.Steps);
            }

            return steps;
        }

        public List<int> GetSleep()
        {
            List<int> sleep = new List<int>();

            foreach (Days day in data.Days)
            {
                sleep.Add(day.SleepMinutes);
            }

            return sleep;
        }
    }
}
