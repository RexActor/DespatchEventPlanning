using DespatchEventPlanning.Database;

using System;
using System.Data;

namespace DespatchEventPlanning.Models
{
	public enum Filter_For_Data_Table
	{
		DepotDate,
		RequiredDate,
		PackingQuantity,
	};

	public class DataTableModel
	{
		private readonly string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		
#pragma warning disable S125 // Sections of code should not be commented out
//private readonly string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
#pragma warning restore S125 // Sections of code should not be commented out
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		
#pragma warning disable S125 // Sections of code should not be commented out
//private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		

		public DataTable GetDataTable()
#pragma warning restore S125 // Sections of code should not be commented out
		{
			DataHandler importedData = new DataHandler();

			DataTable packingPlanDataTable = importedData.ReadExcelFile("PackingPlan", filePath);
			packingPlanDataTable.DefaultView.Sort = $"{COLUMN_HEADER_PACKING_DATE_FILTER} ASC";

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = typeof(bool);
			dataColumn.DefaultValue = false;
			dataColumn.ColumnName = "Testing";
			dataColumn.ReadOnly = false;

			packingPlanDataTable.Columns.Add(dataColumn);

			foreach (DataColumn _dataColumn in packingPlanDataTable.Columns)
			{
				if (_dataColumn.ColumnName == "Testing" || _dataColumn.ColumnName == COLUMN_HEADER_PACKING_QUANTITY)
				{
					_dataColumn.ReadOnly = false;
				}
				else
				{
					_dataColumn.ReadOnly = true;
				}
			}

			return packingPlanDataTable;
		}

		/// <summary>
		/// Filters Dataview row with single parameter
		/// </summary>
		/// <param name="view"></param>
		/// <param name="filter"></param>
		/// <param name="filterValue1"></param>
		/// <returns></returns>
		public DataView FilterDataTable(DataView view, Filter_For_Data_Table filter, DateTime filterValue1)
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
		public DataView FilterDataTable(DataView view, Filter_For_Data_Table filter, DateTime filterValue1, Filter_For_Data_Table filter2, DateTime filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' AND {filter2}='{filterValue2}'";

			return view;
		}
	}
}