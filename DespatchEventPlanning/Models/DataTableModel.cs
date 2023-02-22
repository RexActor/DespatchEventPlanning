using DespatchEventPlanning.Database;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		 string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		 string savePath = $"{AppDomain.CurrentDomain.BaseDirectory}NewFilePlan.xlsx";

		private const string COLUMN_HEADER_PACKING_QUANTITY = "PackingQuantity";
		private const string COLUMN_HEADER_PACKING_DATE_FILTER = "RequiredDate";
		private const string COLUMN_HEADER_DEPOT_DATE_FILTER = "DepotDate";

		

		DataTable? packingPlanDataTable;
		DataHandler? importedData;
		DataColumn dtColumn;
		DataView dataView;
		
		public DataTable GetDataTable()
		{
			importedData = new DataHandler();

			packingPlanDataTable = importedData.ReadExcelFile("PackingPlan", filePath);
			packingPlanDataTable.DefaultView.Sort = $"{COLUMN_HEADER_PACKING_DATE_FILTER} ASC";


			dtColumn = new DataColumn();
			dtColumn.DataType = typeof(bool);
			dtColumn.DefaultValue = false;
			dtColumn.ColumnName = "Testing";
			dtColumn.ReadOnly = false;
			//packingPlanDataTable.Columns.Add(new DataColumn(
			//	"Selected",typeof(bool)
			//));
			packingPlanDataTable.Columns.Add(dtColumn);

							

			foreach (DataColumn dtColumn in packingPlanDataTable.Columns)
			{
				if (dtColumn.ColumnName == "Testing" || dtColumn.ColumnName == COLUMN_HEADER_PACKING_QUANTITY)
				{
					dtColumn.ReadOnly = false;
				}

				else
				{
					dtColumn.ReadOnly = true;
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
