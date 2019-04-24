using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Configuration;

namespace ADOConsole
{
	public class Program
	{
		public static void Main(string[] args)
		{
			DataProviderFactory();

			Console.ReadLine();
		}

		#region Data Provider Factory
		public static void DataProviderFactory()
		{
			String dataProvider = ConfigurationManager.AppSettings["provider"];
			String connectionString = ConfigurationManager.ConnectionStrings["AutoLotSqlProvider"].ConnectionString;

			DbProviderFactory factory = DbProviderFactories.GetFactory(dataProvider);

			using (DbConnection connection = factory.CreateConnection())
			{
				if(connectionString==null)
				{
					ShowError("Connection");
					return;
				}
				Console.WriteLine($"Your connection is a: {connection.GetType().Name}");
				connection.ConnectionString = connectionString;
				connection.Open();

				// Casting connection to derived type
				var sqlConnection = connection as SqlConnection;
				if(sqlConnection!=null)
					Console.WriteLine($"SQL Server version is: {sqlConnection.ServerVersion}");

				// Make command object
				DbCommand command = factory.CreateCommand();
				if(command==null)
				{
					ShowError("Command");
					return;
				}
				Console.WriteLine($"Your command object is a: {command.GetType().Name}");
				command.Connection = connection;
				command.CommandText = "SELECT * FROM dbo.Inventory";
				using (DbDataReader dataReader = command.ExecuteReader())
				{
					Console.WriteLine($"Your data reader object is a: {dataReader.GetType().Name}");
					Console.WriteLine("\n***** Current Inventory *****");
					while(dataReader.Read())
						Console.WriteLine($"-> Car #{dataReader["CarId"]} is a {dataReader["Make"]}.");
				}
			}
		}

		private static void ShowError(String objectName)
		{
			Console.WriteLine($"There was an issue creating the {objectName}");
		}
		#endregion

		#region Very Simple Connection Factory
		public enum DataProvider
		{
			SqlServer, OleDb, Odbc, None
		}

		public static void ConnectionFactoryMain()
		{
			Console.WriteLine("***** Very Simple Connection Factory *****");
			String dataProviderString = ConfigurationManager.AppSettings["provider"];
			var dataProvider = DataProvider.None;
			if(Enum.IsDefined(typeof(DataProvider), dataProviderString))
				dataProvider=(DataProvider)Enum.Parse(typeof(DataProvider), dataProviderString);
			else
			{
				Console.WriteLine("Sorry, no provider exists!");
				return;
			}
			IDbConnection connection = GetConnection(dataProvider);
			Console.WriteLine($"Your connection is a {connection.GetType().Name ?? "unrecognized type"}");

			Console.ReadLine();
		}

		public static IDbConnection GetConnection(DataProvider dataProvider)
		{
			IDbConnection connection = null;
			switch(dataProvider)
			{
				case DataProvider.SqlServer:
					connection = new SqlConnection();
					break;
				case DataProvider.OleDb:
					connection = new OleDbConnection();
					break;
				case DataProvider.Odbc:
					connection = new OdbcConnection();
					break;
			}

			return connection;
		}
		#endregion
	}
}
