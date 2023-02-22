﻿
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

namespace DespatchEventPlanning
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";
		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";


		DataTable packingPlanDataTable;
		bool changesMade = false;
		double previousValue =0d;
		double newValue = 0d;
		bool packingQuantityCellSelected = false;

		DateTime selectedPackingDate;
		DateTime? selectedDepotDate;
		DataView dataView;
		DataHandler importedData;
		DataColumn dtColumn;
		public MainWindow()
		{
			
			InitializeComponent();
		

			importedData = new DataHandler();
						
			packingPlanDataTable = importedData.ReadExcelFile("PackingPlan", filePath);
			packingPlanDataTable.DefaultView.Sort = $"{COLUMN_HEADER_PACKING_DATE_FILTER} ASC";
		

			dtColumn = new DataColumn();
			dtColumn.DataType = typeof(bool);
			dtColumn.DefaultValue = false;
			dtColumn.ColumnName = "Testing";
			dtColumn.ReadOnly = false;
			//packingPlanDataTable.Columns.Add(new DataColumn(
			//	"Selected",typeof(bool)
			//));
			packingPlanDataTable.Columns.Add(dtColumn);

			


			packingPlanDataTable.RowChanged += PackingPlanDataTable_RowChanged;

			foreach (DataColumn dtColumn in packingPlanDataTable.Columns)
			{
				if (dtColumn.ColumnName == "Testing" || dtColumn.ColumnName == COLUMN_HEADER_PACKING_QUANTITY)
				{
					dtColumn.ReadOnly = false;
				}
				
				else
				{
					dtColumn.ReadOnly = true;
				}
			}
				


			excelDataGrid.PreparingCellForEdit += ExcelDataGrid_PreparingCellForEdit;
			
			
			dataView = packingPlanDataTable.DefaultView;



			if(PackingDateCalendar.SelectedDate != null)
			{

				selectedPackingDate = PackingDateCalendar.SelectedDate.Value;
				dataView.RowFilter = $"{COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
				//dataView.RowFilter = $"{COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
			}
			

			

			excelDataGrid.ItemsSource = dataView;

			//excelDataGrid.AutoGeneratedColumns += ExcelDataGrid_AutoGeneratedColumns;

			PackingDateCalendar.SelectedDate = DateTime.Now.Date;
			selectedPackingDate = DateTime.Now.Date;

		}

	

		private void ExcelDataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
		{
			
			if (e.Column.Header.ToString() == COLUMN_HEADER_PACKING_QUANTITY)
			{
				
				//e.Column.IsReadOnly = false;
				packingQuantityCellSelected = true;
				var cellInfo = excelDataGrid.SelectedCells[0];
				var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
				
				TextBox? temp = cellContent as TextBox;

				//bool tryConvert = Double.TryParse(temp.Text, out var value);
				previousValue = Double.TryParse(temp.Text, out  previousValue) ? previousValue : 0;
				

			}
			else
			{
				//e.Column.IsReadOnly = true;
			}
			
		}


		private void PackingPlanDataTable_RowChanged(object sender, DataRowChangeEventArgs e)
		{
			if (packingQuantityCellSelected == true)
			{
				
			
				newValue = Double.TryParse(e.Row[COLUMN_HEADER_PACKING_QUANTITY].ToString(), out newValue) ? newValue : 0;


				if (previousValue != newValue)
				{
					changesMade = true;
					MessageBox.Show($"Old Value: {previousValue} | new Value: {newValue}");
					previousValue = 0;
					newValue = 0;
					packingQuantityCellSelected = false;
				}
			}
		}

		private void SaveDataButton_Click(object sender, RoutedEventArgs e)
		{
			if (changesMade)
			{
				importedData.SaveDatatableToExcel(packingPlanDataTable, filePath);
			}
			
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			

			selectedPackingDate = PackingDateCalendar.SelectedDate.Value;

			selectedDepotDate = DepotDateCalendar.SelectedDate;
			
			if (selectedDepotDate!= null)
			{
				dataView.RowFilter = $"{COLUMN_HEADER_DEPOT_DATE_FILTER} = '{selectedDepotDate.Value}' AND {COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
			}
			else
			{
				dataView.RowFilter = $"{COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
			}
			

			
			excelDataGrid.ItemsSource = dataView;
		}

		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{
			
			//dataView.RowFilter = null;
			dataView.RowFilter = $"{COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
			excelDataGrid.ItemsSource = dataView;
			selectedDepotDate = null;
		}
		private void ClearPackingDateButton_Click(object sender, RoutedEventArgs e)
		{

			if (selectedDepotDate != null)
			{
				dataView.RowFilter = $"{COLUMN_HEADER_DEPOT_DATE_FILTER}='{selectedDepotDate}'";
			}
			else
			{
				dataView.RowFilter = null;
			}
			
			//dataView.RowFilter = null;
			//dataView.RowFilter = $"{COLUMN_HEADER_PACKING_DATE_FILTER}='{selectedPackingDate}'";
			excelDataGrid.ItemsSource = dataView;
		}
	}
}
