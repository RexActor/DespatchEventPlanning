using DespatchEventPlanning.Helpers;
using DespatchEventPlanning.ObjectClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace DespatchEventPlanning.Database
{
	internal class HandleExcelFiles
	{
		private List<PackingProductInformationClass> packingPlanList;
		private DataHandler handler;

		private readonly string packingPlanFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}PackingPlan.xlsx";
		private readonly string forecastFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}Forecast.xlsx";

		private readonly string productInformationPath = $"{AppDomain.CurrentDomain.BaseDirectory}ProductInformation.xlsx";
		private readonly string depotSplitPath = $"{AppDomain.CurrentDomain.BaseDirectory}DepotSplits.xlsx";
		private readonly string defaultDepotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultDepotSplits.xlsx";
		private DatabaseClass db = new DatabaseClass();

		public void CheckDatabaseTable()
		{
			if (db.checkDatabaseTableExists("ProductionPlan"))
			{
				//packingPlanList = GeneratePackingListFromExcelFile();

				packingPlanList = GeneratePackingListFromDatabase();

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

		public List<PackingProductInformationClass> GeneratePackingListFromDatabase()
		{
			DataTable packingPlan = db.getPackingPlanInDataTable();

			var convertedList = packingPlan.AsEnumerable().Select(item => new PackingProductInformationClass
			{
				winNumber = Convert.ToInt32(item[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.WinNumber}"]),
				productDescription = Convert.ToString(item[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.ProductDescription}"]),
				productGroup = Convert.ToString(item["productGroup"]),
				packingDate = Convert.ToString(item[$"packingDate"]),
				depotDate = Convert.ToString(item[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.DepotDate}"]),
				packingQty = Convert.ToDouble(item[$"{EnumClass.PACKINGPLAN_DATATABLE_COLUMN_NAMES.PackingQuantity}"])
			}).ToList();

			return convertedList;
		}



		public List<PackingProductInformationClass> GeneratePackingListFromExcelFile()
		{
			List<PackingPlanClass> packingPlan = GeneratePackingPlan();

			var convertedList = packingPlan.AsEnumerable().Select(item => new PackingProductInformationClass
			{
				winNumber = item.winNumber,
				productDescription = item.productDescription,
				productGroup = item.productGroup,
				packingDate = item.packingDate,
				depotDate = item.depotDate,
				packingQty = item.packingQuantity
			}).ToList();

			return convertedList;
		}



		public List<ProductInformationClass> GenerateProductInformation()
		{
			handler = new DataHandler();

			var convertedList = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.ProductInformation}", productInformationPath).AsEnumerable().Select(item => new ProductInformationClass()
			{
				winNumber = Convert.ToInt32(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.WinNumber}"]),
				productNumber = Convert.ToInt32(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.ProductNumber}"]),
				productDescription = Convert.ToString(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.ProductDescription}"]),
				packsPerPallet = Convert.ToInt32(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.PacksPerPallet}"]),
				productGroup = Convert.ToString(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.ProductGroup}"]),
				weightOfOuter = Convert.ToInt32(item[$"{EnumClass.PRODUCTINFORMATION_DATATABLE_COLUMN_NAMES.WeightOfOuter}"])
			}).ToList();

			return convertedList;
		}

		public List<depotSplitClass> GenerateDepotSplits()
		{
			handler = new DataHandler();
			var convertedList = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.DepotSplits}", depotSplitPath).AsEnumerable().Select(item => new depotSplitClass()
			{
				winNumber = Convert.ToInt32(item[$"WinNumber"]),
				productDescription = Convert.ToString(item[$"ProductDescription"]),
				depotNumber = Convert.ToInt32(item[$"DepotNumber"]),
				depotName = Convert.ToString(item[$"DepotName"]),
				depotDate = Convert.ToString(item[$"DepotDate"]),
				qty = Convert.ToInt32(item[$"Qty"]),
			}).ToList();

			return convertedList;
		}

		public List<DefaultDepotSplitsClass> GenerateDefaultDepotSplits()
		{
			handler = new DataHandler();
			var convertedList = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.DepotSplits}", defaultDepotSplitsPath).AsEnumerable().Select(item => new DefaultDepotSplitsClass()
			{
				productGroup = Convert.ToString(item["ProductGroup"]),
				winNumber = Convert.ToInt32(item["WinNumber"]),
				productDescription = Convert.ToString(item["ProductDescription"]),
				depotName = Convert.ToString(item["DepotName"]),
				qty = (float)Math.Round(Convert.ToDouble(item["Qty"]), 3)
			}).ToList();

			return convertedList;
		}

		public List<PackingPlanClass> GeneratePackingPlan()
		{
			handler = new DataHandler();

			var convertedList = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.PackingPlan}", packingPlanFilePath).AsEnumerable().Select(item => new PackingPlanClass()
			{
				winNumber = Convert.ToInt32(item["WinNumber"]),
				productDescription = Convert.ToString(item["ProductDescription"]),
				productGroup = Convert.ToString(item["Group"]),
				packingDate = Convert.ToString(item["RequiredDate"]),
				depotDate = Convert.ToString(item["DepotDate"]),
				packingQuantity = Convert.ToInt32(item["PackingQuantity"])
			}).ToList();

			return convertedList;
		}

		public List<ForecastClass> GenerateForecast()
		{
			handler = new DataHandler();
			var convertedList = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.Forecast}", forecastFilePath).AsEnumerable().Select(item => new ForecastClass()
			{
				winNumber = Convert.ToInt32(item["WinNumber"]),
				productDescription = Convert.ToString(item["ProductDescription"]),
				depotDate = Convert.ToString(item["DepotDate"]),
				qty = Convert.ToInt32(item["Qty"])
			}).ToList();

			return convertedList;
		}
	}
}