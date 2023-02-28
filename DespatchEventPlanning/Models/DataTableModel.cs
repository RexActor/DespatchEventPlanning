using DespatchEventPlanning.Database;
using DespatchEventPlanning.Helpers;

using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace DespatchEventPlanning.Models
{
	public class DataTableModel
	{
		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";
		private readonly string depotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DepotSplits.xlsx";
		private readonly string defaultDepotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultDepotSplits.xlsx";

		
		public DataTable GetDataTable(string sheetName, EnumClass.FILE_NAME fileName)
#pragma warning restore S125 // Sections of code should not be commented out
		{
			DataHandler importedData = new DataHandler();
			DataTable generatedDataTable = null;
			string _chosenFilePath = string.Empty;
			switch (fileName)
			{
				case EnumClass.FILE_NAME.PackingPlan:
					_chosenFilePath = packingPlanFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate} ASC";

					break;

				case EnumClass.FILE_NAME.DepotSplits:
					_chosenFilePath = depotSplitsPath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotDate} ASC";
					break;

				case EnumClass.FILE_NAME.DefaultDepotSplits:
					_chosenFilePath = defaultDepotSplitsPath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber} ASC";
					break;

				case EnumClass.FILE_NAME.Forecast:
					_chosenFilePath = forecastFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate} ASC";

					break;
			}
			return generatedDataTable;
		}

		/// <summary>
		/// Filters Dataview row with single parameter
		/// </summary>
		/// <param name="view"></param>
		/// <param name="filter"></param>
		/// <param name="filterValue1"></param>
		/// <returns></returns>
		public DataView FilterDataTable(DataView view, EnumClass.Filter_For_Data_Table filter, string filterValue1)
		{
			view.RowFilter = $"{filter}='{filterValue1}'";

			return view;
		}

		/// <summary>
		/// Filters Dataview row with 2 parameters
		/// </summary>
		/// <param name="view"></param>
		/// <param name="filter"></param>
		/// <param name="filterValue1"></param>
		/// <param name="filter2"></param>
		/// <param name="filterValue2"></param>
		/// <returns></returns>
		public DataView FilterDataTable_AND(DataView view, EnumClass.Filter_For_Data_Table filter, string filterValue1, EnumClass.Filter_For_Data_Table filter2, string filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' AND {filter2}='{filterValue2}'";

			return view;
		}

		public DataView FilterDataTable_OR(DataView view, EnumClass.Filter_For_Data_Table filter, DateTime filterValue1, EnumClass.Filter_For_Data_Table filter2, DateTime filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' OR {filter2}='{filterValue2}'";

			return view;
		}
	}
}