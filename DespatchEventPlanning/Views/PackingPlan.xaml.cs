using DespatchEventPlanning.Models;

using DocumentFormat.OpenXml.Drawing.Diagrams;

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DespatchEventPlanning.Views
{
	internal enum FILTER_OPTION
	{
		PACKINGDATE,
		DEPOTDATE,
		BOTH,
	}

	/// <summary>
	/// Interaction logic for PackingPlan.xaml
	/// </summary>
	public partial class PackingPlan : UserControl
	{
#pragma warning disable S125 // Sections of code should not be commented out
		//readonly string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		//readonly string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
#pragma warning restore S125 // Sections of code should not be commented out
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";

#pragma warning disable S125 // Sections of code should not be commented out
		//private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		private DataTable packingPlanDataTable;
#pragma warning restore S125 // Sections of code should not be commented out
	
		private DataView dataView;
		private DataTableModel dataTableModel;
		private Label? dateLabel;
		private bool changesMade = false;
		
		private double totalPackingQuantity;
		
		public DataTableModel GetPackingPlanDataTableModel()
		{
			return dataTableModel;
		}
		public PackingPlan()
		{
			InitializeComponent();

			dataTableModel = new DataTableModel();

			packingPlanDataTable = dataTableModel.GetDataTable("PackingPlan",FILE_NAME.PackingPlan);

			dataView = packingPlanDataTable.DefaultView;
			excelDataGrid.ItemsSource = packingPlanDataTable.DefaultView;
			
		

			PackingDateCalendar.SelectedDate = DateTime.Now.Date;
			
		}

		private void SaveDataButton_Click(object sender, RoutedEventArgs e)
		{
			if (changesMade)
#pragma warning disable S125 // Sections of code should not be commented out
			{
				//importedData.SaveDatatableToExcel(packingPlanDataTable, filePath);
			}
#pragma warning restore S125 // Sections of code should not be commented out
		}

		private void GenerateLabel(DateTime _selectedPackingDate)
		{
			DepotDateLabelGrid.Children.Clear();
			if (_selectedPackingDate != null)
			{
				int rowLimit = 7;
				int rowLocation = 0;
				int colLocation = 0;

				List<DateTime> allocatedDates = new List<DateTime>();
				allocatedDates.Clear();
				DataView temp_dataView = (DataView)packingPlanDataTable.DefaultView;
				foreach (DataRowView _dataRow in temp_dataView)
				{
					DateTime packingDate = Convert.ToDateTime(_dataRow[COLUMN_HEADER_PACKING_DATE_FILTER]);

					if (!allocatedDates.Contains(packingDate) && packingDate == _selectedPackingDate)
					{
						allocatedDates.Add(packingDate);
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

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
				foreach (DateTime _allocatedDate in allocatedDates)
				{
					ToolTip toolTip = new ToolTip();
					toolTip.Content = _allocatedDate.Date.ToShortDateString();

					///GENERATE LABEL ///////

					dateLabel = new Label();
					dateLabel.Height = 30;

					dateLabel.Content = $"{_allocatedDate.Date.ToShortDateString()} Packing {GetPackingQuantity(_allocatedDate.Date)} cases";

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
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions
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

			_dataView = dataTableModel.FilterDataTable(_dataView, Filter_For_Data_Table.RequiredDate, packingDate.ToShortDateString());

			foreach (DataRowView _dataRow in _dataView)
			{
				totalPackingQuantity += (Double)_dataRow[COLUMN_HEADER_PACKING_QUANTITY];
			}

			return totalPackingQuantity;
		}

		private void Calendar_PackingDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateDataGrid(FILTER_OPTION.PACKINGDATE);
		}

		private void Calendar_DepotDatesChanged(object sender, SelectionChangedEventArgs e)
		{
		}

		private void UpdateDataGrid(FILTER_OPTION filterOption)
		{
			DataView _tempDataView = new DataView();
			switch (filterOption)
			{
				case FILTER_OPTION.DEPOTDATE:

					break;

				case FILTER_OPTION.PACKINGDATE:
					dataView = dataTableModel.FilterDataTable(dataView, Filter_For_Data_Table.RequiredDate, PackingDateCalendar.SelectedDate.Value.ToShortDateString());

					break;

				case FILTER_OPTION.BOTH:

					break;

				default:

					break;
			}

			GenerateLabel(PackingDateCalendar.SelectedDate.Value);
			excelDataGrid.ItemsSource = dataView;
		}

		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{
		}

		private void ClearPackingDateButton_Click(object sender, RoutedEventArgs e)
		{
			dataView.RowFilter = null;
			excelDataGrid.ItemsSource = dataView;
			DepotDateLabelGrid.Children.Clear();
		}
	}
}