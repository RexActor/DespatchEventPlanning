namespace DespatchEventPlanning.Helpers
{
	public class EnumClass
	{

		internal enum FILTER_OPTION
		{
			PACKINGDATE,
			DEPOTDATE,
			BOTH,
		}

		public enum DEPOTS
		{
			ERITH = 7461,
			LUTTERWORTH = 7445,
			ROCHDALE = 7479,
			SKELMERSDALE = 7452,
			WAKEFIELD = 7447,
			WASHINGTON = 7457,
			FALKIRK = 7460,
			LARNE = 7430,
			BRISTOL = 7498,
			BEDFORD = 7439,
		}

		public enum SHEETNAMES
		{
			PackingPlan,
			Forecast,
			SDepotSplits,
			DepotSplits,
		}

		public enum FILE_NAME
		{
			PackingPlan,
			Forecast,
			DepotSplits,
			DefaultDepotSplits,
		}

		public enum Filter_For_Data_Table
		{
			DepotDate,
			RequiredDate,
			PackingQuantity,
		};

		public enum FORECAST_DATATABLE_COLUMN_NAMES
		{
			DepotDate,
			Qty,
			WinNumber,
			Difference,
			PackQuantity,
			ForecastMatch,
		}

		public enum PACKINGPLAN_DATATABLE_COLUMN_NAMES
		{
			PackingQuantity,
			DepotDate,
			RequiredDate,
			WinNumber,
		}

		public enum DEPOTSPLITS_DATATABLE_COLUMN_NAMES
		{
			WinNumber,
			DepotName,
			DepotDate,
			Qty,
		}
	}
}