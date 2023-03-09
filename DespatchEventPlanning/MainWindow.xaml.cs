using DespatchEventPlanning.Views;
using DespatchEventPlanning.Models;

using System.Windows;
using System.Diagnostics;
using System;
using DespatchEventPlanning.Database;

namespace DespatchEventPlanning
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Stopwatch pw = new Stopwatch();
			pw.Start();
			
			InitializeComponent();

			pw.Stop();
			Debug.WriteLine($"Elapsed: {pw.Elapsed}") ;

			



		}

		private void ImportPlanButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Visible;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
			
		}

		private void AllocateLoadsButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Visible;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
		}

		private void DepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Visible;
			forecastUserControl.Visibility = Visibility.Hidden;
		}

		private void GenerateDatabase_Click(object sender, RoutedEventArgs e)
		{
			HandleExcelFiles excelFilehandler = new HandleExcelFiles();
			excelFilehandler.CheckDatabaseTable();
		}

		private void ForecastButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility= Visibility.Visible;
		}
	}
}