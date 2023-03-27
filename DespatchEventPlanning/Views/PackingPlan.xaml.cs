using DespatchEventPlanning.Database;
using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
		private DatabaseClass db;

		public static List<StorageInformation> storageInformationList = new List<StorageInformation>();
		private string selectedProductionPlanVersion = string.Empty;

		public PackingPlan()
		{
			InitializeComponent();

		

				db = new DatabaseClass();
			resetPackingPlanTableList();


		}


		public void resetPackingPlanTableList()
		{


			if (availableProductionPlansList.Items.Count > 0)
			{
				availableProductionPlansList.Items.Clear();
			}



			availableProductionPlansList.Items.Add("Please Choose Packing Plan");
			db.GetProductionPlanTables().AsEnumerable().ToList().ForEach(table =>
			{
				availableProductionPlansList.Items.Add(table);
			});
			

			if (selectedProductionPlanVersion!=string.Empty)
			{
				availableProductionPlansList.SelectedItem = selectedProductionPlanVersion;
			}
			else
			{
				availableProductionPlansList.SelectedIndex = 0;
			}

			

		}

		private void PackingDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			if (selectedProductionPlanVersion != string.Empty)
			{
				excelDataGrid.ItemsSource = db.getInformationInList(selectedProductionPlanVersion).Where(item => item.packingDate == PackingDateCalendar.SelectedDate.Value.ToShortDateString());
			}
		}

		private void ClearPackingDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			excelDataGridItemSourceReset();
		}

		private void excelDataGridItemSourceReset()
		{
			if (selectedProductionPlanVersion != string.Empty)
			{
				excelDataGrid.ItemsSource = db.getInformationInList(selectedProductionPlanVersion).OrderBy(item => item.packingDate);
			}
		}

		private List<SiteCapacityClass> DisplayCapacity(List<PackingProductInformationClass> productList)
		{
			List<SiteCapacityClass> siteCapacity = new List<SiteCapacityClass>();

			productList.AsEnumerable().OrderByDescending(item => Convert.ToDateTime(item.packingDate)).Select(item => item.packingDate).Distinct().ToList().ForEach(packingDate =>
			{
				int palletsGenerated = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) == Convert.ToDateTime(packingDate)).Sum(item => item.palletsGenerated);
				int palletsDirect = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) == Convert.ToDateTime(packingDate).AddDays(1)).Sum(item => item.palletsGenerated);

				int palletsToStorage = db.getStorageInformationInList(selectedProductionPlanVersion).AsEnumerable().Where(item => Convert.ToDateTime(item.storageDate) == Convert.ToDateTime(packingDate)).Sum(item => item.quantityPalletsAllocated);
				int storageForDepotDate = db.getStorageInformationInList(selectedProductionPlanVersion).AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) == Convert.ToDateTime(packingDate).AddDays(1)).Sum(item => item.quantityPalletsAllocated);
				int totalPalletsGenerated = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) <= Convert.ToDateTime(packingDate)).Sum(item => item.palletsGenerated);

				int generatedPreviousDay = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) == Convert.ToDateTime(packingDate).AddDays(-1)).Sum(item => item.palletsGenerated);
				int palletsToStoragePreviousDay = db.getStorageInformationInList(selectedProductionPlanVersion).AsEnumerable().Where(item => Convert.ToDateTime(item.storageDate) == Convert.ToDateTime(packingDate).AddDays(-1)).Sum(item => item.quantityPalletsAllocated);
				int directsPreviousDay = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) == Convert.ToDateTime(packingDate)).Sum(item => item.palletsGenerated);

				int totalDirects = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) <= Convert.ToDateTime(packingDate).AddDays(1)).Sum(item => item.palletsGenerated);
				int totalStorageForDepotDate = db.getStorageInformationInList(selectedProductionPlanVersion).AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) <= Convert.ToDateTime(packingDate).AddDays(1)).Sum(item => item.quantityPalletsAllocated);

				int totalStorage = db.getStorageInformationInList(selectedProductionPlanVersion).AsEnumerable().Where(item => Convert.ToDateTime(item.storageDate) <= Convert.ToDateTime(packingDate)).Sum(item => item.quantityPalletsAllocated);

				int TotalOutbound = totalDirects - totalStorageForDepotDate + totalStorage;

				int totalPalletsOutboundPreviousDay = directsPreviousDay - storageForDepotDate + palletsToStoragePreviousDay;

				int leftOnSitePreviousDay = generatedPreviousDay - totalPalletsOutboundPreviousDay;

				int directsWithRemovedStorage = palletsDirect - storageForDepotDate;
				int totalPalletsOutbound = directsWithRemovedStorage + palletsToStorage;

				int leftOnSite = palletsGenerated - totalPalletsOutbound;

				siteCapacity.Add(new SiteCapacityClass()
				{
					produceDate = packingDate,

					palletsGenerating = palletsGenerated,
					palletsGeneratedTotal = totalPalletsGenerated,

					palletsDirect = directsWithRemovedStorage,

					palletsToStorage = palletsToStorage,

					palletsOutbound = TotalOutbound,

					leftOnSitePreviousDay = leftOnSitePreviousDay,
					palletsInBound = TotalOutbound
				});
			});
			return siteCapacity;
		}

		private void AddToLoadAllocation_Click(object sender, RoutedEventArgs e)
		{
			SiteCapacityClass siteCapacityClass = (SiteCapacityClass)siteCapacityGrid.SelectedItem;

			if (!storageInformationList.Any(item => item.allocationDate == siteCapacityClass.produceDate))
			{
				storageInformationList.Add(new StorageInformation()
				{
					allocationDate = siteCapacityClass.produceDate,
					allocationQuantity = siteCapacityClass.siteCapacity,
					Group = "ALL",
					productionPlanVersion = selectedProductionPlanVersion
				});
			}
		}

		private void AddFlowersToLoadAllocation_Click(object sender, RoutedEventArgs e)
		{
			SiteCapacityClass siteCapacityClass = (SiteCapacityClass)siteCapacityGrid.SelectedItem;
			
			if (!storageInformationList.Any(item => item.allocationDate == siteCapacityClass.produceDate))
			{
				storageInformationList.Add(new StorageInformation()
				{
					allocationDate = siteCapacityClass.produceDate,
					allocationQuantity = siteCapacityClass.siteCapacity,
					Group = "FLOWERS",
					productionPlanVersion = selectedProductionPlanVersion
				});
			}
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (selectedProductionPlanVersion != string.Empty)
			{
				siteCapacityGrid.ItemsSource = DisplayCapacity(db.getInformationInList(selectedProductionPlanVersion)).OrderBy(item => item.produceDate).ToList().OrderBy(item => item.produceDate);
				siteCapacityFlowersGrid.ItemsSource = DisplayCapacity(db.getInformationInList(selectedProductionPlanVersion).Where(item => item.productGroup.Contains("FLOWERS")).ToList()).OrderBy(item => item.produceDate);
				siteCapacityPlantsGrid.ItemsSource = DisplayCapacity(db.getInformationInList(selectedProductionPlanVersion).Where(item => item.productGroup.Contains("PLANTS")).ToList()).OrderBy(item => item.produceDate);
			}
			resetPackingPlanTableList();
		}

		private void UpdateProductInformation_Click(object sender, RoutedEventArgs e)
		{
			PackingProductInformationClass selectedProduct = (PackingProductInformationClass)excelDataGrid.SelectedItem;

			MessageBox.Show($"{selectedProduct.packingDate}");
		}

		private void AddProductInformation_Click(object sender, RoutedEventArgs e)
		{
		}

		private void availableProductionPlansList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			

			if (availableProductionPlansList.SelectedIndex > 0)
			{
				selectedProductionPlanVersion = availableProductionPlansList.SelectedItem.ToString();

				excelDataGrid.ItemsSource = db.getInformationInList(availableProductionPlansList.SelectedItem.ToString()).OrderBy(item => item.packingDate);

				siteCapacityGrid.ItemsSource = DisplayCapacity(db.getInformationInList(availableProductionPlansList.SelectedItem.ToString())).ToList();
				siteCapacityFlowersGrid.ItemsSource = DisplayCapacity(db.getInformationInList(availableProductionPlansList.SelectedItem.ToString()).Where(item => item.productGroup.Contains("FLOWERS")).ToList());
				siteCapacityPlantsGrid.ItemsSource = DisplayCapacity(db.getInformationInList(availableProductionPlansList.SelectedItem.ToString()).Where(item => item.productGroup.Contains("PLANTS")).ToList());
			};
		}
	}

	public class StorageInformation
	{
		public string allocationDate { get; set; }
		public int allocationQuantity { get; set; }
		public string Group { get; set; }
		public string productionPlanVersion { get; set; }
	}
}