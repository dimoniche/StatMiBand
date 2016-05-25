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
            data = ((App)Application.Current).XmlData;

            foreach (int year in data.GetYearsInData())
            {
                Years.Items.Add(year);
            }

            Years.SelectedIndex = 0;
        }
        
        private void OnStartDetail(object sender, RoutedEventArgs e)
        {
            double average = data.GetAverageSleep();
            double min = data.GetMinimumSleep();
            double max = data.GetMaxmumSleep();
            double total = data.GetTotalSleep();

            AverageSleep.Text = "Average sleep " + (int)(average) + " hours " + (int)(average * 60) % 60 + " minute.";
            MinimumSleep.Text = "Minimum sleep " + (int)(min) + " hours " + (int)(min * 60) % 60 + " minute.";
            MaximumSleep.Text = "Maximum sleep " + (int)(max) + " hours " + (int)(max * 60) % 60 + " minute.";
            TotalSleep.Text = "Total sleep " + (int)(total) + " hours " + (int)(total * 60) % 60 + " minute.";
        }
  
        private void Years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (ColumnChart.Series[0] as ColumnSeries).Title = "Sleeps";
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = data.GetSleep();

            (ColumnChart.Series[0] as ColumnSeries).DependentValuePath = "Amount";
            (ColumnChart.Series[0] as ColumnSeries).IndependentValuePath = "Time";

            var Axis = new LinearAxis()
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Interval = 30,
            };

            (ColumnChart.Series[0] as ColumnSeries).IndependentAxis = Axis;
        }
    }
}
