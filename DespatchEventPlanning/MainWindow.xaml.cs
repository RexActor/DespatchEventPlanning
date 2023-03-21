﻿using DespatchEventPlanning.Database;
using DespatchEventPlanning.ObjectClasses;

using DocumentFormat.OpenXml.Office.CustomUI;

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
			manageDatabaseUserControl.Visibility = Visibility.Hidden;
		}

		private void ForecastButton_Click(object sender, RoutedEventArgs e)
		{
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Visible;
			manageDatabaseUserControl.Visibility = Visibility.Hidden;
		}

		private void AllocateLoadsButton_Click(object sender, RoutedEventArgs e)
		{
			this.Status.Message = "Allocating loads...";
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Visible;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
			manageDatabaseUserControl.Visibility = Visibility.Hidden;
		}

		private void DepotSplitsButton_Click(object sender, RoutedEventArgs e)
		{
			this.Status.Message = "DepotSplits loads...";
			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Visible;
			forecastUserControl.Visibility = Visibility.Hidden;
			manageDatabaseUserControl.Visibility = Visibility.Hidden;
		}

		private void ManageDatabaseButton_Click(object sender, RoutedEventArgs e)
		{

			packingPlanUserControl.Visibility = Visibility.Hidden;
			allocateLoadsUSerControl.Visibility = Visibility.Hidden;
			depotSplitsUserControl.Visibility = Visibility.Hidden;
			forecastUserControl.Visibility = Visibility.Hidden;
			manageDatabaseUserControl.Visibility = Visibility.Visible;
		}
			
			
			
		
		

		private void ClearDatabase_Click(object sender, RoutedEventArgs e)
		{
			DatabaseClass db = new DatabaseClass();

			db.clearDatbaseTable("ProductionPlan");
			db.clearDatbaseTable("StorageAllocation");

		}

		private void RootWindow_Loaded(object sender, RoutedEventArgs e)
		{

			string dir = AppDomain.CurrentDomain.BaseDirectory;

			ListBox fileList = new ListBox();
			fileList.Items.Add("test1");
			fileList.Items.Add("test2");


		
		}
	}
}