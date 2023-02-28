using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

			packingPlanDataTable = dataTableModel.GetDataTable("PackingPlan", EnumClass.FILE_NAME.PackingPlan);

			dataView = packingPlanDataTable.DefaultView;

			excelDataGrid.ItemsSource = dataView;

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

			int rowLimit = 7;
			int rowLocation = 0;
			int colLocation = 0;

			List<DateTime> allocatedDates = new List<DateTime>();

			DataView temp_dataView = packingPlanDataTable.DefaultView;

			temp_dataView.Cast<DataRowView>().ToList().ForEach(_dataRow => {
				DateTime packingDate = Convert.ToDateTime(_dataRow[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate}"]);

				if (!allocatedDates.Contains(packingDate) && packingDate == _selectedPackingDate)
				{
					allocatedDates.Add(packingDate);
				}

			});

			

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

			allocatedDates.ToList().ForEach(_allocatedDate =>
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
			});
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

			_dataView = dataTableModel.FilterDataTable(_dataView, EnumClass.Filter_For_Data_Table.RequiredDate, packingDate.ToShortDateString());


			_dataView.Cast<DataRowView>().ToList().ForEach(_dataRow =>
			{
				totalPackingQuantity += (Double)_dataRow[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"];
			});



		

			return totalPackingQuantity;
		}

		private void Calendar_PackingDatesChanged(object sender, SelectionChangedEventArgs e)
		{
			UpdateDataGrid(EnumClass.FILTER_OPTION.PACKINGDATE);
		}

		private void UpdateDataGrid(EnumClass.FILTER_OPTION filterOption)
		{
			if (PackingDateCalendar.SelectedDate == null) { return; }

			switch (filterOption)
			{
				case EnumClass.FILTER_OPTION.DEPOTDATE:

					break;

				case EnumClass.FILTER_OPTION.PACKINGDATE:
					dataView = dataTableModel.FilterDataTable(dataView, EnumClass.Filter_For_Data_Table.RequiredDate, PackingDateCalendar.SelectedDate.Value.ToShortDateString());

					break;

				case EnumClass.FILTER_OPTION.BOTH:

					break;
			}

			GenerateLabel(PackingDateCalendar.SelectedDate.Value);
			excelDataGrid.ItemsSource = dataView;
		}

		private void ClearPackingDateButton_Click(object sender, RoutedEventArgs e)
		{
			dataView.RowFilter = null;
			excelDataGrid.ItemsSource = dataView;
			DepotDateLabelGrid.Children.Clear();
		}
	}
}