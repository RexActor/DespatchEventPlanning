using DespatchEventPlanning.Database;
using DespatchEventPlanning.Helpers;

using System;
using System.Data;

namespace DespatchEventPlanning.Models
{
	public class DataTableModel
	{
		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";
		private readonly string depotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DepotSplits.xlsx";
		private readonly string defaultDepotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultDepotSplits.xlsx";
		private readonly string productInformationPath = $"{AppDomain.CurrentDomain.BaseDirectory}ProductInformation.xlsx";

		public DataTable GetDataTable(string sheetName, EnumClass.FILE_NAME fileName)

		{
			DataHandler importedData = new DataHandler();
			DataTable generatedDataTable;
			string _chosenFilePath = string.Empty;
			switch (fileName)
			{
				case EnumClass.FILE_NAME.PackingPlan:
					_chosenFilePath = packingPlanFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate} ASC";
					return generatedDataTable;

				case EnumClass.FILE_NAME.ProductInformation:
					_chosenFilePath = productInformationPath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					return generatedDataTable;

				case EnumClass.FILE_NAME.DepotSplits:
					_chosenFilePath = depotSplitsPath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotDate} ASC";
					return generatedDataTable;

				case EnumClass.FILE_NAME.DefaultDepotSplits:
					_chosenFilePath = defaultDepotSplitsPath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber} ASC";
					return generatedDataTable;

				case EnumClass.FILE_NAME.Forecast:
					_chosenFilePath = forecastFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate} ASC";

					return generatedDataTable;

				default: throw new ArgumentNullException($"{_chosenFilePath} ");
			}
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