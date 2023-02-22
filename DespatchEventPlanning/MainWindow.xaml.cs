
using DespatchEventPlanning.Database;
using System;

using System.Windows;
using System.Windows.Controls;

using System.Data;
using System.Media;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Windows.Media;
using System.Diagnostics;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Irony.Parsing;
using System.Globalization;
using DespatchEventPlanning.Models;
using DespatchEventPlanning.Views;

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

		}

		private void AllocateLoadsButton_Click(object sender, RoutedEventArgs e)
		{

			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Visible;
			depotSplitsUserControl.Visibility = Visibility.Hidden;

		}

		private void DepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Visible;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			//packingPlanUserControl.Visibility = Visibility.Hidden;
		}
	}
}
