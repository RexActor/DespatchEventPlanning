using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.Models;

using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;


namespace DespatchEventPlanning.Views
{
	/// <summary>
	/// Interaction logic for ForecastView.xaml
	/// </summary>
	/// 
	
	public partial class ForecastView : UserControl
	{
		private DataTableModel? dataTableModel;
		private DataTable? forecastDataTable;
		private DataTable? packingplanDataTable;
		private DataTable? temporaryDataTable;
		private DataTable? depotSplitDataTable;
		private DataTable? defaultDepotSplitsDataTable;

		private DataColumn? forecastMatchColumn;
		private DataColumn? forecastDifferenceColumn;
		private DataColumn? packingQuantity;
		private DataColumn? depotColumn;

		private DataView view;

		public ForecastView()
		{
			InitializeComponent();
			dataTableModel = new DataTableModel();
			view = new DataView();

			forecastDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.Forecast}", EnumClass.FILE_NAME.Forecast);
			packingplanDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.PackingPlan}", EnumClass.FILE_NAME.PackingPlan);
			defaultDepotSplitsDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.DepotSplits}", EnumClass.FILE_NAME.DefaultDepotSplits);

			depotSplitDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.DepotSplits}", EnumClass.FILE_NAME.DepotSplits);
			GenerateExtraColumns(forecastDataTable);
			CheckForecast(forecastDataTable);
			GetDepotSplits(forecastDataTable);

			view = forecastDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}") != 0).AsDataView();

			ForecastDataGrid.ItemsSource = view;

			

		}

		private void GetDepotSplits(DataTable forecastDataTable)
		{
			if (depotSplitDataTable == null || defaultDepotSplitsDataTable == null) { return; }

			forecastDataTable.Rows.Cast<DataRow>().ToList().ForEach(forecastRow =>
			{

				Enum.GetNames(typeof(EnumClass.DEPOTS)).OrderBy(x => x).ToList().ForEach(_depotNameInEnum
					=>
				{
					var result_found = depotSplitDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)forecastRow[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}"]).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotDate}") == forecastRow[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate}"].ToString()).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == _depotNameInEnum).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));

					if (double.TryParse(result_found.ToString(), out double result))
					{
						if (result_found > 0)
						{
							forecastRow[_depotNameInEnum] = result_found;
						}
						else
						{
							var defaultSplit = defaultDepotSplitsDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)forecastRow[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}"]).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == _depotNameInEnum).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));
							forecastRow[_depotNameInEnum] = Math.Round((double)forecastRow[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}"] * defaultSplit);
						}
					}
					else
					{
						forecastRow[_depotNameInEnum] = -1;
					}
				});






			});

		
		}
		
		private void CheckForecast(DataTable _dataTable)
		{
			if (packingplanDataTable == null) { return; }

			bool forecastMatch = false;

			double packingQuantityToAssign = 0;

			Stopwatch sw = new Stopwatch();
			sw.Start();

			_dataTable.Rows.Cast<DataRow>().ToList().ForEach(row =>
			{
				packingQuantityToAssign = packingplanDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}"]).Where(item => item.Field<string>($"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}") == row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate}"].ToString()).Sum(item => item.Field<double>($"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"));

				forecastMatch = Convert.ToDouble(row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.PackQuantity}"]) == packingQuantityToAssign;

				row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Difference}"] = (double)row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}"] - packingQuantityToAssign;
				row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.PackQuantity}"] = packingQuantityToAssign;
				row[$"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.ForecastMatch}"] = forecastMatch;
			});


			sw.Stop();
			Debug.WriteLine($"Elapsed = {sw.Elapsed}");

		
		}

		private void GenerateExtraColumns(DataTable _dataTable)
		{
			forecastMatchColumn = new DataColumn();
			forecastDifferenceColumn = new DataColumn();
			packingQuantity = new DataColumn();

			forecastMatchColumn.ColumnName = "ForecastMatch";
			forecastMatchColumn.DataType = typeof(string);
			forecastMatchColumn.DefaultValue = false.ToString();

			forecastDifferenceColumn.ColumnName = "Difference";
			forecastDifferenceColumn.DataType = typeof(int);
			forecastDifferenceColumn.DefaultValue = 0;

			packingQuantity.ColumnName = "PackQuantity";
			packingQuantity.DataType = typeof(int);
			packingQuantity.DefaultValue = 0;

			_dataTable.Columns.Add(packingQuantity);
			_dataTable.Columns.Add(forecastDifferenceColumn);

			_dataTable.Columns.Add(forecastMatchColumn);


			Enum.GetNames(typeof(EnumClass.DEPOTS)).OrderBy(x => x).ToList().ForEach(_enum =>
			{
				depotColumn = new DataColumn();
				depotColumn.ColumnName = _enum;
				depotColumn.DataType = typeof(double);
				depotColumn.DefaultValue = 0;
				_dataTable.Columns.Add(depotColumn);
			});


			
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

			if (forecastDataTable != null && DepotDateDatePicker.SelectedDate != null)
			{
				var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<string>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate}") == DepotDateDatePicker.SelectedDate.Value.ToShortDateString());
				if (rows.Any())
				{
					temporaryDataTable = rows.CopyToDataTable();
				}
			}
			ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
		}

		private void SearchWinTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (forecastDataTable == null)
			{
				return;
			}
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
					var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}") == value);
					if (rows.Any())
					{
						temporaryDataTable = rows.CopyToDataTable();
						ForecastDataGrid.ItemsSource = temporaryDataTable.DefaultView;
					}
					else
					{
						ForecastDataGrid.ItemsSource = view;
					}
				}
				else if (SearchWinTextBox.Text == string.Empty)
				{
					ForecastDataGrid.ItemsSource = view;
				}
			}
		}

		private void QuantityRemoveCheckbox_Checked(object sender, RoutedEventArgs e)
		{
			if (forecastDataTable == null) { return; }

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
				var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}") != quantityToRemove);

				if (rows.Any())
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
			if (forecastDataTable == null) { return; }

			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			else
			{
				temporaryDataTable = new DataTable();
			}

			var rows = forecastDataTable.AsEnumerable().Where(item => item.Field<string>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.ForecastMatch}") == ForecastMismatchCheckBox.IsChecked.ToString());

			if (rows.Any())
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
				ForecastDataGrid.ItemsSource = view;
				//DepotDateDatePicker.SelectedDate
			}
		}

		private void ResetAllButon_Click(object sender, RoutedEventArgs e)
		{
			if (temporaryDataTable != null)
			{
				temporaryDataTable.Clear();
			}
			ForecastDataGrid.ItemsSource = view;
			ForecastMismatchCheckBox.IsChecked = false;
			QuantityRemoveCheckbox.IsChecked = false;
			SearchWinTextBox.Text = string.Empty;
		}
	}
}