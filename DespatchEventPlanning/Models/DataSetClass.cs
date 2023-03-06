using DespatchEventPlanning.Helpers;

using System;
using System.Data;
using System.Diagnostics;

namespace DespatchEventPlanning.Models
{
	internal static class DataSetClass

	{
		private static DataTableModel dataTableModel = new DataTableModel();

		private static DataSet dataSet;

		private static DataTable packingDataTable;
		private static DataTable depotSplitsDataTable;
		private static DataTable defaultDepotSplitsDataTable;
		private static DataTable forecastDataTable;

		internal static void SetUpDataTables()
		{
			dataSet = new DataSet();
			packingDataTable = dataTableModel.GetDataTable("PackingPlan", EnumClass.FILE_NAME.PackingPlan);
			depotSplitsDataTable = dataTableModel.GetDataTable("DepotSplits", EnumClass.FILE_NAME.DepotSplits);
			defaultDepotSplitsDataTable = dataTableModel.GetDataTable("DepotSplits", EnumClass.FILE_NAME.DefaultDepotSplits);
			forecastDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.Forecast}", EnumClass.FILE_NAME.Forecast);

			packingDataTable.TableName = EnumClass.DATATABLE_NAME.packingPlanDataTable.ToString();
			depotSplitsDataTable.TableName = EnumClass.DATATABLE_NAME.depotSplitsDataTable.ToString();
			defaultDepotSplitsDataTable.TableName = EnumClass.DATATABLE_NAME.defaultDepotSplits.ToString();
			forecastDataTable.TableName = EnumClass.DATATABLE_NAME.forecast.ToString();

			dataSet.Tables.Add(forecastDataTable);
			dataSet.Tables.Add(depotSplitsDataTable);
			dataSet.Tables.Add(defaultDepotSplitsDataTable);
			dataSet.Tables.Add(packingDataTable);
		}

		internal static DataTable GetDataTable(EnumClass.DATATABLE_NAME tableName)
		{
			if (dataSet == null) { return null; }

				return dataSet.Tables[$"{tableName}"];
		
			
		}
	}
}