using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Utilities;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for AllocateLoadsUserControl.xaml
	/// </summary>
	public partial class AllocateLoadsUserControl : UserControl
	{
		private StorageAllocationClass storage = new StorageAllocationClass();

		private List<Storage> tempList = new();

		private ComboBox depotDateComboBox;
		private Label storageDateLabel;
		private ComboBox storageDatesComboBox;
		private Label depotNameLabel;
		private ComboBox depotNamesComboBox;
		private Label depotDateLabel;

		private string storageDateSelected = string.Empty;
		private string depotNameSelected = string.Empty;
		private string depotDateSelected = string.Empty;


	

		
	

		public AllocateLoadsUserControl()
		{
			InitializeComponent();

			ToolTip toolTip = new ToolTip();
			
			

			toolTip.Content = $"This Grid displays product list for chosen packing date from which one storage should be allocated\n" +
				$"Available pallets are total <b>SUM</b> of depot available pallets";

			loadsToAllocateDataGrid.ToolTip = toolTip;

			

		}

	

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{

			storageSummary.ItemsSource = storage.GetAllocatedLoadsSummary();


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
			informationGrid.Children.Clear();

			informationGrid.RowDefinitions.Clear();
			informationGrid.ColumnDefinitions.Clear();

			depotDateComboBox = new ComboBox();
			storageDateLabel = new Label();
			storageDatesComboBox = new ComboBox();
			depotNameLabel = new Label();
			depotNamesComboBox = new ComboBox();
			depotDateLabel = new Label();

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

			storageDateLabel.Content = "Storage Date";
			storageDateLabel.Margin = new Thickness(0, 5, 0, 0);
			Grid.SetRow(storageDateLabel, 0);
			Grid.SetColumn(storageDateLabel, 0);
			informationGrid.Children.Add(storageDateLabel);

			storageDatesComboBox.SelectedIndex = 0;
			storageDatesComboBox.Items.Add("Please Select");
			storageDatesComboBox.Height = 25;
			storageDatesComboBox.Margin = new Thickness(0, 5, 0, 0);
			storageDatesComboBox.SelectionChanged += StorageDatesComboBox_SelectionChanged;
			storageDatesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
			storageDatesComboBox.VerticalAlignment = VerticalAlignment.Top;

			Grid.SetRow(storageDatesComboBox, 0);
			Grid.SetColumn(storageDatesComboBox, 1);
			informationGrid.Children.Add(storageDatesComboBox);

			depotNameLabel.Content = "Depot Name";
			depotNameLabel.Margin = new Thickness(0, 5, 0, 0);
			Grid.SetRow(depotNameLabel, 1);
			Grid.SetColumn(depotNameLabel, 0);
			informationGrid.Children.Add(depotNameLabel);

			depotNamesComboBox.SelectedIndex = 0;
			depotNamesComboBox.Items.Add("Please Select");
			depotNamesComboBox.SelectionChanged += DepotNamesComboBox_SelectionChanged;
			depotNamesComboBox.Height = 25;
			depotNamesComboBox.Margin = new Thickness(0, 5, 0, 0);
			depotNamesComboBox.HorizontalAlignment = HorizontalAlignment.Left;
			depotNamesComboBox.VerticalAlignment = VerticalAlignment.Top;

			Grid.SetRow(depotNamesComboBox, 1);
			Grid.SetColumn(depotNamesComboBox, 1);
			informationGrid.Children.Add(depotNamesComboBox);

			depotDateLabel.Content = "Depot Date";
			depotDateLabel.Margin = new Thickness(0, 5, 0, 0);
			Grid.SetRow(depotDateLabel, 2);
			Grid.SetColumn(depotDateLabel, 0);
			informationGrid.Children.Add(depotDateLabel);

			depotDateComboBox.SelectedIndex = 0;
			depotDateComboBox.Items.Add("Please Select");
			depotDateComboBox.Height = 25;
			depotDateComboBox.Margin = new Thickness(0, 5, 0, 0);
			depotDateComboBox.SelectionChanged += DepotDateComboBox_SelectionChanged;
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

		private void FilterDataGrid()
		{
			if (storageDateSelected != string.Empty && depotNameSelected == string.Empty)
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.storageDate == storageDateSelected).Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
			else if (storageDateSelected != string.Empty && depotNameSelected != string.Empty && depotDateSelected == string.Empty)
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.storageDate == storageDateSelected).Where(item => item.depotName == depotNameSelected).Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
			else if (storageDateSelected != string.Empty && depotNameSelected != string.Empty && depotDateSelected != string.Empty)
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.storageDate == storageDateSelected).Where(item => item.depotName == depotNameSelected).Where(item => item.depotDate == depotDateSelected).Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
		}

		private void StorageDatesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBoxSelection = sender as ComboBox;

			if (comboBoxSelection.SelectedIndex != 0)
			{
				storageDateSelected = comboBoxSelection.SelectedItem.ToString();

				depotNamesComboBox.SelectedIndex = 0;
				FilterDataGrid();
			}
			else
			{
				storageDateSelected = string.Empty;
				FilterDataGrid();
			}
		}

		private void DepotNamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBoxSelection = sender as ComboBox;
			if (storageDateSelected == String.Empty) { comboBoxSelection.SelectedIndex = 0; return; }

			if (comboBoxSelection.SelectedIndex != 0)
			{
				depotNameSelected = comboBoxSelection.SelectedItem.ToString();

				depotDateComboBox.SelectedIndex = 0;
				FilterDataGrid();
			}
			else
			{
				depotNameSelected = string.Empty;
				FilterDataGrid();
			}
		}

		private void DepotDateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBoxSelection = sender as ComboBox;
			if (depotNameSelected == string.Empty && storageDateSelected == string.Empty) { comboBoxSelection.SelectedIndex = 0; return; }
			if (comboBoxSelection.SelectedIndex != 0)
			{
				depotDateSelected = comboBoxSelection.SelectedItem.ToString();
				FilterDataGrid();
			}
			else
			{
				depotDateSelected = string.Empty;
				FilterDataGrid();
			}
		}

		private void AllocateProductButton_Click(object sender, RoutedEventArgs e)
		{
			Storage storageItemToAdd = ((FrameworkElement)sender).DataContext as Storage;
			string lastloadReference = string.Empty;
			string loadReferenceToBeAssigned = $"{storageItemToAdd.depotName.Substring(0, 3)}";

			if (storage.GetAmountOfLoadsWithDepotDate(loadReferenceToBeAssigned, storageItemToAdd.depotDate,storageItemToAdd.storageDate) > 0)
			{
				lastloadReference = storage.GetLastLoadReferenceWithDepotDate(loadReferenceToBeAssigned, storageItemToAdd.depotDate,storageItemToAdd.storageDate);
			}
			else
			{
				if (storage.GetAmountOfLoads(loadReferenceToBeAssigned) > 0)
				{
					lastloadReference = storage.GetLastLoadReference(loadReferenceToBeAssigned);

					string[] reference = lastloadReference.Split('-');
					int loadNumber = Convert.ToInt32(reference[1]);
					lastloadReference = $"{reference[0]}-{loadNumber + 1}";
				}
				else
				{
					lastloadReference = $"{loadReferenceToBeAssigned}-1";
				}
			}

			int palletsAllocated = storageItemToAdd.quantityPalletsToAllocate;
			int casesAllocated = palletsAllocated * storageItemToAdd.packsPerPallet;
			int availablePalletSpaces = 26 - storage.GetTotalPalletsInLoad(lastloadReference);

			if (availablePalletSpaces >= palletsAllocated)
			{
				AddProductToLoad(storageItemToAdd, lastloadReference, palletsAllocated, casesAllocated);
			}
			else
			{
				palletsAllocated = availablePalletSpaces;

				casesAllocated = palletsAllocated * storageItemToAdd.packsPerPallet;
				AddProductToLoad(storageItemToAdd, $"{lastloadReference}", palletsAllocated, casesAllocated);

				palletsAllocated = storageItemToAdd.quantityPalletsToAllocate - palletsAllocated;
				casesAllocated = palletsAllocated * storageItemToAdd.packsPerPallet;

				string[] loadSplit = lastloadReference.Split('-');
				int loadNumber = Convert.ToInt32(loadSplit[1]);

				AddProductToLoad(storageItemToAdd, $"{loadSplit[0]}-{loadNumber + 1}", palletsAllocated, casesAllocated);
			}

			
			Button button = sender as Button;
			button.IsEnabled = false;
		}

		private void AddProductToLoad(Storage storageItemToAdd, string loadReference, int palletsAllocated, int casesAllocated)
		{
			Storage amendedProduct = new Storage()
			{
				winNumber = storageItemToAdd.winNumber,
				loadReference = loadReference,
				productDescription = storageItemToAdd.productDescription,
				quantityPalletsAllocated = palletsAllocated,
				depotName = storageItemToAdd.depotName,
				quantityCases = casesAllocated,
				depotDate = storageItemToAdd.depotDate,
				storageDate = storageItemToAdd.storageDate,
				
			};


		

			storage.AddProductToStorageTruck(amendedProduct);

			storageSummary.ItemsSource = storage.GetAllocatedLoadsSummary();
		}

		private void storageSummary_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
		{
			//MessageBox.Show("Changed Source");
			//storageSummary.ItemsSource = storage.GetAllocatedLoadsSummary();
		}

		private void CommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
		{

		}

		
	}
}