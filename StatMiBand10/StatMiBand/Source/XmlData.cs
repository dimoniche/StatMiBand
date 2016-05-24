using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StatMiBand.Source
{
    class JsonData
    {
        public int Version;
        public DateTime TimeStamp;

        public List<Days> Days;

        public static Exception InvalidVersion { get; private set; }

        public static JsonData FromJson(JObject objects)
        {
            JsonData data = new JsonData();

            foreach (var obj in objects)
            {
                if (obj.Key == "Version")
                {
                    int.TryParse(obj.Value.ToString(), out data.Version);
                    if (data.Version > 1) throw InvalidVersion;
                }
                else if(obj.Key == "TimeStamp")
                {
                    data.TimeStamp = DateTime.Parse(obj.Value.ToString());
                }
                else if (obj.Key == "Days")
                {
                    data.Days = new List<Days>();

                    foreach (JToken val in obj.Value)
                        data.Days.Add(Source.Days.FromJson(val));
                }
            }
            return data;
        }
    }

    class Days
    {
        public DateTime Date;
        public int Steps;
        public int StepsGoal;
        public int SleepMinutes;
        public int SleepGoalMinutes;
        public bool WasRunning;

        public static Days FromJson(JToken values)
        {
            Days days = new Days();

            foreach (JProperty val in values)
            {
                if (val.Name == "Date")
                {
                    days.Date = DateTime.Parse(val.Value.ToString());
                }
                else if (val.Name == "Steps")
                {
                    int.TryParse(val.Value.ToString(), out days.Steps);
                }
                else if (val.Name == "StepsGoal")
                {
                    int.TryParse(val.Value.ToString(), out days.StepsGoal);
                }
                else if (val.Name == "SleepMinutes")
                {
                    int.TryParse(val.Value.ToString(), out days.SleepMinutes);
                }
                else if (val.Name == "SleepGoalMinutes")
                {
                    int.TryParse(val.Value.ToString(), out days.SleepGoalMinutes);
                }
                else if (val.Name == "WasRunning")
                {
                    bool.TryParse(val.Value.ToString(), out days.WasRunning);
                }
            }

            return days;
        }
    }
}
