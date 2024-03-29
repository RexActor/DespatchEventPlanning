﻿using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
		private DataTable depotSplitsDataTable;
		private DataTable defaultDepotSplits;
		private DataTable productInformation;

		private DataView dataView;
		private DataTableModel dataTableModel;

		private Label? dateLabel;
		private bool changesMade = false;
		private DataColumn column;
		private double totalPackingQuantity;

		public DataTableModel GetPackingPlanDataTableModel()
		{
			return dataTableModel;
		}

		public PackingPlan()
		{
			InitializeComponent();
			dataTableModel = new DataTableModel();

			depotSplitsDataTable = DataSetClass.GetDataTable(EnumClass.DATATABLE_NAME.depotSplitsDataTable);
			defaultDepotSplits = DataSetClass.GetDataTable(EnumClass.DATATABLE_NAME.defaultDepotSplits);

			packingPlanDataTable = DataSetClass.GetDataTable(EnumClass.DATATABLE_NAME.packingPlanDataTable);

			productInformation = DataSetClass.GetDataTable(EnumClass.DATATABLE_NAME.productInformation);

			dataView = packingPlanDataTable.DefaultView;

			GenerateDepotColumns();

			excelDataGrid.ItemsSource = dataView;

			PackingDateCalendar.SelectedDate = DateTime.Now.Date;
			GetCapacity();
		}

		private void GenerateDepotColumns()
		{
			Enum.GetNames(typeof(EnumClass.DEPOTS)).OrderBy(x => x).ToList().ForEach(column =>
			{
				this.column = new DataColumn();
				this.column.ColumnName = column;
				this.column.DefaultValue = -1;
				this.packingPlanDataTable.Columns.Add(this.column);
			});

			packingPlanDataTable.Rows.Cast<DataRow>().ToList().ForEach(packingRow =>
			{
				Enum.GetNames(typeof(EnumClass.DEPOTS)).OrderBy(x => x).ToList().ForEach(column =>
				{
					packingRow[column] = GetdepotSplit((double)packingRow[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"], packingRow[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"].ToString(), column, (double)packingRow[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"]);
				});
			});
		}

		private double GetdepotSplit(double winNumber, string depotDate, string depotName, double qty = default)
		{
			double res = depotSplitsDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == winNumber).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotDate}") == depotDate).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == depotName).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));

			if (res == 0)
			{
				double split = defaultDepotSplits.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == winNumber).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == depotName).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));
				res = Math.Round(split * qty);
			}

			return res;
		}

		private void SaveDataButton_Click(object sender, RoutedEventArgs e)
		{
			if (changesMade)

			{
				//importedData.SaveDatatableToExcel(packingPlanDataTable, filePath);
			}
		}

		private void GenerateLabel(DateTime _selectedPackingDate)
		{
			DepotDateLabelGrid.Children.Clear();

			int rowLimit = 7;
			int rowLocation = 0;
			int colLocation = 0;

			List<DateTime> allocatedDates = new List<DateTime>();

			DataView temp_dataView = packingPlanDataTable.DefaultView;

			temp_dataView.Cast<DataRowView>().ToList().ForEach(_dataRow =>
			{
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
			if (dataView.Count == 0)
			{
				dataView.RowFilter = null;
				dataView.Sort = $"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate} ASC";
				excelDataGrid.ItemsSource = dataView;
			}
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

		private void GetCapacity()
		{
			double palletsProduced = 0;
			double packingQuantity = 0;

			
			

			packingPlanDataTable.Rows.Cast<DataRow>().Select(item => item.Field<string>($"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate}")).Distinct().OrderBy(x => x).ToList().ForEach(m =>
			{



				packingPlanDataTable.AsEnumerable().Where(item => item.Field<string>($"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate}") == m).ToList().ForEach(subItem =>
				{
					double packsPerPallet = productInformation.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)subItem[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]).Sum(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.PacksPerPallet}"));

					double palletsGenerated = Math.Ceiling((double)subItem[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"] / packsPerPallet);

					palletsProduced += palletsGenerated;



					Debug.WriteLine($"On {m} Will be packed Depot {subItem[EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate.ToString()]} with total of {palletsProduced}");


				});


				palletsProduced = 0;
			});
		}
	}
}