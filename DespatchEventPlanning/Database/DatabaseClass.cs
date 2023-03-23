using DespatchEventPlanning.ObjectClasses;

using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;

namespace DespatchEventPlanning.Database
{
	internal class DatabaseClass
	{
		private readonly string _databaseSource = "Database\\despatchPlan.db";

		public bool checkDatabaseTableExists(string tableName)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
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
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
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
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
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
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
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

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
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
							packsPerPallet = (int)reader.GetInt64(reader.GetOrdinal("packsPerPallet"))
						});
					}

					conn.Close();
				}
			}

			return output;
		}

		public void saveProductInformation(string table_name, int winNumber, int productNumber, string productDescription, int packsPerPallet, string productGroup, int weightOfOuter)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productNumber],[productDescription],[packsPerPallet],[productGroup],[weightOfOuter]) values(@winNumber,@productNumber,@productDescription,@packsPerPallet,@productGroup,@weightOfOuter)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productNumber", productNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@packsPerPallet", packsPerPallet);
					cmd.Parameters.AddWithValue("@productGroup", productGroup);
					cmd.Parameters.AddWithValue("@weightOfOuter", weightOfOuter);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void saveDepotSplits(string table_name, int winNumber, string productDescription, int depotNumber, string depotName, string depotDate, int qty)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productDescription],[depotNumber],[depotName],[depotDate],[qty]) values(@winNumber,@productDescription,@depotNumber,@depotName,@depotDate,@qty)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@depotNumber", depotNumber);
					cmd.Parameters.AddWithValue("@depotName", depotName);
					cmd.Parameters.AddWithValue("@depotDate", depotDate);
					cmd.Parameters.AddWithValue("@qty", qty);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void saveDefaultDepotSplits(string table_name, string productGroup, int winNumber, string productDescription, string depotName, float qty)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([productGroup],[winNumber],[productDescription],[depotName],[qty]) values(@productGroup,@winNumber,@productDescription,@depotName,@qty)";

					cmd.Parameters.AddWithValue("@productGroup", productGroup);
					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@depotName", depotName);
					cmd.Parameters.AddWithValue("@qty", Math.Round(qty, 3));

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void saveForecast(string table_name, int winNumber, string productDescription, string depotDate, int qty)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productDescription],[depotDate],[qty]) values(@winNumber,@productDescription,@depotDate,@qty)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@depotDate", depotDate);
					cmd.Parameters.AddWithValue("@qty", qty);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void savePackingPlan(string table_name, int winNumber, string productDescription, string productGroup, string packingDate, string depotDate, int packingQuantity)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productDescription],[productGroup],[packingDate],[depotDate],[packingQuantity]) values(@winNumber,@productDescription,@productGroup,@packingDate,@depotDate,@packingQuantity)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@productGroup", productGroup);
					cmd.Parameters.AddWithValue("@packingDate", packingDate);
					cmd.Parameters.AddWithValue("@depotDate", depotDate);
					cmd.Parameters.AddWithValue("@packingQuantity", packingQuantity);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public void saveProductIntoStorageLoad(string table_name, int winNumber, string productDescription, string storageDate, string depotDate, string depotName, int allocatedCases, string loadReference, int allocatedPallets)
		{
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"INSERT INTO [{table_name}] ([winNumber],[productDescription],[storageDate],[depotDate],[depotName],[allocatedCases],[loadReference],[allocatedPallets]) values(@winNumber,@productDescription,@storageDate,@depotDate,@depotName,@allocatedCases,@loadReference,@allocatedPallets)";

					cmd.Parameters.AddWithValue("@winNumber", winNumber);
					cmd.Parameters.AddWithValue("@productDescription", productDescription);
					cmd.Parameters.AddWithValue("@storageDate", storageDate);
					cmd.Parameters.AddWithValue("@depotDate", depotDate);
					cmd.Parameters.AddWithValue("@depotName", depotName);
					cmd.Parameters.AddWithValue("@allocatedCases", allocatedCases);
					cmd.Parameters.AddWithValue("@loadReference", loadReference);
					cmd.Parameters.AddWithValue("@allocatedPallets", allocatedPallets);

					cmd.Connection = conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();
				}
			}
		}

		public DataTable getInformationInDataTable()
		{
			var output = new DataTable();

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT * FROM ProductionPlan";

					cmd.Connection = conn;
					conn.Open();
					SqliteDataReader reader = cmd.ExecuteReader();

					output.Load(reader);

					return output;
				}
			}
		}

		public List<Storage> getStorageInformationInList()
		{
			var output = new List<Storage>();

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT * FROM StorageAllocation";

					cmd.Connection = conn;
					conn.Open();

					SqliteDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						output.Add(new Storage()
						{
							winNumber = (int)reader.GetInt64(reader.GetOrdinal("winNumber")),
							productDescription = reader.GetString(reader.GetOrdinal("productDescription")),
							storageDate = reader.GetString(reader.GetOrdinal("storageDate")),
							depotDate = reader.GetString(reader.GetOrdinal("depotDate")),
							depotName = reader.GetString(reader.GetOrdinal("depotName")),
							quantityCases = (int)reader.GetInt64(reader.GetOrdinal("allocatedCases")),
							loadReference = reader.GetString(reader.GetOrdinal("loadReference")),
							quantityPalletsAllocated = (int)reader.GetInt64(reader.GetOrdinal("allocatedPallets"))
						});
					}

					conn.Close();
				}
			}

			return output;
		}

		public string productExistsInStorage(int winNumber, string depotDate, string storageDate, string depotName)
		{
			string result = string.Empty;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM StorageAllocation Where winNumber = {winNumber} AND  storageDate =@storageDate AND depotDate =@depotDate AND depotName =@depotName";
					cmd.Parameters.Add("@storageDate", SqliteType.Text).Value = storageDate;
					cmd.Parameters.Add("@depotDate", SqliteType.Text).Value = depotDate;
					cmd.Parameters.Add("@depotName", SqliteType.Text).Value = depotName;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = "FALSE";
					}
					else
					{
						result = "TRUE";
					}

					conn.Close();
				}
			}

			return result;
		}

		public bool productExistsInProductInformationTable(int winNumber, int productNumber)
		{
			bool result = false;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM ProductInformation Where winNumber = {winNumber} AND  productNumber =@productNumber";
					cmd.Parameters.Add("@productNumber", SqliteType.Integer).Value = productNumber;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = true;
					}
					else
					{
						result = false;
					}

					conn.Close();
				}
			}

			return result;
		}

		public bool productExistsIndepotSplitTable(int winNumber, int depotNumber, string depotDate)
		{
			bool result = false;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM DepotSplits Where winNumber = {winNumber} AND  depotNumber =@depotNumber AND depotDate=@depotDate";
					cmd.Parameters.Add("@depotNumber", SqliteType.Integer).Value = depotNumber;
					cmd.Parameters.Add("@depotDate", SqliteType.Text).Value = depotDate;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = true;
					}
					else
					{
						result = false;
					}

					conn.Close();
				}
			}

			return result;
		}

		public bool productExistsInDefaultDepotSplitTable(int winNumber, string depotName)
		{
			bool result = false;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM DefaultDepotSplits Where winNumber = {winNumber} AND  depotName =@depotName";
					cmd.Parameters.Add("@depotName", SqliteType.Text).Value = depotName;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = true;
					}
					else
					{
						result = false;
					}

					conn.Close();
				}
			}

			return result;
		}

		public bool productExistsInForecastTable(int winNumber, string depotDate)
		{
			bool result = false;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM Forecast Where winNumber = {winNumber} AND  depotDate =@depotDate";
					cmd.Parameters.Add("@depotDate", SqliteType.Text).Value = depotDate;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = true;
					}
					else
					{
						result = false;
					}

					conn.Close();
				}
			}

			return result;
		}

		public bool productExistsInPackingPlanTable(int winNumber, string packingDate, string depotDate)
		{
			bool result = false;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT Count(*) FROM PackingPlan Where winNumber = {winNumber}  AND packingDate=@packingDate AND  depotDate=@depotDate";
					cmd.Parameters.Add("@packingDate", SqliteType.Text).Value = packingDate;
					cmd.Parameters.Add("@depotDate", SqliteType.Text).Value = depotDate;

					cmd.Connection = conn;
					conn.Open();

					int entryFound = (int)Convert.ToInt64(cmd.ExecuteScalar());

					if (entryFound > 0)
					{
						result = true;
					}
					else
					{
						result = false;
					}

					conn.Close();
				}
			}

			return result;
		}

		public List<string> GetDatabaseTables()
		{
			List<string> result = new List<string>();

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"SELECT name FROM sqlite_sequence";

					cmd.Connection = conn;
					conn.Open();
					cmd.ExecuteNonQuery();

					SqliteDataReader reader = cmd.ExecuteReader();

					while (reader.Read())
					{
						result.Add(reader.GetString(0));
					}

					conn.Close();
				}
			}
			return result;
		}

		public void clearDatbaseTable(string tableName)
		{
			string result = string.Empty;

			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}; Mode=ReadWrite;"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{
					cmd.CommandText = $"DELETE FROM {tableName}";

					cmd.Connection = conn;
					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}