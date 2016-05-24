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

        public SleepPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SourceData data = ((App)Application.Current).XmlData;

            Random rand = new Random();
            System.Collections.Generic.List<FinancialStuff> financialStuffList = new List<FinancialStuff>();
            financialStuffList.Add(new FinancialStuff() { Name = "MSFT", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "AAPL", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "GOOG", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "BBRY", Amount = rand.Next(0, 200) });
            (PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
            //(ColumnChart.Series[0] as ColumnSeries).ItemsSource = financialStuffList;
            //(LineChart.Series[0] as LineSeries).ItemsSource = financialStuffList;
        }

  
    }

    public class FinancialStuff
    {
        public string Name { get; set; }
        public int Amount { get; set; }
    }

}
