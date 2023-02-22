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
using DespatchEventPlanning.Models;

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
		DataTableModel dataTableModel;
	

		public MainWindow()
		{
			
			InitializeComponent();

			 dataTableModel = new DataTableModel();

			packingPlanDataTable = dataTableModel.GetDataTable();
			packingPlanDataTable.RowChanged += PackingPlanDataTable_RowChanged;


			excelDataGrid.PreparingCellForEdit += ExcelDataGrid_PreparingCellForEdit;
						
			dataView = packingPlanDataTable.DefaultView;
												

			PackingDateCalendar.SelectedDate = DateTime.Now.Date;
			selectedPackingDate = DateTime.Now.Date;

		}

	

		private void ExcelDataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
		{
			
			if (e.Column.Header.ToString() == COLUMN_HEADER_PACKING_QUANTITY)
			{			
				
				packingQuantityCellSelected = true;
				var cellInfo = excelDataGrid.SelectedCells[0];
				var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
				
				TextBox? temp = cellContent as TextBox;

				
				previousValue = Double.TryParse(temp.Text, out  previousValue) ? previousValue : 0;
				

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
				//importedData.SaveDatatableToExcel(packingPlanDataTable, filePath);
			}
			
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{			

			selectedPackingDate = PackingDateCalendar.SelectedDate.Value;
			
			selectedDepotDate = DepotDateCalendar.SelectedDate;
			
			if (selectedDepotDate!= null)
			{
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate,Filter_For_Data_Table.DepotDate,selectedDepotDate.Value);
			}
			else
			{
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate);
			}
					

			excelDataGrid.ItemsSource = dataView;

		}

		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{
			
		
			excelDataGrid.ItemsSource = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate);
			selectedDepotDate = null;
		}
		private void ClearPackingDateButton_Click(object sender, RoutedEventArgs e)
		{

			if (selectedDepotDate != null)
			{
				
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.DepotDate, selectedDepotDate.Value);
			}
			else
			{
				dataView.RowFilter = null;
			}
				

			excelDataGrid.ItemsSource = dataView;
		}
	}
}
