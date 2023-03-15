using DespatchEventPlanning.ObjectClasses;

using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Math;

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
		StorageAllocationClass storage = new StorageAllocationClass();
		
		
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
			if((bool)e.NewValue)
			{
				Debug.WriteLine(PackingPlan.storageInformationList.Count);
				if (PackingPlan.storageInformationList.Count>0)
				{

					storage.ClearAllocatedLoads();


					PackingPlan.storageInformationList.AsEnumerable().ToList().ForEach(item => {

						storage.AllocateStorage(item.allocationDate);

						switch (item.Group)
						{
							case "FLOWERS":
								loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().Where(item => item.storageDate != Convert.ToDateTime(item.depotDate).AddDays(-1).ToShortDateString()).Where(item=>item.productGroup.Contains("FLOWERS")).OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
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
			for (int i = 0; i < 2; i++)
			{
				ColumnDefinition colDef = new ColumnDefinition();
				informationGrid.ColumnDefinitions.Add(colDef);
			}
			
			

			storage.GetAllocatedLoads().AsEnumerable().Select(item => item.storageDate).OrderBy(item=>item).Distinct().ToList().ForEach(item =>
			{
				Label information = new Label();
				Label siteCapacity = new Label();
				information.Content = item;
				siteCapacity.Content =PackingPlan.storageInformationList.Where(subItem =>subItem.allocationDate ==item).Sum(subItem=>subItem.allocationQuantity);

				RowDefinition rowDef = new RowDefinition();
				rowDef.Height = new GridLength(40);
				
				informationGrid.RowDefinitions.Add(rowDef);
				Grid.SetRow(information, informationGrid.RowDefinitions.Count - 1);
				Grid.SetColumn(siteCapacity,informationGrid.ColumnDefinitions.Count);
				Grid.SetRow(siteCapacity, informationGrid.RowDefinitions.Count - 1);
				informationGrid.Children.Add(siteCapacity);
				informationGrid.Children.Add(information);
				
			});
		}

		private void DepotListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBoxItem selection = (ComboBoxItem)(sender as ComboBox).SelectedItem;

			


			if (DepotListComboBox.SelectedIndex != 0)
			{

				

				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item=>item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate).Where(item=>item.depotName==selection.Content.ToString());
			}
			else
			{
				loadsToAllocateDataGrid.ItemsSource = storage.GetAllocatedLoads().OrderBy(item => item.storageDate).ThenBy(item => item.depotName).ThenBy(item => item.depotDate).ThenByDescending(item => item.quantityPalletsToAllocate);
			}
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