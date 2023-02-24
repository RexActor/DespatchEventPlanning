using DespatchEventPlanning.Models;

using System;
using System.Data;
using System.Linq;
using System.Windows.Controls;

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

		private DataTableModel dataTableModel;
		private DataTable forecastDataTable;
		private DataTable packingplanDataTable;

		private DataColumn forecastMatchColumn;
		private DataColumn forecastDifferenceColumn;
		private DataColumn packingQuantity;

		public ForecastView()
		{
			InitializeComponent();
			dataTableModel = new DataTableModel();
			forecastDataTable = dataTableModel.GetDataTable(SHEET_NAME_FORECAST, FILE_NAME.Forecast);
			packingplanDataTable = dataTableModel.GetDataTable(SHEET_NAME_PACKINGPLAN, FILE_NAME.PackingPlan);

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

			forecastDataTable.Columns.Add(packingQuantity);
			forecastDataTable.Columns.Add(forecastDifferenceColumn);
		
			forecastDataTable.Columns.Add(forecastMatchColumn);


			CheckForecast(forecastDataTable);

			ForecastDataGrid.ItemsSource = forecastDataTable.DefaultView;
		}

		private void CheckForecast(DataTable _dataTable)
		{
			bool forecastMatch = false;
			int difference = 0;
			double packingQuantity = 0;

			foreach (DataRow row in _dataTable.Rows)
			{
				//dataTableModel.FilterDataTable(packingplanDataTable.DefaultView, Filter_For_Data_Table.DepotDate, row[FORECAST_DEPOT_DATE].ToString()).d;

				packingQuantity = packingplanDataTable.AsEnumerable().Where(item => item.Field<double>(PACKINGPLAN_WINNUMBER) == (double)row[FORECAST_WINNUMBER]).Where(item => item.Field<string>(PACKINGPLAN_DEPOTDATE) == row[FORECAST_DEPOT_DATE].ToString()).Sum(item => item.Field<double>(PACKINGPLAN_QTY));

				row[FORECAST_DIFFERENCE] = (double)row[FORECAST_QTY] - packingQuantity;
				forecastMatch = Convert.ToDouble(row[FORECAST_PACK_QTY]) == packingQuantity;
				row[FORECAST_PACK_QTY] = packingQuantity;

				row[FORECAST_MATCH] = forecastMatch;
				forecastMatch = false;
			}
		}
	}
}