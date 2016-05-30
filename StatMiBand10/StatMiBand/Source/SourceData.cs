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
                System.IO.Stream stream = await this.OneDriveClient.Drive.Root.ItemWithPath("/Bind Mi Band/Activity.db").Content.Request().GetAsync();
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

        public List<GraphData> GetSleep(int year,int month)
        {
            List<GraphData> sleep = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year && day.Date.Month == month)
                {
                    sleep.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.SleepMinutes * 1000 / 60) / 1000.0 });
                }
            }

            return sleep;
        }

        public List<GraphData> GetSleep(int year, int month,int Day)
        {
            List<GraphData> sleep = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year && day.Date.Month == month && day.Date.Day == Day)
                {
                    sleep.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.SleepMinutes * 1000 / 60) / 1000.0 });
                }
            }

            return sleep;
        }

        public double GetAverageSleep()
        {
            return GetSleep().Where(a => a.Amount != 0).Average(a => a.Amount);
        }


        public double GetAverageSleep(int year,int month)
        {
            return GetSleep(year, month).Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumSleep(int year, int month)
        {
            return GetSleep(year, month).Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep(int year, int month)
        {
            return GetSleep(year, month).Max(a => a.Amount);
        }

        public double GetAverageSleep(int year)
        {
            return GetSleep(year).Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumSleep(int year)
        {
            return GetSleep(year).Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep(int year)
        {
            return GetSleep(year).Max(a => a.Amount);
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

        public double GetTotalSleep(int Year)
        {
            return GetSleep(Year).Sum(a => a.Amount);
        }

        public int GetTotalDays()
        {
            return data.Days.Count;
        }

        public double GetTotalSleep(int Year,int month, int day)
        {
            return GetSleep(Year,month,day).Sum(a => a.Amount);
        }

        public double GetTotalSleep(int Year, int month)
        {
            return GetSleep(Year, month).Sum(a => a.Amount);
        }

        public List<int> GetYearsInData()
        {
            List<int> years = new List<int>();
            int year = 0;

            if (data.Days == null) return years;

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

        public List<GraphData> GetStep()
        {
            List<GraphData> sleep = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                sleep.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.Steps * 1000) / 1000.0 });
            }

            return sleep;
        }

        public List<GraphData> GetStep(int year)
        {
            List<GraphData> step = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    step.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.Steps * 1000) / 1000.0 });
                }
            }

            return step;
        }

        public List<GraphData> GetStep(int year, int month)
        {
            List<GraphData> step = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year && day.Date.Month == month)
                {
                    step.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.Steps * 1000) / 1000.0 });
                }
            }

            return step;
        }

        public List<GraphData> GetStep(int year, int month, int Day)
        {
            List<GraphData> step = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year && day.Date.Month == month && day.Date.Day == Day)
                {
                    step.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.Steps * 1000) / 1000.0 });
                }
            }

            return step;
        }

        public double GetAverageStep()
        {
            return GetStep().Where(a => a.Amount != 0).Average(a => a.Amount);
        }


        public double GetAverageStep(int year, int month)
        {
            return GetStep(year, month).Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumStep(int year, int month)
        {
            return GetStep(year, month).Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep(int year, int month)
        {
            return GetStep(year, month).Max(a => a.Amount);
        }

        public double GetAverageStep(int year)
        {
            return GetStep(year).Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumStep(int year)
        {
            return GetStep(year).Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep(int year)
        {
            return GetStep(year).Max(a => a.Amount);
        }

        public double GetMinimumStep()
        {
            return GetStep().Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep()
        {
            return GetStep().Max(a => a.Amount);
        }

        public double GetTotalStep()
        {
            return GetStep().Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year)
        {
            return GetStep(Year).Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year, int month, int day)
        {
            return GetStep(Year, month, day).Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year, int month)
        {
            return GetStep(Year, month).Sum(a => a.Amount);
        }

    }

    public class GraphData
    {
        public int Time { get; set; }
        public double Amount { get; set; }
    }

    public class ChartData
    {
        public string DataName { get; set; }
        public int DataValue { get; set; }
    }
}
