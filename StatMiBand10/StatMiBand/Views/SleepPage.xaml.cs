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
using System.Collections.Generic;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace StatMiBand.Views
{
    public sealed partial class SleepPage : Page
    {
        SourceData data;

        public SleepPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            if(data == null)
            {
                data = ((App)Application.Current).XmlData;
            }

            if (Years.SelectedItem == null)
            {
                Years.Items.Clear();

                foreach (int year in data.GetYearsInData())
                {
                    Years.Items.Add(year);
                }

                Years.SelectedIndex = 0;
            }

            double average = data.GetAverageSleep();
            double min = data.GetMinimumSleep();
            double max = data.GetMaxmumSleep();
            double total = data.GetTotalSleep();

            Average.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            Minimum.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            Maximum.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";
            Total.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
        }

        private void OnStartDetail(object sender, RoutedEventArgs e)
        {
            if (data == null)
            {
                data = ((App)Application.Current).XmlData;
            }

            double average = data.GetAverageSleep();
            double min = data.GetMinimumSleep();
            double max = data.GetMaxmumSleep();
            double total = data.GetTotalSleep();

            AverageSleep.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleep.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleep.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";
            TotalSleep.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";

            //
            List<ChartData> TotalChartInfo = new List<ChartData>();

            TotalChartInfo.Add(new ChartData() { DataName = "Sleeping", DataValue = (int)data.GetTotalSleep() });
            TotalChartInfo.Add(new ChartData() { DataName = "Waking", DataValue = data.GetTotalDays() * 24 - (int)data.GetTotalSleep() });

            (TotalPieChart.Series[0] as PieSeries).ItemsSource = TotalChartInfo;

            average = data.GetAverageSleep();
            min = data.GetMinimumSleep();
            max = data.GetMaxmumSleep();

            AverageSleepDay.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";

            total = data.GetTotalSleep(DateTime.Now.Year, DateTime.Now.Month);
            average = data.GetAverageSleep(DateTime.Now.Year, DateTime.Now.Month);
            min = data.GetMinimumSleep(DateTime.Now.Year, DateTime.Now.Month);
            max = data.GetMaxmumSleep(DateTime.Now.Year, DateTime.Now.Month);

            TotalSleepMonth.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
            AverageSleepMonth.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleepMonth.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleepMonth.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";

            total = data.GetTotalSleep(DateTime.Now.Year);
            average = data.GetAverageSleep(DateTime.Now.Year);
            min = data.GetMinimumSleep(DateTime.Now.Year);
            max = data.GetMaxmumSleep(DateTime.Now.Year);

            TotalSleepYear.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
            AverageSleepYear.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleepYear.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleepYear.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";

            CalendarDay.Date = DateTime.Now;
            CalendarMonth.Date = DateTime.Now;
            CalendarYear.Date = DateTime.Now;
        }

        private void Years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Years.SelectedItem == null)
            {
                Years.Items.Clear();

                foreach (int year in data.GetYearsInData())
                {
                    Years.Items.Add(year);
                }

                Years.SelectedIndex = 0;
            }

            (ColumnChart.Series[0] as ColumnSeries).Title = "Sleeps";
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = data.GetSleep((int)Years.SelectedItem);

            (ColumnChart.Series[0] as ColumnSeries).DependentValuePath = "Amount";
            (ColumnChart.Series[0] as ColumnSeries).IndependentValuePath = "Time";

            var Axis = new LinearAxis()
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Interval = 30,
            };

            (ColumnChart.Series[0] as ColumnSeries).IndependentAxis = Axis;

            //
            List<ChartData> ChartInfo = new List<ChartData>();

            ChartInfo.Add(new ChartData() { DataName = "Sleeping", DataValue = (int)data.GetTotalSleep((int)Years.SelectedItem) });
            ChartInfo.Add(new ChartData() { DataName = "Waking up", DataValue = data.GetTotalDays()*24 - (int)data.GetTotalSleep((int)Years.SelectedItem) });

            (PieChart.Series[0] as PieSeries).ItemsSource = ChartInfo;

        }

        private void CalendarDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalSleep(CalendarDay.Date.Value.Year, CalendarDay.Date.Value.Month, CalendarDay.Date.Value.Day);

            AverageSleepDay.Text = "Average sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
        }

        private void MonthChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalSleep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double average = data.GetAverageSleep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double min = data.GetMinimumSleep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double max = data.GetMaxmumSleep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);

            TotalSleepMonth.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
            AverageSleepMonth.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleepMonth.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleepMonth.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";
        }

        private void YearChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalSleep(CalendarYear.Date.Value.Year);
            double average = data.GetAverageSleep(CalendarYear.Date.Value.Year);
            double min = data.GetMinimumSleep(CalendarYear.Date.Value.Year);
            double max = data.GetMaxmumSleep(CalendarYear.Date.Value.Year);

            TotalSleepYear.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
            AverageSleepYear.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleepYear.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleepYear.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";
        }
    }
}
