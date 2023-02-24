using DespatchEventPlanning.Models;

using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for ForecastView.xaml
	/// </summary>
	public partial class ForecastView : UserControl
	{
		private const string SHEET_NAME_PACKINGPLAN = "PackingPlan";
		private const string SHEET_NAME_FORECAST = "Forecast";

		private const string FORECAST_DEPOT_DATE = "DepotDAte";
		private const string FORECAST_QTY = "Qty";
		private const string FORECAST_WINNUMBER = "WinNumber";
		private const string FORECAST_DIFFERENCE = "Difference";
		private const string FORECAST_PACK_QTY = "Pack Quantity";
		private const string FORECAST_MATCH = "Forecast Match";

		private const string PACKINGPLAN_QTY = "PackingQuantity";
		private const string PACKINGPLAN_DEPOTDATE = "DepotDate";
		private const string PACKINGPLAN_PACKINGDATE = "RequiredDate";
		private const string PACKINGPLAN_WINNUMBER = "WinNumber";

		private DataTableModel? dataTableModel;
		private DataTable? forecastDataTable;
		private DataTable? packingplanDataTable;
		private DataTable? temporaryDataTable;

		private DataColumn? forecastMatchColumn;
		private DataColumn? forecastDifferenceColumn;
		private DataColumn? packingQuantity;

		public ForecastView()
		{
			InitializeComponent();
			dataTableModel = new DataTableModel();
			forecastDataTable = dataTableModel.GetDataTable(SHEET_NAME_FORECAST, FILE_NAME.Forecast);
			packingplanDataTable = dataTableModel.GetDataTable(SHEET_NAME_PACKINGPLAN, FILE_NAME.PackingPlan);

			GenerateExtraColumns(forecastDataTable);
			CheckForecast(forecastDataTable);

			ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
		}

		private void CheckForecast(DataTable _dataTable)
		{
			bool forecastMatch = false;

			double packingQuantity = 0;

			foreach (DataRow row in _dataTable.Rows)
			{
				packingQuantity = packingplanDataTable.AsEnumerable().Where(item => item.Field<double>(PACKINGPLAN_WINNUMBER) == (double)row[FORECAST_WINNUMBER]).Where(item => item.Field<string>(PACKINGPLAN_DEPOTDATE) == row[FORECAST_DEPOT_DATE].ToString()).Sum(item => item.Field<double>(PACKINGPLAN_QTY));

				forecastMatch = Convert.ToDouble(row[FORECAST_PACK_QTY]) == packingQuantity;

				row[FORECAST_DIFFERENCE] = (double)row[FORECAST_QTY] - packingQuantity;
				row[FORECAST_PACK_QTY] = packingQuantity;
				row[FORECAST_MATCH] = forecastMatch;

				forecastMatch = false;
			}
		}

		private void GenerateExtraColumns(DataTable _dataTable)
		{
			forecastMatchColumn = new DataColumn();
			forecastDifferenceColumn = new DataColumn();
			packingQuantity = new DataColumn();

			forecastMatchColumn.ColumnName = "Forecast Match";
			forecastMatchColumn.DataType = typeof(string);
			forecastMatchColumn.DefaultValue = false.ToString();

			forecastDifferenceColumn.ColumnName = "Difference";
			forecastDifferenceColumn.DataType = typeof(int);
			forecastDifferenceColumn.DefaultValue = 0;

			packingQuantity.ColumnName = "Pack Quantity";
			packingQuantity.DataType = typeof(int);
			packingQuantity.DefaultValue = 0;

			_dataTable.Columns.Add(packingQuantity);
			_dataTable.Columns.Add(forecastDifferenceColumn);

			_dataTable.Columns.Add(forecastMatchColumn);
		}

		private void DepotDateDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			else
			{
				temporaryDataTable = new DataTable();
			}

			var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<string>(FORECAST_DEPOT_DATE) == DepotDateDatePicker.SelectedDate.Value.ToShortDateString());

			if (rows.Count() > 0)
			{
				temporaryDataTable = rows.CopyToDataTable();
			}
			ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
		}

		private void SearchWinTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			else
			{
				temporaryDataTable = new DataTable();
			}
			if (Keyboard.IsKeyDown(System.Windows.Input.Key.Enter))
			{
				if (Double.TryParse(SearchWinTextBox.Text, out double value))
				{
					var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<double>(FORECAST_WINNUMBER) == value);
					if (rows.Count() > 0)
					{
						temporaryDataTable = rows.CopyToDataTable();
						ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
					}
					else
					{
						ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
					}
				}
				else if (SearchWinTextBox.Text == string.Empty)
				{
					ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
				}
			}
		}

		private void QuantityRemoveCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			double quantityToRemove = 0;
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			else
			{
				temporaryDataTable = new DataTable();
			}

			if (QuantityRemoveCheckbox.IsChecked == true)
			{
				var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<double>(FORECAST_QTY) != quantityToRemove);

				if (rows.Count() > 0)
				{
					temporaryDataTable = rows.CopyToDataTable();
				}
				ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
			}
			else
			{
				ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
			}
		}

		private void ForecastMismatchCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			else
			{
				temporaryDataTable = new DataTable();
			}

			var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<string>(FORECAST_MATCH) == ForecastMismatchCheckBox.IsChecked.ToString());

			if (rows.Count() > 0)
			{
				temporaryDataTable = rows.CopyToDataTable();
			}

			ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
		}

		private void ClearDepotDateButton_Click(object sender, RoutedEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
				ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
				//DepotDateDatePicker.SelectedDate
			}
		}

		private void ResetAllButon_Click(object sender, RoutedEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
			ForecastMismatchCheckBox.IsChecked = false;
			QuantityRemoveCheckbox.IsChecked = false;
			SearchWinTextBox.Text = string.Empty;
		}
	}
}