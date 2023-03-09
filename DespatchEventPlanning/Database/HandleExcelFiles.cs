﻿using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.Database
{
	internal class HandleExcelFiles
	{

		private List<PackingProductInformationClass> packingPlanList;
		private DataHandler handler;

		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";

		private readonly string productInformationPath = $"{AppDomain.CurrentDomain.BaseDirectory}ProductInformation.xlsx";
		private DatabaseClass db = new DatabaseClass();

		public void CheckDatabaseTable()
		{
			if (db.checkDatabaseTableExists("ProductionPlan"))
			{

				

				packingPlanList = GeneratePackingListFromExcelFile();

				packingPlanList.AsEnumerable().ToList().ForEach(item =>
				{
					db.saveProductionPlanIntoDatabaseParameterized("ProductionPlan", item.winNumber, item.productDescription, item.productGroup, item.packingDate, item.depotDate, (int)item.packingQty, (int)item.forecastQty, (int)item.difference, item.packsPerPallet, (int)item.palletsGenerated, (int)item.BEDFORD, (int)item.ERITH, (int)item.LUTTERWORTH, (int)item.ROCHDALE, (int)item.SKELMERSDALE, (int)item.WAKEFIELD, (int)item.WASHINGTON, (int)item.FALKIRK, (int)item.LARNE, (int)item.BRISTOL);
					
					
				});
				
				Debug.WriteLine($"Database Created and uploaded");
			}
			
			else
			{
				db.GenerateDatabaseTable("ProductionPlan");
				Debug.WriteLine($"Database table Created");
			}
		}


		private List<PackingProductInformationClass> GeneratePackingListFromExcelFile()
		{
			handler = new DataHandler();

			DataTable table = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.PackingPlan}", packingPlanFilePath);

			DataTable forecastTable = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.Forecast}", forecastFilePath);

			DataTable productInfomation = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.ProductInformation}", productInformationPath);

			var convertedList = table.AsEnumerable().Select(row => new PackingProductInformationClass()
			{
				winNumber = Convert.ToInt32(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]),
				productDescription = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.ProductDescription}"]) ?? "N/A",
				productGroup = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.Group}"]) ?? "N/A",
				packingDate = row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.RequiredDate}"].ToString() ?? "N/A",
				depotDate = Convert.ToString(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"]) ?? "N/A",
				packingQty = Convert.ToDouble(row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"]),
				forecastQty = forecastTable.AsEnumerable().Where(item => item.Field<string>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.DepotDate}") == row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"].ToString()).Where(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]).Sum(item => item.Field<double>($"{EnumClass.FORECAST_DATATABLE_COLUMN_NAMES.Qty}")),
				packsPerPallet = (int)productInfomation.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)row[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]).Sum(item => item.Field<double>($"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.PacksPerPallet}")),
			}).ToList();
			return convertedList;
		}



	}
}