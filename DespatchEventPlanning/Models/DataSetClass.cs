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
		private static DataTable productInformationDatataTable;

		internal static void SetUpDataTables()
		{
			dataSet = new DataSet();
			packingDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.PackingPlan}", EnumClass.FILE_NAME.PackingPlan);
			depotSplitsDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.DepotSplits}", EnumClass.FILE_NAME.DepotSplits);
			defaultDepotSplitsDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.DepotSplits}", EnumClass.FILE_NAME.DefaultDepotSplits);
			forecastDataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.Forecast}", EnumClass.FILE_NAME.Forecast);
			productInformationDatataTable = dataTableModel.GetDataTable($"{EnumClass.SHEETNAMES.ProductInformation}", EnumClass.FILE_NAME.ProductInformation);


			packingDataTable.TableName = $"{EnumClass.DATATABLE_NAME.packingPlanDataTable}";
			depotSplitsDataTable.TableName = $"{EnumClass.DATATABLE_NAME.depotSplitsDataTable}";
			defaultDepotSplitsDataTable.TableName = $"{EnumClass.DATATABLE_NAME.defaultDepotSplits}";
			forecastDataTable.TableName = $"{EnumClass.DATATABLE_NAME.forecast}";
			productInformationDatataTable.TableName = $"{EnumClass.DATATABLE_NAME.productInformation}";

			dataSet.Tables.Add(forecastDataTable);
			dataSet.Tables.Add(depotSplitsDataTable);
			dataSet.Tables.Add(defaultDepotSplitsDataTable);
			dataSet.Tables.Add(packingDataTable);
			dataSet.Tables.Add(productInformationDatataTable);

		}

		internal static DataTable GetDataTable(EnumClass.DATATABLE_NAME tableName)
		{
			if (dataSet == null) { return null; }

				return dataSet.Tables[$"{tableName}"];
		
			
		}
	}
}