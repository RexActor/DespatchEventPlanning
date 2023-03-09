using DespatchEventPlanning.ObjectClasses;

using DocumentFormat.OpenXml.Office2010.ExcelAc;

using Microsoft.Data.Sqlite;
using Microsoft.Diagnostics.Runtime.DacInterface;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DespatchEventPlanning.Database
{
	internal class DatabaseClass
	{
		private readonly string _databaseSource = "Database\\despatchPlan.db";

		public bool checkDatabaseTableExists(string tableName)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";

					cmd.Connection = conn;
					conn.Open();

					SqliteDataReader reader = cmd.ExecuteReader();

					if (reader.HasRows)
					{
						return true;
					}
					else { return false; }

					
				}
			}
		}

		public void GenerateDatabaseTable(string tableName)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"CREATE TABLE {tableName} (id INTEGER PRIMARY KEY, " +
						$"winNumber numeric," +
						$"productDescription Text," +
						$"productGroup Text," +
						$"packingDate Text," +
						$"depotDate Text," +
						$"packingQuantity INT," +
						$"forecast INT," +
						$"difference INT," +
						$"packsPerPallet INT," +
						$"palletsGenerated INT," +
						$"BEDFORD INT," +
						$"ERITH INT," +
						$"LUTTERWORTH INT," +
						$"ROCHDALE INT," +
						$"SKELMERSDALE INT," +
						$"WAKEFIELD INT," +
						$"WASHINGTON INT," +
						$"FALKIRK INT," +
						$"LARNE INT," +
						$"BRISTOL INT" +
						$");";

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void saveProductionPlanIntoDatabase(string database_name, int winNumber, string productDescription, string productGroup, string packingDate, string depotDate, int packingQuantity, int forecast, int difference, int packsPerPallet, int palletsGenerated, int bedford, int erith, int lutterworth, int rochdale, int skelmersdale, int wakefield, int washington, int falkirk, int larne, int bristol)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					string sqlQuery = $"INSERT INTO [{database_name}] ([winNumber],[productDescription],[productGroup],[packingDate],[depotDate],[packingQuantity],[forecast],[difference],[packsPerPallet],[palletsGenerated],[BEDFORD],[ERITH],[LUTTERWORTH],[ROCHDALE],[SKELMERSDALE],[WAKEFIELD],[WASHINGTON],[FALKIRK],[LARNE],[BRISTOL]) values({winNumber},'{productDescription}','{productGroup}','{packingDate}','{depotDate}',{packingQuantity},{forecast},{difference},{packsPerPallet},{palletsGenerated},{bedford},{erith},{lutterworth},{rochdale},{skelmersdale},{wakefield},{washington},{falkirk},{larne},{bristol})";

					cmd.CommandText = sqlQuery;
					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();
					

					conn.Close();
				}
			}
		}

		public void saveProductionPlanIntoDatabaseParameterized(string table_name, int winNumber, string productDescription, string productGroup, string packingDate, string depotDate, int packingQuantity, int forecast, int difference, int packsPerPallet, int palletsGenerated, int bedford, int erith, int lutterworth, int rochdale, int skelmersdale, int wakefield, int washington, int falkirk, int larne, int bristol)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productDescription],[productGroup],[packingDate],[depotDate],[packingQuantity],[forecast],[difference],[packsPerPallet],[palletsGenerated],[BEDFORD],[ERITH],[LUTTERWORTH],[ROCHDALE],[SKELMERSDALE],[WAKEFIELD],[WASHINGTON],[FALKIRK],[LARNE],[BRISTOL]) values(@winNumber,@productDescription,@productGroup,@packingDate,@depotDate,@packingQuantity,@forecast,@difference,@packsPerPallet,@palletsGenerated,@bedford,@erith,@lutterworth,@rochdale,@skelmersdale,@wakefield,@washington,@falkirk,@larne,@bristol)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@productGroup", productGroup);
					cmd.Parameters.AddWithValue("@packingDate", packingDate);
					cmd.Parameters.AddWithValue("@depotDate", depotDate);
					cmd.Parameters.AddWithValue("@packingQuantity", packingQuantity);
					cmd.Parameters.AddWithValue("@forecast", forecast);
					cmd.Parameters.AddWithValue("@difference", difference);
					cmd.Parameters.AddWithValue("@packsPerPallet", packsPerPallet);
					cmd.Parameters.AddWithValue("@palletsGenerated", palletsGenerated);
					cmd.Parameters.AddWithValue("@bedford", bedford);
					cmd.Parameters.AddWithValue("@erith", erith);
					cmd.Parameters.AddWithValue("@lutterworth", lutterworth);
					cmd.Parameters.AddWithValue("@rochdale", rochdale);
					cmd.Parameters.AddWithValue("@skelmersdale", skelmersdale);
					cmd.Parameters.AddWithValue("@wakefield", wakefield);
					cmd.Parameters.AddWithValue("@washington", washington);
					cmd.Parameters.AddWithValue("@falkirk", falkirk);
					cmd.Parameters.AddWithValue("@larne", larne);
					cmd.Parameters.AddWithValue("@bristol", bristol);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public List<PackingProductInformationClass> getInformationInList()
		{
			var output = new List<PackingProductInformationClass>();
			
			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}"))
			{
				
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT * FROM ProductionPlan";

					
					cmd.Connection = conn;
					conn.Open();
					
					SqliteDataReader reader = cmd.ExecuteReader();
				
					while (reader.Read())
					{
						output.Add(new PackingProductInformationClass()
						{
							winNumber = (int)reader.GetInt64(reader.GetOrdinal("winNumber")),
							productDescription = reader.GetString(reader.GetOrdinal("productDescription")),
							productGroup = reader.GetString(reader.GetOrdinal("productGroup")),
							packingDate = reader.GetString(reader.GetOrdinal("packingDate")),
							depotDate = reader.GetString(reader.GetOrdinal("depotDate")),
							packingQty = reader.GetInt64(reader.GetOrdinal("packingQuantity")),
							forecastQty = reader.GetInt64(reader.GetOrdinal("forecast")),
							packsPerPallet =(int) reader.GetInt64(reader.GetOrdinal("packsPerPallet"))

						});
						
						
					}
				
					conn.Close();
				}
			}

			return output;

		}
	}
}