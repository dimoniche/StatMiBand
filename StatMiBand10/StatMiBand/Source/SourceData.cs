using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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

        public List<GraphData> GetAverageSleeps(int year)
        {
            List<GraphData> val = new List<GraphData>();
            double Val = 0.0;
            double averageVal = 0.0;
            int count = 1;

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    averageVal += day.SleepMinutes;
                    Val = averageVal / count;
                    count++;
                    val.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (Val * 1000 / 60) / 1000.0 });
                }
            }

            return val;
        }

        public List<GraphData> GetAverageSteps(int year)
        {
            List<GraphData> val = new List<GraphData>();
            double Val = 0.0;
            double averageVal = 0.0;
            int count = 1;

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    averageVal += day.Steps;
                    Val = averageVal / count;
                    count++;
                    val.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (Val * 1000) / 1000.0 });
                }
            }

            return val;
        }

        public double GetGoalSleep(int year)
        {
            int val = 0;
            int count = 0;

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    val += day.SleepGoalMinutes;
                    count++;
                }
            }

            return (val * 1000 / count / 60)/1000.0;
        }

        public double GetGoalStep(int year)
        {
            int val = 0;
            int count = 0;

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    val += day.StepsGoal;
                    count++;
                }
            }

            return (val * 1000 / count) / 1000.0;
        }

        internal IEnumerable GetGoalSleeps(int year)
        {
            List<GraphData> val = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    val.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.SleepGoalMinutes * 1000 / 60) / 1000.0 });
                }
            }

            return val;
        }

        internal IEnumerable GetGoalSteps(int year)
        {
            List<GraphData> val = new List<GraphData>();

            foreach (Days day in data.Days)
            {
                if (day.Date.Year == year)
                {
                    val.Add(new GraphData() { Time = day.Date.DayOfYear, Amount = (day.StepsGoal * 1000) / 1000.0 });
                }
            }

            return val;
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
            List<GraphData> val = GetSleep();

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }


        public double GetAverageSleep(int year,int month)
        {
            List<GraphData> val = GetSleep(year, month);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumSleep(int year, int month)
        {
            List<GraphData> val = GetSleep(year, month);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep(int year, int month)
        {
            List<GraphData> val = GetSleep(year, month);

            if (val.Count < 1) return 0;

            return val.Max(a => a.Amount);
        }

        public double GetAverageSleep(int year)
        {
            List<GraphData> val = GetSleep(year);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumSleep(int year)
        {
            List<GraphData> val = GetSleep(year);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep(int year)
        {
            List<GraphData> val = GetSleep(year);

            if (val.Count < 1) return 0;

            return GetSleep(year).Max(a => a.Amount);
        }

        public double GetMinimumSleep()
        {
            List<GraphData> val = GetSleep();

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumSleep()
        {
            List<GraphData> val = GetSleep();

            if (val.Count < 1) return 0;

            return val.Max(a => a.Amount);
        }

        public double GetTotalSleep()
        {
            List<GraphData> val = GetSleep();

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
        }

        public double GetTotalSleep(int Year)
        {
            List<GraphData> val = GetSleep();

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
        }

        public int GetTotalDays()
        {
            return data.Days.Count;
        }

        public double GetTotalSleep(int Year,int month, int day)
        {
            List<GraphData> val = GetSleep(Year, month,day);

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
        }

        public double GetTotalSleep(int Year, int month)
        {
            List<GraphData> val = GetSleep(Year, month);

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
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
            List<GraphData> val = GetStep();

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }


        public double GetAverageStep(int year, int month)
        {
            List<GraphData> val = GetStep(year, month);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumStep(int year, int month)
        {
            List<GraphData> val = GetStep(year, month);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep(int year, int month)
        {
            List<GraphData> val = GetStep(year, month);

            if (val.Count < 1) return 0;

            return val.Max(a => a.Amount);
        }

        public double GetAverageStep(int year)
        {
            List<GraphData> val = GetStep(year);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Average(a => a.Amount);
        }

        public double GetMinimumStep(int year)
        {
            List<GraphData> val = GetStep(year);

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep(int year)
        {
            List<GraphData> val = GetStep(year);

            if (val.Count < 1) return 0;

            return val.Max(a => a.Amount);
        }

        public double GetMinimumStep()
        {
            List<GraphData> val = GetStep();

            if (val.Count < 1) return 0;

            return val.Where(a => a.Amount != 0).Min(a => a.Amount);
        }

        public double GetMaxmumStep()
        {
            List<GraphData> val = GetStep();

            if (val.Count < 1) return 0;

            return val.Max(a => a.Amount);
        }

        public double GetTotalStep()
        {
            List<GraphData> val = GetStep();

            if (val.Count < 1) return 0;

            return GetStep().Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year)
        {
            List<GraphData> val = GetStep();

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year, int month, int day)
        {
            List<GraphData> val = GetStep(Year, month, day);

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
        }

        public double GetTotalStep(int Year, int month)
        {
            List<GraphData> val = GetStep(Year, month);

            if (val.Count < 1) return 0;

            return val.Sum(a => a.Amount);
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
