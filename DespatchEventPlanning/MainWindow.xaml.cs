
using DespatchEventPlanning.Database;
using System;

using System.Windows;
using System.Windows.Controls;

using System.Data;
using System.Media;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Windows.Media;
using System.Diagnostics;

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


		DataTable packingPlanDataTable;
		bool changesMade = false;
		double previousValue =0d;
		double newValue = 0d;
		bool packingQuantityCellSelected = false;

		DataHandler importedData;
		public MainWindow()
		{
			
			InitializeComponent();
					

			importedData = new DataHandler();
						
			packingPlanDataTable = importedData.ReadExcelFile("PackingPlan", filePath);
			packingPlanDataTable.RowChanged += PackingPlanDataTable_RowChanged;
			packingPlanDataTable.RowChanging += PackingPlanDataTable_RowChanging;
			excelDataGrid.PreparingCellForEdit += ExcelDataGrid_PreparingCellForEdit;
			excelDataGrid.SelectionChanged += ExcelDataGrid_SelectionChanged;
			excelDataGrid.ItemsSource= packingPlanDataTable.DefaultView;
			

		}

		private void ExcelDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			
			MessageBox.Show(previousValue.ToString());
		}

		private void ExcelDataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
		{
			
			if (e.Column.Header.ToString() == COLUMN_HEADER_PACKING_QUANTITY)
			{
				
				e.Column.IsReadOnly = false;
				packingQuantityCellSelected = true;
				var cellInfo = excelDataGrid.SelectedCells[0];
				var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);
				
				TextBox? temp = cellContent as TextBox;

				//bool tryConvert = Double.TryParse(temp.Text, out var value);
				previousValue = Double.TryParse(temp.Text, out  previousValue) ? previousValue : 0;
				

			}
			else
			{
				e.Column.IsReadOnly = true;
			}
			//;
		
		}

		private void PackingPlanDataTable_RowChanging(object sender, DataRowChangeEventArgs e)
		{
			
			
			//previousValue =(double) e.Row["PackingQuantity"];


			
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
	}
}
