using DespatchEventPlanning.Database;

using System;
using System.Data;

namespace DespatchEventPlanning.Models
{
	public enum FILE_NAME
	{
		PackingPlan,
		Forecast,
	}

	public enum Filter_For_Data_Table
	{
		DepotDate,
		RequiredDate,
		PackingQuantity,
	};

	public class DataTableModel
	{
		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";

#pragma warning disable S125 // Sections of code should not be commented out
		//private readonly string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
#pragma warning restore S125 // Sections of code should not be commented out
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";

		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		public DataTable GetDataTable(string sheetName, FILE_NAME fileName)
#pragma warning restore S125 // Sections of code should not be commented out
		{
			DataHandler importedData = new DataHandler();
			DataTable generatedDataTable= null;
			string _chosenFilePath = string.Empty;
			switch (fileName)
			{
				case FILE_NAME.PackingPlan:
					_chosenFilePath = packingPlanFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{COLUMN_HEADER_PACKING_DATE_FILTER} ASC";
					
					break;

				case FILE_NAME.Forecast:
					_chosenFilePath = forecastFilePath;
					generatedDataTable = importedData.ReadExcelFile(sheetName, _chosenFilePath);
					generatedDataTable.DefaultView.Sort = $"{COLUMN_HEADER_DEPOT_DATE_FILTER} ASC";
				
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
		public DataView FilterDataTable(DataView view, Filter_For_Data_Table filter, string filterValue1)
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
		public DataView FilterDataTable_AND(DataView view, Filter_For_Data_Table filter, string filterValue1, Filter_For_Data_Table filter2, string filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' AND {filter2}='{filterValue2}'";

			return view;
		}

		public DataView FilterDataTable_OR(DataView view, Filter_For_Data_Table filter, DateTime filterValue1, Filter_For_Data_Table filter2, DateTime filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' OR {filter2}='{filterValue2}'";

			return view;
		}
	}
}