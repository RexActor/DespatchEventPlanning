using DespatchEventPlanning.Database;
using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
		private List<PackingProductInformationClass> packingPlanList;
		private DataHandler handler;

		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";
		private readonly string depotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DepotSplits.xlsx";
		private readonly string defaultDepotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultDepotSplits.xlsx";
		private readonly string productInformationPath = $"{AppDomain.CurrentDomain.BaseDirectory}ProductInformation.xlsx";

		public PackingPlan()
		{
			InitializeComponent();

			packingPlanList = GeneratePackingClass();

			excelDataGridItemSourceReset();

			DatabaseClass db = new DatabaseClass();
			db.saveProductionPlanIntoDatabase(111221);


			db.getInformation();


			/*
			packingPlanList.ForEach(item => item.depotSplitInfotmation.ForEach(x =>
			{

				Debug.WriteLine($"{item.productDescription}\t {item.depotDate}\t {x.depotName}\t{x.depotNumber}\t{x.depotSplit}");

			}));
			*/
		}

		private List<PackingProductInformationClass> GeneratePackingClass()
		{
			handler = new DataHandler();

			DataTable table = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.PackingPlan}", packingPlanFilePath);

			DataTable forecastTable = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.Forecast}", forecastFilePath);

			DataTable productInfomation = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.ProductInformation}", productInformationPath);

			

		

			var convertedList = table.AsEnumerable().Select(row => new PackingProductInformationClass()
			{
				winNumber = Convert.ToInt32(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]),
				productDescription = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.ProductDescription}"]) ?? "N/A",
				productGroup = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.Group}"]) ?? "N/A",
				packingDate = row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate}"].ToString() ?? "N/A",
				depotDate = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"]) ?? "N/A",
				packingQty = Convert.ToDouble(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"]),
				forecastQty = forecastTable.AsEnumerable().Where(item => item.Field<string>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate}") == row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"].ToString()).Where(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]).Sum(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}")),
				packsPerPallet = (int)productInfomation.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]).Sum(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.PacksPerPallet}")),

				
			}).ToList();
			return convertedList;
		}

		private void PackingDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			excelDataGrid.ItemsSource = packingPlanList.Where(item => item.packingDate == PackingDateCalendar.SelectedDate.Value.ToShortDateString());
		}

		private void ClearPackingDateButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			excelDataGridItemSourceReset();
		}

		private void excelDataGridItemSourceReset()
		{
			excelDataGrid.ItemsSource = packingPlanList.OrderBy(item => item.packingDate);
		}
	}
}