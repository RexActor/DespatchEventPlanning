using DespatchEventPlanning.Database;
using DespatchEventPlanning.ObjectClasses;

using System.Diagnostics;
using System.Windows;

namespace DespatchEventPlanning
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public StatusUpdate Status { get; set; } = new StatusUpdate();

		public MainWindow()
		{
			Stopwatch pw = new Stopwatch();
			pw.Start();

			InitializeComponent();

			pw.Stop();
			Debug.WriteLine($"Elapsed: {pw.Elapsed}");
			this.Status.Message = "Ready";
		}

		private void ImportPlanButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Visible;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
		}

		private void ForecastButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Visible;
		}

		private void AllocateLoadsButton_Click(object sender, RoutedEventArgs e)
		{
			this.Status.Message = "Allocating loads...";
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Visible;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
		}

		private void DepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			this.Status.Message = "DepotSplits loads...";
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Visible;
			forecastUserControl.Visibility = Visibility.Hidden;
		}

		private void GenerateDatabaseButton_Click(object sender, RoutedEventArgs e)
		{
			this.Status.Message = "Uploading database...";
			HandleExcelFiles excelFilehandler = new HandleExcelFiles();
			excelFilehandler.CheckDatabaseTable();
		}
	}
}