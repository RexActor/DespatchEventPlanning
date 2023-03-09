using DocumentFormat.OpenXml.ExtendedProperties;

using Microsoft.Data.Sqlite;

using System;
using System.Diagnostics;

namespace DespatchEventPlanning.Database
{
	internal class DatabaseClass
	{

		private readonly string _databaseSource = "Database\\despatchPlan.db";

		public void saveProductionPlanIntoDatabase(int input)
		{


			
			using (SqliteConnection conn = new SqliteConnection($"data source ={_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{


					string sqlQuery = $"INSERT INTO [ProductionPlan] ([winNumber]) values({input})";

					cmd.CommandText = sqlQuery;
					cmd.Connection= conn;
					conn.Open();

					cmd.ExecuteNonQuery();

					conn.Close();

				}
			}
		}

		public void getInformation()
		{
			string output = string.Empty;
			using (SqliteConnection conn = new SqliteConnection($"data source = {_databaseSource}"))
			{
				using (SqliteCommand cmd = new SqliteCommand())
				{


					string sqlQuery = $"SELECT * FROM ProductionPlan";

					cmd.CommandText = sqlQuery;
					cmd.Connection = conn;
					conn.Open();

					SqliteDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						Debug.WriteLine(reader.GetString(1));
					}

					conn.Close();


				}
			}

			
		}



	}
}