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

        public List<GraphData> GetSteps()
        {
            List<GraphData> steps = new List<GraphData>();

            foreach(Days day in data.Days)
            {
                steps.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = day.Steps });
            }

            return steps;
        }

        public List<GraphData> GetSleep()
        {
            List<GraphData> sleep = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                sleep.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.SleepMinutes*1000/60)/1000.0 });
            }

            return sleep;
        }

        public List<GraphData> GetSleep(int year)
        {
            List<GraphData> sleep = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    sleep.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.SleepMinutes * 1000 / 60) / 1000.0 });
                }
            }

            return sleep;
        }

        public double GetAverageSleep()
        {
            double average = 0;
            int count = 0;
            List<GraphData> sleeps = GetSleep();

            foreach (GraphData sleep in sleeps)
            {
                average += sleep.Amount;
                count++;
            }

            return average/count;
        }

        public double GetMinimumSleep()
        {
            return GetSleep().Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep()
        {
            return GetSleep().Max(a => a.Amount);
        }

        public double GetTotalSleep()
        {
            return GetSleep().Sum(a => a.Amount);
        }

        public List<int> GetYearsInData()
        {
            List<int> years = new List<int>();
            int year = 0;

            foreach (Days day in data.Days)
            {
                if (day.Date.Year != year)
                {
                    years.Add(day.Date.Year);
                    year = day.Date.Year;
                }
            }
            return years;
        }
    }

    public class GraphData
    {
        public int Time { get; set; }
        public double Amount { get; set; }
    }
}
