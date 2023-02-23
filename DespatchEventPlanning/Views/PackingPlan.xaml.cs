using DespatchEventPlanning.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
		//readonly string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		//readonly string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		private DataTable packingPlanDataTable;
		private DateTime? selectedPackingDate;
		private DateTime? selectedDepotDate;
		private DataView dataView;
		private DataTableModel dataTableModel;
		private Label? dateLabel;
		private bool changesMade = false;
		private double previousValue = 0d;
		private double newValue = 0d;
		private bool packingQuantityCellSelected = false;
		private double totalPackingQuantity;

		public PackingPlan()
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
				if (temp is not null)
				{
					previousValue = Double.TryParse(temp.Text, out previousValue) ? previousValue : 0;
				}
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
			if (_dataGrid != null)
			{
				DepotDateLabelGrid.Children.Clear();
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
					}
				}

				if (allocatedDates.Count > rowLimit)
				{
					for (int i = 0; i < Math.Ceiling((float)allocatedDates.Count / rowLimit); i++)
					{
						var colDef = new ColumnDefinition();
						colDef.Width = new GridLength(110);

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

					dateLabel.Content = $"{item.Date.ToShortDateString()} Packing {GetPackingQuantity(item.Date)} cases";

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
				totalPackingQuantity += (Double)_dataRow[COLUMN_HEADER_PACKING_QUANTITY];
			}

			return totalPackingQuantity;
		}

		private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			if (PackingDateCalendar.SelectedDate != null)
			{
				selectedPackingDate = PackingDateCalendar.SelectedDate.Value;
			}
			selectedDepotDate = DepotDateCalendar.SelectedDate;
			if (selectedDepotDate != null && selectedPackingDate!=null)
			{
				dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate.Value, Filter_For_Data_Table.DepotDate, selectedDepotDate.Value);
			}
			else if(selectedPackingDate!=null)
			{
					dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate.Value);
			
			}

			excelDataGrid.ItemsSource = dataView;
			GenerateLabel(excelDataGrid);
		}

		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{
			if (selectedPackingDate != null)
			{
				excelDataGrid.ItemsSource = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, selectedPackingDate.Value);
				selectedDepotDate = null;
				GenerateLabel(excelDataGrid);
			}
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