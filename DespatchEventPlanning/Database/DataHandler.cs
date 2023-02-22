using ClosedXML.Excel;

using DocumentFormat.OpenXml.Spreadsheet;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DespatchEventPlanning.Database
{
	internal class DataHandler

	{
		///<summary>
		///Load Excel File into Datatable
		///</summary>

		public DataTable ReadExcelFile(string sheetName, string path)
		{

			using (OleDbConnection connection = new OleDbConnection())
			{
				DataTable table = new DataTable();
				string importFile = path;
				string importFileExtension = Path.GetExtension(path);
				if (importFileExtension == ".xls")
				{
					connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + importFile + ";" + "Extended Properties ='Excel 8.0; HDR=YES;'";
						
				}

				if (importFileExtension == ".xlsx")
				{
					connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + importFile + ";" + "Extended Properties ='Excel 12.0 Xml; HDR=YES;'";

				}


				using (OleDbCommand command = connection.CreateCommand())
				{
					command.CommandText = "Select * from [" + sheetName + "$]";
					command.Connection = connection;
					using (OleDbDataAdapter adapter = new OleDbDataAdapter())
					{
						adapter.SelectCommand = command;
						adapter.Fill(table);
						return table;
					}
				}
			}
		}


		public void SaveDatatableToExcel(DataTable table,string path)
		{
			XLWorkbook wb = new XLWorkbook();
			
			wb.Worksheets.Add(table,"PackingPlan");
			wb.SaveAs(path);
		}
		

		

	}
}
