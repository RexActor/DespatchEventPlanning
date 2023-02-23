using DespatchEventPlanning.Models;

using DocumentFormat.OpenXml.Math;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DespatchEventPlanning.Views
{
    /// <summary>
    /// Interaction logic for PackingPlan.xaml
    /// </summary>
    public partial class PackingPlan : UserControl
    {

		string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";


		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";


		DataTable packingPlanDataTable;
		bool changesMade = false;
		double previousValue = 0d;
		double newValue = 0d;
		bool packingQuantityCellSelected = false;


		DateTime selectedPackingDate;
		DateTime? selectedDepotDate;
		DataView dataView;
		DataTableModel dataTableModel;
		Label dateLabel;
		double totalPackingQuantity;

		public PackingPlan()
        {
            InitializeComponent();

			dataTableModel = new DataTableModel();

			packingPlanDataTable = dataTableModel.GetDataTable();
			packingPlanDataTable.RowChanged += PackingPlanDataTable_RowChanged;


			excelDataGrid.PreparingCellForEdit += ExcelDataGrid_PreparingCellForEdit;
			excelDataGrid.SourceUpdated += ExcelDataGrid_SourceUpdated;
			excelDataGrid.ColumnDisplayIndexChanged += ExcelDataGrid_ColumnDisplayIndexChanged;
			
			

			dataView = packingPlanDataTable.DefaultView;
			//dataView.ListChanged += DataView_ListChanged;

			PackingDateCalendar.SelectedDate = DateTime.Now.Date;
			selectedPackingDate = DateTime.Now.Date;

		}

		private void DataView_ListChanged(object? sender, System.ComponentModel.ListChangedEventArgs e)
		{
			
		}

		private void ExcelDataGrid_ColumnDisplayIndexChanged(object? sender, DataGridColumnEventArgs e)
		{
			
		}

		private void ExcelDataGrid_SourceUpdated(object? sender, DataTransferEventArgs e)
		{
			
		}

		private void ExcelDataGrid_PreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
		{

			if (e.Column.Header.ToString() == COLUMN_HEADER_PACKING_QUANTITY)
			{

				packingQuantityCellSelected = true;
				var cellInfo = excelDataGrid.SelectedCells[0];
				var cellContent = cellInfo.Column.GetCellContent(cellInfo.Item);

				TextBox? temp = cellContent as TextBox;


				previousValue = Double.TryParse(temp.Text, out previousValue) ? previousValue : 0;


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


		private void GenerateLabel(DataGrid _dataGrid)
		{
			DepotDateLabelGrid.Children.Clear();
			
			if (_dataGrid != null)
			{

				int rowLimit = 7;
				int rowLocation = 0;
				int colLocation = 0;
				

				List<DateTime> allocatedDates = new List<DateTime>();
				allocatedDates.Clear();


				foreach (DataRowView item in _dataGrid.ItemsSource)
				{

					DateTime date = Convert.ToDateTime(item[COLUMN_HEADER_PACKING_DATE_FILTER]);
					
					if (!allocatedDates.Contains(date))
					{
						allocatedDates.Add(date);
						//Debug.WriteLine($"List have entries: {allocatedDates.Count} ");
					}

				}
				
				if (allocatedDates.Count > rowLimit)
				{

					for (int i = 0; i < Math.Ceiling((float)allocatedDates.Count / rowLimit); i++)
					{
						var colDef = new ColumnDefinition();
						colDef.Width = new GridLength(110);
						//colDef.Name= i.ToString();
						DepotDateLabelGrid.ColumnDefinitions.Add(colDef);
					}

				}

				for (int i = 0; i < rowLimit; i++)
				{
					var def = new RowDefinition();
					def.Height = new GridLength(40);

					DepotDateLabelGrid.RowDefinitions.Add(def);
				}


				foreach (DateTime item in allocatedDates)
				{

					ToolTip toolTip = new ToolTip();
					toolTip.Content = item.Date.ToShortDateString();

					///GENERATE LABEL ///////

					dateLabel = new Label();
					dateLabel.Height = 30;


					dateLabel.Content = $"{item.Date.ToShortDateString()} Packing { GetPackingQuantity(item.Date)} cases";


					dateLabel.Margin = new Thickness(5, 2, 0, 2);



					dateLabel.BorderThickness = new Thickness(1);
					dateLabel.BorderBrush = Brushes.Red;
					dateLabel.ToolTip = toolTip;


					if (rowLocation >= rowLimit)
					{
						colLocation++;
						rowLocation = 0;
					}

					Grid.SetRow(dateLabel, rowLocation);
					Grid.SetColumn(dateLabel, colLocation);

					DepotDateLabelGrid.Children.Add(dateLabel);
					rowLocation++;
					totalPackingQuantity = 0;
				}
			}
			
		}
		

		/// <summary>
		/// Returns total sum of PackingQuantityColumn from dataview
		/// DataView is being filtered out based on given param!
		/// </summary>
		/// <param name="packingDate"></param>
		/// <returns></returns>

		private double GetPackingQuantity(DateTime packingDate)
		{
			
			DataView _dataView = dataView;

			_dataView = dataTableModel.FilterDataTable(_dataView, Filter_For_Data_Table.RequiredDate, packingDate);
			

			foreach (DataRowView _dataRow in _dataView)
			{
				//Debug.WriteLine(_dataRow[COLUMN_HEADER_PACKING_QUANTITY].ToString());
				totalPackingQuantity += (Double)_dataRow[COLUMN_HEADER_PACKING_QUANTITY];

							}

			return totalPackingQuantity;

		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{

			selectedPackingDate = PackingDateCalendar.SelectedDate.Value;
						

			selectedDepotDate = DepotDateCalendar.SelectedDate;
			if (selectedDepotDate != null)
			{
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate, Filter_For_Data_Table.DepotDate, selectedDepotDate.Value);
			}
			else
			{
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate);
			}
				
			excelDataGrid.ItemsSource = dataView;
			GenerateLabel(excelDataGrid);


		}
		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{


			excelDataGrid.ItemsSource = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate);
			selectedDepotDate = null;
			GenerateLabel(excelDataGrid);
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
			GenerateLabel(excelDataGrid);
		}



	}
}
