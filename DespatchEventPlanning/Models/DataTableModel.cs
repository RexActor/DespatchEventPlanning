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
		// string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		//DataView? dataView;

		public DataTable GetDataTable()
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

		public DataView FilterDataTable(DataView view, Filter_For_Data_Table filter, DateTime filterValue1)
		{
			view.RowFilter = $"{filter}='{filterValue1}'";

			return view;
		}

		public DataView FilterDataTable(DataView view, Filter_For_Data_Table filter, DateTime filterValue1, Filter_For_Data_Table filter2, DateTime filterValue2)
		{
			view.RowFilter = $"{filter} = '{filterValue1}' AND {filter2}='{filterValue2}'";

			return view;
		}
	}
}