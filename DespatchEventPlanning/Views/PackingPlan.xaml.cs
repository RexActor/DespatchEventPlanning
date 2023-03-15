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

		public static List<StorageInformation> storageInformationList;

		public PackingPlan()
		{
			InitializeComponent();
			db = new DatabaseClass();
			storageInformationList = new List<StorageInformation>();
			
			excelDataGrid.ItemsSource = db.getInformationInList();
			siteCapacityGrid.ItemsSource = DisplayCapacity(db.getInformationInList()).OrderBy(item => item.produceDate);
			siteCapacityFlowersGrid.ItemsSource = DisplayCapacity(db.getInformationInList().Where(item => item.productGroup.Contains("FLOWERS")).ToList()).OrderBy(item => item.produceDate);
			siteCapacityPlantsGrid.ItemsSource = DisplayCapacity(db.getInformationInList().Where(item => item.productGroup.Contains("PLANTS")).ToList()).OrderBy(item => item.produceDate);
			//excelDataGrid.ItemsSource = db.getInformationInDataTable().DefaultView;
		}

		private void PackingDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			excelDataGrid.ItemsSource = db.getInformationInList().Where(item => item.packingDate == PackingDateCalendar.SelectedDate.Value.ToShortDateString());
		}

		private void ClearPackingDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			excelDataGridItemSourceReset();
		}

		private void excelDataGridItemSourceReset()
		{
			excelDataGrid.ItemsSource = db.getInformationInList().OrderBy(item => item.packingDate);
		}

		private List<SiteCapacityClass> DisplayCapacity(List<PackingProductInformationClass> productList)
		{
			List<SiteCapacityClass> siteCapacity = new List<SiteCapacityClass>();

			productList.AsEnumerable().Select(x => x.packingDate).Distinct().ToList().ForEach(x =>
			{
				//(int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) <= Convert.ToDateTime(x).AddDays(1)).Sum(item => item.palletsGenerated)
				siteCapacity.Add(new SiteCapacityClass()
				{
					produceDate = x,

					palletsGenerating = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) == Convert.ToDateTime(x)).Sum(item => item.palletsGenerated),
					palletsOutbound = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) == Convert.ToDateTime(x).AddDays(1)).Sum(item => item.palletsGenerated),
					//palletsGeneratedTotal= (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) < Convert.ToDateTime(x)).Sum(item => item.palletsGenerated),

					palletsGeneratedTotal = (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.packingDate) <= Convert.ToDateTime(x)).Sum(item => item.palletsGenerated) - (int)productList.AsEnumerable().Where(item => Convert.ToDateTime(item.depotDate) < Convert.ToDateTime(x).AddDays(1)).Sum(item => item.palletsGenerated),

					palletsInBound = 0
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
					Group = "ALL"
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
					Group = "FLOWERS"
				});
			}

		}
	}

	public class StorageInformation
	{
		public string allocationDate { get; set; }
		public int allocationQuantity { get; set; }
		public string Group { get; set; }
	}
}