﻿using DespatchEventPlanning.ObjectClasses;

using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for AllocateLoadsUserControl.xaml
	/// </summary>
	public partial class AllocateLoadsUserControl : UserControl
	{
		private StorageAllocationClass storage = new StorageAllocationClass();

		public AllocateLoadsUserControl()
		{
			InitializeComponent();

			ToolTip toolTip = new ToolTip();

			toolTip.Content = $"This Grid displays product list for chosen packing date from which one storage should be allocated\n" +
				$"Available pallets are total <b>SUM</b> of depot available pallets";

			loadsToAllocateDataGrid.ToolTip = toolTip;
		}

		private void AllocationCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Adding load to the list?");
		}

		private void AllocationCheckbox_Unchecked(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Removing product from list?");
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			//MessageBox.Show("User Control loaded");
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				Debug.WriteLine(PackingPlan.storageInformationList.Count);
				if (PackingPlan.storageInformationList.Count > 0)
				{
					storage.ClearAllocatedLoads();

					PackingPlan.storageInformationList.AsEnumerable().ToList().ForEach(item =>
					{
						storage.AllocateStorage(item.allocationDate);

						switch (item.Group)
						{
							case "FLOWERS":
								loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.productGroup.Contains("FLOWERS")).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate).Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString());
								break;

							case "ALL":
								loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
								break;
						}
					});

					GenerateInformationGrid();
				}
			}
		}

		private void GenerateInformationGrid()
		{
			/*
			 <ComboBox x:Name="DepotListComboBox" Grid.Row="0" Grid.Column="1" Width="120" Height="25" SelectedIndex="0" SelectionChanged="DepotListComboBox_SelectionChanged" >

			<ComboBoxItem Content="Please Select"/>
			<ComboBoxItem Content="ERITH"/>
			<ComboBoxItem Content="LUTTERWORTH"/>

			<ComboBoxItem Content="ROCHDALE"/>
			<ComboBoxItem Content="SKELMERSDALE"/>
			<ComboBoxItem Content="WAKEFIELD"/>
			<ComboBoxItem Content="WASHINGTON"/>
			<ComboBoxItem Content="FALKIRK"/>
			<ComboBoxItem Content="LARNE"/>
			<ComboBoxItem Content="BRISTOL"/>
			<ComboBoxItem Content="BEDFORD"/>

		</ComboBox>
			 */
			informationGrid.Children.Clear();
			informationGrid.RowDefinitions.Clear();
			informationGrid.ColumnDefinitions.Clear();
			for (int i = 0; i < 2; i++)
			{
				ColumnDefinition colDef = new ColumnDefinition();
				colDef.Width = new GridLength(100);
				informationGrid.ColumnDefinitions.Add(colDef);
			}

			for (int i = 0; i < 3; i++)
			{
				RowDefinition rowdef = new RowDefinition();
				rowdef.Height = new GridLength(30);
				informationGrid.RowDefinitions.Add(rowdef);
			}

			Label storageDateLabel = new Label();
			storageDateLabel.Content = "Storage Date";
			Grid.SetRow(storageDateLabel, 0);
			Grid.SetColumn(storageDateLabel, 0);
			informationGrid.Children.Add(storageDateLabel);

			ComboBox storageDatesComboBox = new ComboBox();
			storageDatesComboBox.SelectedIndex = 0;
			storageDatesComboBox.Items.Add("Please Select");
			storageDatesComboBox.Height = 25;
			storageDatesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
			storageDatesComboBox.VerticalAlignment = VerticalAlignment.Top;

			Grid.SetRow(storageDatesComboBox, 0);
			Grid.SetColumn(storageDatesComboBox, 1);
			informationGrid.Children.Add(storageDatesComboBox);

			Label depotNameLabel = new Label();
			depotNameLabel.Content = "Depot Name";
			Grid.SetRow(depotNameLabel, 1);
			Grid.SetColumn(depotNameLabel, 0);
			informationGrid.Children.Add(depotNameLabel);


			ComboBox depotNamesComboBox = new ComboBox();
			depotNamesComboBox.SelectedIndex = 0;
			depotNamesComboBox.Items.Add("Please Select");
			depotNamesComboBox.Height = 25;
			depotNamesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
			depotNamesComboBox.VerticalAlignment = VerticalAlignment.Top;

			Grid.SetRow(depotNamesComboBox, 1);
			Grid.SetColumn(depotNamesComboBox, 1);
			informationGrid.Children.Add(depotNamesComboBox);



			Label depotDateLabel = new Label();
			depotDateLabel.Content = "Depot Date";
			Grid.SetRow(depotDateLabel, 2);
			Grid.SetColumn(depotDateLabel, 0);
			informationGrid.Children.Add(depotDateLabel);


			ComboBox depotDateComboBox = new ComboBox();
			depotDateComboBox.SelectedIndex = 0;
			depotDateComboBox.Items.Add("Please Select");
			depotDateComboBox.Height = 25;
			depotDateComboBox.HorizontalAlignment = HorizontalAlignment.Left;
			depotDateComboBox.VerticalAlignment = VerticalAlignment.Top;

			Grid.SetRow(depotDateComboBox, 2);
			Grid.SetColumn(depotDateComboBox, 1);
			informationGrid.Children.Add(depotDateComboBox);



			storage.GetAllocatedLoads().AsEnumerable().Select(item => item.storageDate).OrderBy(item => item).Distinct().ToList().ForEach(item =>
			{
				storageDatesComboBox.Items.Add(item);
			});

			storage.GetAllocatedLoads().AsEnumerable().Select(item => item.depotName).OrderBy(item => item).Distinct().ToList().ForEach(item =>
			{
				depotNamesComboBox.Items.Add(item);
			});

			storage.GetAllocatedLoads().AsEnumerable().Select(item => item.depotDate).OrderBy(item => item).Distinct().ToList().ForEach(item =>
			{
				depotDateComboBox.Items.Add(item);
			});

		}

		private void DepotListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem selection = (ComboBoxItem)(sender as ComboBox).SelectedItem;

			/*
			if (DepotListComboBox.SelectedIndex != 0)
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.depotName == selection.Content.ToString()).Where(item=>item.storageDate!=Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item=>item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
			else
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
			*/
		}

		private void AllocateProductButton_Click(object sender, RoutedEventArgs e)
		{
			Storage storage = ((FrameworkElement)sender).DataContext as Storage;

			MessageBox.Show($"Allocating product {storage.productDescription}\n" +
				$"Packed On: {storage.storageDate}\n" +
				$"For Depot Date: {storage.depotDate}\n" +
				$"Available Pallets: {storage.quantityPalletsToAllocate}\n" +
				$"For Depot: {storage.depotName}\n" +
				$"Cases Packed: {storage.quantityCases}\n" +
				$"With {storage.packsPerPallet} cases per Pallet");
		}
	}
}