using DespatchEventPlanning.Database;
using DespatchEventPlanning.Helpers;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DespatchEventPlanning.ObjectClasses
{



	internal class PackingProductInformationClass
	{
		static readonly string depotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DepotSplits.xlsx";
		static readonly string defaultDepotSplitsPath = $"{AppDomain.CurrentDomain.BaseDirectory}DefaultDepotSplits.xlsx";
		static DataHandler handler = new DataHandler();
		static DatabaseClass db = new DatabaseClass();
		//static DataTable depotSplits = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.DepotSplits}", depotSplitsPath);
		//static DataTable defaultDepotSplits = handler.ReadExcelFile($"{EnumClass.SHEETNAMES.DepotSplits}",defaultDepotSplitsPath);
				

		public int winNumber { get; set; }
		public string productDescription { get; set; } = string.Empty;
		public string productGroup { get; set; } = string.Empty;
		public string packingDate { get; set; } = string.Empty;
		public string depotDate { get; set; } = string.Empty;
		public double packingQty { get; set; } = 0f;
		public double forecastQty { get { return getForecast(winNumber, depotDate); } }				
		public double difference { get { return forecastQty - packingQty; } }
		public int packsPerPallet { get { return Convert.ToInt32(getProductInformation(winNumber, "PacksPerPallet")); } }
		public double palletsGenerated { get { return Math.Ceiling(packingQty/packsPerPallet); } }	
		public double BEDFORD { get { return getDepotSplit(forecastQty, packingQty, "BEDFORD", depotDate, winNumber); } }
		public double ERITH { get { return getDepotSplit(forecastQty, packingQty, "ERITH", depotDate, winNumber); } }
		public double LUTTERWORTH { get { return getDepotSplit(forecastQty, packingQty, "LUTTERWORTH", depotDate, winNumber); } }
		public double ROCHDALE { get { return getDepotSplit(forecastQty, packingQty, "ROCHDALE", depotDate, winNumber); } }
		public double SKELMERSDALE { get { return getDepotSplit(forecastQty, packingQty, "SKELMERSDALE", depotDate, winNumber); } }
		public double WAKEFIELD { get { return getDepotSplit(forecastQty, packingQty, "WAKEFIELD", depotDate, winNumber); } }
		public double WASHINGTON { get { return getDepotSplit(forecastQty, packingQty, "WASHINGTON", depotDate, winNumber); } }
		public double FALKIRK { get { return getDepotSplit(forecastQty, packingQty, "FALKIRK", depotDate, winNumber); } }
		public double LARNE { get { return getDepotSplit(forecastQty, packingQty, "LARNE", depotDate, winNumber); } }
		public double BRISTOL { get { return getDepotSplit(forecastQty, packingQty, "BRISTOL", depotDate, winNumber); } }
		


		private string getProductInformation(int winNumber,string fieldInTable)
		{
			if(db.GetProductInformation(winNumber, fieldInTable).Length == 0)
			{
				return "1";
			}
			else
			{
				return db.GetProductInformation(winNumber, fieldInTable);
			}
			
		
		}



		private double getForecast(int winNumber, string depotDate)
		{
			return db.GetForecastforProduct(winNumber, depotDate);
		}



		private double getDepotSplit(double forecast, double packQuantity,string depotName,string depotDate,int winNumber)
		{
			double result = 0;
			double splitFound = 0;


			splitFound = db.GetDepotSplit(winNumber, depotName, depotDate);

			//splitFound = depotSplits.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)winNumber).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotDate}") == depotDate).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == depotName).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));

			if (forecast != packingQty)
			{
				result = Math.Round(packQuantity*(splitFound / forecast));
			}
			else
			{
				result = splitFound;
			}
			

			if (splitFound == 0)
			{

				splitFound = db.GetDefaultDepotSplit(winNumber, depotName);
				//splitFound = defaultDepotSplits.AsEnumerable().Where(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.WinNumber}") == (double)winNumber).Where(item => item.Field<string>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.DepotName}") == depotName).Sum(item => item.Field<double>($"{EnumClass.DEPOTSPLITS_DATATABLE_COLUMN_NAMES.Qty}"));

				result = Math.Round(packQuantity * splitFound);
			}
			



			return result;
		}




	}




	

	

}
