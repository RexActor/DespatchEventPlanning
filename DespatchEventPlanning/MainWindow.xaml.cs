using System.Windows;

namespace DespatchEventPlanning
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
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

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			//packingPlanUserControl.Visibility = Visibility.Hidden;
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