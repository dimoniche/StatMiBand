using System;
using StatMiBand.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Microsoft.OneDrive.Sdk;
using System.Diagnostics;
using StatMiBand.Source;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using System.Collections.Generic;

namespace StatMiBand.Views
{
    public sealed partial class StepPage : Page
    {
        SourceData data;

        public StepPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            if (data == null)
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

            double average = data.GetAverageStep();
            double min = data.GetMinimumStep();
            double max = data.GetMaxmumStep();
            double total = data.GetTotalStep();

            Average.Text = "Average step " + (int)(average) + " km ";
            Minimum.Text = "Minimum step " + (int)(min) + " km ";
            Maximum.Text = "Maximum step " + (int)(max) + " km ";
            Total.Text = "Total step " + (int)(total) + " km ";
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

            (ColumnChart.Series[0] as ColumnSeries).Title = "Steps";
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = data.GetStep((int)Years.SelectedItem);

            (ColumnChart.Series[0] as ColumnSeries).DependentValuePath = "Amount";
            (ColumnChart.Series[0] as ColumnSeries).IndependentValuePath = "Time";

            var Axis = new LinearAxis()
            {
                Orientation = AxisOrientation.X,
                Location = AxisLocation.Bottom,
                Interval = 30,
            };

            (ColumnChart.Series[0] as ColumnSeries).IndependentAxis = Axis;

            (ColumnChart.Series[0] as ColumnSeries).IndependentAxis = Axis;

            ColumnChart.Series.Add(new LineSeries());

            (ColumnChart.Series[1] as LineSeries).Title = "Average Steps";
            (ColumnChart.Series[1] as LineSeries).ItemsSource = data.GetAverageSteps((int)Years.SelectedItem);

            (ColumnChart.Series[1] as LineSeries).DependentValuePath = "Amount";
            (ColumnChart.Series[1] as LineSeries).IndependentValuePath = "Time";

            ColumnChart.Series.Add(new LineSeries());

            (ColumnChart.Series[2] as LineSeries).Title = "Goal";
            (ColumnChart.Series[2] as LineSeries).ItemsSource = data.GetGoalSteps((int)Years.SelectedItem);

            (ColumnChart.Series[2] as LineSeries).DependentValuePath = "Amount";
            (ColumnChart.Series[2] as LineSeries).IndependentValuePath = "Time";

            //
            //List<ChartData> ChartInfo = new List<ChartData>();

            //ChartInfo.Add(new ChartData() { DataName = "Steps", DataValue = (int)data.GetTotalStep((int)Years.SelectedItem) });
            //ChartInfo.Add(new ChartData() { DataName = "Waking up", DataValue = data.GetTotalDays() * 24 - (int)data.GetTotalStep((int)Years.SelectedItem) });

            //(PieChart.Series[0] as PieSeries).ItemsSource = ChartInfo;

        }

        private void OnStartDetail(object sender, RoutedEventArgs e)
        {
            if (data == null)
            {
                data = ((App)Application.Current).XmlData;
            }

            double average = data.GetAverageStep();
            double min = data.GetMinimumStep();
            double max = data.GetMaxmumStep();
            double total = data.GetTotalStep();

            AverageStep.Text = "Average Step " + (int)(average) + " km ";
            MinimumStep.Text = "Minimum Step " + (int)(min) + " km ";
            MaximumStep.Text = "Maximum Step " + (int)(max) + " km ";
            TotalStep.Text = "Total Step " + (int)(total) + " km ";

            //
            /*List<ChartData> TotalChartInfo = new List<ChartData>();

            TotalChartInfo.Add(new ChartData() { DataName = "Steping", DataValue = (int)data.GetTotalStep() });
            TotalChartInfo.Add(new ChartData() { DataName = "Waking", DataValue = data.GetTotalDays() * 24 - (int)data.GetTotalStep() });

            (TotalPieChart.Series[0] as PieSeries).ItemsSource = TotalChartInfo;*/

            average = data.GetAverageStep();
            min = data.GetMinimumStep();
            max = data.GetMaxmumStep();

            AverageStepDay.Text = "Step " + (int)(average) + " km ";

            total = data.GetTotalStep(DateTime.Now.Year, DateTime.Now.Month);
            average = data.GetAverageStep(DateTime.Now.Year, DateTime.Now.Month);
            min = data.GetMinimumStep(DateTime.Now.Year, DateTime.Now.Month);
            max = data.GetMaxmumStep(DateTime.Now.Year, DateTime.Now.Month);

            TotalStepMonth.Text = "Total Step " + (int)(total) + " km ";
            AverageStepMonth.Text = "Average Step " + (int)(average) + " km ";
            MinimumStepMonth.Text = "Minimum Step " + (int)(min) + " km ";
            MaximumStepMonth.Text = "Maximum Step " + (int)(max) + " km ";

            total = data.GetTotalStep(DateTime.Now.Year);
            average = data.GetAverageStep(DateTime.Now.Year);
            min = data.GetMinimumStep(DateTime.Now.Year);
            max = data.GetMaxmumStep(DateTime.Now.Year);

            TotalStepYear.Text = "Total Step " + (int)(total) + " km ";
            AverageStepYear.Text = "Average Step " + (int)(average) + " km ";
            MinimumStepYear.Text = "Minimum Step " + (int)(min) + " km ";
            MaximumStepYear.Text = "Maximum Step " + (int)(max) + " km ";

            CalendarDay.Date = DateTime.Now;
            CalendarMonth.Date = DateTime.Now;
            CalendarYear.Date = DateTime.Now;
        }

        private void CalendarDateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalStep(CalendarDay.Date.Value.Year, CalendarDay.Date.Value.Month, CalendarDay.Date.Value.Day);

            AverageStepDay.Text = "Step " + (int)(total) + " km ";
        }

        private void MonthChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalStep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double average = data.GetAverageStep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double min = data.GetMinimumStep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);
            double max = data.GetMaxmumStep(CalendarMonth.Date.Value.Year, CalendarMonth.Date.Value.Month);

            TotalStepMonth.Text = "Total Step " + (int)(total) + " km " ;
            AverageStepMonth.Text = "Average Step " + (int)(average) + " km ";
            MinimumStepMonth.Text = "Minimum Step " + (int)(min) + " km ";
            MaximumStepMonth.Text = "Maximum Step " + (int)(max) + " km ";
        }

        private void YearChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            double total = data.GetTotalStep(CalendarYear.Date.Value.Year);
            double average = data.GetAverageStep(CalendarYear.Date.Value.Year);
            double min = data.GetMinimumStep(CalendarYear.Date.Value.Year);
            double max = data.GetMaxmumStep(CalendarYear.Date.Value.Year);

            TotalStepYear.Text = "Total Step " + (int)(total) + " km ";
            AverageStepYear.Text = "Average Step " + (int)(average) + " km ";
            MinimumStepYear.Text = "Minimum Step " + (int)(min) + " km ";
            MaximumStepYear.Text = "Maximum Step " + (int)(max) + " km ";
        }
    }
}
